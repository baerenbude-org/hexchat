<!-- SPDX-License-Identifier: GPL-2.0-or-later -->
<!-- HexChat (Avalonia Port) - .agents/rules/avalonia_ui_guidelines.md -->
<!-- Description: Richtlinien für Avalonia UI 11, XAML, MVVM Toolkit, CompiledBindings und Threading. -->

# Avalonia UI 11 & MVVM Guidelines — HexChat

## 1. MVVM Architektur & CommunityToolkit.Mvvm

Wir nutzen das moderne, quellcodegenerierte `CommunityToolkit.Mvvm` für alle ViewModels:

1. **ObservableObject & ObservableProperty:**
   ```csharp
   public partial class ChannelViewModel : ObservableObject
   {
       [ObservableProperty]
       private string _name = string.Empty;

       [ObservableProperty]
       private string _topic = string.Empty;

       [ObservableProperty]
       private int _userCount;

       [RelayCommand]
       private async Task SendMessageAsync(string text, CancellationToken cancellationToken)
       {
           // ...
       }
   }
   ```
2. **Commands:**
   - Nutze `[RelayCommand]` mit `async Task`.
   - Schütze Commands gegen Mehrfachausführung über `CanExecute` oder Flags (`IsBusy`).

---

## 2. Typsichere XAML Bindings (CompiledBindings)

1. **Strikte CompiledBindings:**
   - Jede AXAML-Datei (Window, UserControl, DataTemplate) MUSS den Typ des DataContext über `x:DataType` deklarieren:
   ```xml
   <UserControl xmlns="https://github.com/avaloniaui"
                xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                xmlns:vm="using:HexChat.UI.ViewModels"
                x:Class="HexChat.UI.Views.ChannelView"
                x:DataType="vm:ChannelViewModel">
       <TextBlock Text="{Binding Name}" />
   </UserControl>
   ```
2. **Keine ungebundenen Runtime-Bindings:**
   - Vermeide Binding-Pfade ohne Compile-Time-Verifikation. Bindingsfehler führen sonst zu stillschweigenden UI-Fehlern.

---

## 3. UI-Thread Sicherheit (Threading Rules)

1. **Hintergrund-Events entkoppeln:**
   - IRC-Sockets laufen auf Hintergrund-Threads (ThreadPool).
   - Jegliche Modifikation von `ObservableCollection<T>` oder Property-Änderungen, an die UI-Elemente gebunden sind, MUSS auf dem UI-Thread erfolgen:
   ```csharp
   await Dispatcher.UIThread.InvokeAsync(() =>
   {
       Messages.Add(newChatMessage);
   });
   ```
2. **UI nicht blockieren:**
   - Niemals rechenintensive Operationen (z. B. IRC-Log-Parsing, DCC File Transfer Checksummen) im UI-Thread durchführen.

---

## 4. Styling, Themes & Responsive Design

1. **Keine festen Farbwerte im XAML:**
   - Nutze `DynamicResource` und Theme-Ressourcen (`SystemControlBackgroundAltHighBrush`, `AccentColor`, HexChat-Farbpaletten):
   ```xml
   <!-- RICHTIG: -->
   <Border Background="{DynamicResource SystemChromeMediumColor}">
   
   <!-- FALSCH: -->
   <Border Background="#252526">
   ```
2. **Dark / Light Theme Support:**
   - Das UI muss sich nahtlos an Systemthemen (Hell/Dunkel) anpassen und HexChat-spezifische Chat-Themes unterstützen.
3. **Barrierefreiheit & Tastatur-Navigation:**
   - HexChat ist traditionell tastaturgesteuert (`Alt+1..9` für Tab-Wechsel, `Tab` für Nickname-Vervollständigung, Pfeiltasten für Befehlshistorie). Diese Hotkeys müssen in Avalonia erhalten bleiben.
