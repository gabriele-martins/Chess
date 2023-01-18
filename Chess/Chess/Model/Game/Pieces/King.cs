namespace Chess.Model.Game.Pieces;

public class King : Piece
{
    #region Constructor
    public King(int row, int column, string color) : base(row, column, color)
    {
        Symbol = "K";
    }
    #endregion

    #region Methods
    public override bool IsPossibleMovement(int destinatedRow, int destinatedColumn, Board b)
    {
        if (Math.Abs(Row - destinatedRow) == 1 || Math.Abs(Row - destinatedRow) == 0)
        {
            if (Math.Abs(Column - destinatedColumn) == 1 || Math.Abs(Column - destinatedColumn) == 0)
            {
                if (Board.Table[destinatedRow, destinatedColumn] == null)
                {
                    return true;
                }
                else if (Board.Table[destinatedRow, destinatedColumn].Color != Color)
                {
                    return true;
                }
            }
        }

        return false;
    }
    #endregion
}
