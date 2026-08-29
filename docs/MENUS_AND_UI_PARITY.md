<!-- SPDX-License-Identifier: GPL-2.0-or-later -->
<!-- HexChat (Avalonia Port) - docs/MENUS_AND_UI_PARITY.md -->
<!-- Description: Lückenlose Menü-, Dialog- und Oberflächen-Paritätsmatrix aus menu.c und setup.c. -->

# 🗂️ HexChat Menü-, Dialog- & UI-Paritätsmatrix

> **Lückenlose Erfassung aller Menüeinträge, Untermenüs, Tastenkürzel, Einstellungs-Reiter und Kontext-Popups**  
> Basierend auf [`legacy/src/fe-gtk/menu.c`](../legacy/src/fe-gtk/menu.c) und [`legacy/src/fe-gtk/setup.c`](../legacy/src/fe-gtk/setup.c).

---

## 📌 1. Hauptmenü-Leiste (Menu Bar)

### 🔹 Menü: He_xChat (`MENU_ID_HEXCHAT`)

| Menüeintrag | Tastenkürzel | Typ | Legacy Callback | Modernes C# / Avalonia Ziel | Status |
| :--- | :---: | :---: | :--- | :--- | :---: |
| **Network Li_st** | `Strg+S` | Standard | `menu_open_server_list` | `OpenServerListCommand` $\rightarrow$ `ServerListView.axaml` | 🟨 |
| *--- Trennlinie ---* | | | | | |
| **_New** | | Untermenü | | Submenu | 🟨 |
| ↳ Server Tab | `Strg+T` | Menüeintrag | `menu_newserver_tab` | `NewServerTabCommand` | 🟨 |
| ↳ Channel Tab | | Menüeintrag | `menu_newchannel_tab` | `NewChannelTabCommand` | 🟨 |
| ↳ Server Window | `Strg+N` | Menüeintrag | `menu_newserver_window` | `NewServerWindowCommand` | ⬜ |
| ↳ Channel Window | | Menüeintrag | `menu_newchannel_window` | `NewChannelWindowCommand` | ⬜ |
| *--- Trennlinie ---* | | | | | |
| **_Load Plugin or Script...** | | Menüeintrag | `menu_loadplugin` | `LoadPluginCommand` $\rightarrow$ OpenFileDialog | ⬜ |
| *--- Trennlinie ---* | | | | | |
| **Detach Tab** | | Menüeintrag | `menu_detach` | `DetachTabCommand` | ⬜ |
| **Close Tab** | `Strg+W` | Menüeintrag | `menu_close` | `CloseTabCommand` | 🟨 |
| *--- Trennlinie ---* | | | | | |
| **_Quit** | `Strg+Q` | Menüeintrag | `menu_quit` | `QuitApplicationCommand` | 🟨 |

---

### 🔹 Menü: _View

| Menüeintrag | Tastenkürzel | Typ | Legacy Callback | Modernes C# / Avalonia Ziel | Status |
| :--- | :---: | :---: | :--- | :--- | :---: |
| **_Menu Bar** | `F9` | Toggle | `menu_bar_toggle_cb` | `IsMenuBarVisible` Binding | 🟨 |
| **_Topic Bar** | | Toggle | `menu_topicbar_toggle` | `IsTopicBarVisible` Binding | 🟨 |
| **_User List** | `F7` | Toggle | `menu_userlist_toggle` | `IsUserListVisible` Binding | 🟨 |
| **U_ser List Buttons** | | Toggle | `menu_ulbuttons_toggle` | `IsUserListButtonsVisible` Binding | ⬜ |
| **M_ode Buttons** | | Toggle | `menu_cmbuttons_toggle` | `IsModeButtonsVisible` Binding | ⬜ |
| *--- Trennlinie ---* | | | | | |
| **_Channel Switcher** | | Untermenü | | RadioGroup | 🟨 |
| ↳ _Tabs | | Radio | `menu_layout_cb` | `SwitcherMode = Tabs` | 🟨 |
| ↳ T_ree | | Radio | `menu_layout_cb` | `SwitcherMode = Tree` | 🟨 |
| **_Network Meters** | | Untermenü | | RadioGroup | ⬜ |
| ↳ Off | | Radio | `menu_metres_off` | `NetworkMeter = Off` | ⬜ |
| ↳ Graph | | Radio | `menu_metres_graph` | `NetworkMeter = Graph` | ⬜ |
| ↳ Text | | Radio | `menu_metres_text` | `NetworkMeter = Text` | ⬜ |
| ↳ Both | | Radio | `menu_metres_both` | `NetworkMeter = Both` | ⬜ |
| *--- Trennlinie ---* | | | | | |
| **_Fullscreen** | `F11` | Toggle | `menu_fullscreen_toggle` | `WindowState = FullScreen` | 🟨 |

