using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Sudoku;

public class SettingsViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    

    public SettingsViewModel()
    {
        // get the saved settings from the preferences
        hideUsedNumbers = Preferences.Default.Get("HideUsedNumbers", true);
        gameDifficulty = Preferences.Default.Get("GameDifficulty", "Easy") == "Hard";
    }

	bool hideUsedNumbers;
	public bool HideUsedNumbers
    {
        get => hideUsedNumbers;
        set
        {
            if (hideUsedNumbers != value)
            {
                // set the value in the preferences
                Preferences.Default.Set("HideUsedNumbers", value);

                hideUsedNumbers = value;
                OnPropertyChanged();
            }
        }
    }

    
	bool gameDifficulty;
	public bool GameDifficulty
    {
        get => gameDifficulty;
        set
        {
            if (gameDifficulty != value)
            {
                // set the value in the preferences
                Preferences.Default.Set("GameDifficulty", value ? "Hard" : "Easy");

                gameDifficulty = value;
                OnPropertyChanged();
            }
        }
    }

    
    // Command to delete saved game
	public ICommand RemoveSavedGame => new Command(() =>
    {
		string cacheDir = FileSystem.Current.CacheDirectory;
		string gameSavePath = Path.Combine(cacheDir, "sudoku.json");

        // delete the old save file if it exists
		if (File.Exists(gameSavePath))
            File.Delete(gameSavePath);

        var toast = Toast.Make("Your saved game was successfully deleted", ToastDuration.Short);
        toast.Show();
    });


}
