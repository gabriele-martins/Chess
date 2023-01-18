using Chess.Controller.Players;
using Chess.Model.Players;
using Chess.View;

namespace Chess.Service.Players;

public class PlayerService : PlayerController
{
    #region Methods
    public static void DetailPlayer()
    {
        string nick, pass;
        while (true)
        {
            nick = Print.AskNickname();
            if (!ValidateNickname(nick))
            {
                Print.ShowNicknameNotFoundMessage();
            }
            else break;
        }
        while (true)
        {
            pass = Print.AskPass();
            if (!ValidatePass(nick, pass))
            {
                Print.ShowInvalidPassMessage();
            }
            else break;
        }
        Console.Clear();
        Console.WriteLine(players.Find(c => c.Nickname == nick).ToString());
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
                    AddPlayer();
                    break;
                case "2":
                    DetailPlayer();
                    break;
                case "3":
                    DeletePlayer();
                    break;
                default:
                    Print.ShowInvalidOption();
                    break;
            }

        } while (option != "0");
    }

    public static void ShowPlayerRanking()
    {
        List<Player> ranking = players.OrderBy(player => player.Score).ThenBy(player => player.Wins).ThenByDescending(player => player.Defeats).ToList();

        Console.Clear();
        Console.WriteLine("\n\tRanking de Jogadores");
        for (int i = ranking.Count - 1, j = 1; i >= 0; i--, j++)
        {
            Console.WriteLine($"\n\t{j}º {ranking[i].Nickname}");
            Console.WriteLine($"\tPontos: {ranking[i].Score}");
        }
        Print.ShowBackMessage();
    }
    #endregion
}