---

### 🔹 Menü: _Server

| Menüeintrag | Tastenkürzel | Typ | Legacy Callback | Modernes C# / Avalonia Ziel | Status |
| :--- | :---: | :---: | :--- | :--- | :---: |
| **_Disconnect** | | Menüeintrag | `menu_disconnect` | `DisconnectCommand` | 🟨 |
| **_Reconnect** | | Menüeintrag | `menu_reconnect` | `ReconnectCommand` | 🟨 |
| **_Join a Channel...** | | Menüeintrag | `menu_join` | `OpenJoinChannelDialogCommand` | ⬜ |
| **Channel _List** | | Menüeintrag | `menu_chanlist` | `OpenChannelListCommand` $\rightarrow$ `ChannelListView.axaml` | ⬜ |
| *--- Trennlinie ---* | | | | | |
| **Marked _Away** | `Strg+A` | Toggle | `menu_away` | `IsAway` Toggle Binding | 🟨 |

---

### 🔹 Menü: _Usermenu (`MENU_ID_USERMENU`)

| Menüeintrag | Typ | Legacy C Implementierung | Modernes C# / Avalonia Ziel | Status |
| :--- | :---: | :--- | :--- | :---: |
| **Dynamische Usermenu-Einträge** | Dynamisch | `menu_build_user_menu` in `menu.c` | Dynamische Bindings über `ObservableCollection<UserMenuItem>` | ⬜ |

---

### 🔹 Menü: S_ettings

| Menüeintrag | Typ | Legacy Callback | Modernes C# / Avalonia Ziel | Status |
| :--- | :---: | :--- | :--- | :---: |
| **_Preferences** | Menüeintrag | `menu_settings` | `OpenPreferencesCommand` $\rightarrow$ `PreferencesView.axaml` | 🟨 |
| *--- Trennlinie ---* | | | | |
| **Auto Replace** | Menüeintrag | `menu_rpopup` | `OpenAutoReplaceEditorCommand` | ⬜ |
| **CTCP Replies** | Menüeintrag | `menu_ctcpguiopen` | `OpenCtcpRepliesEditorCommand` | ⬜ |
| **Dialog Buttons** | Menüeintrag | `menu_dlgbuttons` | `OpenDialogButtonsEditorCommand` | ⬜ |
| **Keyboard Shortcuts** | Menüeintrag | `menu_keypopup` | `OpenKeyBindingsEditorCommand` | ⬜ |
| **Text Events** | Menüeintrag | `menu_evtpopup` | `OpenTextEventsEditorCommand` | ⬜ |
| **URL Handlers** | Menüeintrag | `menu_urlhandlers` | `OpenUrlHandlersEditorCommand` | ⬜ |
| **User Commands** | Menüeintrag | `menu_usercommands` | `OpenUserCommandsEditorCommand` | ⬜ |
| **User List Buttons** | Menüeintrag | `menu_ulbuttons` | `OpenUserListButtonsEditorCommand` | ⬜ |
| **User List Popup** | Menüeintrag | `menu_ulpopup` | `OpenUserListPopupEditorCommand` | ⬜ |

---

### 🔹 Menü: _Window

