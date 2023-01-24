using Chess.Controller.Players;
using Chess.Model.Enum;
using Chess.Model.Game;
using Chess.Model.Game.Pieces;
using Chess.Model.Players;
using Chess.Repository;
using Chess.View;

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
    private static int _round;
    private static string _destinatedPosition;
    private static string _move;
    private static string _movesTextPgn;
    private static string _textToRemove;
    private static string _textPgn;
    private static string _castling;
    private static string _promotion;
    private static bool _enPassant;
    private static bool _check;
    private static bool _turn;
    #endregion

    #region Methods
    public static void Play()
    {
        GetPlayers();

        _winnerPlayer = new Player();

        _board = new Board();

        _round = 0;

        _move = string.Empty;

        _movesTextPgn = string.Empty;

        _check = true; 

        _turn = true;

        _textPgn = string.Empty;

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
                    _movesTextPgn = "1/2-1/2";
                    WriteMovesTextPgn(_movesTextPgn);
                    break;
                }
                else
                {
                    _turn = !_turn;
                    _round--;
                    EraseMovesTextPgn("1/2-1/2");
                    continue;
                }
            }

            if (KingIsInCheckMate(_currentPlayer.Color, _board))
            {
                if (_currentPlayer.Color == Color.White)
                {
                    _winnerPlayer = _playerBlack;
                    _textPgn = _textPgn.Trim();
                    _movesTextPgn += "# 0-1";
                }
                else
                {
                    _winnerPlayer = _playerWhite;
                    _textPgn = _textPgn.Trim();
                    _movesTextPgn += "# 1-0";
                }

                BoardService.ShowChessboard();
                Print.ShowCheckMateMessage(_winnerPlayer);
                WriteMovesTextPgn(_movesTextPgn);
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
                {
                    _winnerPlayer = _playerBlack;
                    _movesTextPgn += "0-1";

                }
                else
                {
                    _winnerPlayer = _playerWhite;
                    _movesTextPgn += "1-0";
                }

                BoardService.ShowChessboard();
                Print.ShowSurrenderMessage(_winnerPlayer);
                WriteMovesTextPgn(_movesTextPgn);
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
            
            if (CheckValidMove(ref _piece, _destinatedRow, _destinatedColumn, _board, ref _enPassant, ref _castling, ref _promotion))
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
        if (_movesTextPgn.Contains("1-0"))
            Pgn.CreatePgnFile(_playerWhite.Nickname, _playerBlack.Nickname, "1-0",_textPgn);
        else if (_movesTextPgn.Contains("0-1"))
            Pgn.CreatePgnFile(_playerWhite.Nickname, _playerBlack.Nickname, "0-1", _textPgn);
        else
            Pgn.CreatePgnFile(_playerWhite.Nickname, _playerBlack.Nickname, "1/2-1/2", _textPgn);
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
            _round++;
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
        string playerNick = $"{_currentPlayer.Nickname} (Peças Pretas)";

        if (_currentPlayer.Color == Color.White)
            playerNick = $"{_currentPlayer.Nickname} (Peças Brancas)";

        while (true)
        {
            BoardService.ShowChessboard();

            Console.WriteLine($"\n\n\tVez de {playerNick}.");

            Console.Write("\n\tDigite o destino: ");

            _destinatedPosition = Console.ReadLine().ToLower();

            if (string.IsNullOrEmpty(_destinatedPosition))
            {
                Print.ShowNullValue();
                continue;
            }
            else if (_destinatedPosition.Length != 2 || !char.IsDigit(_destinatedPosition[1]) || !char.IsLetter(_destinatedPosition[0]) || _destinatedPosition.Length != 2)
            {
                Print.ShowInvalidValue();
                continue;
            }
            else if (!Board.Rows.Contains(Convert.ToString(_destinatedPosition[1])) || !Board.Columns.Contains(Convert.ToString(_destinatedPosition[0])))
            {
                Print.ShowNonExistentPosition();
                continue;
            }
            else
            {
                _destinatedRow = Array.IndexOf(Board.Rows, Convert.ToString(_destinatedPosition[1]));
                _destinatedColumn = Array.IndexOf(Board.Columns, Convert.ToString(_destinatedPosition[0]));
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

        _pieceTaken = Board.BoardTable[_destinatedRow, _destinatedColumn];

        if (_piece.Color == Color.White)
        {
            _movesTextPgn = _round.ToString() + ". ";
        }
        
        WriteMovesPiecePgn();
        WriteMovesTextPgn(_movesTextPgn);
        _textToRemove = _movesTextPgn;
        _movesTextPgn = string.Empty;

        _piece.Row = _destinatedRow;
        _piece.Column = _destinatedColumn;
        _piece.Moves++;

        if (_piece.Color == Color.White)
        {
            if (_piece.IsPossibleMovement(Board.BlackKingsPosition[0], Board.BlackKingsPosition[1], _board))
                _textPgn = _textPgn.Trim() + "+ ";
        }
        else
        {
            if (_piece.IsPossibleMovement(Board.WhiteKingsPosition[0], Board.WhiteKingsPosition[1], _board))
                _textPgn = _textPgn.Trim() + "+ ";
        }

        Board.BoardTable[_currentRow, _currentColumn] = new Piece(_currentRow, _currentColumn);
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

        EraseMovesTextPgn(_textToRemove);
        _textToRemove = string.Empty;

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

    private static void WriteMovesTextPgn(string text)
    {
        _textPgn += text;
    }

    private static void EraseMovesTextPgn(string text)
    {
        _textPgn = _textPgn.Replace(text,"");
    }

    private static void WriteMovesPiecePgn()
    {
        if (_promotion != string.Empty)
        {
            _movesTextPgn += _destinatedPosition + "=" + _promotion + " ";
            return;
        }

        switch (_piece.Symbol)
        {
            case Symbol.K:
                if (_pieceTaken.Symbol == Symbol.Empty)
                {
                    if (_castling == "Minor")
                        _movesTextPgn += "O-O ";
                    else if (_castling == "Major")
                        _movesTextPgn += "O-O-O ";
                    else
                        _movesTextPgn += "K" + _destinatedPosition + " ";
                }
                else
                    _movesTextPgn += "Kx" + _destinatedPosition + " ";
                    break;
            case Symbol.Q:
                if (_pieceTaken.Symbol == Symbol.Empty)
                    _movesTextPgn += "Q" + _destinatedPosition + " ";
                else
                    _movesTextPgn += "Qx" + _destinatedPosition + " ";
                break;
            case Symbol.R:
                if (_pieceTaken.Symbol == Symbol.Empty)
                    _movesTextPgn += "R" + _destinatedPosition + " ";
                else
                    _movesTextPgn += "Rx" + _destinatedPosition + " ";
                break;
            case Symbol.B:
                if (_pieceTaken.Symbol == Symbol.Empty)
                    _movesTextPgn += "B" + _destinatedPosition + " ";
                else
                    _movesTextPgn += "Bx" + _destinatedPosition + " ";
                break;
            case Symbol.N:
                if (_pieceTaken.Symbol == Symbol.Empty)
                    _movesTextPgn += "N" + _destinatedPosition + " ";
                else
                    _movesTextPgn += "Nx" + _destinatedPosition + " ";
                break;
            case Symbol.P:
                if (_enPassant || _pieceTaken.Symbol != Symbol.Empty)
                    _movesTextPgn += Board.Columns[_piece.Column] + "x" + _destinatedPosition + " ";
                else
                    _movesTextPgn += _destinatedPosition + " ";
                break;
        }
    }
    #endregion
}
