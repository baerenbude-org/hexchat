---
name: hexchat-dev-workflow
description: >-
  Build, test, run, and port features for HexChat (.NET 10 / Avalonia UI Port).
  Use this skill whenever building the solution, running tests, launching the Avalonia Desktop client,
  or porting legacy C/GTK features to C# and Avalonia UI.
---

# HexChat Development Workflow Skill

Dieser Skill stellt die Standard-Workflows für Entwicklung, Build, Testing und Portierung im HexChat-Repository bereit.

---

## 1. Bauen und Testen der .NET Solution

### Kompilieren
Führe folgenden Befehl aus, um alle Projekte (`HexChat.Core`, `HexChat.UI`, `HexChat.Desktop`, `HexChat.Core.Tests`) zu kompilieren:
```powershell
dotnet build HexChat.sln
```

### Tests ausführen
Führe die xUnit-Testsuite aus:
```powershell
dotnet test HexChat.sln
```

---

## 2. Starten der Desktop-Anwendung (Avalonia UI)

Starte den Avalonia Desktop-Host direkt aus dem Repository:
```powershell
dotnet run --project src/HexChat.Desktop/HexChat.Desktop.csproj
```

---

## 3. Workflow zur Feature-Portierung (C/GTK nach Avalonia)

Wenn ein Dialog oder Feature aus dem originalen HexChat portiert wird:

1. **Referenzcode analysieren:**
   - C-Logik in `src/common/` und GTK-Oberfläche in `src/fe-gtk/` prüfen.
2. **Core-Dienste anlegen:**
   - Modelle und Schnittstellen in `src/HexChat.Core/` implementieren.
   - Unit-Tests in `tests/HexChat.Core.Tests/` schreiben und verifizieren.
3. **UI-Komponenten anlegen:**
   - ViewModel in `src/HexChat.UI/ViewModels/` (basierend auf `[ObservableProperty]`, `[RelayCommand]`).
   - AXAML View in `src/HexChat.UI/Views/` mit `x:DataType="vm:..."`.
4. **Validieren:**
   - `dotnet test HexChat.sln`
   - Manueller Test über `dotnet run --project src/HexChat.Desktop/HexChat.Desktop.csproj`.

---

## 4. GitHub CI, PRs und Security-Status abfragen

Wenn eine `.env`-Datei mit `GITHUB_TOKEN` vorhanden ist, können Status und Logs direkt via PowerShell abgefragt werden:

```powershell
# Neueste Actions-Runs anzeigen
.\tools\Get-GitHubInfo.ps1 -Action runs

# Logs und Schritte eines fehlgeschlagenen Runs inspizieren
.\tools\Get-GitHubInfo.ps1 -Action run-logs -RunId <RUN_ID>

# PRs oder Issues einsehen
.\tools\Get-GitHubInfo.ps1 -Action prs
.\tools\Get-GitHubInfo.ps1 -Action issues

# Dependabot & Security Alerts prüfen
.\tools\Get-GitHubInfo.ps1 -Action security
```

