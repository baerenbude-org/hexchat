<!-- SPDX-License-Identifier: GPL-2.0-or-later -->
<!-- HexChat (Avalonia Port) - .agents/rules/architecture_and_layers.md -->
<!-- Description: Architekturregeln, Schichtenmodell und Interaktionsmuster zwischen Core, UI und Desktop. -->

# Architecture & Layering Rules — HexChat

## 1. Das 3-Schichten-Modell (.NET 10 / Avalonia)

Die C# Codebasis ist strikt hierarchisch gegliedert. Abhängigkeiten dürfen nur von oben nach unten fließen:

```text
HexChat.Desktop ──► HexChat.UI ──► HexChat.Core
```

### A. HexChat.Core (Unabhängige Protokoll- und Geschäftslogik)
- **Keine UI-Abhängigkeiten:** `HexChat.Core` darf niemals Avalonia-Namespaces, XAML oder UI-Framework-Klassen referenzieren.
- **Zuständigkeit:**
  - IRC Protokoll-Engine (RFC 1459, 2812, IRCv3 Spezifikationen).
  - Netzwerk-Kommunikation (`TcpClient`, `SslStream`, `System.IO.Pipelines`).
  - Message-Parser (`IrcMessageParser`, `Span`-basierte Tokenisierung).
  - Verbindungs- & Kanal-Zustandsverwaltung (`IrcServer`, `IrcChannel`, `IrcUser`).
  - Konfigurations- und Speichermodelle (Netzwerklisten, Serverfavoriten, Logs).
- **Testbarkeit:** Sämtliche Komponenten in `HexChat.Core` müssen 100 % isoliert ohne UI-Kontext testbar sein.

### B. HexChat.UI (Präsentationsschicht & MVVM)
- **MVVM-Pattern:** Klare Trennung zwischen Views (AXAML) und ViewModels (C# mit `CommunityToolkit.Mvvm`).
- **Zuständigkeit:**
  - Avalonia 11 Views, UserControls, Fenster und Dialoge (z. B. MainWindow, ChannelView, ServerListDialog).
  - ViewModels zur Zustandsaufbereitung für die Oberfläche.
  - ValueConverter, XAML-Styles, Resource-Dictionaries und Themes.
- **Event-Entkopplung:** Nachrichten und Zustandsänderungen aus `HexChat.Core` werden über asynchrone Events, `ObservableCollection<T>` oder Reactive-Streams an die ViewModels übermittelt.

### C. HexChat.Desktop (Host & Plattform-Bootstrapper)
- **Zuständigkeit:**
  - Einstiegspunkt `Program.cs` und `App.axaml.cs`.
  - Initialisierung der Dependency Injection (`IServiceCollection` / `ServiceProvider`).
  - Desktop-spezifische Lebenszyklusverwaltung (`IClassicDesktopStyleApplicationLifetime`).
  - Registrierung von systemweiten Diensten (z. B. Tray-Icon, Desktop-Benachrichtigungen).

---

## 2. Rolle der C/GTK Legacy-Codebasis ([`legacy/`](file:///d:/Quelltext/hexchat/legacy))

1. **Funktionale Referenz:** Der archivierte C-Code in `legacy/src/common/` und `legacy/src/fe-gtk/` dient als "Ground Truth" für IRC-Verhaltensweisen, Formatierungs-Codes (MIRC-Farben, Bold, Underline), CTCP/DCC-Handshakes und Menüstrukturen.
2. **Keine neuen C-Features:** Neue Features und Erweiterungen werden ausschließlich in C# / Avalonia implementiert.
3. **Schrittweise Portierung:** Dialoge und Logikmodule werden inkrementell portiert. Siehe [`.agents/checklists/porting-c-to-avalonia.md`](../checklists/porting-c-to-avalonia.md).

---

## 3. Dependency Injection & Service Lifetime

- **Singleton Services:** `IIrcClientFactory`, `IConfigurationService`, `IThemeService`.
- **Scoped/Transient Services:** ViewModels für temporäre Dialoge, Sub-Parser.
- **Service Locator vermeiden:** Konstruktor-Injektion bevorzugen.
