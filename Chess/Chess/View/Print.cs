namespace Chess.View;

public class Print
{
    #region Opening and Closing
    public static void StartChessGame()
    {
        Console.WriteLine("\n\tBem-vindo(a) ao Chess.");
        Console.Write("\n\tPressione qualquer tecla para começar ");
        Console.ReadKey();
    }

    public static void ExitChessGame()
    {
        Console.Clear();
        Console.WriteLine("\n\tEncerrando o Chess.\n\n\tObrigado por jogar.\n");
    }
    #endregion

    #region Menus
    public static void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("\n\tEscolha uma opção para continuar: ");
        Console.WriteLine("\n\t1 - Manipular conta de Jogador");
        Console.WriteLine("\t2 - Ranking de Jogadores");
        Console.WriteLine("\t3 - Jogar Xadrez");
        Console.WriteLine("\t0 - Sair do jogo");
        Console.Write("\n\tDigite a opção desejada: ");
    }

    public static void ShowPlayerMenu()
    {
        Console.Clear();
        Console.WriteLine("\n\tEscolha uma opção para continuar: ");
        Console.WriteLine("\n\t1 - Cadastrar novo Jogador");
        Console.WriteLine("\t2 - Atualizar um Jogador");
        Console.WriteLine("\t3 - Detalhes de um Jogador");
        Console.WriteLine("\t4 - Deletar Jogador");
        Console.WriteLine("\t0 - Voltar para o Menu Principal");
        Console.Write("\n\tDigite a opção desejada: ");
    }
    #endregion

    #region Error Messages
    public static void ShowInvalidOption()
    {
        Console.Clear();
        Console.WriteLine("\n\tOpção inserida invalida.");
        Console.Write("\n\tPressione qualquer tecla para continuar");
        Console.ReadKey();
    }



    #endregion
}
