using Chess.Model.Game;
using Chess.Model.Game.Pieces;
using Chess.Model.Players;
using Chess.View;

namespace Chess.Service.Game;

public class Rules
{
    #region Methods
    protected static bool CheckValidMove(ref Piece piece, int destinatedRow, int destinatedColumn, Board b)
    {
        if (piece.Symbol == "P" && CheckEnPassant(piece, destinatedRow, destinatedColumn))
        {
            return true;
        }
        else if (piece.Symbol == "K" && CheckCastling(piece, destinatedRow, destinatedColumn, b))
        {
            return true;
        }

        if (piece.IsPossibleMovement(destinatedRow, destinatedColumn, b))
        {
            if (piece.Symbol == "P" && (destinatedRow == 0 || destinatedRow == 7))
            {
                piece = CheckPawnPromotion(piece, destinatedRow, destinatedColumn);
            }
            return true;
        }
        else return false;
    }

    protected static Piece CheckPawnPromotion(Piece piece, int destinatedRow, int destinatedColumn)
    {
        int promotionRow;
        string symbol, color;

        if (piece.Color == "White")
        {
            promotionRow = 0;
            color = "White";
        }
        else
        {
            promotionRow = 7;
            color = "Black";
        }

        if (destinatedRow == promotionRow)
        {
            while (true)
            {
                Print.ShowPawnPromotionMenu();
                symbol = Console.ReadLine();
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
        if(piece.Color == "White")
        {
            if (piece.Row == 3 && destinatedRow == 2)
            {
                if (Math.Abs(piece.Column - destinatedColumn) == 1 && Board.Table[3,destinatedColumn] != null && Board.Table[3, destinatedColumn].Color != piece.Color)
                {
                    Board.Table[3, destinatedColumn] = null;
                    return true;
                }
            }
        }
        else
        {
            if (piece.Row == 4 && destinatedRow == 5)
            {
                if (Math.Abs(piece.Column - destinatedColumn) == 1 && Board.Table[4, destinatedColumn] != null && Board.Table[4, destinatedColumn].Color != piece.Color)
                {
                    Board.Table[4, destinatedColumn] = null;
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
            if (WillBeInCheck(piece, b, destinatedRow, destinatedColumn))
                return false;

            if (piece.Moves == 0)
            {
                if (destinatedColumn == 6 && Board.Table[destinatedRow, 7] != null && Board.Table[destinatedRow, 7].Moves == 0)
                {
                    if (Board.Table[destinatedRow, 5] == null && Board.Table[destinatedRow, 6] == null)
                    {
                        Board.Table[destinatedRow, 5] = Board.Table[destinatedRow, 7];
                        Board.Table[destinatedRow, 7] = null;
                        Board.Table[destinatedRow, 5].Row = destinatedRow;
                        Board.Table[destinatedRow, 5].Column = 5;
                        Board.Table[destinatedRow, 5].Moves++;

                        return true;
                    }
                }
                if (destinatedColumn == 2 && Board.Table[destinatedRow, 0] != null && Board.Table[destinatedRow, 0].Moves == 0)
                {
                    if (Board.Table[destinatedRow, 1] == null && Board.Table[destinatedRow, 2] == null && Board.Table[destinatedRow, 3] == null)
                    {
                        Board.Table[destinatedRow, 3] = Board.Table[destinatedRow, 0];
                        Board.Table[destinatedRow, 0] = null;
                        Board.Table[destinatedRow, 3].Row = destinatedRow;
                        Board.Table[destinatedRow, 3].Column = 3;
                        Board.Table[destinatedRow, 3].Moves++;

                        return true;
                    }
                }
            }
        }
        return false;
    }

    protected static bool WillBeInCheck(Piece piece, Board b, int destinatedRow, int destinatedColumn)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Board.Table[i, j] != null && Board.Table[i, j].Color != piece.Color && Board.Table[i, j].IsPossibleMovement(destinatedRow, destinatedColumn, b))
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected static string IsInCheckOrCheckMate(Player player, Board b)
    {
        string adversaryColor;
        string result="";
        int kingRow = 0, kingColumn = 0;
        int possiblePositions = 0, possibleCaptures = 0; 

        if (player.Color == "White")
            adversaryColor = "Black";
        else
            adversaryColor = "White";

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Board.Table[i, j] != null && Board.Table[i, j].Symbol == "K" && Board.Table[i, j].Color != adversaryColor)
                {
                    kingRow = i;
                    kingColumn = j;
                    i = 8;
                    break;
                }
            }
        }

        int[,] kingsSurroudings = new int[8,2] { {kingRow+1, kingColumn}, {kingRow-1, kingColumn}, {kingRow,kingColumn+1}, {kingRow,kingColumn-1}, {kingRow+1,kingColumn+1}, {kingRow+1,kingColumn-1}, {kingRow-1,kingColumn+1}, {kingRow-1,kingColumn-1} };

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Board.Table[i, j] != null && Board.Table[i, j].Color == adversaryColor && Board.Table[i, j].IsPossibleMovement(kingRow, kingColumn, b))
                {
                    result = "Check";
                    i = 8;
                    break;
                }
            }
        }

        for (int k = 0; k < 8; k++)
        {
            if (kingsSurroudings[k, 0] != 8 && kingsSurroudings[k, 1] != 8 && kingsSurroudings[k, 0] != -1 && kingsSurroudings[k, 1] != -1)
            {
                if (Board.Table[kingsSurroudings[k, 0], kingsSurroudings[k, 1]]==null)
                {
                    possiblePositions++;
                }
                else if (Board.Table[kingsSurroudings[k, 0], kingsSurroudings[k, 1]].Color == adversaryColor)
                {
                    possiblePositions++;
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

        for (int k=0; k<8; k++)
        {
            if (kingsSurroudings[k, 0] != -2 && kingsSurroudings[k, 1] != -2)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (Board.Table[i, j] != null && Board.Table[i, j].Color == adversaryColor && Board.Table[i, j].IsPossibleMovement(kingsSurroudings[k, 0], kingsSurroudings[k, 1], b))
                        {
                            possibleCaptures++;
                        }
                    }
                }
            }
            else
            {
                continue;
            }
        }

        if (result == "Check" && possiblePositions <= possibleCaptures) return "Check Mate";
        else if (result == "Check") return result;
        else return "NoCheck";
    }
    #endregion
}
