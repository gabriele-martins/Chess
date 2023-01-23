using Chess.Model.Players;
using Chess.Repository;
using Chess.Service.Players;
using Chess.View;

namespace Chess.Controller.Players;

public class PlayerController
{
    #region Attributes
    public static List<Player> players = Json.Desserializar();
    #endregion

    #region Methods
    public static bool ValidateNickname(string nick)
    {
        if (players == null)
        {
            players = new List<Player>();
            return false;
        }
        else if (players.Exists(player => player.Nickname == nick)) return true;
        else return false;
    }

    public static bool ValidatePass(string nick, string pass)
    {
        if (players.Find(player => player.Nickname == nick).Pass != pass) return false;
        else return true;
    }

    public static string GetNickname()
    {
        string nick;
        while (true)
        {
            try
            {
                nick = Print.AskNickname();
                break;
            }
            catch (Exception e)
            {
                Print.WriteRed($"\n\t{e.Message}");
                Print.ShowTryAgainMessage();
            }
        }
        return nick;
    }

    public static string GetPass()
    {
        string pass;
        while (true)
        {
            try
            {
                pass = Print.AskPass();
                break;
            }
            catch (Exception e)
            {
                Print.WriteRed($"\n\t{e.Message}");
                Print.ShowTryAgainMessage();
            }
        }
        return pass;
    }

    public static void AddPlayer()
    {
        Player newPlayer = new Player();

        newPlayer.Nickname = GetNickname();
        newPlayer.Pass = GetPass();

        players.Add(newPlayer);
        Json.Serializar(players);
        Print.ShowPlayerAddedMessage();
    }

    public static void DeletePlayer()
    {
        string nick = PlayerService.FindNickName();
        string pass = PlayerService.CheckPass(nick);

        players.RemoveAll(cliente => cliente.Nickname == nick);
        Json.Serializar(players);
        Print.ShowPlayerDeletedMessage();
    }

    public static void UpdatePlayers()
    {
        Json.Serializar(players);
    }
    #endregion
}
