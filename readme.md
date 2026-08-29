# HexChat (Avalonia Port)

[![Build & Test](https://github.com/baerenbude-org/hexchat/actions/workflows/windows-build.yaml/badge.svg)](https://github.com/baerenbude-org/hexchat)
[![License: GPL v2](https://img.shields.io/badge/License-GPL%20v2-blue.svg)](COPYING)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![Avalonia UI](https://img.shields.io/badge/UI-Avalonia%2011-purple.svg)](https://avaloniaui.net/)

Dieser Fork von **HexChat** portiert den beliebten, klassischen IRC-Client in eine moderne, plattformübergreifende Architektur auf Basis von **C# / .NET 8** und **Avalonia UI** (entwickelt im agilen Vibe-Coding-Ansatz).

---

## 🎯 Ziele des Ports

- **Moderne UI**: Reaktives, hochperformantes und plattformübergreifendes UI mit [Avalonia UI](https://avaloniaui.net/) (Windows, Linux, macOS) unter Beibehaltung des beliebten HexChat-Workflows.
- **Saubere Architektur**: Klare Trennung von Protokoll-Engine ([HexChat.Core](src/HexChat.Core)), MVVM-ViewModels & Views ([HexChat.UI](src/HexChat.UI)) und Desktop-Host ([HexChat.Desktop](src/HexChat.Desktop)).
- **Robuste IRC-Engine**: Asynchrone Verbindungsverwaltung (TLS/SSL, SASL, IPv6, Bouncer-Support), IRCv3-Unterstützung und umfassende Testabdeckung.
- **Referenz & Parität**: Der originale C/GTK-Code bleibt zur Referenz und Sicherstellung voller Funktionsparität im Repository erhalten.

---

## 🏗️ Projektstruktur (.NET & Avalonia)

Die .NET-Lösung befindet sich in [`HexChat.sln`](HexChat.sln):

```text
hexchat/
├── HexChat.sln                  # Haupt-Solution für .NET 8 / Avalonia
├── src/
│   ├── HexChat.Core/            # IRC-Protokoll, Parser, State, Netzwerk & Logik
│   ├── HexChat.UI/              # Avalonia XAML Views, ViewModels, Themes, Controls
│   ├── HexChat.Desktop/         # Desktop-Starter & Plattforminitialisierung
│   ├── common/                  # Originaler C HexChat Core (Referenz)
│   ├── fe-gtk/                  # Originales GTK Frontend (Referenz)
│   └── fe-text/                 # Originales Text Frontend (Referenz)
├── tests/
│   └── HexChat.Core.Tests/      # Unit- & Integrationstests
└── win32/ & meson.build         # Ursprüngliche Build-Skripte für C/C++
```

---

## 🚀 Schnellstart (.NET 8)

### Voraussetzungen
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Empfohlen für IDEs: [Antigravity IDE](https://antigravity.google) / VS Code mit der **Avalonia for VS Code** & **C#** Erweiterung

### Bauen & Ausführen

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
