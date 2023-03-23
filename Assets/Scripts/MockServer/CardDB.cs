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

    public void CreateStarterDeck()
    {
        if (_decks.Count > 0 && _decks.Any(d => d.DeckName.Equals("Starter")))
            return;

        CardDeck deck = new CardDeck();
        deck.DeckName = "Start";
        deck.AddCard(new RoomCard(Guid.NewGuid().ToString(), "Test_0", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(0, false)) ));
        deck.AddCard(new RoomCard(Guid.NewGuid().ToString(), "Test_1", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(1, false)) ));
        deck.AddCard(new RoomCard(Guid.NewGuid().ToString(), "Test_2", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(2, false)) ));
        deck.AddCard(new RoomCard(Guid.NewGuid().ToString(), "Test_3", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(3, false)) ));
        deck.AddCard(new RoomCard(Guid.NewGuid().ToString(), "Test_4", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(4, false)) ));
        deck.AddCard(new RoomCard(Guid.NewGuid().ToString(), "Test_5", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(5, false)) ));
        deck.AddCard(new RoomCard(Guid.NewGuid().ToString(), "Test_6", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(6, false)) ));
        deck.AddCard(new RoomCard(Guid.NewGuid().ToString(), "Test_7", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(7, false)) ));
        deck.AddCard(new RoomCard(Guid.NewGuid().ToString(), "Test_8", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(8, false)) ));
        deck.AddCard(new RoomCard(Guid.NewGuid().ToString(), "Test_9", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(9, false)) ));
        deck.AddCard(new RoomCard(Guid.NewGuid().ToString(), "Test_10", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(10, false)) ));
        _decks.Add(deck);
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
