# 📘 Projekt-Journal: MoodTracker

## ℹ️ Info

Dieses Projektjournal dokumentiert die Umsetzung des Prototyps **MoodTracker**. Die App wurde im Rahmen des FerienPlausch-Projekts als einfache Feedback-App erstellt. Teilnehmerinnen und Teilnehmer können ein Angebot auswählen, ihre Stimmung erfassen und optional eine kurze Notiz speichern.

Ziel dieses Journals ist es, den aktuellen Projektstand, wichtige Entscheidungen, technische Herausforderungen und den Fortschritt nachvollziehbar festzuhalten.

**Projektname:** MoodTracker  
**Repository/Projekt:** prototype1  
**Technologie:** C# mit .NET MAUI  
**Zielplattform im Prototyp:** Windows Machine; Android ist im Projekt vorbereitet, das lokale Emulator-Deployment war jedoch durch die Schulungsumgebung blockiert.

---

## Inhaltsverzeichnis

- [📘 Projekt-Journal: MoodTracker](#-projekt-journal-moodtracker)
  - [ℹ️ Info](#️-info)
  - [Tagesaktivitäten](#tagesaktivitäten)
  - [🧩 Phase 1: Setup und Initialisierung](#-phase-1-setup-und-initialisierung)
  - [🧱 Phase 2: Planung](#-phase-2-planung)
    - [📂 Verwendete Datenstruktur](#-verwendete-datenstruktur)
    - [🔷 UML Klassendiagramm](#-uml-klassendiagramm)
    - [🖼️ Layout](#️-layout)
    - [✅ Validierung](#-validierung)
  - [🚀 Phase 3: Umsetzung](#-phase-3-umsetzung)
  - [⚙️ Phase 4: Technische Reflexion](#️-phase-4-technische-reflexion)
  - [🔒 Phase 5: Abschluss und Präsentation](#-phase-5-abschluss-und-präsentation)
  - [📸 Screenshots vom Fortschritt](#-screenshots-vom-fortschritt)
  - [⚠️ Herausforderungen und Blockaden](#️-herausforderungen-und-blockaden)
  - [💻 Besonders erwähnenswerte Code-Beispiele](#-besonders-erwähnenswerte-code-beispiele)
  - [Reflexion](#reflexion)

---

## Tagesaktivitäten

| Datum | Aktivität |
| --- | --- |
| Di, 07.07.2026 | Projekt analysiert und Visual-Studio/.NET-Runtime-Problem untersucht. |
| Di, 07.07.2026 | MoodTracker-Prototyp erstellt: Kursauswahl, Stimmung, Notiz und Verlauf. |
| Di, 07.07.2026 | Windows-Build erfolgreich getestet. Android-Build erfolgreich, Android-Deployment wegen lokaler Emulator-/JDK-Konfiguration blockiert. |
| Di, 07.07.2026 | GitHub-Repository erstellt und Projekt hochgeladen. |
| Di, 07.07.2026 | Projekt von `experiment1` auf `prototype1` umbenannt und Encoding-Fehler korrigiert. |

---

## 🧩 Phase 1: Setup und Initialisierung

**Was habe ich in Phase 1 umgesetzt:**

- .NET MAUI-Projekt in Visual Studio geöffnet.
- Standard-Startseite geprüft.
- Lokales Problem mit Visual Studio und .NET-Runtime analysiert.
- Fehlerhafte Umgebungsvariable `DOTNET_ROOT` als Ursache für den Windows-Startfehler identifiziert.
- App erfolgreich über **Windows Machine** gestartet.

![Hello World / Startzustand](images/start.png)

---

## 🧱 Phase 2: Planung

**Was habe ich in Phase 2 umgesetzt:**

Der Prototyp wurde bewusst einfach gehalten. Es gibt keine API-Anbindung, keine Benutzeranmeldung und kein Administrations-Backend. Die App besteht aus einer Hauptseite mit Formular und Verlauf.

### 📂 Verwendete Datenstruktur

Die App speichert Stimmungseinträge lokal. Ein Eintrag enthält:

```json
[
  {
    "Course": "Sportkurs",
    "Mood": "Gut",
    "Note": "Hat Spass gemacht.",
    "CreatedAt": "2026-07-07T11:46:00"
  }
]
```

Die Datenstruktur wird in folgender C# Klasse abgebildet:

```csharp
public class MoodEntry
{
    public string Course { get; set; } = string.Empty;
    public string Mood { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public string MoodIcon => Mood switch
    {
        "Gut" => "\U0001F60A",
        "Neutral" => "\U0001F610",
        "Schlecht" => "\U0001F641",
        _ => "\u2022"
    };
}
```

Die Speicherung erfolgt mit `Preferences`. Dadurch bleiben Einträge lokal erhalten, auch wenn die App neu gestartet wird.

---

### 🔷 UML Klassendiagramm

```mermaid
classDiagram
    class MainPage {
        -string selectedMood
        +ObservableCollection~MoodEntry~ MoodEntries
        +string TodayText
        -OnMoodClicked()
        -OnSaveClicked()
        -OnResetFormClicked()
        -OnClearHistoryClicked()
        -LoadEntries()
        -SaveEntries()
        -UpdateEntryCount()
    }

    class MoodEntry {
        +string Course
        +string Mood
        +string Note
        +DateTime CreatedAt
        +string MoodIcon
        +string CreatedAtText
        +string NotePreview
    }

    MainPage "1" --> "*" MoodEntry
```

---

### 🖼️ Layout

Ich habe mich für ein einseitiges Layout entschieden:

**Bereich 1: Kopfbereich**

- App-Name `MoodTracker`
- Untertitel `FerienPlausch Feedback`
- aktuelles Datum

**Bereich 2: Eingabeformular**

- Auswahl eines Angebots
- Auswahl der Stimmung
- optionale Notiz
- Speichern- und Zurücksetzen-Button

**Bereich 3: Verlauf**

- Liste aller bisherigen Einträge
- Datum, Stimmung, Angebot und Notiz
- Button zum Löschen des Verlaufs

![MoodTracker Layout](images/layout.png)

---

### ✅ Validierung

Folgende Felder werden validiert:

| Feld | Regel | Rückmeldung |
| --- | --- | --- |
| Angebot | Muss ausgewählt sein | Dialog: `Bitte wähle zuerst ein Angebot aus.` |
| Stimmung | Muss ausgewählt sein | Dialog: `Bitte wähle deine Stimmung aus.` |
| Notiz | Optional | Keine Validierung nötig |

---

## 🚀 Phase 3: Umsetzung

**Was habe ich in Phase 3 umgesetzt:**

- Standard-`Hello World`-Seite entfernt.
- Eigene MoodTracker-Oberfläche in `MainPage.xaml` erstellt.
- Kursliste mit `Picker` umgesetzt.
- Stimmungsauswahl mit drei Buttons umgesetzt.
- Optionale Notiz mit `Editor` umgesetzt.
- Verlauf mit `CollectionView` umgesetzt.
- Lokale Speicherung mit `Preferences` umgesetzt.
- Projekt in GitHub hochgeladen.
- Projektdateien auf `prototype1` umbenannt.

![MoodTracker Formular](images/verlauf.png)

---

## ⚙️ Phase 4: Technische Reflexion

**Was habe ich in Phase 4 umgesetzt und untersucht:**

- Windows-Build erfolgreich geprüft.
- Android-Build erfolgreich geprüft.
- Android-Deployment untersucht.
- Problem mit Java/JDK und Android Emulator dokumentiert.
- Encoding-Problem nach der Umbenennung erkannt und korrigiert.

Das Projekt folgt bewusst einer einfachen Struktur. Es wurde kein separates ViewModel erstellt, weil die Aufgabenstellung eine vereinfachte Architektur erlaubt. Die View und die einfache Logik befinden sich gemeinsam in `MainPage.xaml` und `MainPage.xaml.cs`.

---

## 🔒 Phase 5: Abschluss und Präsentation

**Aktueller Stand:**

- Prototyp ist über Windows lauffähig.
- GitHub-Repository ist eingerichtet.
- Kernfunktionen der User Stories sind umgesetzt.
- Android ist im Projekt grundsätzlich vorbereitet, aber das Deployment ist auf dem lokalen Rechner durch fehlende bzw. falsch konfigurierte Android-Tools blockiert.

**Nicht enthalten:**

- API-Anbindung
- User Authentication
- Administrations-Backend

---

## 📸 Screenshots vom Fortschritt

| Datum | Feature | Screenshot |
| --- | --- | --- |
| 07.07.2026 | Startseite | ![](images/start.png) |
| 07.07.2026 | Eingabeformular | ![](images/layout.png) |
| 07.07.2026 | Verlauf | ![](images/verlauf.png) |

---

## ⚠️ Herausforderungen und Blockaden

### Android Emulator und JDK

- Problem: Visual Studio verwendete eine Java-8-JRE-Installation statt einer vollständigen JDK-Installation.
- Folge: Android-Deployment konnte nicht korrekt ausgeführt werden.
- Beobachtung: `adb` zeigte keine verbundenen Geräte.
- Lösung/Entscheidung: Für den Prototyp wurde der funktionierende Windows-Start verwendet. Android bleibt als Zielplattform im Projekt enthalten.

### Visual Studio Runtime-Problem

- Problem: Visual Studio meldete, dass eine .NET Desktop Runtime installiert werden müsse.
- Ursache: Eine fehlerhafte `DOTNET_ROOT`-Umgebungsvariable zeigte auf einen unvollständigen lokalen Ordner.
- Lösung: Die fehlerhafte Benutzer-Variable wurde entfernt. Danach konnte die Windows-App gestartet werden.

### Encoding nach Projektumbenennung

- Problem: Nach dem Umbenennen von `experiment1` auf `prototype1` waren Umlaute und Emojis beschädigt.
- Ursache: PowerShell 5.1 hatte UTF-8-Dateien mit falscher Zeichenkodierung neu geschrieben.
- Lösung: Die betroffenen Dateien wurden erneut sauber als UTF-8 geschrieben. In XAML werden Sonderzeichen teilweise als XML-Entities genutzt.

---

## 💻 Besonders erwähnenswerte Code-Beispiele

### Speichern eines Stimmungseintrags

```csharp
var entry = new MoodEntry
{
    Course = course,
    Mood = selectedMood,
    Note = NoteEditor.Text?.Trim() ?? string.Empty,
    CreatedAt = DateTime.Now
};

MoodEntries.Insert(0, entry);
SaveEntries();
```

### Lokale Speicherung mit Preferences

```csharp
private void SaveEntries()
{
    var serializedEntries = JsonSerializer.Serialize(MoodEntries.ToList());
    Preferences.Set(StorageKey, serializedEntries);
}
```

### Laden gespeicherter Einträge

```csharp
private void LoadEntries()
{
    var storedEntries = Preferences.Get(StorageKey, string.Empty);

    if (string.IsNullOrWhiteSpace(storedEntries))
    {
        return;
    }

    var entries = JsonSerializer.Deserialize<List<MoodEntry>>(storedEntries)
        ?? new List<MoodEntry>();

    foreach (var entry in entries.OrderByDescending(entry => entry.CreatedAt))
    {
        MoodEntries.Add(entry);
    }
}
```

### Validierung vor dem Speichern

```csharp
if (CoursePicker.SelectedItem is not string course)
{
    await DisplayAlertAsync(
        "Angebot fehlt",
        "Bitte wähle zuerst ein Angebot aus.",
        "OK");
    return;
}

if (string.IsNullOrWhiteSpace(selectedMood))
{
    await DisplayAlertAsync(
        "Stimmung fehlt",
        "Bitte wähle deine Stimmung aus.",
        "OK");
    return;
}
```

---

## Reflexion

Das Projekt war hilfreich, um den Ablauf einer kleinen MAUI-App zu verstehen. Besonders wichtig war die Erkenntnis, dass nicht jeder Fehler direkt im eigenen Code liegt. Ein grosser Teil der Arbeit bestand darin, Visual Studio, .NET, Android-Tools und GitHub korrekt einzuordnen.

Der MoodTracker-Prototyp erfüllt die wichtigsten User Stories: Stimmung erfassen, Notiz speichern und bisherige Einträge anzeigen. Für eine spätere Weiterentwicklung wären eine echte API-Anbindung, Benutzerverwaltung und ein Administrationsbereich sinnvolle nächste Schritte.

