using Chess.Controller.Players;
using Chess.Model.Players;
using Chess.View;

namespace Chess.Service.Players;

public class PlayerService
{
    #region Methods
    public static string FindNickName()
    {
        string nick;
        while (true)
        {
            nick = Print.AskNickname();
            if (!PlayerController.ValidateNickname(nick))
            {
                Print.ShowNicknameNotFoundMessage();
            }
            else break;
        }
        return nick;
    }

    public static string CheckPass(string nick)
    {
        string pass;
        while (true)
        {
            pass = Print.AskPass();
            if (!PlayerController.ValidatePass(nick, pass))
            {
                Print.ShowInvalidPassMessage();
            }
            else break;
        }
        return pass;
    }

    public static void DetailPlayer()
    {
        string nick = FindNickName(); 
        string pass = CheckPass(nick);
        
        Console.Clear();
        Console.WriteLine(PlayerController.players.Find(c => c.Nickname == nick).ToString());
        Print.ShowBackMessage();
    }

    public static void ManipulatePlayerAccount()
    {
        string option;
        
        do
        {
            Print.ShowPlayerMenu();

            option = Console.ReadLine();

            switch (option)
            {
                case "0":
                    break;
                case "1":
                    PlayerController.AddPlayer();
                    break;
                case "2":
                    DetailPlayer();
                    break;
                case "3":
                    PlayerController.DeletePlayer();
                    break;
                default:
                    Print.ShowInvalidOption();
                    break;
            }

        } while (option != "0");
    }

    public static void ShowPlayersRanking()
    {
        try
        {
            List<Player> ranking = PlayerController.players.OrderBy(player => player.Score).ThenBy(player => player.Wins).ThenByDescending(player => player.Defeats).ToList();

            Console.Clear();
            Console.WriteLine("\n\tRanking de Jogadores");
            for (int i = ranking.Count - 1, j = 1; i >= 0; i--, j++)
            {
                Console.WriteLine($"\n\t{j}º {ranking[i].Nickname}");
                Console.WriteLine($"\tPontos: {ranking[i].Score}");
            }
            Print.ShowBackMessage();
        }
        catch 
        {
            Print.ShowNoRegisteredPlayersMessage();
            return;
        }
    }
    #endregion
}
