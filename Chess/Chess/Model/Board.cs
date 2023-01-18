using Chess.Model.Pieces;

namespace Chess.Model;

public class Board
{
    #region Properties
    public static Piece?[,] Table { get; set; }
    public static string[] Rows { get; set; }
    public static string[] Columns { get; set; }
    #endregion

    #region Constructor
    public Board()
    {
        //Creating the rows and columns labels
        Rows = new string[8] { "8", "7", "6", "5", "4", "3", "2", "1" };
        Columns = new string[8] { "a", "b", "c", "d", "e", "f", "g", "h" };

        //Creating the Board with a 2D array
        Table = new Piece[8, 8];

        //Initial placement of Pawns on the Board
        for (int i = 0; i < 8; i++)
        {
            Table[1, i] = new Pawn(1, i, "Black");
            Table[6, i] = new Pawn(6, i, "White");
        }

        //Initial placement of Rooks on the Board
        Table[7, 0] = new Rook(7, 0, "White");
        Table[7, 7] = new Rook(7, 0, "White");
        Table[0, 0] = new Rook(0, 0, "Black");
        Table[0, 7] = new Rook(0, 7, "Black");

        //Initial placement of Knights on the Board
        Table[7, 1] = new Knight(7, 1, "White");
        Table[7, 6] = new Knight(7, 6, "White");
        Table[0, 1] = new Knight(0, 1, "Black");
        Table[0, 6] = new Knight(0, 6, "Black");

        //Initial placement of Bishops on the Board
        Table[7, 2] = new Bishop(7, 2, "White");
        Table[7, 5] = new Bishop(7, 5, "White");
        Table[0, 2] = new Bishop(0, 2, "Black");
        Table[0, 5] = new Bishop(0, 5, "Black");

        //Initial placement of Queens on the Board
        Table[7, 3] = new Queen(7, 3, "White");
        Table[0, 3] = new Queen(0, 3, "Black");

        //Initial placement of Kings on the Board
        Table[7, 4] = new King(7, 4, "White");
        Table[0, 4] = new King(0, 4, "Black");
    }
    #endregion
}