<!-- SPDX-License-Identifier: GPL-2.0-or-later -->
<!-- HexChat (Avalonia Port) - .agents/checklists/pre-pr-checklist.md -->
<!-- Description: Checkliste zur Verifikation von Code-Qualität, Tests, Bindings und Sicherheit vor Commits und Pull Requests. -->

# Pre-PR & Release Checklist — HexChat

Vor dem Erstellen von Commits, Pull Requests oder dem Zusammenführen von Änderungen in den Hauptzweig sollte diese Checkliste sorgfältig durchlaufen werden:

---

## 🏗️ 1. Build & Kompilierung
- [ ] Die .NET Solution baut fehlerfrei ohne Warnungen:
  ```powershell
  dotnet build HexChat.sln -c Release
  ```
- [ ] Sofern C-Code (`src/common/`, `src/fe-gtk/`) modifiziert wurde:
  ```bash
  ninja -C build test # oder MSBuild win32/hexchat.sln
  ```

---

## 🧪 2. Tests & Qualitätssicherung
- [ ] Alle Unit- und Integrationstests laufen erfolgreich durch:
  ```powershell
  dotnet test HexChat.sln
  ```
- [ ] Neue IRC-Befehle, Parser-Modi oder State-Transitions sind durch dedizierte Unit-Tests in `HexChat.Core.Tests` abgedeckt.
- [ ] Keine hängenden oder flaky asynchronen Tasks im Testlauf.

---

## 🎨 3. Avalonia UI & MVVM Konformität
- [ ] Alle AXAML-Dateien deklarieren typsichere `x:DataType` CompiledBindings.
- [ ] Keine direkten Zugriffe auf `ObservableCollection<T>` von Hintergrund-Threads (Nutzung von `Dispatcher.UIThread`).
- [ ] Farben und Styles nutzen `DynamicResource` und Theme-Keys (keine hartcodierten Farbwerte).
- [ ] Tastaturkürzel und Tab-Navigation funktionieren wie erwartet.

---

## 🔒 4. Sicherheit & Hygiene
- [ ] Keine sensiblen Daten, API-Keys, Passwörter oder Server-Token im Code.
- [ ] `git status` prüfen: Keine temporären Dateien (`.vs/`, `bin/`, `obj/`, `*.log`, `TestResults/`) im Staging-Bereich.
- [ ] Code entspricht `.editorconfig` (korrekte Einrückung, LF-Zeilenenden, keine Trailing-Spaces).

---

## ⚡ 5. Schnellbefehl zur Gesamtvalidierung
```powershell
# Vollständiger Validierungslauf
dotnet build HexChat.sln && dotnet test HexChat.sln
```
