using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomGenerator))]
public class CustomRoomGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RoomGenerator gen = (RoomGenerator)target;

        if(GUILayout.Button("Generate Room"))
        {
            //gen.GenerateRoom();

        }
    }
}
