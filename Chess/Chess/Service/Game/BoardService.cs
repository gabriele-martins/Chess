using Chess.Model.Game;

namespace Chess.Service.Game;

public class BoardService : Board
{
    #region Methods
    public static void ShowChessboard()
    {
        Console.Clear();
        Console.Write("\n\t   ");
        for (int i = 0; i < 8; i++)
        {
            Console.Write($" {Columns[i]} ");
        }

        for (int i = 0; i < 8; i++)
        {
            Console.Write($"\n\t {Rows[i]} ");
            for (int j = 0; j < 8; j++)
            {
                if (i % 2 == 0)
                {
                    if (j % 2 == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        if (Table[i, j] == null) Console.Write(" ");
                        else if (Table[i, j].Color == "White")
                            Console.ForegroundColor = ConsoleColor.White;
                        else
                            Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($" {Table[i, j]} ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        if (Table[i, j] == null) Console.Write(" ");
                        else if (Table[i, j].Color == "White")
                            Console.ForegroundColor = ConsoleColor.White;
                        else
                            Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($" {Table[i, j]} ");
                        Console.ResetColor();
                    }

                }
                else
                {
                    if (j % 2 == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        if (Table[i, j] == null) Console.Write(" ");
                        else if (Table[i, j].Color == "White")
                            Console.ForegroundColor = ConsoleColor.White;
                        else
                            Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($" {Table[i, j]} ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        if (Table[i, j] == null) Console.Write(" ");
                        else if (Table[i, j].Color == "White")
                            Console.ForegroundColor = ConsoleColor.White;
                        else
                            Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($" {Table[i, j]} ");
                        Console.ResetColor();
                    }
                }
            }
        }
    }
    #endregion
}
