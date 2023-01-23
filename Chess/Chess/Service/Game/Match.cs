using Chess.Controller.Players;
using Chess.Model.Enum;
using Chess.Model.Game;
using Chess.Model.Game.Pieces;
using Chess.Model.Players;
using Chess.View;
using System.Numerics;

namespace Chess.Service.Game;

public class Match : Rules
{
    #region Attributes
    private static Player _playerWhite;
    private static Player _playerBlack;
    private static Player _currentPlayer;
    private static Player _winnerPlayer;
    private static Board _board;
    private static Piece _piece;
    private static Piece _pieceTaken;
    private static int _currentRow;
    private static int _currentColumn;
    private static int _destinatedRow;
    private static int _destinatedColumn;
    private static string _move;
    private static bool _check;
    private static bool _turn;
    public static int round;
    #endregion

    #region Methods
    public static void Play()
    {
        GetPlayers();

        _winnerPlayer = new Player();

        _board = new Board();

        round = 0;

        _move = string.Empty;

        _check = true; 
        _turn = true;

        do
        {
            ChangePlayer();

            if (_move == "empate")
            {
                GetMove();
                if(_move == "sim")
                {
                    BoardService.ShowChessboard();
                    Print.ShowTieMessage();
                    break;
                }
                else
                {
                    _turn = !_turn;
                    continue;
                }
            }

            if (KingIsInCheckMate(_currentPlayer.Color, _board))
            {
                if (_currentPlayer.Color == Color.White)
                    _winnerPlayer = _playerBlack;
                else
                    _winnerPlayer = _playerWhite;

                BoardService.ShowChessboard();
                Print.ShowCheckMateMessage(_winnerPlayer);
                break;
            }

            GetMove();

            if (_move == "empate")
            {
                _turn = !_turn;
                continue;
            }
            else if (_move == "render")
            {
                if (_currentPlayer.Color == Color.White)
                    _winnerPlayer = _playerBlack;
                else
                    _winnerPlayer = _playerWhite;

                BoardService.ShowChessboard();
                Print.ShowSurrenderMessage(_winnerPlayer);
                break;
            }

            GetDestinatedPosition();

            _piece = Board.BoardTable[_currentRow, _currentColumn];

            if (_piece.Symbol == Symbol.K)
            {
                if (KingWillBeInCheck(_currentPlayer.Color, _board, _destinatedRow, _destinatedColumn))
                {
                    Print.ShowPuttingKingInCheckMessage();
                    continue;
                }
            }
            else if (!KingIsInCheck(_currentPlayer.Color, _board))
            {
                _check = false;
            }
            
            if (CheckValidMove(ref _piece, _destinatedRow, _destinatedColumn, _board))
            {
                MovePiece();
            }
            else
            {
                Print.ShowInvalidMovement();
                continue;
            }

            if (KingIsInCheck(_currentPlayer.Color, _board))
            {
                UndoMovePiece();
                continue;
            }

            _check = true;

        } while (true);

        UpdateScore();
    }

    private static Player AskForPlayer(string cor)
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

    private static void GetPlayers()
    {
        do
        {
            _playerWhite = AskForPlayer("Brancas");
            _playerWhite.Color = Color.White;

            _playerBlack = AskForPlayer("Pretas");
            _playerBlack.Color = Color.Black;

            if (_playerWhite.Nickname == _playerBlack.Nickname)
                Print.ShowCantPlayWithYourselfMessage();
            else
                break;
        } while (true);
    }

    private static void ChangePlayer()
    {
        if (_turn)
        {
            _currentPlayer = _playerWhite;
            round++;
        }
        else
            _currentPlayer = _playerBlack;
    }

