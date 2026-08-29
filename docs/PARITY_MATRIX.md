<!-- SPDX-License-Identifier: GPL-2.0-or-later -->
<!-- HexChat (Avalonia Port) - docs/PARITY_MATRIX.md -->
<!-- Description: Master-Paritätsmatrix: Lückenlose Gegenüberstellung aller C-Quellcodedateien mit C# / Avalonia 12 Pendants. -->

# 📊 HexChat Master Paritätsmatrix (C/GTK2 ➔ C# 13 / Avalonia 12)

> **Lückenloser Abgleich aller Module, Dateien, Dialoge, Engines und Funktionen**  
> Ziel: **100% funktionale und visuelle Parität** mit dem originalen HexChat C-Codebase ([`legacy/`](../legacy)).

---

## 📌 Übersicht & Status-Legende

| Symbol | Bedeutung | Kriterien |
| :---: | :--- | :--- |
| 🟩 | **DONE** | Vollständig in C# & Avalonia implementiert, funktional paritär und durch Unit-/UI-Tests abgedeckt. |
| 🟨 | **IN PROGRESS** | Teilweise implementiert (z. B. Core-Logik vorhanden, UI-View oder Tests fehlen noch). |
| ⬜ | **PLANNED** | Im Backlog erfasst, Konzeption oder C-Referenzanalyse abgeschlossen, Implementierung steht aus. |

---

## 🖼️ Bereich A: Fenster, Dialoge & UI-Views (`legacy/src/fe-gtk/`)

