<!-- SPDX-License-Identifier: GPL-2.0-or-later -->
<!-- HexChat (Avalonia Port) - docs/COMMANDS_PARITY.md -->
<!-- Description: Vollständige Befehls-Paritätsmatrix: Alle 70+ internen Slash-Befehle aus xc_cmds (outbound.c) und RFC-Kommandos. -->

# ⌨️ HexChat Slash-Commands & Befehls-Paritätsmatrix

> **Lückenloser Abgleich aller 70+ internen HexChat-Befehle aus [`legacy/src/common/outbound.c`](../legacy/src/common/outbound.c)**  
> Ziel: **100% Unterstützung** aller bekannten Chat-, Steuerungs- und Moderationsbefehle in C#.

---

## 📌 Befehlsübersicht & Status

| Befehl | Syntax / Beschreibung | Legacy C Funktion | Modernes C# Ziel | Status | Berechtigung / Kontext |
| :--- | :--- | :--- | :--- | :---: | :--- |
| `/ADDBUTTON` | `ADDBUTTON <name> <action>` — Fügt Button unter Userliste hinzu | `cmd_addbutton` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | ⬜ | Lokal / GUI |
| `/ADDSERVER` | `ADDSERVER <NewNet> <host/port>` — Fügt Netzwerk/Server hinzu | `cmd_addserver` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | ⬜ | Lokal / Config |
| `/ALLCHAN` | `ALLCHAN <cmd>` — Sendet Befehl an alle offenen Channels | `cmd_allchannels` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Global Session |
| `/ALLCHANL` | `ALLCHANL <cmd>` — Sendet Befehl an alle Channels auf aktuellem Server | `cmd_allchannelslocal` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Server Session |
| `/ALLSERV` | `ALLSERV <cmd>` — Sendet Befehl an alle verbundenen Server | `cmd_allservers` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Global Session |
| `/AWAY` | `AWAY [<reason>]` — Setzt den Away-Status | `cmd_away` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459/2812 |
| `/BACK` | `BACK` — Hebt Away-Status auf | `cmd_back` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459/2812 |
| `/BAN` | `BAN <mask> [<bantype>]` — Bannt Maske auf aktuellem Channel | `cmd_ban` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/CHANOPT` | `CHANOPT [-quiet] <var> [<val>]` — Setzt/liest Channel-Optionen | `cmd_chanopt` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | ⬜ | Channel lokal |
| `/CHARSET` | `CHARSET [<encoding>]` — Ändert Zeichenkodierung (z.B. UTF-8) | `cmd_charset` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | ⬜ | Verbindung lokal |
| `/CLEAR` | `CLEAR [ALL\|HISTORY\|[-]<n>]` — Leert Chatfenster oder Verlauf | `cmd_clear` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | GUI Buffer |
| `/CLOSE` | `CLOSE [-m]` — Schließt aktuellen Tab oder alle Query-Fenster | `cmd_close` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | GUI Window |
| `/COUNTRY` | `COUNTRY [-s] <code>` — Löst Ländercode auf (z. B. de = Germany) | `cmd_country` | `CountryService.cs` | ⬜ | Tool |
| `/CTCP` | `CTCP <nick> <message>` — Sendet CTCP-Anfrage (VERSION, TIME etc.) | `cmd_ctcp` | `CtcpCommandHandler.cs` | ⬜ | RFC 1459 / CTCP |
| `/CYCLE` | `CYCLE [<channel>]` — Verlässt Kanal und tritt sofort wieder bei | `cmd_cycle` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Channel |
| `/DCC` | `DCC {GET\|SEND\|PSEND\|LIST\|CHAT\|PCHAT\|CLOSE}` — DCC File & Chat | `cmd_dcc` | `DccCommandHandler.cs` | ⬜ | DCC Subsystem |
| `/DEBUG` | `DEBUG` — Interne Debug-Ausgaben | `cmd_debug` | `DebugCommandHandler.cs` | ⬜ | Entwicklermodus |
| `/DEHOP` | `DEHOP <nick>` — Entzieht Half-Op Status (-h) | `cmd_dehop` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/DELBUTTON` | `DELBUTTON <name>` — Löscht Button unter Userliste | `cmd_delbutton` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | ⬜ | GUI lokal |
| `/DEOP` | `DEOP <nick>` — Entzieht Operator-Status (-o) | `cmd_deop` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/DEVOICE` | `DEVOICE <nick>` — Entzieht Voice-Status (-v) | `cmd_devoice` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/DISCON` | `DISCON` — Trennt Verbindung zum Server | `cmd_discon` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Verbindung |
| `/DNS` | `DNS <nick\|host\|ip>` — Löst Hostnamen oder IP-Adresse auf | `cmd_dns` | `DnsCommandHandler.cs` | ⬜ | Netzwerk |
| `/DOAT` | `DOAT <channel,list,/network> <command>` — Führt Befehl remote aus | `cmd_doat` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | ⬜ | Session Routing |
| `/ECHO` | `ECHO <text>` — Gibt lokalen Text im aktuellen Puffer aus | `cmd_echo` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | GUI Buffer |
| `/EXEC` | `EXEC [-o] <cmd>` — Führt Systembefehl aus (optional in Channel ausgeben) | `cmd_exec` | `ExecCommandHandler.cs` | ⬜ | System |
| `/EXECCONT` | `EXECCONT` — Sendet laufendem Prozess SIGCONT | `cmd_execc` | `ExecCommandHandler.cs` | ⬜ | System |
| `/EXECKILL` | `EXECKILL [-9]` — Beendet laufenden Subprozess | `cmd_execk` | `ExecCommandHandler.cs` | ⬜ | System |
| `/EXECSTOP` | `EXECSTOP` — Pausiert Subprozess (SIGSTOP) | `cmd_execs` | `ExecCommandHandler.cs` | ⬜ | System |
| `/EXECWRITE` | `EXECWRITE [-q] <data>` — Schreibt in stdin des Prozesses | `cmd_execw` | `ExecCommandHandler.cs` | ⬜ | System |
| `/FLUSHQ` | `FLUSHQ` — Leert die Sendewarteschlange des Servers | `cmd_flushq` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Verbindung |
| `/GATE` | `GATE <host> [<port>]` — Proxy-Verbindung über Gateway | `cmd_gate` | `GateCommandHandler.cs` | ⬜ | Verbindung |
| `/GETBOOL` | `GETBOOL <cmd> <title> <prompt>` — Öffnet Ja/Nein-Dialog für Skripte | `cmd_getbool` | `DialogCommandHandler.cs` | ⬜ | Scripting / GUI |
| `/GETFILE` | `GETFILE [-folder\|-multi\|-save] <cmd> <title>` — Datei-Dialog | `cmd_getfile` | `DialogCommandHandler.cs` | ⬜ | Scripting / GUI |
| `/GETINT` | `GETINT <default> <cmd> <prompt>` — Öffnet Ganzzahl-Eingabedialog | `cmd_getint` | `DialogCommandHandler.cs` | ⬜ | Scripting / GUI |
| `/GETSTR` | `GETSTR <default> <cmd> <prompt>` — Öffnet Text-Eingabedialog | `cmd_getstr` | `DialogCommandHandler.cs` | ⬜ | Scripting / GUI |
| `/GHOST` | `GHOST <nick> [password]` — Trennt Ghost-Verbindung via NickServ | `cmd_ghost` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | NickServ |
| `/GUI` | `GUI {APPLY\|ATTACH\|DETACH\|FOCUS\|FLASH\|MENU\|COLOR}` | `cmd_gui` | `GuiCommandHandler.cs` | ⬜ | GUI Control |
| `/HELP` | `HELP [<command>]` — Zeigt Hilfe und Syntax zu Befehlen an | `cmd_help` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Info |
| `/HOP` | `HOP <nick>` — Vergibt Half-Op Status (+h) | `cmd_hop` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/ID` | `ID <password>` — Identifiziert Benutzer bei NickServ | `cmd_id` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | NickServ |
| `/IGNORE` | `IGNORE <mask> <types..> <options..>` — Ignoriert Benutzer | `cmd_ignore` | `IgnoreCommandHandler.cs` | ⬜ | Ignore Engine |
| `/INVITE` | `INVITE <nick> [<channel>]` — Lädt Benutzer in Kanal ein | `cmd_invite` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459 |
| `/JOIN` | `JOIN <channel> [<key>]` — Tritt einem Kanal bei | `cmd_join` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459 |
| `/KICK` | `KICK <nick> [reason]` — Wirft Benutzer aus dem Kanal | `cmd_kick` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/KICKBAN` | `KICKBAN <nick> [reason]` — Bannt und wirft Benutzer aus dem Kanal | `cmd_kickban` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/KILLALL` | `KILLALL` — Sofortiges Beenden des Clients ohne Bestätigung | `cmd_killall` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Client Lifecycle |
| `/LAGCHECK` | `LAGCHECK` — Erzwingt manuelle Latenz-Messung (Ping) | `cmd_lagcheck` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Ping/Pong |
| `/LASTLOG` | `LASTLOG [-h] [-m] [-r] <pattern>` — Durchsucht Puffer nach Text | `cmd_lastlog` | `ChatBufferService.cs` | ⬜ | Buffer Search |
| `/LIST` | `LIST [<args>]` — Fragt Kanalliste vom Server ab | `cmd_list` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459 |
| `/LOAD` | `LOAD [-e] <file>` — Lädt Plugin oder Skript | `cmd_load` | `PluginCommandHandler.cs` | ⬜ | Plugin Engine |
| `/MDEHOP` | `MDEHOP` — Mass-Dehop aller Benutzer im aktuellen Kanal | `cmd_mdehop` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/MDEOP` | `MDEOP` — Mass-Deop aller Operatoren im Kanal | `cmd_mdeop` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/ME` | `ME <action>` — Sendet Aktionsnachricht (/me winkt) | `cmd_me` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | CTCP ACTION |
| `/MENU` | `MENU [-opts] {ADD\|DEL} <path> [cmd]` — Dynamisches Menü | `cmd_menu` | `MenuCommandHandler.cs` | ⬜ | GUI Menu |
| `/MHOP` | `MHOP` — Mass-Hop aller berechtigten Benutzer | `cmd_mhop` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/MKICK` | `MKICK` — Mass-Kick aller Benutzer außer einem selbst | `cmd_mkick` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/MODE` | `MODE <target> [<modes>] [<args>]` — Kanal- oder User-Modi setzen | `cmd_mode` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459 |
| `/MOP` | `MOP` — Mass-Op aller Benutzer im Kanal | `cmd_mop` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/MSG` | `MSG <target> <text>` — Sendet private Nachricht | `cmd_msg` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459 PRIVMSG |
| `/NAMES` | `NAMES [<channel>]` — Fragt Benutzerliste des Kanals ab | `cmd_names` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459 |
| `/NCTCP` | `NCTCP <nick> <message>` — Sendet CTCP Notice | `cmd_nctcp` | `CtcpCommandHandler.cs` | ⬜ | CTCP / NOTICE |
| `/NEWSERVER` | `NEWSERVER [-noconnect] <host> [<port>]` — Neues Server-Tab | `cmd_newserver` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | GUI Session |
| `/NICK` | `NICK <newnick>` — Ändert den eigenen Nicknamen | `cmd_nick` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459 |
| `/NOTICE` | `NOTICE <target> <text>` — Sendet Notice-Nachricht | `cmd_notice` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459 |
| `/NOTIFY` | `NOTIFY [-n net] [<nick>]` — Zeigt/bearbeitet Notify-Freundesliste | `cmd_notify` | `NotifyCommandHandler.cs` | ⬜ | Friends Monitor |
| `/OP` | `OP <nick>` — Vergibt Kanal-Operator-Status (+o) | `cmd_op` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/PART` | `PART [<channel>] [<reason>]` — Verlässt Kanal | `cmd_part` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459 |
| `/PING` | `PING [<nick\|channel>]` — Sendet Ping-Anfrage | `cmd_ping` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | CTCP / RFC |
| `/QUERY` | `QUERY [-nofocus] <nick> [msg]` — Öffnet privates Chatfenster | `cmd_query` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | GUI Window |
| `/QUIET` | `QUIET <mask> [<type>]` — Setzt Quiet-Mode (+q) auf Benutzer | `cmd_quiet` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/QUIT` | `QUIT [<reason>]` — Beendet Verbindung mit Abschiedsnachricht | `cmd_quit` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459 |
| `/QUOTE` | `QUOTE <raw-text>` — Sendet unveränderten Text direkt an IRC-Server | `cmd_quote` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Raw IRC |
| `/RECONNECT` | `RECONNECT [-ssl] [<host>] [<port>] [<pass>]` — Neuverbindung | `cmd_reconnect` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Verbindung |
| `/RECV` | `RECV <raw-data>` — Simuliert empfangene Server-Nachricht lokal | `cmd_recv` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Debug / Test |
| `/RELOAD` | `RELOAD <name>` — Lädt Skript oder Plugin neu | `cmd_reload` | `PluginCommandHandler.cs` | ⬜ | Plugin Engine |
| `/SAY` | `SAY <text>` — Sendet Text an aktuellen Kanal/Query | `cmd_say` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Message |
| `/SEND` | `SEND <nick> [<file>]` — Startet DCC-Dateiversand | `cmd_send` | `DccCommandHandler.cs` | ⬜ | DCC Subsystem |
| `/SERVCHAN` | `SERVCHAN [-ssl] <host> <port> <chan>` — Verbindet und joint direkt | `cmd_servchan` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Verbindung |
| `/SERVER` | `SERVER [-ssl] <host> [<port>] [<pass>]` — Verbindet mit Server | `cmd_server` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Verbindung |
| `/SET` | `SET [-e] [-off\|-on] [-quiet] <var> [<val>]` — Ändert Konfiguration | `cmd_set` | `ConfigCommandHandler.cs` | ⬜ | Config Engine |
| `/SETCURSOR` | `SETCURSOR [-+]<pos>` — Positioniert Eingabecursor | `cmd_setcursor` | `GuiCommandHandler.cs` | ⬜ | GUI Input |
| `/SETTAB` | `SETTAB <new-name>` — Benennt aktuellen Tab um | `cmd_settab` | `GuiCommandHandler.cs` | ⬜ | GUI Tab |
| `/SETTEXT` | `SETTEXT <new-text>` — Ersetzt Text in Eingabezeile | `cmd_settext` | `GuiCommandHandler.cs` | ⬜ | GUI Input |
| `/SPLAY` | `SPLAY <soundfile>` — Spielt Audiodatei ab | `cmd_splay` | `AudioNotificationService.cs` | ⬜ | Audio Engine |
| `/TOPIC` | `TOPIC [<newtopic>]` — Zeigt oder ändert Kanal-Thema | `cmd_topic` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459 |
| `/TRAY` | `TRAY {-f\|-i\|-t\|-b}` — Steuert System-Tray-Icon & Ballons | `cmd_tray` | `TrayCommandHandler.cs` | ⬜ | Tray Subsystem |
| `/UNBAN` | `UNBAN <mask> [<mask2>...]` — Entfernt Bann-Masken | `cmd_unban` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/UNIGNORE` | `UNIGNORE <mask> [QUIET]` — Hebt Ignorierung auf | `cmd_unignore` | `IgnoreCommandHandler.cs` | ⬜ | Ignore Engine |
| `/UNLOAD` | `UNLOAD <name>` — Entlädt Skript oder Plugin | `cmd_unload` | `PluginCommandHandler.cs` | ⬜ | Plugin Engine |
| `/UNQUIET` | `UNQUIET <mask> [<mask2>...]` — Entfernt Quiet-Status (-q) | `cmd_unquiet` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/URL` | `URL <url>` — Öffnet URL im Standard-Browser | `cmd_url` | `UrlCommandHandler.cs` | ⬜ | OS Shell |
| `/USELECT` | `USELECT [-a] [-s] <nick1> <nick2>...` — Markiert Nicks in Userliste | `cmd_uselect` | `GuiCommandHandler.cs` | ⬜ | GUI Userlist |
| `/USERLIST` | `USERLIST` — Gibt Userliste mit Rängen und Latenz im Buffer aus | `cmd_userlist` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | Buffer Output |
| `/VOICE` | `VOICE <nick>` — Vergibt Voice-Status (+v) | `cmd_voice` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp benötigt |
| `/WALLCHOP` | `WALLCHOP <message>` — Sendet Notice an alle ChanOps | `cmd_wallchop` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | ChanOp Notice |
| `/WHOIS` | `WHOIS <nick>` — Fragt detaillierte Benutzerinfos vom Server ab | `proto-irc.c` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459 |
| `/WHOWAS` | `WHOWAS <nick>` — Fragt Offline-Benutzer-Historie ab | `proto-irc.c` | [`HexChatCommandHandler.cs`](file:///d:/Quelltext/hexchat/src/HexChat.Core/Commands/HexChatCommandHandler.cs) | 🟨 | RFC 1459 |
