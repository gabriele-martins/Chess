using Chess.Model.Players;
using Chess.View;
using Newtonsoft.Json;

namespace Chess.Repository;

public class Json
{
    #region Attributes
    private static string path = "Dados.json";
    #endregion

    #region Methods
    private static void CriarJson()
    {
        if (!File.Exists(path)) File.Create(path).Close();
    }

    public static void Serializar(List<Player> players)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, players);
            }
        }
        catch (Exception e)
        {
            Print.WriteRed($"\n\t{e.Message}");
        }
    }

    public static List<Player> Desserializar()
    {
        CriarJson();
        List<Player> players = new List<Player>();
        try
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string jsonString = sr.ReadToEnd();
                players = JsonConvert.DeserializeObject<List<Player>>(jsonString);
            }
        }
        catch (Exception e)
        {
            Print.WriteRed($"\n\t{e.Message}");
        }
        return players;
    }
    #endregion
}
