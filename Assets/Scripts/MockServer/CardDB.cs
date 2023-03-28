using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CardDB : Singleton<CardDB>
{

    private List<CardDeck> _decks = new List<CardDeck>();

    public List<CardDeck> Decks { get { return _decks; } }

    public void CreateNewDeck()
    {

    }





    public void LoadDecks(string player_id)
    {
        if (File.Exists(GlobalVariables.DeckConfig))
        {
            using (StreamReader str = new StreamReader(GlobalVariables.DeckConfig))
            {
                _decks = JsonConvert.DeserializeObject<List<CardDeck>>(str.ReadToEnd());
            }

        }
    }


    public void SaveDecks()
    {
        using (StreamWriter stw = new StreamWriter(GlobalVariables.DeckConfig))
        {
            string json = JsonConvert.SerializeObject(_decks, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            });
            stw.Write(json);
        }
    }


}
