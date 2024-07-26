namespace Sudoku;

public static class SudokuGenerator
{
    public enum Difficulty
    {
        Easy = 40,
        Medium = 30,
        Hard = 20
    }

    // algorithm to generate a new sudoku board
    // based on: https://stackoverflow.com/questions/6924216/how-to-generate-sudoku-boards-with-unique-solutions#answer-61442050
    public static int[,] Generate()
    {    
        int[,] board = new int[9, 9] {
                {1,2,3,  4,5,6,  7,8,9},
                {4,5,6,  7,8,9,  1,2,3},
                {7,8,9,  1,2,3,  4,5,6},

                {2,3,1,  5,6,4,  8,9,7},
                {5,6,4,  8,9,7,  2,3,1},
                {8,9,7,  2,3,1,  5,6,4},

                {3,1,2,  6,4,5,  9,7,8},
                {6,4,5,  9,7,8,  3,1,2},
                {9,7,8,  3,1,2,  6,4,5}
        };


        shuffleNumbers(ref board);
        shuffleRows(ref board);
        shuffleCols(ref board);

        return board;
    }

    private static void shuffleNumbers(ref int [,] board) {
        Random rand = new Random();
        for (int i = 0; i < 9; i++) {
            int ranNum = rand.Next(9);
            swapNumbers(ref board, i, ranNum);
        }
    }

    private static  void swapNumbers(ref int[,] board, int n1, int n2) {
        for (int y = 0; y<9; y++) {
            for (int x = 0; x<9; x++) {
                if (board[x,y] == n1) {
                    board[x,y] = n2;
                } else if (board[x,y] == n2) {
                    board[x,y] = n1;
                }
            }
        }
    }

    private static  void shuffleRows(ref int [,] board) {
        int blockNumber;

        Random rand = new Random();
        for (int i = 0; i < 9; i++) {
            int ranNum = rand.Next(3);
            blockNumber = i / 3;
            swapRows(ref board, i, blockNumber * 3 + ranNum);
        }
    }

    static void swapRows(ref int [,] board, int r1, int r2) {
        int rowVal;
        for (int i = 0; i < 9; i++){
            rowVal = board[r1,i];
            board[r1,i] = board[r2,i];
            board[r2,i] = rowVal;
        }
    }

    static void shuffleCols(ref int [,] board) {
        int blockNumber;

        Random rand = new Random();
        for (int i = 0; i < 9; i++) {
            int ranNum = rand.Next(3);
            blockNumber = i / 3;
            swapCols(ref board, i, blockNumber * 3 + ranNum);
        }
    }
    static void swapCols(ref int [,] board, int c1, int c2) {
        int colVal;
        for (int i = 0; i < 9; i++){
            colVal = board[i,c1];
            board[i,c1] = board[i,c2];
            board[i,c2] = colVal;
        }
    }

    public static Tuple<int?[], int[]> GetBoard(Difficulty difficulty)
    {
        int?[] board = new int?[81];
        int[] providedNumbers = new int[(int)difficulty];

        // generate a new board
        int[,] solvedBoard = Generate();

        // copy the board to a 1D array
        var rand = new Random();
        for (int i = 0; i < (int)difficulty; i++)
        {
            int copyIndex = rand.Next(0, 81);

            board[copyIndex] = solvedBoard[copyIndex / 9, copyIndex % 9];
            providedNumbers[i] = copyIndex;
        }

        return new Tuple<int?[], int[]>(board, providedNumbers);
    }


}
