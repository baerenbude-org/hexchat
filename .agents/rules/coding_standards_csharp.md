<!-- SPDX-License-Identifier: GPL-2.0-or-later -->
<!-- HexChat (Avalonia Port) - .agents/rules/coding_standards_csharp.md -->
<!-- Description: C# 13 und .NET 10 Entwicklungsstandards, High-Performance Parsing und Code-Hygiene. -->

# C# 13 & .NET 10 Coding Standards — HexChat

## 1. Moderne C# Sprachfeatures

1. **File-Scoped Namespaces:** Jede C#-Datei nutzt Dateibereichs-Namespaces zur Reduzierung von Einrückungsebenen:
   ```csharp
   namespace HexChat.Core.Protocol;
   ```
2. **Primary Constructors:** Klassen und Records nutzen Primary Constructors, wo dies die Lesbarkeit erhöht:
   ```csharp
   public sealed class IrcMessage(string rawText, string command, IReadOnlyList<string> parameters)
   {
       public string RawText { get; } = rawText;
       public string Command { get; } = command;
       public IReadOnlyList<string> Parameters { get; } = parameters;
   }
   ```
3. **Records für unveränderliche Daten (DTOs / Events):**
   ```csharp
   public sealed record IrcUserPrefix(string Nickname, string? Username = null, string? Hostname = null);
   ```
4. **Pattern Matching:** Switch-Expressions und Typ-Patterns statt verschachtelter `if-else` Ketten und Typecasts.

---

## 2. Nullability & Typ-Sicherheit

1. **Strikte Nullable Reference Types:** Nullable ist im gesamten Projekt aktiviert (`#nullable enable`).
2. **Keine Null-Forgiving Operatoren (`!`):** Vermeide den `!` Operator, außer bei nachweislich zur Laufzeit garantiert initialisierten Properties (z. B. Designer/XAML-Properties).
3. **Guard Clauses:** Argumente und Vorbedingungen mit `ArgumentNullException.ThrowIfNull(param)` validieren.

---

## 3. High-Performance & Zero-Allocation Parsing

Der IRC-Stream kann Tausende von Nachrichten pro Sekunde liefern (z. B. bei `/LIST`, großen Channel-Joins oder Channel-Playback via IRCv3 `chathistory`).

1. **Span-basiertes Parsing:**
   - Nutze `ReadOnlySpan<char>` und `MemoryExtensions.Split` / `IndexOf` zur Tokenisierung von Tags, Prefixen und Commandos.
   - Vermeide `string.Split()`, `string.Substring()` oder temporäre `List<string>` Allokationen in Schleifen.
2. **System.IO.Pipelines:**
   - Eingehende TCP-Streams werden über `PipeReader` / `PipeWriter` verarbeitet, um Pufferwiederverwendung und Zero-Copy Line-Framing (`\r\n`) zu gewährleisten.
3. **String Pooling:**
   - Häufig wiederkehrende IRC-Befehle (`PRIVMSG`, `JOIN`, `PART`, `QUIT`, `PING`, `PONG`, `001`–`999`) sollten über statisch gecachte Instanzen oder String-Pools aufgelöst werden.

---

## 4. Asynchrone Programmierung (`async` / `await`)

1. **Echte Asynchronität:**
   - Nutze durchgängig `async Task` oder `ValueTask`.
   - Niemals synchrone Blocking-Calls (`.Result`, `.GetAwaiter().GetResult()`, `.Wait()`) ausführen.
2. **CancellationTokens:**
   - Jede asynchrone Methode, die I/O, Socket-Verbindungen oder Delays ausführt, MUSS einen `CancellationToken cancellationToken = default` Parameter entgegennehmen und an unterlagerte Methoden weiterleiten.
3. **IAsyncEnumerable<T>:**
   - Eingehende IRC-Nachrichten-Streams werden bevorzugt als `IAsyncEnumerable<IrcMessage>` oder über `System.Threading.Channels.Channel<T>` bereitgestellt.

---

## 5. Logging & Fehlerbehandlung

1. **Strukturiertes Logging:**
   - Nutze `Microsoft.Extensions.Logging.ILogger<T>`.
   - Logging-Nachrichten formatieren mit Template-Parametern (`logger.LogInformation("Connected to {Host}:{Port}", host, port)`), niemals String-Interpolation im Logaufruf.
2. **Exception Handling:**
   - Fange nur spezifische Exceptions ab (`SocketException`, `AuthenticationException`, `OperationCanceledException`).
   - Niemals leere `catch {}` Blöcke ohne Logging oder Handhabung hinterlassen.
