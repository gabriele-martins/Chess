using Chess.Controller.Players;
using Chess.Model.Game;
using Chess.Model.Game.Pieces;
using Chess.Model.Players;
using Chess.Service.Players;
using Chess.View;

namespace Chess.Service.Game;

public class Match : Rules
{
    #region Methods
    public static void Play()
    {
        Player playerWhite = GetPlayer("Brancas");
        playerWhite.Color = "White";
        Player playerBlack = GetPlayer("Pretas");
        playerBlack.Color = "Black";
        Player wildPlayer, winnerPlayer = new Player();

        Board board = new Board();

        Piece piece, ghostPiece;

        int currentRow = 0, currentColumn = 0, destinatedRow = 0, destinatedColumn = 0;

        int turn = 0;

        int round = 0;

        string move = "";

        do
        {
            if (turn % 2 == 0)
                wildPlayer = playerWhite;
            else
                wildPlayer = playerBlack;

            if (move == "empate")
            {
                GetMove(wildPlayer, ref currentRow, ref currentColumn, ref move);
                if(move == "sim")
                {
                    BoardService.ShowChessboard();
                    Print.ShowTieMessage();
                    break;
                }
                else
                {
                    turn++;
                    continue;
                }
            }

            if (IsInCheckOrCheckMate(wildPlayer, board) == "Check Mate")
            {
                if (wildPlayer.Color == "White")
                    winnerPlayer = playerBlack;
                else
                    winnerPlayer = playerWhite;

                BoardService.ShowChessboard();
                Print.ShowCheckMateMessage(winnerPlayer);
                break;
            }

            GetMove(wildPlayer, ref currentRow, ref currentColumn, ref move);

            if (move == "empate")
            {
                turn++;
                continue;
            }
            else if (move == "render")
            {
                if (wildPlayer.Color == "White")
                    winnerPlayer = playerBlack;
                else
                    winnerPlayer = playerWhite;

                BoardService.ShowChessboard();
                Print.ShowSurrenderMessage(winnerPlayer);
                break;
            }

            GetDestinatedPosition(wildPlayer, ref destinatedRow, ref destinatedColumn);

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
                piece.Moves++;
                Board.Table[currentRow, currentColumn] = null;
                Board.Table[destinatedRow, destinatedColumn] = piece;
                turn++;
            }
            else
            {
                Print.ShowInvalidMovement();
                continue;
            }

            if (IsInCheckOrCheckMate(wildPlayer, board) == "Check")
            {
                Print.ShowKingInCheckMessage();
                piece.Row = currentRow;
                piece.Column = currentColumn;
                Board.Table[currentRow, currentColumn] = piece;
                Board.Table[destinatedRow, destinatedColumn] = ghostPiece;
                turn--;
                continue;
            }

        } while (true);

        UpdateScore(move, playerWhite, playerBlack, wildPlayer, winnerPlayer);
    }
    private static Player GetPlayer(string cor)
    {
        string playerNick;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n\tDigite o nickname dos jogadores.");
            Console.Write($"\n\tJogador (Peças {cor}): ");
            playerNick = Console.ReadLine();
            if (PlayerController.ValidateNickname(playerNick))
            {
                return PlayerController.players.Find(player => player.Nickname == playerNick);
            }
            else
            {
                Print.ShowNicknameNotFoundMessage();
            }
        }
    }

    private static void GetMove(Player player, ref int currentRow, ref int currentColumn, ref string move)
    {
        string jogador;

        if (player.Color == "White")
            jogador = $"{player.Nickname} (Peças Brancas)";
        else
            jogador = $"{player.Nickname} (Peças Pretas)";

        while (true)
        {
            BoardService.ShowChessboard();

            Console.WriteLine($"\n\n\tVez de {jogador}.");

            if (move == "empate")
            {
                Print.AskTie();
            }

            Console.Write("\n\tDigite sua jogada: ");

            move = Console.ReadLine().ToLower();

            if (string.IsNullOrEmpty(move))
            {
                Print.ShowNullValue();
                continue;
            }
            else if (move == "empate" || move == "render")
            {
                break;
            }
            else if (move == "sim" || move == "não")
            {
                break;
            }
            else if (move.Length != 2 || !char.IsLetter(move[0]) || !char.IsDigit(move[1]))
            {
                Print.ShowInvalidValue();
                continue;
            }
            else if (!Board.Rows.Contains(Convert.ToString(move[1])) || !Board.Columns.Contains(Convert.ToString(move[0])))
            {
                Print.ShowNonExistentPosition();
                continue;
            }
            else if (Board.Table[Array.IndexOf(Board.Rows, Convert.ToString(move[1])), Array.IndexOf(Board.Columns, Convert.ToString(move[0]))] == null)
            {
                Print.ShowNoPieceHere();
                continue;
            }
            else if (Board.Table[Array.IndexOf(Board.Rows, Convert.ToString(move[1])), Array.IndexOf(Board.Columns, Convert.ToString(move[0]))].Color != player.Color)
            {
                Print.ShowOpponetsPiece();
                continue;
            }
            else
            {
                currentRow = Array.IndexOf(Board.Rows, Convert.ToString(move[1]));
                currentColumn = Array.IndexOf(Board.Columns, Convert.ToString(move[0]));
                break;
            }
        }
    }

    private static void GetDestinatedPosition(Player player, ref int destinatedRow, ref int destinatedColumn)
    {
        string destinatedPosition;
        string jogador;

        if (player.Color == "White")
            jogador = $"{player.Nickname} (Peças Brancas)";
        else
            jogador = $"{player.Nickname} (Peças Pretas)";

        while (true)
        {
            BoardService.ShowChessboard();

            Console.WriteLine($"\n\n\tVez do {jogador}.");

            Console.Write("\n\tDigite o destino: ");

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

    private static void UpdateScore(string move, Player playerWhite, Player playerBlack, Player wildPlayer, Player winnerPlayer)
    {
        if(move == "sim")
        {
            playerWhite.Matches++;
            playerWhite.Ties++;
            playerWhite.Score++;

            playerBlack.Matches++;
            playerBlack.Ties++;
            playerBlack.Score++;
        }
        else if (move == "render")
        {
            winnerPlayer.Matches++;
            winnerPlayer.Wins++;
            winnerPlayer.Score += 3;

            wildPlayer.Matches++;
            wildPlayer.Defeats++;
        }
        else
        {
            winnerPlayer.Matches++;
            winnerPlayer.Wins++;
            winnerPlayer.Score += 3;

            wildPlayer.Matches++;
            wildPlayer.Defeats++;
        }
        PlayerService.UpdatePlayers();
    }
    #endregion
}