| Komponente / Dialog | Legacy C-Quellcode | Modernes C# / Avalonia Pendant | Status | Tests & Notizen |
| :--- | :--- | :--- | :---: | :--- |
| **Hauptfenster & Shell** | [`fe-gtk.c`](../legacy/src/fe-gtk/fe-gtk.c), [`maingui.c`](../legacy/src/fe-gtk/maingui.c) | [`MainWindow.axaml`](file:///d:/Quelltext/hexchat/src/HexChat.UI/Views/MainWindow.axaml), [`MainViewModel.cs`](file:///d:/Quelltext/hexchat/src/HexChat.UI/ViewModels/MainViewModel.cs) | 🟨 | Grundlayout, Channel-/User-Liste und Input-Box vorhanden. Feinschliff für Splitter, Badges & Focus. |
| **Netzwerk- & Serverliste** | [`servlistgui.c`](../legacy/src/fe-gtk/servlistgui.c) | `ServerListView.axaml`, `ServerListViewModel.cs` | ⬜ | Netzwerke hinzufügen/bearbeiten, Server-URLs, SSL/TLS, SASL, Fav-Channels, Auto-Connect. |
| **Einstellungen (Preferences)** | [`setup.c`](../legacy/src/fe-gtk/setup.c) | `PreferencesView.axaml`, `PreferencesViewModel.cs` | ⬜ | Alle 11 Konfigurations-Reiter (Appearance, Input, Userlist, Tabs, Colors, General, Alerts, Sound, Logs, Advanced, Network). |
| **Kanal-Listen Dialog** | [`chanlist.c`](../legacy/src/fe-gtk/chanlist.c) | `ChannelListView.axaml`, `ChannelListViewModel.cs` | ⬜ | Suche/Filterung nach Kanalname, Mindest-/Höchst-User, Topic und Regex (`/LIST`). |
| **Bann- & Moderationslisten** | [`banlist.c`](../legacy/src/fe-gtk/banlist.c) | `BanListView.axaml`, `BanListViewModel.cs` | ⬜ | Bans (+b), Quiets (+q), Excepts (+e), Invites (+I) mit Setzer und Datum. |
| **DCC Dateiübertragungen & Chat** | [`dccgui.c`](../legacy/src/fe-gtk/dccgui.c) | `DccManagerView.axaml`, `DccChatWindow.axaml` | ⬜ | Fortschrittsanzeige, Transfer-Geschwindigkeiten, Resume, Abort, Auto-Accept. |
| **Freunde- & Notify-Liste** | [`notifygui.c`](../legacy/src/fe-gtk/notifygui.c) | `NotifyListView.axaml`, `NotifyListViewModel.cs` | ⬜ | Online/Offline-Statusanzeige mit Netzwerk-Filtern und Doppel-Klick Query. |
| **Ignore-Listen Manager** | [`ignoregui.c`](../legacy/src/fe-gtk/ignoregui.c) | `IgnoreListView.axaml`, `IgnoreListViewModel.cs` | ⬜ | Filter nach Maske (`*!*@*.host`), Typen (PRIV, CHAN, NOTI, CTCP, DCC, INVI, ALL). |
| **Raw Log Traffic Inspektor** | [`rawlog.c`](../legacy/src/fe-gtk/rawlog.c) | `RawLogView.axaml`, `RawLogViewModel.cs` | ⬜ | Inbound/Outbound IRC-Rohdatenanzeige mit Timestamp, Filter und Pause-Button. |
| **URL Grabber Fenster** | [`urlgrab.c`](../legacy/src/fe-gtk/urlgrab.c) | `UrlGrabberView.axaml`, `UrlGrabberViewModel.cs` | ⬜ | Erfassung aller Links in Channels, Kopieren, im Browser öffnen, Historie leeren. |
| **Plugin- & Script-Manager** | [`plugingui.c`](../legacy/src/fe-gtk/plugingui.c) | `PluginManagerView.axaml`, `PluginManagerViewModel.cs` | ⬜ | Laden/Entladen von DLLs/Python-Skripten, Anzeige von Metadaten und Beschreibungen. |
| **Text Events Editor** | [`textgui.c`](../legacy/src/fe-gtk/textgui.c) | `TextEventsView.axaml`, `TextEventsViewModel.cs` | ⬜ | Anpassung aller Event-Format-Strings (`pevents.conf`), Farbwahl und Test-Vorschau. |
| **Tastenkürzel & F-Keys** | [`fkeys.c`](../legacy/src/fe-gtk/fkeys.c) | `KeyBindingsView.axaml`, `KeyBindingsViewModel.cs` | ⬜ | Eigene Tastenkürzel für IRC-Befehle und UI-Aktionen definieren. |
| **Listen-Editor (Popup/Auto-Replace)** | [`editlist.c`](../legacy/src/fe-gtk/editlist.c) | `EditListView.axaml`, `EditListViewModel.cs` | ⬜ | Wiederverwendbarer Editor für Auto-Replace, User-Commands, CTCP-Replies und Popups. |
| **Kanal beitreten Dialog** | [`joind.c`](../legacy/src/fe-gtk/joind.c) | `JoinChannelDialog.axaml`, `JoinChannelViewModel.cs` | ⬜ | Schnelleingabe für Channel-Name und Passwort/Key. |
| **ASCII-Zeichentabelle** | [`ascii.c`](../legacy/src/fe-gtk/ascii.c) | `AsciiChartView.axaml`, `AsciiChartViewModel.cs` | ⬜ | Zeichentabelle zum schnellen Einfügen von Sonderzeichen und Steuerzeichen. |
| **Farbpalette & Textformatierer** | [`palette.c`](../legacy/src/fe-gtk/palette.c) | `ColorPalettePicker.axaml`, `ColorPaletteViewModel.cs` | ⬜ | Farbwähler für mIRC Farbcodes 0–15, 16–99 und TrueColor. |
| **Textsuche im Chat-Buffer** | `search.c`, [`xtext.c`](../legacy/src/fe-gtk/xtext.c) | `BufferSearchView.axaml`, `BufferSearchViewModel.cs` | ⬜ | Suche nach oben/unten (F3 / Shift+F3, Strg+F), Regex, Groß-/Kleinschreibung. |

---

## 📋 Bereich B: Menüs, Untermenüs & Kontext-Popups (`legacy/src/fe-gtk/menu.c`)

| Menü-Pfad / Popup | Legacy C Referenz | Modernes C# / Avalonia Pendant | Status | Details & Tastenkürzel |
| :--- | :--- | :--- | :---: | :--- |
| **Menü: He_xChat** | [`menu.c`](../legacy/src/fe-gtk/menu.c#L1775) | `HexChatMenu.axaml` in `MainWindow.axaml` | 🟨 | Network List (`Strg+S`), New Server Tab (`Strg+T`), New Window (`Strg+N`), Load Plugin, Detach, Close, Quit (`Strg+Q`). |
| **Menü: _View** | [`menu.c`](../legacy/src/fe-gtk/menu.c#L1796) | `ViewMenu.axaml` in `MainWindow.axaml` | 🟨 | Menu Bar (`F9`), Topic Bar, User List (`F7`), Channel Switcher (Tabs vs Tree), Fullscreen (`F11`). |
| **Menü: _Server** | [`menu.c`](../legacy/src/fe-gtk/menu.c#L1819) | `ServerMenu.axaml` in `MainWindow.axaml` | 🟨 | Disconnect, Reconnect, Join Channel, Channel List, Marked Away (`Strg+A`). |
| **Menü: _Usermenu** | [`menu.c`](../legacy/src/fe-gtk/menu.c#L1828) | `UserMenu.axaml` in `MainWindow.axaml` | ⬜ | Dynamisch durch Benutzer und Plugins erweiterbares Usermenu (`/MENU`). |
| **Menü: S_ettings** | [`menu.c`](../legacy/src/fe-gtk/menu.c#L1830) | `SettingsMenu.axaml` in `MainWindow.axaml` | 🟨 | Preferences, Auto Replace, CTCP Replies, Keyboard Shortcuts, Text Events, URL Handlers, User Commands. |
| **Menü: _Window** | [`menu.c`](../legacy/src/fe-gtk/menu.c#L1843) | `WindowMenu.axaml` in `MainWindow.axaml` | 🟨 | Ban List, Character Chart, Direct Chat, File Transfers, Friends List, Ignore List, Plugins, Raw Log, URL Grabber, Clear Text, Search (`Strg+F`). |
| **Menü: _Help** | [`menu.c`](../legacy/src/fe-gtk/menu.c#L1866) | `HelpMenu.axaml` in `MainWindow.axaml` | 🟨 | Contents (`F1`), About Dialog. |
| **Userlist Kontextmenü** | [`userlistgui.c`](../legacy/src/fe-gtk/userlistgui.c) | `UserListContextMenu.axaml` | ⬜ | Whois, Query, Op/Deop, Voice/Devoice, Ban, Kick, KickBan, DCC Send, CTCP Ping/Time/Version, Ignore. |
| **Channel Tab Kontextmenü** | [`chanview.c`](../legacy/src/fe-gtk/chanview.c) | `ChannelTabContextMenu.axaml` | ⬜ | Close Tab, Detach Window, Part Channel, Rejoin / Cycle, Clear Buffer, Channel Settings. |
| **Chat Text Auswahl Popup** | [`xtext.c`](../legacy/src/fe-gtk/xtext.c) | `ChatViewContextMenu.axaml` | ⬜ | Copy Selection, Copy Link, Open URL, Google Search, Clear Window. |

---

## ⚡ Bereich C: Slash Commands & Direktiven (`legacy/src/common/outbound.c`)

> Detaillierte Befehlsaufschlüsselung siehe [`docs/COMMANDS_PARITY.md`](COMMANDS_PARITY.md).

| Befehlskategorie | Befehle | Status | Ziel-Implementierung |
| :--- | :--- | :---: | :--- |
| **Kanal-Verwaltung** | `/JOIN`, `/PART`, `/CYCLE`, `/TOPIC`, `/MODE`, `/BAN`, `/UNBAN`, `/QUIET`, `/UNQUIET`, `/KICK`, `/KICKBAN`, `/INVITE`, `/NAMES`, `/LIST`, `/CHANOPT` | 🟨 | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) |
| **Privat- & Messaging** | `/MSG`, `/QUERY`, `/NOTICE`, `/ME`, `/SAY`, `/WALLCHOP`, `/DOAT`, `/ALLCHAN`, `/ALLCHANL`, `/ALLSERV` | 🟨 | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) |
| **Moderation & Ränge** | `/OP`, `/DEOP`, `/VOICE`, `/DEVOICE`, `/HOP`, `/DEHOP`, `/MOP`, `/MDEOP`, `/MHOP`, `/MDEHOP`, `/MKICK` | 🟨 | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) |
| **Server & Verbindung** | `/SERVER`, `/SERVCHAN`, `/NEWSERVER`, `/RECONNECT`, `/DISCON`, `/QUIT`, `/QUOTE`, `/RAW`, `/PING`, `/GHOST`, `/ID` | 🟨 | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) |
| **Client-Steuerung & UI** | `/CLEAR`, `/CLOSE`, `/SET`, `/GETBOOL`, `/GETINT`, `/GETSTR`, `/GETFILE`, `/GUI`, `/MENU`, `/SETCURSOR`, `/SETTAB`, `/SETTEXT`, `/TRAY`, `/ECHO`, `/HELP` | 🟨 | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) |
| **Subsysteme & Tools** | `/DCC`, `/CTCP`, `/NCTCP`, `/IGNORE`, `/UNIGNORE`, `/NOTIFY`, `/URL`, `/DNS`, `/COUNTRY`, `/EXEC`, `/EXECKILL`, `/LOAD`, `/UNLOAD`, `/RELOAD` | ⬜ | Subsystem-Handler in `HexChat.Core/Commands/` |

---

## 🌐 Bereich D: IRC Protokoll, Numerics & IRCv3 Capabilities (`legacy/src/common/proto-irc.c`, `inbound.c`)

| Protokoll-Element | Legacy C Referenz | Modernes C# Pendant | Status | Tests & RFC-Bezug |
| :--- | :--- | :--- | :---: | :--- |
| **Message-Parsing (RFC 1459/2812)** | [`proto-irc.c`](../legacy/src/common/proto-irc.c) | [`IrcMessage.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Protocol/IrcMessage.cs) | 🟩 | [`IrcMessageParserTests.cs`](file:///d:/Quelltext/hexchat/tests/HexChat.Core.Tests/IrcMessageParserTests.cs) (100% grün) |
| **RFC Numerics (001–999)** | [`proto-irc.c`](../legacy/src/common/proto-irc.c) | [`IrcNumerics.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Protocol/IrcNumerics.cs) | 🟩 | Standard-Numerics (RPL_WELCOME, RPL_ISUPPORT 005, ERR_NOSUCHNICK etc.) |
| **Message Tags (IRCv3.2)** | [`inbound.c`](../legacy/src/common/inbound.c) | [`IrcTags.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Protocol/IrcTags.cs) | 🟩 | `server-time`, `account-tag`, `msgid`, `batch` etc. |
| **Capability Negotiation (`CAP LS/REQ/ACK`)** | [`proto-irc.c`](../legacy/src/common/proto-irc.c) | [`IrcClient.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/State/IrcClient.cs) | 🟨 | `sasl`, `message-tags`, `server-time`, `echo-message`, `chathistory` |
| **SASL PLAIN Authentifizierung** | [`proto-irc.c`](../legacy/src/common/proto-irc.c) | [`SaslPlain.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Security/SaslPlain.cs) | 🟩 | [`SaslTests.cs`](file:///d:/Quelltext/hexchat/tests/HexChat.Core.Tests/SaslTests.cs) |
| **SASL SCRAM-SHA-256 Authentifizierung** | [`scram.c`](../legacy/src/common/scram.c) | [`SaslScramSha256.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Security/SaslScramSha256.cs) | 🟩 | [`SaslTests.cs`](file:///d:/Quelltext/hexchat/tests/HexChat.Core.Tests/SaslTests.cs) |
| **SASL EXTERNAL (CertFP / Client Cert)** | [`ssl.c`](../legacy/src/common/ssl.c) | `SaslExternal.cs` | ⬜ | Geplant |
| **Batch Support (`BATCH`)** | [`inbound.c`](../legacy/src/common/inbound.c) | `IrcBatchHandler.cs` | ⬜ | Geplant |
| **ISUPPORT (005) Token Parser** | [`proto-irc.c`](../legacy/src/common/proto-irc.c) | `IServerCapabilities.cs` | 🟨 | CHANMODES, PREFIX, CHANTYPES, NETWORK, CASEMAPPING |

---

## 🧠 Bereich E: Core Engine & State Management (`legacy/src/common/server.c`, `userlist.c` etc.)

| Modul | Legacy C Referenz | Modernes C# Pendant | Status | Details |
| :--- | :--- | :--- | :---: | :--- |
| **Client Session & Lifecycle** | [`server.c`](../legacy/src/common/server.c), [`hexchat.c`](../legacy/src/common/hexchat.c) | [`IrcClient.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/State/IrcClient.cs) | 🟨 | Verbindungsaufbau, Reconnect-Schleifen, Auto-Perform, Ping/Pong Heartbeat. |
| **Channel State & Modes** | [`chanopt.c`](../legacy/src/common/chanopt.c), [`modes.c`](../legacy/src/common/modes.c) | [`IrcChannel.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/State/IrcChannel.cs) | 🟨 | Channel-Modi (`+ntikmlbeI`), Topic, Topic-Setter, Timestamp. |
| **User & Nicklist State** | [`userlist.c`](../legacy/src/common/userlist.c) | [`IrcUser.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/State/IrcUser.cs) | 🟨 | Nick, Hostmask, Ränge (Owner, Admin, Op, Halfop, Voice), Away-Status. |
| **Chat Scrollback Buffer** | [`history.c`](../legacy/src/common/history.c) | `ChatBuffer.cs` | 🟨 | Unbegrenzter/konfigurierbarer Scrollback, Marker-Line, Log-Persistenz. |
| **User Ignore Engine** | [`ignore.c`](../legacy/src/common/ignore.c) | `IgnoreService.cs` | ⬜ | Wildcard- und Masken-Matching für automatische Blockierung. |
| **Freunde- & Notify-Monitor** | [`notify.c`](../legacy/src/common/notify.c) | `NotifyService.cs` | ⬜ | IRC `MONITOR` / `WATCH` bzw. `ISON` Polling für Nick-Überwachung. |
| **Per-Channel Overrides (chanopt)** | [`chanopt.c`](../legacy/src/common/chanopt.c) | `ChannelOptionsService.cs` | ⬜ | Kanalspezifische Einstellungen für Logging, Alerts, Join/Part-Muting. |

---

## 💾 Bereich F: Konfiguration, Persistenz & Defaults (`legacy/src/common/cfgfiles.c`)

| Einstellungsbereich | Legacy C Datei | Modernes C# Format | Status | Details |
| :--- | :--- | :--- | :---: | :--- |
| **Hauptkonfiguration** | `hexchat.conf` ([`cfgfiles.c`](../legacy/src/common/cfgfiles.c)) | `appsettings.json` / `HexChatConfig.cs` | 🟨 | Alle Booleans, Integers und Strings aus HexChat C. |
| **Netzwerk- & Serverliste** | `servlist.conf` ([`servlist.c`](../legacy/src/common/servlist.c)) | `networks.json` / `ServerListConfig.cs` | 🟨 | Vordefinierte IRC-Netzwerke + benutzerdefinierte Profile. |
| **Text-Event Formatvorlagen** | `pevents.conf` ([`text.c`](../legacy/src/common/text.c)) | `textevents.json` / `TextEventConfig.cs` | ⬜ | Mehr als 100 Text-Event Templates mit Farb- und Parameter-Codes. |
| **Farbschemata & Themes** | `colors.conf` ([`palette.c`](../legacy/src/fe-gtk/palette.c)) | `themes.json` / Avalonia ResourceDictionaries | 🟨 | 16 Standard-mIRC-Farben + 99 erweiterte Farben + UI-Themes. |
| **Benutzer-Befehle / Aliase** | `commands.conf` ([`outbound.c`](../legacy/src/common/outbound.c)) | `aliases.json` / `UserCommandService.cs` | ⬜ | Benutzerdefinierte Makros mit `$1`, `$2`, `$*` Ersetzung. |
| **Tastatur-Shortcuts** | `keybindings.conf` ([`fkeys.c`](../legacy/src/fe-gtk/fkeys.c)) | `keybindings.json` / `KeyBindingService.cs` | ⬜ | Frei anpassbare Key-Mappings. |
| **Auto-Replace Ersetzungen** | `replace.conf` ([`editlist.c`](../legacy/src/fe-gtk/editlist.c)) | `autoreplace.json` / `AutoReplaceService.cs` | ⬜ | Text-Kürzel Expansion bei Eingabe (z. B. `btw` $\rightarrow$ `by the way`). |
| **URL-Handler & Browser** | `urlhandlers.conf` ([`url.c`](../legacy/src/common/url.c)) | `urlhandlers.json` | ⬜ | Protokoll-Parser (`http`, `https`, `irc`, `magnet`, `spotify` etc.). |

---

## 🛠️ Bereich G: Subsysteme (CTCP, DCC, URL, Sound, Text Events)

| Subsystem | Legacy C Quellcode | Modernes C# Pendant | Status | Details |
| :--- | :--- | :--- | :---: | :--- |
| **CTCP Protokoll-Engine** | [`ctcp.c`](../legacy/src/common/ctcp.c) | `CtcpService.cs` | ⬜ | PING, TIME, VERSION, USERINFO, CLIENTINFO, ACTION, SOUND. |
| **DCC Dateiübertragung** | [`dcc.c`](../legacy/src/common/dcc.c) | `DccTransferService.cs` | ⬜ | Active & Passive Mode, UPnP Port Mapping, Resume, MD5/SHA256. |
| **DCC Direct Chat** | [`dcc.c`](../legacy/src/common/dcc.c) | `DccChatService.cs` | ⬜ | Direkte Peer-to-Peer IRC-Chatverbindung über TCP Sockets. |
| **URL & Link Detection** | [`url.c`](../legacy/src/common/url.c) | `UrlDetector.cs` | ⬜ | Regex-basierte Erkennung von URLs und Kanallinks im Chatverlauf. |
| **mIRC & TrueColor Formatierung** | [`text.c`](../legacy/src/common/text.c), [`palette.c`](../legacy/src/fe-gtk/palette.c) | [`MircColorParser.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Formatting/MircColorParser.cs) | 🟩 | [`MircColorParserTests.cs`](file:///d:/Quelltext/hexchat/tests/HexChat.Core.Tests/MircColorParserTests.cs) |
| **Sound & Audio Feedback** | [`fe-gtk.c`](../legacy/src/fe-gtk/fe-gtk.c) | `AudioNotificationService.cs` | ⬜ | WAV-Audioausgabe bei Highlights, Beep und privaten Nachrichten. |
| **Logging & Archivierung** | [`text.c`](../legacy/src/common/text.c) | `ChatLogger.cs` | ⬜ | Automatische Protokollierung in Textdateien mit Zeitstempel und Masken. |

---

## 🧩 Bereich H: Plugins, Scripting & Erweiterungen (`legacy/plugins/`)

| Plugin / Erweiterung | Legacy C Ordner | Modernes C# Pendant | Status | Details |
| :--- | :--- | :--- | :---: | :--- |
| **Python 3 Scripting** | [`plugins/python/`](../legacy/plugins/python) | `HexChat.Plugins.Python` | ⬜ | Volle API-Kompatibilität zu HexChat Python-Skripten (`import hexchat`). |
| **C# Native Plugin SDK** | [`plugin.c`](../legacy/src/common/plugin.c) | `HexChat.Core.Plugins.IPlugin` | ⬜ | Stark typisierte .NET-Pluginschnittstelle für Third-Party Extensions. |
| **System Info (/sysinfo)** | [`plugins/sysinfo/`](../legacy/plugins/sysinfo) | `SysInfoPlugin.cs` | ⬜ | Hardware-, CPU-, RAM-, OS- und Uptime-Statistiken für `/sysinfo`. |
| **FiSHLiM Verschlüsselung** | [`plugins/fishlim/`](../legacy/plugins/fishlim) | `FishLimPlugin.cs` | ⬜ | Blowfish ECB/CBC Verschlüsselung für sichere Kanäle und Direktnachrichten. |
| **System Tray & Alerts** | [`fe-gtk/plugin-tray.c`](../legacy/src/fe-gtk/plugin-tray.c) | `SystemTrayService.cs` | ⬜ | Tray-Icon mit Nachrichten-Blinken, Zählern und Tooltips. |
| **External Exec / Piping** | [`plugins/exec/`](../legacy/plugins/exec) | `ExecCommandService.cs` | ⬜ | Ausführung externer Konsolenbefehle und Piping in Channel (`/EXEC`). |
| **DCC Checksum Prüfer** | [`plugins/checksum/`](../legacy/plugins/checksum) | `DccChecksumPlugin.cs` | ⬜ | Integritätsprüfung empfangener Dateien (SHA-256, MD5, CRC32). |
