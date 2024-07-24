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
	

	public ICommand SolveCommand => new Command<string>((cellNumber) =>
	{
		Int32.TryParse(cellNumber, out int cellNumberInt);

		int row = cellNumberInt / 9;
		int col = cellNumberInt % 9;

		SelectedCellRow = row + 1 + row / 3;
		SelectedCellColumn = col + 1 + col / 3;

		OnPropertyChanged(nameof(SudokuArray));
	});
}

