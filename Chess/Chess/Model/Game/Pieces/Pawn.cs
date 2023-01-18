namespace Chess.Model.Game.Pieces;

public class Pawn : Piece
{
    #region Constructor
    public Pawn(int row, int column, string color) : base(row, column, color)
    {
        Symbol = "P";
    }
    #endregion

    #region Methods
    public override bool IsPossibleMovement(int destinatedRow, int destinatedColumn, Board b)
    {
        switch (Color)
        {
            case "White":
                if (Moves != 0)
                {
                    if (destinatedRow == Row - 1)
                    {
                        if (destinatedColumn == Column && Board.Table[destinatedRow, destinatedColumn] == null)
                        {
                            return true;
                        }
                        else if ((destinatedColumn == Column - 1 || destinatedColumn == Column + 1) && Board.Table[destinatedRow, destinatedColumn] != null)
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (destinatedRow == Row - 1)
                    {
                        if (destinatedColumn == Column && Board.Table[destinatedRow, destinatedColumn] == null)
                        {
                            return true;
                        }
                        else if ((destinatedColumn == Column - 1 || destinatedColumn == Column + 1) && Board.Table[destinatedRow, destinatedColumn] != null)
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                    else if (destinatedRow == Row - 2)
                    {
                        if (destinatedColumn == Column && Board.Table[destinatedRow, destinatedColumn] == null && Board.Table[Row - 1, destinatedColumn] == null)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            case "Black":
                if (Moves != 0)
                {
                    if (destinatedRow == Row + 1)
                    {
                        if (destinatedColumn == Column && Board.Table[destinatedRow, destinatedColumn] == null)
                        {
                            return true;
                        }
                        else if ((destinatedColumn == Column - 1 || destinatedColumn == Column + 1) && Board.Table[destinatedRow, destinatedColumn] != null)
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (destinatedRow == Row + 1)
                    {
                        if (destinatedColumn == Column && Board.Table[destinatedRow, destinatedColumn] == null)
                        {
                            return true;
                        }
                        else if ((destinatedColumn == Column - 1 || destinatedColumn == Column + 1) && Board.Table[destinatedRow, destinatedColumn] != null)
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                    else if (destinatedRow == Row + 2)
                    {
                        if (destinatedColumn == Column && Board.Table[destinatedRow, destinatedColumn] == null && Board.Table[Row + 1, destinatedColumn] == null)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            default:
                return false;
        }
    }
    #endregion
}
