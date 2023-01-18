namespace Chess.Model.Game.Pieces;

public class Queen : Piece
{
    #region Constructor
    public Queen(int row, int column, string color) : base(row, column, color)
    {
        Symbol = "Q";
    }
    #endregion

    #region Methods
    public override bool IsPossibleMovement(int destinatedRow, int destinatedColumn, Board b)
    {
        Bishop bishop = new Bishop(Row, Column, "Whatever");
        Rook rook = new Rook(Row, Column, "Whatever");

        if (bishop.IsPossibleMovement(destinatedRow, destinatedColumn, b) || rook.IsPossibleMovement(destinatedRow, destinatedColumn, b))
        {
            return true;
        }
        else return false;
    }
    #endregion  
}
