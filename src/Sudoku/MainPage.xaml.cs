using System.Windows.Input;

namespace Sudoku;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();

		BindingContext = this;

	}

	int?[] sudokuArray = new int? [81] {
		5, 3, 0, 0, 7, 0, 0, 0, null,
		6, 0, 0, 1, 9, 5, 0, 0, null,
		0, 9, 8, 0, 0, 0, 0, 6, 0,
		8, 0, 0, 0, 6, 0, 0, 0, 3,
		4, 0, 0, 8, 0, 3, 0, 0, 1,
		7, 0, 0, 0, 2, 0, 0, 0, 6,
		0, 6, 0, 0, 0, 0, 2, 8, 0,
		0, 0, 0, 4, 1, 9, 0, 0, 5,
		0, 0, 0, 0, 8, 0, 0, 7, 9
	};

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

	int? selectedCellRow;
	public int? SelectedCellRow
    {
        get => selectedCellRow;
        set
        {
            if (selectedCellRow != value)
            {
                selectedCellRow = value;
                OnPropertyChanged();
            }
        }
    }

	int? selectedCellColumn;
	public int? SelectedCellColumn
    {
        get => selectedCellColumn;
        set
        {
            if (selectedCellColumn != value)
            {
                selectedCellColumn = value;
                OnPropertyChanged();
            }
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
            }
        }
    }


	public ICommand SolveCommand => new Command<string>((cellNumber) =>
	{
		SelectedNumber = Int32.Parse(cellNumber);

		int row = SelectedNumber.Value / 9;
		int col = SelectedNumber.Value % 9;

		SelectedCellRow = row + 1 + row / 3;
		SelectedCellColumn = col + 1 + col / 3;
	});

	
	public ICommand PlaceNumber => new Command<string>((cellNumber) =>
	{
		if (SelectedNumber == null || !SelectedNumber.HasValue)
			return;

		Int32.TryParse(cellNumber, out int cellNumberInt);

		SudokuArray[SelectedNumber.Value] = cellNumberInt;

		OnPropertyChanged(nameof(SudokuArray));
	});

    public bool IsValidNumber(int number, int index) {
        
		int row = index.Value / 9;
		int col = index.Value % 9;
        int box = index.value / 3;

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

