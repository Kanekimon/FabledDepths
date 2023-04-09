using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[CustomEditor(typeof(RoomEditorManager))]
public class CutomRoomEditorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RoomEditorManager manager = (RoomEditorManager)target;

        manager.LoadPlaceables();
        foreach (Placeable pl in manager.Placeables)
        {
            UnityEditor.EditorGUILayout.BeginHorizontal();
            pl.PlaceablePrefab = (GameObject)UnityEditor.EditorGUILayout.ObjectField(pl.PlaceablePrefab, typeof(GameObject), false);
            pl.PlaceableType = (PlaceableType)UnityEditor.EditorGUILayout.EnumPopup(pl.PlaceableType);
            pl.RoomType = (RoomType)UnityEditor.EditorGUILayout.EnumFlagsField(pl.RoomType);
            UnityEditor.EditorGUILayout.EndHorizontal();
        }
   


        if (GUILayout.Button("Add Placeable"))
        {
            Placeable.CreatePlaceable();
            manager.LoadPlaceables();
        }

    }

}

