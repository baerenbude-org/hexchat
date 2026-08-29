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
