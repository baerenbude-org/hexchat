<!-- SPDX-License-Identifier: GPL-2.0-or-later -->
<!-- HexChat (Avalonia Port) - .agents/rules/irc_protocol_and_testing.md -->
<!-- Description: IRC-Protokollrichtlinien (RFC 1459/2812, IRCv3) und Unit-Testing Standards. -->

# IRC Protocol & Testing Guidelines — HexChat

## 1. IRC Standards & Spezifikationen

HexChat unterstützt sowohl klassische als auch moderne IRC-Netzwerke:

1. **Klassisches IRC (RFC 1459 / 2812):**
   - Framing: Zeilen enden strikt mit `\r\n` (CRLF). Maximale Zeilenlänge beträgt 512 Bytes (inkl. CRLF).
   - Format: `[:prefix] <command> [params...] [:trailing]`
   - Numerics: `001` (RPL_WELCOME), `005` (RPL_ISUPPORT / PROTOCTL), `332` (RPL_TOPIC), `353` (RPL_NAMREPLY), etc.
2. **IRCv3 Spezifikationen (IRCv3.org):**
   - `cap-notify` / `CAP LS 302`: Verhandlung von Server-Fähigkeiten beim Handshake.
   - `sasl` (PLAIN, EXTERNAL / CertFP, SCRAM-SHA-256): Sichere Authentifizierung vor dem Registrierungsabschluss.
   - `message-tags`: Client- und Server-Tags (Präfix `@tag1=val;tag2 :prefix COMMAND...`).
   - `server-time`: Exakte Server-Zeitstempel (`@time=2026-08-29T05:30:00.000Z`).
   - `batch`: Gruppierung zusammenhängender Nachrichten (z. B. NETSPLIT / NETJOIN oder Chathistory).
   - `echo-message` & `labeled-response`: Bestätigung selbst gesendeter Nachrichten für Multi-Client-Synchronisation.
   - `chathistory`: Standardisiertes Nachladen historischer Nachrichten (`CHATHISTORY LATEST/BEFORE/AFTER`).

---

## 2. Text- und Farbformatierung (mIRC Codes)

1. **Format-Tokens:**
   - `0x02` = Bold
   - `0x1D` = Italic
   - `0x1F` = Underline
   - `0x16` = Reverse Color
   - `0x0F` = Reset all formatting
   - `0x03` = Color Code (`\x03[FG][,BG]`, z. B. `\x0304,01` für Rot auf Schwarz)
   - `0x04` = Hex/RGB Color Code (modernes IRCv3 Feature)
2. **Avalonia Inlines Parser:**
   - Der formatierte IRC-Text wird in Avalonia `FormattedText` oder `InlineCollection` (Run, Bold, Underline, Span) überführt.

---

## 3. Unit-Testing & QA Standards

1. **Framework & Tools:**
   - xUnit (`tests/HexChat.Core.Tests`).
   - FluentAssertions für aussagekräftige Assertions (`result.Should().Be("expected");`).
   - NSubstitute für das Mocking von Services und I/O-Schnittstellen.
2. **100 % Deterministische Tests:**
   - Unit-Tests dürfen niemals externe Sockets öffnen oder auf das Internet zugreifen.
   - Nutze `System.IO.Pipelines.Pipe` oder `MemoryStream`, um Netzwerk-Streams deterministisch im Test zu simulieren.
3. **Muster für Message-Parser Tests:**
   ```csharp
   [Theory]
   [InlineData(":nick!user@host PRIVMSG #channel :Hello World!", "nick", "PRIVMSG", "#channel", "Hello World!")]
   [InlineData("@time=2026-08-29T00:00:00.000Z :server 001 nick :Welcome", "server", "001", "nick", "Welcome")]
   public void Parse_ValidMessage_ReturnsCorrectStructure(
       string raw, string expectedPrefix, string expectedCmd, string expectedTarget, string expectedTrailing)
   {
       var msg = IrcMessageParser.Parse(raw);
       msg.Prefix.Should().Be(expectedPrefix);
       msg.Command.Should().Be(expectedCmd);
       msg.Parameters[0].Should().Be(expectedTarget);
       msg.Trailing.Should().Be(expectedTrailing);
   }
   ```