| Menüeintrag | Tastenkürzel | Typ | Legacy Callback | Modernes C# / Avalonia Ziel | Status |
| :--- | :---: | :---: | :--- | :--- | :---: |
| **_Ban List** | | Menüeintrag | `menu_banlist` | `OpenBanListCommand` $\rightarrow$ `BanListView.axaml` | ⬜ |
| **Character Chart** | | Menüeintrag | `ascii_open` | `OpenAsciiChartCommand` $\rightarrow$ `AsciiChartView.axaml` | ⬜ |
| **Direct Chat** | | Menüeintrag | `menu_dcc_chat_win` | `OpenDccChatCommand` | ⬜ |
| **File _Transfers** | | Menüeintrag | `menu_dcc_win` | `OpenDccTransfersCommand` $\rightarrow$ `DccManagerView.axaml` | ⬜ |
| **Friends List** | | Menüeintrag | `notify_opengui` | `OpenFriendsListCommand` $\rightarrow$ `NotifyListView.axaml` | ⬜ |
| **Ignore List** | | Menüeintrag | `ignore_gui_open` | `OpenIgnoreListCommand` $\rightarrow$ `IgnoreListView.axaml` | ⬜ |
| **_Plugins and Scripts** | | Menüeintrag | `menu_pluginlist` | `OpenPluginListCommand` $\rightarrow$ `PluginManagerView.axaml` | ⬜ |
| **_Raw Log** | | Menüeintrag | `menu_rawlog` | `OpenRawLogCommand` $\rightarrow$ `RawLogView.axaml` | ⬜ |
| **_URL Grabber** | | Menüeintrag | `url_opengui` | `OpenUrlGrabberCommand` $\rightarrow$ `UrlGrabberView.axaml` | ⬜ |
| *--- Trennlinie ---* | | | | | |
| **Reset Marker Line** | `Strg+M` | Menüeintrag | `menu_resetmarker` | `ResetMarkerLineCommand` | ⬜ |
| **Move to Marker Line** | `Strg+Shift+M` | Menüeintrag | `menu_movetomarker` | `ScrollToMarkerLineCommand` | ⬜ |
| **_Copy Selection** | `Strg+Shift+C` | Menüeintrag | `menu_copy_selection` | `CopyChatSelectionCommand` | 🟨 |
| **C_lear Text** | | Menüeintrag | `menu_flushbuffer` | `ClearChatBufferCommand` | 🟨 |
| **Save Text...** | | Menüeintrag | `menu_savebuffer` | `SaveChatBufferCommand` | ⬜ |
| **Search** | | Untermenü | | Submenu | 🟨 |
| ↳ Search Text... | `Strg+F` | Menüeintrag | `menu_search` | `OpenSearchDialogCommand` | 🟨 |
| ↳ Search Next | `F3` / `Strg+G` | Menüeintrag | `menu_search_next` | `FindNextCommand` | 🟨 |
| ↳ Search Previous | `Shift+F3` | Menüeintrag | `menu_search_prev` | `FindPreviousCommand` | 🟨 |

---

### 🔹 Menü: _Help

| Menüeintrag | Tastenkürzel | Typ | Legacy Callback | Modernes C# / Avalonia Ziel | Status |
| :--- | :---: | :---: | :--- | :--- | :---: |
| **_Contents** | `F1` | Menüeintrag | `menu_docs` | `OpenHelpDocsCommand` | ⬜ |
| **_About** | | Menüeintrag | `menu_about` | `OpenAboutDialogCommand` $\rightarrow$ `AboutView.axaml` | 🟨 |

---

## ⚙️ 2. Einstellungen / Preferences-Dialog (`setup.c`)

Der Einstellungsdialog besteht aus **11 Konfigurations-Kategorien**, die 1:1 in Avalonia als Navigations-Sidebar oder TabControl abgebildet werden:

```text
┌─────────────────────────────────────────────────────────────┐
│                       Preferences                           │
├───────────────────┬─────────────────────────────────────────┤
│ Interface         │ [Reiter-Inhalt]                         │
│  ├─ Appearance    │                                         │
│  ├─ Input box     │  • Schriftart & Textgrößen               │
│  ├─ User list     │  • Farbschema & mIRC-Farben             │
│  ├─ Channel sw.   │  • Zeitstempel-Format                   │
│  └─ Colors        │  • Nick-Coloring Algorithmus            │
│ Chatting          │  • Signal-Töne & Benachrichtigungen     │
│  ├─ General       │  • Auto-Logging in Textdateien          │
│  ├─ Alerts        │  • Proxy & Identd Server                │
│  ├─ Sounds        │                                         │
│  ├─ Logging       │                                         │
│  └─ Advanced      │                                         │
│ Network           │                                         │
│  ├─ Network setup │                                         │
│  ├─ File transfers│                                         │
│  └─ Identd        │                                         │
└───────────────────┴─────────────────────────────────────────┘
```

