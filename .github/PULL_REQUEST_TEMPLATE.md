## Beschreibung der Änderungen

Kurze Zusammenfassung der vorgenommenen Änderungen, behobenen Probleme oder neuen Features.

Schließt Issue: #<!-- Issue-Nummer hier eintragen, z. B. #123 -->

## Art der Änderung
- [ ] 🐛 Bugfix (nicht-brechende Änderung zur Fehlerbehebung)
- [ ] ✨ Neues Feature (nicht-brechende Änderung für neue Funktionalität)
- [ ] 💥 Breaking Change (Änderung, die bestehende Konfigurationen oder Plugin-APIs betrifft)
- [ ] 🧹 Refactoring / Code-Qualität (ohne funktionale Änderungen)
- [ ] 🎨 UI / Avalonia XAML Anpassungen (Design, Theme, Controls)
- [ ] 📝 Dokumentation / Übersetzungen (PO-Dateien)
- [ ] 🔧 Build-System / CI-Anpassungen

## Checkliste vor dem Einreichen
- [ ] Mein Code entspricht den Formatierungsrichtlinien des Projekts (`.editorconfig`, `.clang-format` für C-Code).
- [ ] Die .NET-Solution baut fehlerfrei (`dotnet build HexChat.sln`).
- [ ] Alle Unit- und Integrationstests wurden ausgeführt und bestehen (`dotnet test HexChat.sln`).
- [ ] Sofern C-Code geändert wurde: C-Build und Tests erfolgreich (`ninja -C build test` oder MSBuild).
- [ ] Avalonia-Bindings nutzen CompiledBindings (`x:DataType`) und sind typsicher.
- [ ] Keine sensitiven Daten, API-Keys oder Passwörter im Code oder in Commit-Histories.
