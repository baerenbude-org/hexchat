# HexChat (Avalonia Port)

[![Build & Test](https://github.com/baerenbude-org/hexchat/actions/workflows/dotnet-build.yaml/badge.svg)](https://github.com/baerenbude-org/hexchat)
[![License: GPL v2](https://img.shields.io/badge/License-GPL%20v2-blue.svg)](COPYING)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![Avalonia UI](https://img.shields.io/badge/UI-Avalonia%2012-purple.svg)](https://avaloniaui.net/)

Dieser Fork von **HexChat** portiert den beliebten, klassischen IRC-Client in eine moderne, plattformübergreifende Architektur auf Basis von **C# / .NET 10** und **Avalonia UI** (entwickelt im agilen Vibe-Coding-Ansatz).

---

## 🎯 Ziele des Ports

- **Moderne UI**: Reaktives, hochperformantes und plattformübergreifendes UI mit [Avalonia UI](https://avaloniaui.net/) (Windows, Linux, macOS) unter Beibehaltung des beliebten HexChat-Workflows.
- **Saubere Architektur**: Klare Trennung von Protokoll-Engine ([`src/HexChat.Core`](src/HexChat.Core)), MVVM-ViewModels & Views ([`src/HexChat.UI`](src/HexChat.UI)) und Desktop-Host ([`src/HexChat.Desktop`](src/HexChat.Desktop)).
- **Robuste IRC-Engine**: Asynchrone Verbindungsverwaltung (TLS/SSL, SASL, IPv6, Bouncer-Support), IRCv3-Unterstützung und umfassende Testabdeckung.
- **Referenz & Parität**: Der originale C/GTK-Code bleibt als funktionale Referenz im Verzeichnis [`legacy/`](legacy/) erhalten.

---

## 🗺️ Roadmap & Portierungsstatus

Der gesamte Funktionsumfang von HexChat wird systematisch 1:1 portiert. Der aktuelle Fortschritt und detaillierte Checklisten sind hier einsehbar:

- **[Master Roadmap & Dashboard](ROADMAP.md)** — Phasenplan (Phasen 1–7) & Status-Dashboard
- **[Master Paritätsmatrix](docs/PARITY_MATRIX.md)** — Lückenloser Abgleich aller C-Dateien mit C# / Avalonia 12
- **[Befehls-Parität](docs/COMMANDS_PARITY.md)** — Alle 70+ internen Slash-Befehle und Parameter
- **[Menü- & Dialog-Parität](docs/MENUS_AND_UI_PARITY.md)** — Alle Menüs, Tastenkürzel und 11 Preferences-Tabs

---

## 🏗️ Projektstruktur (.NET & Avalonia)

Die .NET-Lösung befindet sich in [`HexChat.sln`](HexChat.sln):

```text
hexchat/
├── HexChat.sln                  # Haupt-Solution für .NET 10 / Avalonia
├── AGENTS.md                    # Universelles Regelwerk für KI-Coding-Assistenten
├── .agents/                     # Hierarchische Agenten-Regeln, Checklisten & Skills
├── .vscode/                     # Bereinigte VS Code / Antigravity IDE Konfiguration
├── src/                         # 100 % .NET 10 / C# Quellcode
│   ├── HexChat.Core/            # IRC-Protokoll, Parser, State, Netzwerk & Logik
│   ├── HexChat.UI/              # Avalonia XAML Views, ViewModels, Themes, Controls
│   └── HexChat.Desktop/         # Desktop-Starter & Plattforminitialisierung
├── tests/
│   └── HexChat.Core.Tests/      # Unit- & Integrationstests
└── legacy/                      # Originaler C/GTK2 Quellcode (Referenzarchiv)
    ├── README.md                # Dokumentation zum Legacy-Code
    ├── src/                     # common, fe-gtk, fe-text, dirent, htm, libenchant
    ├── plugins/                 # C-Plugins (Perl, Python, Lua, etc.)
    └── win32/ & data/ & po/     # Windows MSVC Build, UI-Dateien & Übersetzungen
```

---

## 🚀 Schnellstart (.NET 10)

### Voraussetzungen

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Empfohlen für IDEs: [Antigravity IDE](https://antigravity.google) / VS Code mit der **Avalonia for VS Code** & **C#** Erweiterung

### Bauen, Testen & Ausführen

```bash
# Solution kompilieren
dotnet build HexChat.sln

# Tests ausführen
dotnet test HexChat.sln

# Desktop-Client (Avalonia UI) starten
dotnet run --project src/HexChat.Desktop/HexChat.Desktop.csproj
```

---

## 📜 Lizenz & Danksagung

Dieses Projekt basiert auf **HexChat** und **X-Chat**:

- X-Chat Copyright (c) 1998–2010 Peter Zelezny
- HexChat Copyright (c) 2009–2024 Berke Viktor und Mitwirkende
- Avalonia Port (c) 2026 baerenbude-org & Mitwirkende

Lizenziert unter der **GNU General Public License v2** (GPLv2) mit OpenSSL-Ausnahme. Siehe [COPYING](COPYING) für Details.  
Originale Dokumentation und Ressourcen: [hexchat.github.io](https://hexchat.github.io).
