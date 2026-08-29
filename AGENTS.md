<!-- SPDX-License-Identifier: GPL-2.0-or-later -->
<!-- HexChat (Avalonia Port) - AGENTS.md -->
<!-- Description: Universelles Regelwerk für KI-Coding-Assistenten und Entwickler im HexChat Repository. -->
<!-- Website: https://github.com/google-deepmind/antigravity -->

# 🤖 AGENTS.md — Guidelines & Operational Rules for HexChat (Avalonia Port)

> **Universal Workspace Rule File for AI Coding Assistants & Human Contributors**  
> Project: **HexChat (.NET 10 & Avalonia UI Port)**  
> Architecture: **3-Tier MVVM & Reactive IRC Engine** | Language: **C# 13 / .NET 10** | License: **GPL-2.0-or-later**

---

## 🎯 1. Mission & Architectural Overview

Dieses Repository beherbergt den modernen Port des klassischen, bewährten **HexChat IRC-Clients** auf eine plattformübergreifende **C# / .NET 10** und **Avalonia UI 12** Architektur.

Ziel des Ports ist es, das vertraute, hochgradig konfigurierbare und performante HexChat-Nutzererlebnis auf moderne Desktop-Plattformen (Windows, Linux, macOS) zu bringen, während gleichzeitig eine saubere, testbare und erweiterbare Codebasis etabliert wird. Der originale C/GTK-Code ist vollständig im Verzeichnis [`legacy/`](legacy/) als funktionale Referenz und Kompatibilitätsanker archiviert.

```text
┌─────────────────────────────────────────────────────────────┐
│                     HexChat.Desktop                         │
│ (Plattform-Bootstrapper, Desktop-Host, App-Lifecycle)       │
└──────────────────────────────┬──────────────────────────────┘
                               │
                               ▼
┌─────────────────────────────────────────────────────────────┐
│                        HexChat.UI                           │
│ (Avalonia 12 XAML, ViewModels [MVVM Toolkit], Theme/Styling)│
└──────────────────────────────┬──────────────────────────────┘
                               │
                               ▼
┌─────────────────────────────────────────────────────────────┐
│                       HexChat.Core                          │
│ (IRC Engine, RFC 1459/2812/IRCv3, TCP/TLS, State & Storage) │
└─────────────────────────────────────────────────────────────┘
```

---

## 🧱 2. Project Structure & Responsibilities

