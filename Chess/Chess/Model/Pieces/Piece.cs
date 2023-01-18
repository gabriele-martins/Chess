namespace Chess.Model.Pieces;

public class Piece
{
    #region Properties
    public int Row { get; set; }
    public int Column { get; set; }
    public int Moves { get; set; }
    public string Color { get; set; }
    public string Symbol { get; set; }
    #endregion

    #region Constructors
    public Piece(int row, int column, string color)
    {
        Row = row;
        Column = column;
        Color = color;
        Symbol = " ";
        Moves = 0;
    }
    #endregion

    #region Methods
    public virtual bool IsPossibleMovement(int destinatedRow, int destinatedColumn, Board b)
    {
        return false;
    }

    public override string ToString()
    {
        return Symbol;
    }
    #endregion
}
