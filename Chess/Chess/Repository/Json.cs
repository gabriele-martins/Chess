using Chess.Model.Players;
using Chess.View;
using Newtonsoft.Json;

namespace Chess.Repository;

public class Json
{
    #region Attributes
    private static string _path = @"C:..\..\..\Repository\PlayersData\Players.json";
    #endregion

    #region Methods
    private static void CriarJson()
    {
        if (!File.Exists(_path)) File.Create(_path).Close();
    }

    public static void Serializar(List<Player> players)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(_path))
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
            using (StreamReader sr = new StreamReader(_path))
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
