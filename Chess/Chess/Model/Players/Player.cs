using Chess.Controller.Players;

namespace Chess.Model.Players;

public class Player
{
    #region Properties
    private string _nickname;
    public string Nickname
    {
        get { return _nickname; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("\n\tNickname vazio não é válido.");
            }
            else if (value.All(a => char.IsDigit(a)))
            {
                throw new Exception("\n\tNickname inválido pois contém apenas números.");
            }
            else if (PlayerController.ValidateNickname(value))
            {
                throw new Exception("\n\tJogador já cadastrado.");
            }
            else
            {
                _nickname = value;
            }
        }
    }

    private string _pass;
    public string Pass 
    { 
        get { return _pass; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("\n\tSenha vazia não é válida.");
            }
            else
            {
                _pass = value;
            }
        } 
    }
    public string Color { get; set; }
    public int Matches { get; set; }
    public int Score { get; set; }
    public int Wins { get; set; }
    public int Defeats { get; set; }
    public int Ties { get; set; }
    #endregion

    #region Constructor
    public Player()
    {
        Color = "";
        Matches = 0;
        Score = 0;
        Wins = 0;
        Defeats = 0;
        Ties = 0;
    }
    #endregion

    #region Methods
    public override string ToString()
    {
        string text = $"\n\tNickname: {Nickname}" +
            $"\n\tSenha: {Pass}" +
            $"\n\n\tPartidas Jogadas: {Matches}" +
            $"\n\tPontos: {Score}" +
            $"\n\tVitórias: {Wins}" +
            $"\n\tDerrotas: {Defeats}" +
            $"\n\tEmpates: {Ties}";
        return text;
    }
    #endregion
}
