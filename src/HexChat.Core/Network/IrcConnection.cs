using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace HexChat.Core.Network;

public sealed class IrcConnectionOptions
{
    public required string Host { get; init; }
    public required int Port { get; init; } = 6697;
    public bool UseTls { get; init; } = true;
    public bool IgnoreInvalidCertificates { get; init; } = false;
    public Encoding Encoding { get; init; } = Encoding.UTF8;
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);
}

/// <summary>
/// Low-level asynchronous TCP and TLS connection handler for IRC.
/// </summary>
public sealed class IrcConnection : IAsyncDisposable
{
    private TcpClient? _tcpClient;
    private Stream? _stream;
    private StreamReader? _reader;
    private StreamWriter? _writer;
    private readonly SemaphoreSlim _sendLock = new(1, 1);
    private CancellationTokenSource? _cts;

    public bool IsConnected => _tcpClient?.Connected == true && _stream != null;

    public event Func<string, Task>? LineReceived;
    public event Func<Exception?, Task>? Disconnected;

    public async Task ConnectAsync(IrcConnectionOptions options, CancellationToken cancellationToken = default)
    {
        Disconnect();

        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _tcpClient = new TcpClient
        {
            NoDelay = true
        };

        await _tcpClient.ConnectAsync(options.Host, options.Port, _cts.Token).ConfigureAwait(false);
        Stream netStream = _tcpClient.GetStream();

        if (options.UseTls)
        {
            var sslStream = new SslStream(
                netStream,
                leaveInnerStreamOpen: false,
                userCertificateValidationCallback: (sender, cert, chain, errors) =>
                {
                    if (options.IgnoreInvalidCertificates) return true;
                    return errors == SslPolicyErrors.None;
                });

            var sslOptions = new SslClientAuthenticationOptions
            {
                TargetHost = options.Host,
                EnabledSslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13
            };

            await sslStream.AuthenticateAsClientAsync(sslOptions, _cts.Token).ConfigureAwait(false);
            _stream = sslStream;
        }
        else
        {
            _stream = netStream;
        }

        _reader = new StreamReader(_stream, options.Encoding, detectEncodingFromByteOrderMarks: false, bufferSize: 4096, leaveOpen: true);
        _writer = new StreamWriter(_stream, options.Encoding, bufferSize: 4096, leaveOpen: true)
        {
            AutoFlush = true,
            NewLine = "\r\n"
        };

        // Start receive loop in background
        _ = Task.Run(() => ReceiveLoopAsync(_cts.Token), CancellationToken.None);
    }

    public async Task SendLineAsync(string line, CancellationToken cancellationToken = default)
    {
        if (_writer == null || !IsConnected)
        {
            throw new InvalidOperationException("Not connected to IRC server.");
        }

        await _sendLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await _writer.WriteLineAsync(line.AsMemory(), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            _sendLock.Release();
        }
    }

    private async Task ReceiveLoopAsync(CancellationToken cancellationToken)
    {
        Exception? disconnectReason = null;
        try
        {
            while (!cancellationToken.IsCancellationRequested && _reader != null)
            {
                string? line = await _reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
                if (line == null)
                {
                    break; // Server closed connection
                }

                if (string.IsNullOrWhiteSpace(line)) continue;

                if (LineReceived != null)
                {
                    await LineReceived.Invoke(line).ConfigureAwait(false);
                }
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Clean exit
        }
        catch (Exception ex)
        {
            disconnectReason = ex;
        }
        finally
        {
            Disconnect();
            if (Disconnected != null)
            {
                await Disconnected.Invoke(disconnectReason).ConfigureAwait(false);
            }
        }
    }

    public void Disconnect()
    {
        _cts?.Cancel();
        _reader?.Dispose();
        _writer?.Dispose();
        _stream?.Dispose();
        _tcpClient?.Dispose();

        _reader = null;
        _writer = null;
        _stream = null;
        _tcpClient = null;
    }

    public async ValueTask DisposeAsync()
    {
        Disconnect();
        _sendLock.Dispose();
        _cts?.Dispose();
        await Task.CompletedTask;
    }
}
