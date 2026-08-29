using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HexChat.Core.Commands;
using HexChat.Core.Formatting;
using HexChat.Core.State;

namespace HexChat.UI.ViewModels;

public sealed partial class ChannelItemViewModel : ViewModelBase
{
    public IrcChannel Channel { get; }

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private int _unreadCount;

    [ObservableProperty]
    private int _highlightCount;

    [ObservableProperty]
    private string? _topic;

    public ObservableCollection<ChatMessage> Messages { get; } = new();
    public ObservableCollection<ChannelUser> Users { get; } = new();

    public ChannelItemViewModel(IrcChannel channel)
    {
        Channel = channel;
        _name = channel.Name;
        _topic = channel.Topic;
    }

    public void RefreshState()
    {
        UnreadCount = Channel.UnreadCount;
        HighlightCount = Channel.HighlightCount;
        Topic = Channel.Topic;
    }
}

public sealed partial class MainViewModel : ViewModelBase
{
    private readonly IrcClient _client = new();

    [ObservableProperty]
    private string _serverName = "irc.libera.chat";

    [ObservableProperty]
    private int _port = 6697;

    [ObservableProperty]
    private bool _useTls = true;

    [ObservableProperty]
    private string _nickname = "HexChatUser";

    [ObservableProperty]
    private string _inputText = string.Empty;

    [ObservableProperty]
    private bool _isConnected;

    [ObservableProperty]
    private ChannelItemViewModel? _selectedChannel;

    public ObservableCollection<ChannelItemViewModel> Channels { get; } = new();

    public MainViewModel()
    {
        // Add Server buffer as default
        var serverItem = new ChannelItemViewModel(_client.ServerBuffer);
        Channels.Add(serverItem);
        SelectedChannel = serverItem;

        _client.Connected += OnConnected;
        _client.Disconnected += OnDisconnected;
        _client.ChannelJoined += OnChannelJoined;
        _client.ChannelParted += OnChannelParted;
        _client.MessageReceived += OnMessageReceived;
        _client.UserListUpdated += OnUserListUpdated;
        _client.TopicUpdated += OnTopicUpdated;
    }

    [RelayCommand]
    private async Task ConnectAsync()
    {
        if (IsConnected) return;

        try
        {
            var options = new IrcClientOptions
            {
                Server = ServerName,
                Port = Port,
                UseTls = UseTls,
                Nickname = Nickname,
                AutoJoinChannels = new[] { "#hexchat", "#avalonia" }
            };

            await _client.ConnectAsync(options);
        }
        catch (Exception ex)
        {
            _client.ServerBuffer.AddMessage(new ChatMessage(
                DateTimeOffset.UtcNow,
                null,
                $"Verbindungsfehler: {ex.Message}",
                Array.Empty<FormattedSpan>(),
                IsSystemNotice: true));
        }
    }

    [RelayCommand]
    private async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(InputText) || SelectedChannel == null) return;

        string textToSend = InputText;
        InputText = string.Empty;

        var ctx = new CommandExecutionContext
        {
            Client = _client,
            ActiveChannel = SelectedChannel.Channel,
            OnSystemMessage = msg =>
            {
                SelectedChannel.Channel.AddMessage(new ChatMessage(DateTimeOffset.UtcNow, null, msg, Array.Empty<FormattedSpan>(), IsSystemNotice: true));
                SelectedChannel.Messages.Add(new ChatMessage(DateTimeOffset.UtcNow, null, msg, Array.Empty<FormattedSpan>(), IsSystemNotice: true));
            },
            OnClearScreen = () =>
            {
                SelectedChannel.Messages.Clear();
            }
        };

        try
        {
            await HexChatCommandHandler.ExecuteAsync(textToSend, ctx);
        }
        catch (Exception ex)
        {
            ctx.OnSystemMessage($"Befehlsfehler: {ex.Message}");
        }
    }

    private void OnConnected()
    {
        IsConnected = true;
    }

    private void OnDisconnected(Exception? ex)
    {
        IsConnected = false;
    }

    private void OnChannelJoined(IrcChannel channel)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            var item = Channels.FirstOrDefault(c => string.Equals(c.Name, channel.Name, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                item = new ChannelItemViewModel(channel);
                Channels.Add(item);
            }
            SelectedChannel = item;
        });
    }

    private void OnChannelParted(IrcChannel channel, string? reason)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            var item = Channels.FirstOrDefault(c => string.Equals(c.Name, channel.Name, StringComparison.OrdinalIgnoreCase));
            if (item != null && item.Channel.Type != ChannelType.Server)
            {
                Channels.Remove(item);
                if (SelectedChannel == item)
                {
                    SelectedChannel = Channels.FirstOrDefault();
                }
            }
        });
    }

    private void OnMessageReceived(IrcChannel channel, ChatMessage msg)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            var item = Channels.FirstOrDefault(c => string.Equals(c.Name, channel.Name, StringComparison.OrdinalIgnoreCase));
            if (item != null)
            {
                item.Messages.Add(msg);
                item.RefreshState();
            }
        });
    }

    private void OnUserListUpdated(IrcChannel channel)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            var item = Channels.FirstOrDefault(c => string.Equals(c.Name, channel.Name, StringComparison.OrdinalIgnoreCase));
            if (item != null)
            {
                item.Users.Clear();
                foreach (var user in channel.Users.Values.OrderBy(u => u.RankOrder).ThenBy(u => u.User.Nick))
                {
                    item.Users.Add(user);
                }
            }
        });
    }

    private void OnTopicUpdated(IrcChannel channel, string? topic)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            var item = Channels.FirstOrDefault(c => string.Equals(c.Name, channel.Name, StringComparison.OrdinalIgnoreCase));
            if (item != null)
            {
                item.Topic = topic;
            }
        });
    }
}
