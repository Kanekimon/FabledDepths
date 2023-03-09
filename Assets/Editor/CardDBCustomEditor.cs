using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardDB))]
public class CardDBCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CardDB cardDB = (CardDB)target;


        if(GUILayout.Button("Create starter deck"))
        {
            cardDB.CreateStarterDeck();
        }

        if(GUILayout.Button("Save decks"))
        {
            cardDB.SaveDecks();
        }


        if(GUILayout.Button("Draw Cards"))
        {
            cardDB.Decks.First().ShuffleDeck();
            List<RoomCard> cards = cardDB.Decks.First().DrawCards(3);
            foreach (RoomCard c in cards)
            {
                UiManager.Instance.CreateCard(c);
            }


        }
    }

}

