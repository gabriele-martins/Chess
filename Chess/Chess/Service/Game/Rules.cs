using Chess.Model.Enum;
using Chess.Model.Game;
using Chess.Model.Game.Pieces;
using Chess.View;

namespace Chess.Service.Game;

public class Rules
{
    #region Methods
    protected static bool CheckValidMove(ref Piece piece, int destinatedRow, int destinatedColumn, Board b)
    {
        if (piece.Symbol == Symbol.P && CheckEnPassant(piece, destinatedRow, destinatedColumn))
        {
            return true;
        }
        else if (piece.Symbol == Symbol.K && CheckCastling(piece, destinatedRow, destinatedColumn, b))
        {
            return true;
        }

        if (piece.IsPossibleMovement(destinatedRow, destinatedColumn, b))
        {
            if (piece.Symbol == Symbol.P && (destinatedRow == 0 || destinatedRow == 7))
            {
                piece = CheckPawnPromotion(piece, destinatedRow, destinatedColumn);
            }
            return true;
        }
        else return false;
    }

    protected static Piece CheckPawnPromotion(Piece piece, int destinatedRow, int destinatedColumn)
    {
        int promotionRow = 7;
        string symbol;
        Color color = Color.Black;

        if (piece.Color == Color.White)
        {
            promotionRow = 0;
            color = Color.White;
        }

        if (destinatedRow == promotionRow)
        {
            while (true)
            {
                Print.ShowPawnPromotionMenu();
                symbol = Console.ReadLine().ToUpperInvariant();
                if (symbol == "Q" || symbol == "R" || symbol == "B" || symbol == "N")
                    break;
                else
                    Print.ShowInvalidOption();
            }

            if (symbol == "Q")
                piece = new Queen(destinatedRow, destinatedColumn, color);
            else if (symbol == "R")
                piece = new Rook(destinatedRow, destinatedColumn, color);
            else if (symbol == "B")
                piece = new Bishop(destinatedRow, destinatedColumn, color);
            else
                piece = new Knight(destinatedRow, destinatedColumn, color);
        }

        return piece;
    }

    protected static bool CheckEnPassant(Piece piece, int destinatedRow, int destinatedColumn)
    {
        if(piece.Color == Color.White)
        {
            if (piece.Row == 3 && destinatedRow == 2)
            {
                if (Math.Abs(piece.Column - destinatedColumn) == 1 && Board.BoardTable[3,destinatedColumn].Symbol != Symbol.Empty && Board.BoardTable[3, destinatedColumn].Color != piece.Color)
                {
                    Board.BoardTable[3, destinatedColumn] = new Piece(3, destinatedColumn);
                    return true;
                }
            }
        }
        else
        {
            if (piece.Row == 4 && destinatedRow == 5)
            {
                if (Math.Abs(piece.Column - destinatedColumn) == 1 && Board.BoardTable[4, destinatedColumn].Symbol != Symbol.Empty && Board.BoardTable[4, destinatedColumn].Color != piece.Color)
                {
                    Board.BoardTable[4, destinatedColumn] = new Piece(4, destinatedColumn);
                    return true;
                }
            }
        }
        return false;
    }

    protected static bool CheckCastling(Piece piece, int destinatedRow, int destinatedColumn, Board b)
    {
        if (destinatedRow == piece.Row && destinatedColumn != piece.Column)
        {
            if (KingIsInCheck(piece.Color, b))
                return false;
            else if (KingWillBeInCheck(piece.Color, b, destinatedRow, destinatedColumn))
                return false;

            if (piece.Moves == 0)
            {
                if (destinatedColumn == 6 && Board.BoardTable[destinatedRow, 7].Symbol == Symbol.R && Board.BoardTable[destinatedRow, 7].Moves == 0)
                {
                    if (Board.BoardTable[destinatedRow, 5].Symbol == Symbol.Empty && Board.BoardTable[destinatedRow, 6].Symbol == Symbol.Empty)
                    {
                        Board.BoardTable[destinatedRow, 5] = Board.BoardTable[destinatedRow, 7];
                        Board.BoardTable[destinatedRow, 7] = new Piece(destinatedRow, 7);
                        Board.BoardTable[destinatedRow, 5].Row = destinatedRow;
                        Board.BoardTable[destinatedRow, 5].Column = 5;
                        Board.BoardTable[destinatedRow, 5].Moves++;

                        return true;
                    }
                }
                if (destinatedColumn == 2 && Board.BoardTable[destinatedRow, 0].Symbol == Symbol.R && Board.BoardTable[destinatedRow, 0].Moves == 0)
                {
                    if (Board.BoardTable[destinatedRow, 1].Symbol == Symbol.Empty && Board.BoardTable[destinatedRow, 2].Symbol == Symbol.Empty && Board.BoardTable[destinatedRow, 3].Symbol == Symbol.Empty)
                    {
                        Board.BoardTable[destinatedRow, 3] = Board.BoardTable[destinatedRow, 0];
                        Board.BoardTable[destinatedRow, 0] = new Piece(destinatedRow, 0);
                        Board.BoardTable[destinatedRow, 3].Row = destinatedRow;
                        Board.BoardTable[destinatedRow, 3].Column = 3;
                        Board.BoardTable[destinatedRow, 3].Moves++;

                        return true;
                    }
                }
            }
        }
        return false;
    }

