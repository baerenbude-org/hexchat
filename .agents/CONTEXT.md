<!-- SPDX-License-Identifier: GPL-2.0-or-later -->
<!-- HexChat (Avalonia Port) - .agents/CONTEXT.md -->
<!-- Description: Schneller Orientierungsindex für KI-Agenten und Entwickler zum Auffinden aller HexChat-Ressourcen. -->

# AI Agent & Developer Context Index — HexChat

> **🚀 START HERE** — Zentraler Einstiegspunkt für AI-Assistenten und Mitwirkende.  
> HexChat ist der moderne plattformübergreifende Port des klassischen IRC-Clients mit **C# / .NET 10** und **Avalonia UI 12**.

---

## 🧭 Ich möchte

### 1. Roadmap & Paritätsstatus einsehen

- **Master Roadmap & Dashboard:** [`ROADMAP.md`](../ROADMAP.md) im Projekt-Hauptverzeichnis.
- **Master Paritätsmatrix (C nach C#):** [`docs/PARITY_MATRIX.md`](../docs/PARITY_MATRIX.md)
- **Befehls-Parität (70+ Slash-Commands):** [`docs/COMMANDS_PARITY.md`](../docs/COMMANDS_PARITY.md)
- **Menü-, Dialog- & UI-Parität:** [`docs/MENUS_AND_UI_PARITY.md`](../docs/MENUS_AND_UI_PARITY.md)

### 2. Architektur & Richtlinien verstehen

- **Globale Agenten-Regeln:** [`AGENTS.md`](../AGENTS.md) im Projekt-Hauptverzeichnis.
- **3-Schichten-Architektur:** [`.agents/rules/architecture_and_layers.md`](rules/architecture_and_layers.md)
- **C# 13 & .NET 10 Coding-Standards:** [`.agents/rules/coding_standards_csharp.md`](rules/coding_standards_csharp.md)
- **Avalonia UI 12 & MVVM Best Practices:** [`.agents/rules/avalonia_ui_guidelines.md`](rules/avalonia_ui_guidelines.md)
- **IRC-Protokoll & Test-Konventionen:** [`.agents/rules/irc_protocol_and_testing.md`](rules/irc_protocol_and_testing.md)

### 3. Ein Feature oder einen Dialog aus C/GTK nach Avalonia portieren

1. **Portierungs-Leitfaden öffnen:** [`.agents/checklists/porting-c-to-avalonia.md`](checklists/porting-c-to-avalonia.md)
2. **C-Referenzcode einsehen:** in [`legacy/src/common/`](../legacy/src/common) (Core-Logik) und [`legacy/src/fe-gtk/`](../legacy/src/fe-gtk) (GTK-Dialoge)
3. **Vorlage für ViewModel/View nutzen:** [`.agents/templates/viewmodel-view-template.md`](templates/viewmodel-view-template.md)
4. **View in `src/HexChat.UI/Views/` und ViewModel in `src/HexChat.UI/ViewModels/` anlegen**
5. **Unit-Tests in `tests/HexChat.Core.Tests/` schreiben**

### 4. Eine formale Architekturentscheidung (ADR) festhalten

1. **Vorlage öffnen:** [`.agents/templates/adr-template.md`](templates/adr-template.md)
2. **Neue Datei anlegen:** in `docs/adr/` als `XXXX-titel.md`
3. **Index in `docs/adr/README.md` aktualisieren**

### 5. Ein Problem, einen XAML-Binding-Fehler oder Verbindungsabbruch debuggen

1. **Troubleshooting-Guide:** [`.agents/troubleshooting/common-errors.md`](troubleshooting/common-errors.md)
2. **Tests ausführen:** `dotnet test HexChat.sln`
3. **Desktop-App im Debug-Modus starten:** `dotnet run --project src/HexChat.Desktop/HexChat.Desktop.csproj` (oder **F5** drücken)

---

## ⚡ Wichtigste CLI-Befehle auf einen Blick

```bash
# 1. Solution kompilieren (.NET 10)
dotnet build HexChat.sln

# 2. Unit-Tests ausführen
dotnet test HexChat.sln

# 3. HexChat Avalonia Desktop starten
dotnet run --project src/HexChat.Desktop/HexChat.Desktop.csproj
```

---

## 📦 Schichten-Übersicht

```text
src/HexChat.Desktop/  ──► Plattform-Host & Dependency Injection (.NET 10)
src/HexChat.UI/       ──► Avalonia 12 XAML, ViewModels (CommunityToolkit.Mvvm), Fluent Styles
src/HexChat.Core/     ──► IRC Protocol Engine, RFC 1459/2812/IRCv3, TCP/TLS Sockets, State
tests/HexChat.Core.Tests/ ──► xUnit Test-Suite für Parser, State und Protocol Handling
legacy/               ──► Original C/GTK2 Referenzarchiv (common, fe-gtk, plugins, data, win32)
```
