using Chess.Service.Game;
using Chess.Service.Players;
using Chess.View;

namespace Chess;

public class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "Chess";

        Print.StartChessGame();

        string option;

        do
        {
            Print.ShowMainMenu();

            option = Console.ReadLine();

            switch (option)
            {
                case "0":
                    Print.ExitChessGame();
                    break;
                case "1":
                    PlayerService.ManipulatePlayerAccount();
                    break;
                case "2":
                    PlayerService.ShowPlayersRanking();
                    break;
                case "3":
                    Match.Play();
                    break;
                default:
                    Print.ShowInvalidOption();
                    break;
            }

        } while(option != "0");
    }
}
