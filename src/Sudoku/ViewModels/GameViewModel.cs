using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Sudoku;

public class GameViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    
    private string GameSavePath {
        get {
            string cacheDir = FileSystem.Current.CacheDirectory;
            return Path.Combine(cacheDir, "sudoku.json");
        }
    }

    public GameViewModel(bool ContinueGame)
    {
        SudokuBoard? generatedSudoku = null;

        if (ContinueGame) {
            if (File.Exists(GameSavePath))
            {
                string json = File.ReadAllText(GameSavePath);
                generatedSudoku = JsonSerializer.Deserialize<SudokuBoard>(json);
            }

            if (generatedSudoku != null)
            {
                var toast = Toast.Make("Your game was successfully loaded", ToastDuration.Short);
                toast.Show();
            }
        }

        // if no saved game, generate a new one
        if (generatedSudoku == null)
            generatedSudoku = SudokuGenerator.GetBoard();

        // populate the sudoku array and provided numbers
        SudokuArray = generatedSudoku.Board;
        providedNumbers = generatedSudoku.PermanentValues;
    }
    
    // Array of the numbers provided by the game that can't be changed
    int[] providedNumbers = new int[0];

    // Array of the sudoku board
	int?[] sudokuArray = new int? [81];
	public int?[] SudokuArray
    {
        get => sudokuArray;
        set
        {
            if (sudokuArray != value)
            {
                sudokuArray = value;
                OnPropertyChanged();
            }
        }
    }

    // Array of possible inputs for the selected cell
	bool[] availableNumbers = new bool[9] {true, true, true, true, true, true, true, true, true};
	public bool[] AvailableNumbers
    {
        get => availableNumbers;
        set
        {
            if (availableNumbers != value)
            {
                availableNumbers = value;
                OnPropertyChanged();
            }
        }
    }

    // Calculated properties based on SelectedNumber
	public int? SelectedRow
    {
        get {
            if (SelectedNumber == null || !SelectedNumber.HasValue)
                return null;

            int row = SelectedNumber.Value / 9;
            return row + 1 + row / 3;
        }
    }

    // Calculated properties based on SelectedNumber
	public int? SelectedColumn
    {
        get  { 
            if (SelectedNumber == null || !SelectedNumber.HasValue)
                return null;

            int col = SelectedNumber.Value % 9;
            return col + 1 + col / 3; 
        }
    }

    // Calculated properties based on SelectedNumber
    public int? SelectedBoxRow { 
        get { 
            if (SelectedNumber == null || !SelectedNumber.HasValue)
                return null;

		    int row = SelectedNumber.Value / 9;
            return (row / 3) * 3 + (row / 3) + 1; 
        } 
    }

    // Calculated properties based on SelectedNumber
    public int? SelectedBoxColumn { 
        get {         
            if (SelectedNumber == null || !SelectedNumber.HasValue)
                return null;
            
    		int col = SelectedNumber.Value % 9;
            return (col / 3) * 3 + col / 3 + 1; 
        }
    }
	
    // Index of the selected cell
	int? selectedNumber;
	public int? SelectedNumber
    {
        get => selectedNumber;
        set
        {
            if (selectedNumber != value)
            {
                selectedNumber = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedRow));
                OnPropertyChanged(nameof(SelectedColumn));
                OnPropertyChanged(nameof(SelectedBoxRow));
                OnPropertyChanged(nameof(SelectedBoxColumn));
            }
        }
    }

    // Command to save the game
	public ICommand SaveGameCommand => new Command(() =>
    {
        // delete the old save file if it exists
		if (File.Exists(GameSavePath))
            File.Delete(GameSavePath);

        // generate a tuple with the sudoku array and the provided numbers
        SudokuBoard sudoku = new SudokuBoard(SudokuArray, providedNumbers);

        // serialize the tuple and save it to a file
        string json = JsonSerializer.Serialize(sudoku);
        File.WriteAllText(GameSavePath, json);
    });

    // Command to highlight and select a cell to be placed
	public ICommand SelectCellCommand => new Command<string>((cellNumber) =>
	{
		Int32.TryParse(cellNumber, out int cellNumberInt);
        
        // check to see if the number can be changed
        if (providedNumbers.Contains(cellNumberInt))
        {
            var toast = Toast.Make("You can't change the numbers that you didn't place", ToastDuration.Short);
            toast.Show();
            return;
        }
        
        // set the selected number to the cell number
		SelectedNumber = cellNumberInt;

        // check if the option to hide used numbers is enabled
        if (Preferences.Default.Get("HideUsedNumbers",true))
        {
            for (int i = 0; i < 9; i++)
            {
                AvailableNumbers[i] = IsValidNumber(i+1, cellNumberInt);
            }

		    OnPropertyChanged(nameof(AvailableNumbers));
        }
	});
	
    // place the selected number in the selected cell
	public ICommand PlaceNumberCommand => new Command<string>((cellNumber) =>
	{
		if (SelectedNumber == null || !SelectedNumber.HasValue)
			return;

		Int32.TryParse(cellNumber, out int cellNumberInt);

        if (!IsValidNumber(cellNumberInt, SelectedNumber.Value))
        {
            var toast = Toast.Make("You can't repeat a number", ToastDuration.Short);
            toast.Show();

            return;
        }

		SudokuArray[SelectedNumber.Value] = cellNumberInt;

		OnPropertyChanged(nameof(SudokuArray));

        SelectedNumber = null;
	});

    // check to see if the selected number can be placed in the selected cell
    public bool IsValidNumber(int number, int cellIndex) {
        
		int row = cellIndex / 9;
		int col = cellIndex % 9;
        int boxRow = row / 3;
        int boxCol = col / 3;

        for (int r = 0; r < 9; r++) {
            int valueIndex = row * 9 + r;

            if (SudokuArray[valueIndex] == number)
                return false;
        }

        for (int c = 0; c < 9; c++) {
            int valueIndex = c * 9 + col;

            if (SudokuArray[valueIndex] == number)
                return false;
        }

        return true;
    }

}
