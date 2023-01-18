using Chess.Model;
using Chess.Model.Pieces;
using Chess.View;

namespace Chess.Service.Game;

public class Rules
{
    protected static bool CheckValidMove(ref Piece piece, int destinatedRow, int destinatedColumn, Board b)
    {
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

    protected static string IsInCheckOrCheckMate(int turn, Board b)
    {
        string adversaryColor;
        string result="";
        int kingRow = 0, kingColumn = 0;
        int possiblePositions = 0, possibleCaptures = 0; 

        if (turn % 2 == 0)
        {
            adversaryColor = "Black";
        }
        else
        {
            adversaryColor = "White";
        }

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

    protected static bool CheckEndGame(Board b)
    {
        int countOfKings = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Board.Table[i, j] != null)
                {
                    if (Board.Table[i, j].Symbol == "K") countOfKings++;
                }
            }
        }
        if (countOfKings == 1) return true;
        else return false;
    }
}
