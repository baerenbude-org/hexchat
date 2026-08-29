<!-- SPDX-License-Identifier: GPL-2.0-or-later -->
<!-- HexChat (Avalonia Port) - .agents/checklists/porting-c-to-avalonia.md -->
<!-- Description: Schritt-für-Schritt Leitfaden zur Portierung von C/GTK-Dialogen und Modulen nach Avalonia MVVM. -->

# Leitfaden: C/GTK nach Avalonia UI Portierung — HexChat

Verwende diesen Leitfaden, wenn du einen bestehenden C/GTK2-Dialog (z. B. Netzwerk-Liste, DCC-Transfer, Channel-Liste, Plugin-Manager, Raw-Log) in das neue Avalonia UI-System überführst.

---

## 🔍 Schritt 1: Analyse des bestehenden C/GTK-Codes

1. **C-Quellcode identifizieren:**
   - GUI/GTK-Code: In [`legacy/src/fe-gtk/`](file:///d:/Quelltext/hexchat/legacy/src/fe-gtk) suchen (z. B. `servlistgui.c`, `dccgui.c`, `chanlist.c`, `rawlog.c`, `urlgrab.c`).
   - Kern-Logik: In [`legacy/src/common/`](file:///d:/Quelltext/hexchat/legacy/src/common) suchen (z. B. `servlist.c`, `dcc.c`, `hexchat.c`).
2. **Datenmodell erfassen:**
   - Welche Structs werden verwendet? (z. B. `struct ircnet`, `struct ircserver`).
   - Welche Signale/Events feuert GTK? (z. B. `gtk_tree_view_get_selection`, `button_clicked`).
3. **Funktionsumfang notieren:**
   - Alle Buttons, Eingabefelder, Checkboxen und deren Aktionen tabellarisch erfassen.

---

## 🏗️ Schritt 2: Core-Modell & Schnittstellen in `HexChat.Core`

1. **DTOs / Records anlegen:**
   - In `src/HexChat.Core/Models/` unveränderliche Records oder Klassen anlegen.
2. **Service / State bereitstellen:**
   - Interface in `src/HexChat.Core/Services/` definieren (z. B. `IServerListService`).
   - Asynchrone Methoden mit `Task` und `CancellationToken` versehen.
3. **Unit-Tests schreiben:**
   - In `tests/HexChat.Core.Tests/` das Laden, Speichern und Verarbeiten des neuen Modells absichern.

---

## 🎨 Schritt 3: ViewModel in `HexChat.UI/ViewModels`

1. **ViewModel-Klasse erstellen:**
   - Nutze die Vorlage aus [`.agents/templates/viewmodel-view-template.md`](../templates/viewmodel-view-template.md).
   - Erbe von `ObservableObject`.
   - Generiere Properties mit `[ObservableProperty]`.
   - Generiere Commands mit `[RelayCommand]`.
2. **Kollektionen binden:**
   - `ObservableCollection<T>` für Listen verwenden.

---

## 🖼️ Schritt 4: Avalonia View in `HexChat.UI/Views`

1. **AXAML Datei anlegen:**
   - Als `UserControl` oder `Window` in `src/HexChat.UI/Views/`.
   - Typsicheres `x:DataType="vm:MyNewViewModel"` setzen.
2. **Layout gestalten:**
   - `Grid`, `StackPanel`, `DockPanel` und Avalonia Controls (`TextBox`, `Button`, `ListBox`, `DataGrid`) nutzen.
   - Theme-Resourcen (`DynamicResource`) für Farben und Abstände einsetzen.

---

## 🧪 Schritt 5: Integration & Testen

1. **Navigation / Dialogaufruf:**
   - Dialog im `MainViewModel` oder über Dependency Injection registrieren.
2. **Manueller UI-Check:**
   - App starten: `dotnet run --project src/HexChat.Desktop/HexChat.Desktop.csproj` (oder **F5** in VS Code).
   - Dialog öffnen, Daten eingeben, Validierung prüfen, ESC-Taste / Schließen testen.
3. **Automatisierte Tests:**
   - `dotnet test HexChat.sln` ausführen.