| Kategorie / Seite | Quellcode-Funktion in `setup.c` | Einstellungs-Schlüssel (Beispiele) | Status |
| :--- | :--- | :--- | :---: |
| **1. Appearance** | `appearance_settings` | Font, Hintergrundbild, Transparenz, Fenster-Layout | ⬜ |
| **2. Input Box** | `inputbox_settings` | Spellcheck, Nick-Completion, History-Größe, Multi-Line | ⬜ |
| **3. User List** | `userlist_settings` | Sortierung nach Op/Voice, Hostmask-Anzeige, Doppelklick-Aktion | ⬜ |
| **4. Channel Switcher** | `tabs_settings` | Tabs oben/unten, Baumansicht, Ungelesene-Zähler, Tab-Breite | ⬜ |
| **5. Colors** | `setup_create_color_page` | Farbcodes 0–15, Marker-Line-Farbe, Text-Vordergrund/-Hintergrund | ⬜ |
| **6. General** | `general_settings` | Nick, Zweitnick, Benutzername, Realname, Abwesenheitsgrund | ⬜ |
| **7. Alerts** | `alert_settings` | Blinken im Panel/Dock, Highlight-Wörter, Private Nachrichten Alerts | ⬜ |
| **8. Sounds** | `setup_create_sound_page` | Sound-Events: Beep, Highlight, Join, Part, Privatnachricht | ⬜ |
| **9. Logging** | `logging_settings` | Auto-Log Pfad, Dateimaske (`%c/%n.log`), Zeitstempel-Präfix | ⬜ |
| **10. Advanced** | `advanced_settings` | Ping-Timeout, Flood-Schutz, Auto-Reconnect Verzögerung | ⬜ |
| **11. Network Setup** | `network_settings` | Bind-IP, IPv4/IPv6 Priorität, Proxy (SOCKS5/HTTP/Tor) | ⬜ |
| **12. File Transfers** | `filexfer_settings` | Download-Verzeichnis, Auto-Accept, Port-Bereich, Bandbreiten-Limit | ⬜ |
| **13. Identd** | `identd_settings` | Identd Port (113), Antwort-Name, Fake-Ident | ⬜ |

---

## 🖱️ 3. Kontextmenüs & Popups

### A. Userlist Kontextmenü (Rechtsklick auf Nick)

- [ ] **Whois** (`/WHOIS %s`)
- [ ] **Query / Privatchat** (`/QUERY %s`)
- [ ] **Send File (DCC)** (`/DCC SEND %s`)
- [ ] **Direct Chat (DCC)** (`/DCC CHAT %s`)
- [ ] **Operator-Aktionen:**
  - [ ] Give Op (`/OP %s`) / Take Op (`/DEOP %s`)
  - [ ] Give Voice (`/VOICE %s`) / Take Voice (`/DEVOICE %s`)
  - [ ] Give Half-Op (`/HOP %s`) / Take Half-Op (`/DEHOP %s`)
  - [ ] Kick (`/KICK %c %s`) / KickBan (`/KICKBAN %c %s`)
  - [ ] Ban Nick/Host (`/BAN %s`) / Quiet (`/QUIET %s`)
- [ ] **CTCP Anfragen:**
  - [ ] Ping (`/CTCP %s PING`)
  - [ ] Version (`/CTCP %s VERSION`)
  - [ ] Time (`/CTCP %s TIME`)
  - [ ] Userinfo (`/CTCP %s USERINFO`)
- [ ] **Ignore User** (`/IGNORE %s ALL`)
- [ ] **Add to Friends/Notify** (`/NOTIFY %s`)

### B. Channel Tab / Tree Kontextmenü (Rechtsklick auf Tab)

- [ ] **Close Window** (`Strg+W`)
- [ ] **Detach Window**
- [ ] **Part Channel** (`/PART`)
- [ ] **Rejoin / Cycle** (`/CYCLE`)
- [ ] **Clear Text Buffer**
- [ ] **Channel Options / Settings**

### C. Chat-Buffer Textauswahl Kontextmenü

- [ ] **Copy** (`Strg+C`)
- [ ] **Copy Link** (bei Rechtsklick auf URL)
- [ ] **Open URL in Browser**
- [ ] **Search Web for Selection**
- [ ] **Clear Buffer**
