using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BuildManager))]
public class CustomBuildManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BuildManager man = (BuildManager)target;

        if(GUILayout.Button("Init Start"))
        {
            man.InitBuildingPhase(0);
        }

        if (GUILayout.Button("Save Dungeon"))
        {
            DungeonSaveManager.Instance.SaveRooms(man.RoomMap);
        }


        if (GUILayout.Button("Erase Dungeon"))
        {
            man.EraseAll();
        }

        if (GUILayout.Button("Load Dungeon"))
        {
            man.EraseAll(false);
            DungeonSaveManager.Instance.LoadRooms("");
        }
    }

}