    private static void GetMove()
    {
        string playerNick = $"{_currentPlayer.Nickname} (Peças Pretas)";

        if (_currentPlayer.Color == Color.White)
            playerNick = $"{_currentPlayer.Nickname} (Peças Brancas)";

        while (true)
        {
            BoardService.ShowChessboard();

            Console.WriteLine($"\n\n\tVez de {playerNick}.");

            if (_move == "empate")
            {
                Print.AskTie();
            }

            Console.Write("\n\tDigite sua jogada: ");

            _move = Console.ReadLine().ToLower();

            if (string.IsNullOrEmpty(_move))
            {
                Print.ShowNullValue();
                continue;
            }
            else if (_move == "empate" || _move == "render")
            {
                break;
            }
            else if (_move == "sim" || _move == "não")
            {
                break;
            }
            else if (_move.Length != 2 || !char.IsLetter(_move[0]) || !char.IsDigit(_move[1]))
            {
                Print.ShowInvalidValue();
                continue;
            }
            else if (!Board.Rows.Contains(Convert.ToString(_move[1])) || !Board.Columns.Contains(Convert.ToString(_move[0])))
            {
                Print.ShowNonExistentPosition();
                continue;
            }
            else if (Board.BoardTable[Array.IndexOf(Board.Rows, Convert.ToString(_move[1])), Array.IndexOf(Board.Columns, Convert.ToString(_move[0]))].Symbol == Symbol.Empty)
            {
                Print.ShowNoPieceHere();
                continue;
            }
            else if (Board.BoardTable[Array.IndexOf(Board.Rows, Convert.ToString(_move[1])), Array.IndexOf(Board.Columns, Convert.ToString(_move[0]))].Color != _currentPlayer.Color)
            {
                Print.ShowOpponetsPiece();
                continue;
            }
            else
            {
                _currentRow = Array.IndexOf(Board.Rows, Convert.ToString(_move[1]));
                _currentColumn = Array.IndexOf(Board.Columns, Convert.ToString(_move[0]));
                break;
            }
        }
    }

    private static void GetDestinatedPosition()
    {
        string destinatedPosition;
        string playerNick = $"{_currentPlayer.Nickname} (Peças Pretas)";

        if (_currentPlayer.Color == Color.White)
            playerNick = $"{_currentPlayer.Nickname} (Peças Brancas)";

        while (true)
        {
            BoardService.ShowChessboard();

            Console.WriteLine($"\n\n\tVez de {playerNick}.");

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
                _destinatedRow = Array.IndexOf(Board.Rows, Convert.ToString(destinatedPosition[1]));
                _destinatedColumn = Array.IndexOf(Board.Columns, Convert.ToString(destinatedPosition[0]));
                break;
            }
        }
    }

    private static void MovePiece()
    {
        if (_piece.Symbol == Symbol.K && _piece.Color == Color.White)
        {
            Board.WhiteKingsPosition[0] = _destinatedRow;
            Board.WhiteKingsPosition[1] = _destinatedColumn;
        }
        else if (_piece.Symbol == Symbol.K)
        {
            Board.BlackKingsPosition[0] = _destinatedRow;
            Board.BlackKingsPosition[1] = _destinatedColumn;
        }
        _piece.Row = _destinatedRow;
        _piece.Column = _destinatedColumn;
        _piece.Moves++;
        Board.BoardTable[_currentRow, _currentColumn] = new Piece(_currentRow, _currentColumn);
        _pieceTaken = Board.BoardTable[_destinatedRow, _destinatedColumn];
        Board.BoardTable[_destinatedRow, _destinatedColumn] = _piece;
        _turn = !_turn;
    }

    private static void UndoMovePiece()
    {
        if (_check == false)
        {
            Print.ShowPuttingKingInCheckMessage();
        }
        else
            Print.ShowKingInCheckMessage();
        _piece.Row = _currentRow;
        _piece.Column = _currentColumn;
        _piece.Moves--;
        Board.BoardTable[_currentRow, _currentColumn] = _piece;
        Board.BoardTable[_destinatedRow, _destinatedColumn] = _pieceTaken;
        _turn = !_turn;
    }

    private static void UpdateScore()
    {
        if(_move == "sim")
        {
            _playerWhite.Matches++;
            _playerWhite.Ties++;
            _playerWhite.Score++;

            _playerBlack.Matches++;
            _playerBlack.Ties++;
            _playerBlack.Score++;
        }
        else if (_move == "render")
        {
            _winnerPlayer.Matches++;
            _winnerPlayer.Wins++;
            _winnerPlayer.Score += 3;

            _currentPlayer.Matches++;
            _currentPlayer.Defeats++;
        }
        else
        {
            _winnerPlayer.Matches++;
            _winnerPlayer.Wins++;
            _winnerPlayer.Score += 3;

            _currentPlayer.Matches++;
            _currentPlayer.Defeats++;
        }
        PlayerController.UpdatePlayers();
    }
    #endregion
}
