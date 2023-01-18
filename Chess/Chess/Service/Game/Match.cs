using Chess.Model;
using Chess.Model.Pieces;
using Chess.View;

namespace Chess.Service.Game;

public class Match : Rules
{
    public static void Play()
    {
        Board board = new Board();

        Piece piece, ghostPiece;

        int currentRow = 0, currentColumn = 0, destinatedRow = 0, destinatedColumn = 0;

        int turn = 0;

        int round = 0;

        bool checkMate = false;

        do
        {
            if (IsInCheckOrCheckMate(turn, board) == "Check Mate")
            {
                BoardService.ShowChessboard();
                Print.ShowCheckMateMessage();
                checkMate = true;
                break;
            }

            GetCurrentPosition(board, turn, ref currentRow, ref currentColumn);
            GetDestinatedPosition(board, turn, ref destinatedRow, ref destinatedColumn);

            piece = Board.Table[currentRow, currentColumn];
            ghostPiece = Board.Table[destinatedRow, destinatedColumn];

            if (piece.Symbol == "K")
            {
                if (WillBeInCheck(piece, board, destinatedRow, destinatedColumn))
                {
                    Print.ShowPuttingKingInCheckMessage();
                    continue;
                }
            }

            if (CheckValidMove(ref piece, destinatedRow, destinatedColumn, board))
            {
                piece.Row = destinatedRow;
                piece.Column = destinatedColumn;
                Board.Table[currentRow, currentColumn] = null;
                Board.Table[destinatedRow, destinatedColumn] = piece;
                turn++;
            }
            else
            {
                Print.ShowInvalidMovement();
                continue;
            }

            if (IsInCheckOrCheckMate(turn-1, board) == "Check")
            {
                Print.ShowKingInCheckMessage();
                piece.Row = currentRow;
                piece.Column = currentColumn;
                Board.Table[currentRow, currentColumn] = piece;
                Board.Table[destinatedRow, destinatedColumn] = ghostPiece;
                turn--;
                continue;
            }

        } while(!(CheckEndGame(board) || checkMate));
    }

    private static void GetCurrentPosition(Board b, int turn, ref int currentRow, ref int currentColumn)
    {
        string currentPosition;
        string color;
        string jogador;

        if (turn % 2 == 0)
        {
            color = "White";
            jogador = "Jogador 1 (Peças Brancas)";
        }
        else
        {
            color = "Black";
            jogador = "Jogador 2 (Peças Pretas)";
        }

        while (true)
        {
            BoardService.ShowChessboard();

            Console.WriteLine($"\n\n\tVez do {jogador}.");

            Console.Write("\n\tDigite, respectivamente, a linha e coluna atuais da peça que deseja mover: ");

            currentPosition = Console.ReadLine().ToLower();

            if (string.IsNullOrEmpty(currentPosition))
            {
                Print.ShowNullValue();
                continue;
            }
            else if (currentPosition.Length != 2 || !char.IsLetter(currentPosition[0]) || !char.IsDigit(currentPosition[1]))
            {
                Print.ShowInvalidValue();
                continue;
            }
            else if (!Board.Rows.Contains(Convert.ToString(currentPosition[1])) || !Board.Columns.Contains(Convert.ToString(currentPosition[0])))
            {
                Print.ShowNonExistentPosition();
                continue;
            }
            else if (Board.Table[Array.IndexOf(Board.Rows, Convert.ToString(currentPosition[1])), Array.IndexOf(Board.Columns, Convert.ToString(currentPosition[0]))] == null)
            {
                Print.ShowNoPieceHere();
                continue;
            }
            else if (Board.Table[Array.IndexOf(Board.Rows, Convert.ToString(currentPosition[1])), Array.IndexOf(Board.Columns, Convert.ToString(currentPosition[0]))].Color != color)
            {
                Print.ShowOpponetsPiece();
                continue;
            }
            else
            {
                currentRow = Array.IndexOf(Board.Rows, Convert.ToString(currentPosition[1]));
                currentColumn = Array.IndexOf(Board.Columns, Convert.ToString(currentPosition[0]));
                break;
            }
        }
    }

    private static void GetDestinatedPosition(Board b, int turn, ref int destinatedRow, ref int destinatedColumn)
    {
        string destinatedPosition;
        string jogador;

        if (turn % 2 == 0)
            jogador = "Jogador 1 (Peças Brancas)";
        else
            jogador = "Jogador 2 (Peças Pretas)";

        while (true)
        {
            BoardService.ShowChessboard();

            Console.WriteLine($"\n\n\tVez do {jogador}.");

            Console.Write("\n\tDigite, respectivamente, a linha e coluna para onde deseja mover a peça: ");

            destinatedPosition = Console.ReadLine().ToLower();

            if (string.IsNullOrEmpty(destinatedPosition))
            {
                Print.ShowNullValue();
                continue;
            }
            else if (destinatedPosition.Length != 2 || !char.IsDigit(destinatedPosition[1]) || !char.IsLetter(destinatedPosition[0]) || destinatedPosition.Length != 2)
            {
                Print.ShowInvalidValue();
                continue;
            }
            else if (!Board.Rows.Contains(Convert.ToString(destinatedPosition[1])) || !Board.Columns.Contains(Convert.ToString(destinatedPosition[0])))
            {
                Print.ShowNonExistentPosition();
                continue;
            }
            else
            {
                destinatedRow = Array.IndexOf(Board.Rows, Convert.ToString(destinatedPosition[1]));
                destinatedColumn = Array.IndexOf(Board.Columns, Convert.ToString(destinatedPosition[0]));
                break;
            }
        }
    }
}