    //protected static void CheckIfPieceProtectsKing(Color color, Board b, int row, int column)
    //{
    //    for (int i = 0; i < 8; i++)
    //    {
    //        for (int j = 0; j < 8; j++)
    //        {
    //            if (Board.BoardTable[i, j].Symbol != Symbol.Empty && Board.BoardTable[i, j].Symbol != Symbol.N && Board.BoardTable[i, j].Color != color && Board.BoardTable[i, j].IsPossibleMovement(row, column, b))
    //            {
    //                Piece adversaryPiece = Board.BoardTable[i, j];
    //                if (adversaryPiece.Row<row && adversaryPiece.Column < column)
    //                {
    //                    for (int r = 0; r < 8; r++)
    //                    {
    //                        for (int c = 0; c < 8; c++)
    //                        {
    //                            if (Board.BoardTable[i, j].Symbol != Symbol.Empty && Board.BoardTable[i, j].Color == color && Board.BoardTable[i, j].IsPossibleMovement(row, column, b))
    //                            {

    //                            }
    //                        }
    //                    }
    //                }
    //                //for (int r = 0; r < 8; r++)
    //                //{
    //                //    for (int c = 0; c < 8; c++)
    //                //    {
    //                //        if (Board.BoardTable[i, j].Symbol != Symbol.Empty && Board.BoardTable[i, j].Color == color && Board.BoardTable[i, j].IsPossibleMovement(row, column, b))
    //                //        {

    //                //        }
    //                //    }
    //                //}
    //            }
    //        }
    //    }
    //}

    protected static bool KingIsInCheck(Color color, Board b)
    {
        int kingRow = Board.BlackKingsPosition[0], kingColumn = Board.BlackKingsPosition[1];

        if (color == Color.White)
        {
            kingRow = Board.WhiteKingsPosition[0];
            kingColumn = Board.WhiteKingsPosition[1];
        }

        if (KingWillBeInCheck(color, b, kingRow, kingColumn))
        {
            return true;
        }

        return false;
    }

    protected static bool KingWillBeInCheck(Color color, Board b, int row, int column)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Board.BoardTable[i, j].Symbol != Symbol.Empty && Board.BoardTable[i, j].Color != color && Board.BoardTable[i, j].IsPossibleMovement(row, column, b))
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected static bool KingIsInCheckMate(Color color, Board b)
    {
        Color adversaryColor = Color.White;
        int kingRow = Board.BlackKingsPosition[0], kingColumn = Board.BlackKingsPosition[1];
        int possiblePositions = 0, possibleCaptures = 0;
        bool check;

        if (color == Color.White)
        {
            adversaryColor = Color.Black;
            kingRow = Board.WhiteKingsPosition[0];
            kingColumn = Board.WhiteKingsPosition[1];
        }

        check = KingIsInCheck(color, b);

        int[,] kingsSurroudings = new int[8,2] { {kingRow+1, kingColumn}, {kingRow-1, kingColumn}, {kingRow, kingColumn+1}, {kingRow, kingColumn-1}, {kingRow+1, kingColumn+1}, {kingRow+1, kingColumn-1}, {kingRow-1, kingColumn+1}, {kingRow-1, kingColumn-1} };

        for (int k = 0; k < 8; k++)
        {
            if (kingsSurroudings[k, 0] != 8 && kingsSurroudings[k, 1] != 8 && kingsSurroudings[k, 0] != -1 && kingsSurroudings[k, 1] != -1)
            {
                if (Board.BoardTable[kingsSurroudings[k, 0], kingsSurroudings[k, 1]].Symbol == Symbol.Empty)
                {
                    possiblePositions++;
                    if (KingWillBeInCheck(color, b, kingsSurroudings[k, 0], kingsSurroudings[k, 1]))
                        possibleCaptures++;
                }
                else if (Board.BoardTable[kingsSurroudings[k, 0], kingsSurroudings[k, 1]].Color == adversaryColor)
                {
                    possiblePositions++;
                    if (KingWillBeInCheck(color, b, kingsSurroudings[k, 0], kingsSurroudings[k, 1]))
                        possibleCaptures++;
                }
                else
                {
                    kingsSurroudings[k, 0] = -2;
                    kingsSurroudings[k, 1] = -2;
                }
            }
            else
            {
                kingsSurroudings[k,0] = -2;
                kingsSurroudings[k,1] = -2;
            }
        }

        if (check && possiblePositions == possibleCaptures) return true;
        else return false;
    }
    #endregion
}
