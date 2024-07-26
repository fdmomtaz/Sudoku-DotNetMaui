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

    
    public GameViewModel()
    {
        Tuple<int?[], int[]>? generatedSudoku = SudokuGenerator.GetBoard(SudokuGenerator.Difficulty.Medium);

        SudokuArray = generatedSudoku.Item1;
        providedNumbers = generatedSudoku.Item2;
    }
    
    int[] providedNumbers = new int[0];

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

	public int? SelectedRow
    {
        get {
            if (SelectedNumber == null || !SelectedNumber.HasValue)
                return null;

            int row = SelectedNumber.Value / 9;
            return row + 1 + row / 3;
        }
    }

	public int? SelectedColumn
    {
        get  { 
            if (SelectedNumber == null || !SelectedNumber.HasValue)
                return null;

            int col = SelectedNumber.Value % 9;
            return col + 1 + col / 3; 
        }
    }

    public int? SelectedBoxRow { 
        get { 
            if (SelectedNumber == null || !SelectedNumber.HasValue)
                return null;

		    int row = SelectedNumber.Value / 9;
            return (row / 3) * 3 + (row / 3) + 1; 
        } 
    }

    public int? SelectedBoxColumn { 
        get {         
            if (SelectedNumber == null || !SelectedNumber.HasValue)
                return null;
            
    		int col = SelectedNumber.Value % 9;
            return (col / 3) * 3 + col / 3 + 1; 
        }
    }
	
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

	public ICommand SaveGame => new Command(() =>
    {
		string cacheDir = FileSystem.Current.CacheDirectory;
        string filePath = Path.Combine(cacheDir, "sudoku.json");

		if (File.Exists(filePath))
            File.Delete(filePath);

        string json = JsonSerializer.Serialize(SudokuArray);
        File.WriteAllText(filePath, json);
    });

	public ICommand SolveCommand => new Command<string>((cellNumber) =>
	{
		Int32.TryParse(cellNumber, out int cellNumberInt);
        
        if (providedNumbers.Contains(cellNumberInt))
        {
            var toast = Toast.Make("You can't change the numbers that you didn't place", ToastDuration.Short);
            toast.Show();
            return;
        }
        
		SelectedNumber = cellNumberInt;

		int row = SelectedNumber.Value / 9;
		int col = SelectedNumber.Value % 9;

        if (Preferences.Default.Get("HideUsedNumbers",true))
        {
            bool[] tmpArray = new bool[9];
            for (int i = 0; i < 9; i++)
            {
                tmpArray[i] = IsValidNumber(i+1, cellNumberInt);
            }

		    AvailableNumbers = tmpArray;
        }
	});

	
	public ICommand PlaceNumber => new Command<string>((cellNumber) =>
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

    public bool IsValidNumber(int number, int index) {
        
		int row = index / 9;
		int col = index % 9;
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
