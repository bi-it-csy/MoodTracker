using System.Collections.ObjectModel;
using System.Text.Json;

namespace prototype1
{
    public partial class MainPage : ContentPage
    {
        private const string StorageKey = "mood_entries";

        private readonly Color accentColor = Color.FromArgb("#1F6F78");
        private readonly Color neutralButtonColor = Color.FromArgb("#E8EEF0");
        private readonly Color neutralButtonTextColor = Color.FromArgb("#15343B");
        private string selectedMood = string.Empty;

        public ObservableCollection<MoodEntry> MoodEntries { get; } = new();

        public string TodayText => DateTime.Now.ToString("dd.MM.yyyy");

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            LoadEntries();
            CoursePicker.SelectedIndex = 0;
            UpdateMoodButtons();
            UpdateEntryCount();
        }

        private void OnMoodClicked(object? sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string mood)
            {
                selectedMood = mood;
                UpdateMoodButtons();
                StatusLabel.Text = $"Stimmung ausgew\u00e4hlt: {mood}";
            }
        }

        private async void OnSaveClicked(object? sender, EventArgs e)
        {
            if (CoursePicker.SelectedItem is not string course)
            {
                await DisplayAlertAsync("Angebot fehlt", "Bitte w\u00e4hle zuerst ein Angebot aus.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(selectedMood))
            {
                await DisplayAlertAsync("Stimmung fehlt", "Bitte w\u00e4hle deine Stimmung aus.", "OK");
                return;
            }

            var entry = new MoodEntry
            {
                Course = course,
                Mood = selectedMood,
                Note = NoteEditor.Text?.Trim() ?? string.Empty,
                CreatedAt = DateTime.Now
            };

            MoodEntries.Insert(0, entry);
            SaveEntries();
            ResetForm();
            UpdateEntryCount();

            StatusLabel.Text = "Eintrag gespeichert. Danke f\u00fcr dein Feedback!";
            SemanticScreenReader.Announce(StatusLabel.Text);
        }

        private void OnResetFormClicked(object? sender, EventArgs e)
        {
            ResetForm();
            StatusLabel.Text = "Formular zur\u00fcckgesetzt.";
        }

        private async void OnClearHistoryClicked(object? sender, EventArgs e)
        {
            if (MoodEntries.Count == 0)
            {
                StatusLabel.Text = "Es gibt noch keinen Verlauf zum L\u00f6schen.";
                return;
            }

            var shouldClear = await DisplayAlertAsync(
                "Verlauf l\u00f6schen",
                "M\u00f6chtest du alle bisherigen Stimmungseintr\u00e4ge l\u00f6schen?",
                "L\u00f6schen",
                "Abbrechen");

            if (!shouldClear)
            {
                return;
            }

            MoodEntries.Clear();
            Preferences.Remove(StorageKey);
            UpdateEntryCount();
            StatusLabel.Text = "Verlauf gel\u00f6scht.";
        }

        private void ResetForm()
        {
            CoursePicker.SelectedIndex = 0;
            NoteEditor.Text = string.Empty;
            selectedMood = string.Empty;
            UpdateMoodButtons();
        }

        private void UpdateMoodButtons()
        {
            SetMoodButtonState(GoodMoodButton, selectedMood == "Gut");
            SetMoodButtonState(NeutralMoodButton, selectedMood == "Neutral");
            SetMoodButtonState(BadMoodButton, selectedMood == "Schlecht");
        }

        private void SetMoodButtonState(Button button, bool isSelected)
        {
            button.BackgroundColor = isSelected ? accentColor : neutralButtonColor;
            button.TextColor = isSelected ? Colors.White : neutralButtonTextColor;
        }

        private void LoadEntries()
        {
            var storedEntries = Preferences.Get(StorageKey, string.Empty);

            if (string.IsNullOrWhiteSpace(storedEntries))
            {
                return;
            }

            try
            {
                var entries = JsonSerializer.Deserialize<List<MoodEntry>>(storedEntries) ?? new List<MoodEntry>();

                foreach (var entry in entries.OrderByDescending(entry => entry.CreatedAt))
                {
                    MoodEntries.Add(entry);
                }
            }
            catch (JsonException)
            {
                Preferences.Remove(StorageKey);
            }
        }

        private void SaveEntries()
        {
            var serializedEntries = JsonSerializer.Serialize(MoodEntries.ToList());
            Preferences.Set(StorageKey, serializedEntries);
        }

        private void UpdateEntryCount()
        {
            EntryCountLabel.Text = MoodEntries.Count switch
            {
                0 => "Noch kein Feedback gespeichert.",
                1 => "1 Feedback gespeichert.",
                _ => $"{MoodEntries.Count} Feedbacks gespeichert."
            };
        }
    }

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

        public string CreatedAtText => $"{CreatedAt:dd.MM.yyyy HH:mm} \u00b7 {Mood}";

        public string NotePreview => string.IsNullOrWhiteSpace(Note)
            ? "Keine Notiz erfasst."
            : Note;
    }
}