| Projekt / Verzeichnis | Verantwortung | Wichtigste Technologien & Muster |
| :--- | :--- | :--- |
| [`src/HexChat.Core`](file:///d:/Quelltext/hexchat/src/HexChat.Core) | IRC-Protokoll-Engine, Socket-Verbindungen (TLS/SASL), Message-Parser, State Management, CTCP, DCC. | C# 13, async/await, `System.IO.Pipelines`, `ReadOnlySpan<char>`, xUnit Tests |
| [`src/HexChat.UI`](file:///d:/Quelltext/hexchat/src/HexChat.UI) | Präsentationsschicht, Avalonia Views, Dialoge, ViewModels, Themes, Konverter. | Avalonia 12, `CommunityToolkit.Mvvm`, CompiledBindings (`x:DataType`) |
| [`src/HexChat.Desktop`](file:///d:/Quelltext/hexchat/src/HexChat.Desktop) | Einstiegspunkt der Desktop-Anwendung, Plattforminitialisierung, Dependency Injection. | .NET 10 Generic Host, Avalonia Desktop Lifetime |
| [`tests/HexChat.Core.Tests`](file:///d:/Quelltext/hexchat/tests/HexChat.Core.Tests) | Unit- und Integrationstests für Parser, State und Protokoll. | xUnit, FluentAssertions, NSubstitute |
| [`legacy/`](file:///d:/Quelltext/hexchat/legacy) | Originale C/GTK2 Codebasis zur funktionalen Referenz und Paritätsprüfung. | C99, GLib, GTK+ 2.x (Archiv & Referenz) |

---

## 🛠 3. Core Development Principles for AI & Contributors

### A. C# & .NET 10 Standards

1. **Modern C# 13 Idioms:** File-scoped Namespaces (`namespace HexChat.Core.Parser;`), Primary Constructors, Pattern Matching, Records für unveränderliche Datenmodelle.
2. **Nullable Reference Types:** Strikte Nullability (`#nullable enable` ist global aktiv). Keine unbegründeten Null-Forgiving Operatoren (`!`).
3. **High-Performance Parsing:** Im IRC-Message-Parsing-Hotpath Speicherallokationen minimieren (`ReadOnlySpan<char>`, `Memory<char>`, Slice-Operationen statt unnötiger Substrings).
4. **Asynchronität:** Durchgängig echte Asynchronität (`async`/`await`, `Task`, `ValueTask`, `IAsyncEnumerable<T>`, `CancellationToken`). Niemals `.Result` oder `.Wait()` auf Tasks aufrufen (Deadlock-Gefahr im UI-Thread).

### B. Avalonia UI & MVVM Best Practices

1. **Compiled Bindings:** Jede AXAML-Datei MUSS `x:DataType="vm:MyViewModel"` deklarieren.
2. **CommunityToolkit.Mvvm:** Nutzung von `[ObservableProperty]`, `[RelayCommand]` und `ObservableObject`.
3. **UI Thread Safety:** UI-Updates und ObservableCollections dürfen nur auf dem UI-Thread modifiziert werden (`Dispatcher.UIThread.InvokeAsync` bzw. `Post`).
4. **Theme & Fluent Design:** Styling über ResourceDictionaries, DynamicResources und ControlThemes; keine festverdrahteten Hex-Farbcodes in Bedienelementen.

### C. IRC Protokoll & Standardtreue

1. **RFC- & IRCv3-Konformität:** Volle Unterstützung von RFC 1459, RFC 2812 sowie modernen IRCv3 Capabilities (`CAP LS 302`, `sasl`, `message-tags`, `server-time`, `batch`, `echo-message`, `chathistory`).
2. **Zeichenkodierung:** Standardmäßig UTF-8 mit sicherem Fallback auf ISO-8859-1 / CP1252 bei fehlerhaften Server-Payloads.

### D. Testing & Qualitätssicherung

1. **Testabdeckung:** Jede neue Parser-Regel, Protokollfunktion oder State-Machine-Änderung MUSS von Unit-Tests in `HexChat.Core.Tests` begleitet werden.
2. **Deterministische Tests:** Tests dürfen keine echten Netzwerkverbindungen aufbauen (Mocking via NSubstitute oder In-Memory Stream Pipes).

---

## 🔒 4. Security & Safety Rules

1. **Keine Secrets in Git:** Niemals Passwörter, NickServ-Passwörter, SASL-Credentials oder API-Keys im Quellcode oder in Commits hinterlegen.
2. **Keine unautorisierten Commits:** Als KI-Assistent niemals eigenständig Git-Commits erstellen, es sei denn, der Benutzer fordert dies explizit an.
3. **Sichere TLS-Validierung:** Keine Deaktivierung der Zertifikatsvalidierung in Produktionspfaden.

---

## 📚 5. Correspondence with Workspace Structure & Roadmaps

Diese `AGENTS.md` dient als Einstiegspunkt und verweist auf die vertiefenden Richtlinien, Checklisten und Paritätsmatrizen im Projekt:

- **Master Roadmap & Dashboard:** [`ROADMAP.md`](file:///d:/Quelltext/hexchat/ROADMAP.md)
- **Post-Port Zukunftsplan (IRCv3):** [`FUTURE-PLAN.md`](file:///d:/Quelltext/hexchat/FUTURE-PLAN.md)
- **Master Paritätsmatrix (C nach C#):** [`docs/PARITY_MATRIX.md`](file:///d:/Quelltext/hexchat/docs/PARITY_MATRIX.md)
- **Befehls-Parität (70+ Slash-Commands):** [`docs/COMMANDS_PARITY.md`](file:///d:/Quelltext/hexchat/docs/COMMANDS_PARITY.md)
- **Menü-, Dialog- & UI-Parität:** [`docs/MENUS_AND_UI_PARITY.md`](file:///d:/Quelltext/hexchat/docs/MENUS_AND_UI_PARITY.md)
- **Orientierungs-Index:** [`.agents/CONTEXT.md`](file:///d:/Quelltext/hexchat/.agents/CONTEXT.md)
- **Architektur & Schichten:** [`.agents/rules/architecture_and_layers.md`](file:///d:/Quelltext/hexchat/.agents/rules/architecture_and_layers.md)
- **C# & .NET 10 Coding-Standards:** [`.agents/rules/coding_standards_csharp.md`](file:///d:/Quelltext/hexchat/.agents/rules/coding_standards_csharp.md)
- **Avalonia UI Guidelines:** [`.agents/rules/avalonia_ui_guidelines.md`](file:///d:/Quelltext/hexchat/.agents/rules/avalonia_ui_guidelines.md)
- **IRC Protokoll & Test-Leitfaden:** [`.agents/rules/irc_protocol_and_testing.md`](file:///d:/Quelltext/hexchat/.agents/rules/irc_protocol_and_testing.md)
- **Pre-PR Checkliste:** [`.agents/checklists/pre-pr-checklist.md`](file:///d:/Quelltext/hexchat/.agents/checklists/pre-pr-checklist.md)
- **C-nach-Avalonia Portierungs-Checkliste:** [`.agents/checklists/porting-c-to-avalonia.md`](file:///d:/Quelltext/hexchat/.agents/checklists/porting-c-to-avalonia.md)
- **Troubleshooting & Fehlerbehebung:** [`.agents/troubleshooting/common-errors.md`](file:///d:/Quelltext/hexchat/.agents/troubleshooting/common-errors.md)
- **ADR Vorlage:** [`.agents/templates/adr-template.md`](file:///d:/Quelltext/hexchat/.agents/templates/adr-template.md)
- **ViewModel & View Vorlage:** [`.agents/templates/viewmodel-view-template.md`](file:///d:/Quelltext/hexchat/.agents/templates/viewmodel-view-template.md)
- **Dev-Workflow Skill:** [`.agents/skills/hexchat-dev-workflow/SKILL.md`](file:///d:/Quelltext/hexchat/.agents/skills/hexchat-dev-workflow/SKILL.md)
