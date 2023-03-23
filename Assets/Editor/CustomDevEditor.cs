using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Dev_Card_Builder))]
public class CustomDevEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        Dev_Card_Builder devBuilder = (Dev_Card_Builder)target; 

        if(GUILayout.Button("Create Starter Deck"))
        {
            devBuilder.AddStarterCards();
        }

        if(GUILayout.Button("Clear All Cards"))
        {
            devBuilder.DeleteAllCardsFromDB();
        }

        if (GUILayout.Button("Draw Cards"))
        {
            devBuilder.DrawCards();
        }


    }

}

