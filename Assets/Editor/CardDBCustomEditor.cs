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


        if(GUILayout.Button("Save decks"))
        {
            cardDB.SaveDecks();
        }

        if (GUILayout.Button("Shuffle"))
        {
            cardDB.Decks.First().ShuffleDeck();
        }


    }

}

