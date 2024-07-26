namespace Sudoku;

public class SudokuBoard
{
    public SudokuBoard(int?[] board, int[] permanentValues)
    {
        Board = board;
        PermanentValues = permanentValues;
    }
    
    public int?[] Board { get; set; }
    public int[] PermanentValues { get; set; }
}
