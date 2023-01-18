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

    #region Color Messages
    public static void WriteRed(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void WriteGreen(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(text);
        Console.ResetColor();
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

    public static void ShowPawnPromotionMenu()
    {
        Console.Clear();
        Console.WriteLine("\n\tParabéns, seu peão foi promovido.");
        Console.WriteLine("\n\tEscolha qual peça seu peão irá se tornar: ");
        Console.WriteLine("\n\tQ - Rainha");
        Console.WriteLine("\tR - Torre;");
        Console.WriteLine("\tB - Bispo;");
        Console.WriteLine("\tN - Cavalo;");
        Console.Write("\n\tDigite a letra da peça: ");
    }
    #endregion

    #region Error Messages
    public static void ShowInvalidOption()
    {
        Console.Clear();
        WriteRed("\n\tOpção inserida invalida.");
        ShowTryAgainMessage();
    }

    public static void ShowNullValue()
    {
        Console.Clear();
        WriteRed("\n\tValor nulo não é válido.");
        ShowTryAgainMessage();
    }

    public static void ShowInvalidValue()
    {
        Console.Clear();
        WriteRed("\n\tValor inserido não é válido. O valor deve ser uma letra (a-h) acompanhado de um número (1-8).");
        ShowTryAgainMessage();
    }

    public static void ShowNonExistentPosition()
    {
        Console.Clear();
        WriteRed("\n\tPosição não existe no tabuleiro.");
        ShowTryAgainMessage();
    }

    public static void ShowNoPieceHere()
    {
        Console.Clear();
        WriteRed("\n\tNão há nenhuma peça nessa posição.");
        ShowTryAgainMessage();
    }

    public static void ShowOpponetsPiece()
    {
        Console.Clear();
        WriteRed("\n\tNão é possível mover uma peça do seu adversário.");
        ShowTryAgainMessage();
    }

    public static void ShowInvalidMovement()
    {
        Console.Clear();
        WriteRed("\n\tEsse movimento não é válido para essa peça.");
        ShowTryAgainMessage();
    }

    public static void ShowPuttingKingInCheckMessage()
    {
        Console.Clear();
        WriteRed("\n\tVocê não pode colocar seu Rei em xeque.");
        ShowTryAgainMessage();
    }

    public static void ShowKingInCheckMessage()
    {
        Console.Clear();
        WriteRed("\n\tSeu rei está em xeque.Proteja-o.");
        ShowTryAgainMessage();
    }

    #endregion

    #region Success Messages
    public static void ShowCheckMateMessage()
    {
        WriteGreen("\n\n\t ------- XEQUE MATE -------");
        ShowContinueMessage();
    }
    #endregion

    #region Continue, Back and Try Again Messages
    public static void ShowContinueMessage()
    {
        Console.Write("\n\tPressione qualquer tecla para continuar");
        Console.ReadKey();
    }

    public static void ShowTryAgainMessage()
    {
        Console.Write("\n\tPressione qualquer tecla para tentar novamente");
        Console.ReadKey();
    }

    public static void ShowBackMessage()
    {
        Console.Write("\n\tPressione qualquer tecla para voltar");
        Console.ReadKey();
    }
    #endregion
}
