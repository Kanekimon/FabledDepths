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
        deck.AddCard(new RoomCard() { Id = Guid.NewGuid().ToString(), Name = "Test_0", Type = RoomType.monster, Room = RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(0)) });
        deck.AddCard(new RoomCard() { Id = Guid.NewGuid().ToString(), Name = "Test_1", Type = RoomType.monster, Room = RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(1)) });
        deck.AddCard(new RoomCard() { Id = Guid.NewGuid().ToString(), Name = "Test_2", Type = RoomType.monster, Room = RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(2)) });
        deck.AddCard(new RoomCard() { Id = Guid.NewGuid().ToString(), Name = "Test_3", Type = RoomType.monster, Room = RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(3)) });
        deck.AddCard(new RoomCard() { Id = Guid.NewGuid().ToString(), Name = "Test_4", Type = RoomType.monster, Room = RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(4)) });
        deck.AddCard(new RoomCard() { Id = Guid.NewGuid().ToString(), Name = "Test_5", Type = RoomType.monster, Room = RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(5)) });
        deck.AddCard(new RoomCard() { Id = Guid.NewGuid().ToString(), Name = "Test_6", Type = RoomType.monster, Room = RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(6)) });
        deck.AddCard(new RoomCard() { Id = Guid.NewGuid().ToString(), Name = "Test_7", Type = RoomType.monster, Room = RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(7)) });
        deck.AddCard(new RoomCard() { Id = Guid.NewGuid().ToString(), Name = "Test_8", Type = RoomType.monster, Room = RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(8)) });
        deck.AddCard(new RoomCard() { Id = Guid.NewGuid().ToString(), Name = "Test_9", Type = RoomType.monster, Room = RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(9)) });
        deck.AddCard(new RoomCard() { Id = Guid.NewGuid().ToString(), Name = "Test_10", Type = RoomType.monster, Room = RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(10)) });
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
