using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class Dev_Card_Builder : Singleton<Dev_Card_Builder>
{
    private CardDeck _cardDeck;

    public CardDeck CardDeck
    {
        get
        {
            if (_cardDeck == null)
            {
                _cardDeck = new CardDeck();
                _cardDeck.AddCards(GetCards());
            }
            return _cardDeck;
        }
    }

    public void DrawCards()
    {
        CardDeck.ShuffleDeck();
        List<RoomCard> cards = CardDeck.DrawCards(3);
        foreach (RoomCard c in cards)
        {
            UiManager.Instance.CreateCard(c);
        }


    }

    public RoomCard GetCardFromDeck(string id)
    {
        return CardDeck.Cards.Where(x => x.Id == id).FirstOrDefault();
    }

    public List<RoomCard> GetCards()
    {
        RoomGenerationManager rm = RoomGenerationManager.Instance;
        List<RoomCard> list = new List<RoomCard>();
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_0", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.east | DoorPlacement.south | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_1", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.east | DoorPlacement.south | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_2", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.east | DoorPlacement.south | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_3", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.east | DoorPlacement.south | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_4", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.south)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_5", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.south)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_6", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.east | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_7", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.east | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_8", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.east)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_9", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.east)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_10", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.south | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_11", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.south | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_12", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_13", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_14", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.east)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_15", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.east)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_16", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.south)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_17", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.south)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_18", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_19", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_20", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.east | DoorPlacement.south)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_21", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.east | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_22", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.south | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_23", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.east | DoorPlacement.south | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_24", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_25", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_26", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.east | DoorPlacement.south)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_27", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.east | DoorPlacement.south)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_28", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.east | DoorPlacement.south | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_29", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.east | DoorPlacement.south | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_30", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.east | DoorPlacement.south | DoorPlacement.west)))));
        list.Add(new RoomCard(Guid.NewGuid().ToString(), "Starter_31", RoomType.monster, RoomSaveData.SaveRoom(RoomGenerationManager.Instance.GenerateRoom(RoomType.monster, (DoorPlacement.north | DoorPlacement.east | DoorPlacement.south | DoorPlacement.west)))));

        return list;
    }

    public void AddStarterCards()
    {
 
        DatabaseManager.Instance.SaveCards(GetCards());
    }


    public void CreateStarterDeck()
    {
        
    }


    public void DeleteAllDecksFroMDB()
    {

    }

    public void DeleteAllCardsFromDB()
    {
        DatabaseManager.Instance.DeleteAllCards();
    }

}

