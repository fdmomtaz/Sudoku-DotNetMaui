using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Text.Json;
using CoreData;

namespace Sudoku;

// [QueryProperty(nameof(ContinueGame), "ContinueGame")]
public partial class GamePage : ContentPage
{
    // public bool ContinueGame
    // {
    //     set
    //     {
    //         if (value) {
    //             string cacheDir = FileSystem.Current.CacheDirectory;
    //             string filePath = Path.Combine(cacheDir, "sudoku.json");

    //             if (File.Exists(filePath))
    //             {
    //                 string json = File.ReadAllText(filePath);
    //                 SudokuArray = JsonSerializer.Deserialize<int?[]>(json);
    //             }

    //         }
    //         else {
    //             Tuple<int?[], int[]>? generatedSudoku = SudokuGenerator.GetBoard(SudokuGenerator.Difficulty.Medium);

    //             SudokuArray = generatedSudoku.Item1;
    //             providedNumbers = generatedSudoku.Item2;
    //         }
    //     }
    // }


	public GamePage()
	{
		InitializeComponent();

		BindingContext = new GameViewModel();
	}
	
	private async void Close_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
}