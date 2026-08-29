# HexChat Legacy C/GTK2 Codebase (Reference & Archive)

Dieses Verzeichnis enthält den originalen C/GTK2-Quellcode von HexChat (und X-Chat) als **funktionale Referenz, Kompatibilitätsanker und historisches Archiv** für den modernen C# / .NET 10 & Avalonia UI Port.

---

## 📂 Struktur des Legacy-Verzeichnisses

- [`src/common/`](file:///d:/Quelltext/hexchat/legacy/src/common): Originale C-Logik, IRC-Protokollverarbeitung, CTCP, DCC, URL-Handling, Serverlisten.
- [`src/fe-gtk/`](file:///d:/Quelltext/hexchat/legacy/src/fe-gtk): Originales GTK+ 2.x Frontend (Dialoge, Fenster, Menüs, Chat-Rendering).
- [`src/fe-text/`](file:///d:/Quelltext/hexchat/legacy/src/fe-text): Ncurses/Text-basiertes Terminal-Frontend.
- [`plugins/`](file:///d:/Quelltext/hexchat/legacy/plugins): Originale C-Plugins (Perl, Python, Lua, SysInfo, Sound, FishLim).
- [`win32/`](file:///d:/Quelltext/hexchat/legacy/win32): Ursprüngliche Visual Studio MSVC Projektdateien und InnoSetup-Skripte.
- [`data/`](file:///d:/Quelltext/hexchat/legacy/data): Icons, Glade UI-Definitionen und Desktop-Integration.
- [`po/`](file:///d:/Quelltext/hexchat/legacy/po): Gettext Übersetzungsdateien.
- [`flatpak/`](file:///d:/Quelltext/hexchat/legacy/flatpak) & [`osx/`](file:///d:/Quelltext/hexchat/legacy/osx): Historische Paketierungs-Skripte.

---

## 🎯 Zweck und Richtlinien

1. **Funktionale Parität:** Bei der Entwicklung neuer Features in C# (`src/HexChat.Core` und `src/HexChat.UI`) dient dieser C-Code als Vorlage für Protokollverhalten und Benutzererlebnis.
2. **Keine neuen C-Features:** Im `legacy/`-Ordner wird kein neuer Produktivcode entwickelt. Alle neuen Entwicklungen finden ausschließlich in C# / .NET 10 und Avalonia UI statt.
3. **Portierungs-Checkliste:** Siehe [`.agents/checklists/porting-c-to-avalonia.md`](../.agents/checklists/porting-c-to-avalonia.md).
