<!-- SPDX-License-Identifier: GPL-2.0-or-later -->
<!-- HexChat (Avalonia Port) - .agents/troubleshooting/common-errors.md -->
<!-- Description: Typische Fehlerbilder, Ursachen und Lösungsstrategien für HexChat (.NET 10 & Avalonia). -->

# Troubleshooting & Common Errors — HexChat

Typische Fehlerbilder, Ursachen und Lösungsstrategien bei der Entwicklung von HexChat mit C# / .NET 10 und Avalonia UI.

---

## 1. Avalonia UI & XAML Bindings

### Problem: `Unable to resolve property or method on DataContext (CompiledBinding error)`
* **Ursache:** Die AXAML-Datei verwendet `x:DataType="vm:MyViewModel"`, aber das gebundene Property existiert nicht, ist privat oder der Datentyp stimmt nicht überein.
* **Lösung:**
  1. Prüfe, ob das Property im ViewModel `public` ist (oder durch `[ObservableProperty]` als `MyProperty` generiert wird).
  2. Stelle sicher, dass `xmlns:vm="using:HexChat.UI.ViewModels"` korrekt deklariert ist.
  3. Solution neu kompilieren (`dotnet build HexChat.sln`), damit die Source-Generatoren des MVVM-Toolkits aktiv werden.

### Problem: `InvalidOperationException: Call from invalid thread to Avalonia UI`
* **Ursache:** Eine UI-gebundene `ObservableCollection<T>` oder ein UI-Element wurde direkt aus einem Hintergrund-Thread (z. B. IRC Socket-Empfangsschleife) modifiziert.
* **Lösung:** Modifikation über den Avalonia UI-Thread kapseln:
  ```csharp
  Dispatcher.UIThread.Post(() =>
  {
      ChannelList.Add(newChannel);
  });
  ```

---

## 2. IRC Protokoll & Parser

### Problem: `Message framing error / truncated IRC lines`
* **Ursache:** Eingehende IRC-Zeilen wurden bei `\n` statt bei `\r\n` getrennt oder TCP-Pakete wurden mitten in einer Nachricht fragmentiert.
* **Lösung:**
  1. Stelle sicher, dass `System.IO.Pipelines` oder der Puffer nach vollständigen `\r\n` Sequenzen sucht.
  2. Maximale Zeilenlänge von 512 Bytes (inkl. CRLF) gemäß RFC 1459/2812 beachten.

### Problem: `Zeichenkodierungsfehler (Umlaute/Sonderzeichen als Fragezeichen oder )`
* **Ursache:** Der Server oder Client sendet ISO-8859-1/Windows-1252, während der Parser strikt UTF-8 ohne Fallback erwartet.
* **Lösung:** Nutze `Encoding.UTF8.GetString()` mit `DecoderExceptionFallback` und schlage bei Fehlern auf `Encoding.Latin1` (ISO-8859-1) fehl.

---

## 3. Build & .NET SDK

### Problem: `The target framework 'net10.0' is not supported by this SDK`
* **Ursache:** Auf dem System ist ein älteres .NET SDK (< 10.0) als Standard-SDK auf dem `PATH` hinterlegt.
* **Lösung:**
  1. Überprüfe die installierten SDKs: `dotnet --list-sdks`.
  2. Falls .NET 10 Preview/Release installiert ist, stelle `global.json` oder das Zielframework in den `.csproj`-Dateien passend ein.

### Problem: `Clean build erforderlich nach XAML-Änderungen`
* **Ursache:** Avalonia XAML Compiler (XBF / IL-Weaving) hält veraltete Zwischendateien in `obj/`.
* **Lösung:**
  ```powershell
  dotnet clean HexChat.sln
  dotnet build HexChat.sln
  ```

---

## 4. TLS/SSL & Netzwerk

### Problem: `AuthenticationException: The remote certificate is invalid according to the validation procedure`
* **Ursache:** Das IRC-Netzwerk nutzt ein selbstsigniertes Zertifikat oder der Hostname stimmt nicht mit dem Zertifikat überein.
* **Lösung:**
  1. In den Servereinstellungen die Option "Akzeptiere ungültige/selbstsignierte SSL-Zertifikate" für diesen Host konfigurieren.
  2. In Unit-Tests niemals echte SSL-Zertifikatsvalidierung im Live-Netzwerk ausführen.
