using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

using EL = UnityEditor.EditorGUILayout;
[CustomEditor(typeof(BaseRoom), true)]
public class CustomBaseRoomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        BaseRoom room = (BaseRoom)target;

        foreach(AdjacendRoom ar in room.Adjacend)
        {
            EL.BeginHorizontal();
            EL.LabelField("Index: ");
            EL.TextField($"({ar.Index.X} | {ar.Index.Y})");
            EL.EndHorizontal();

            EL.BeginHorizontal();
            EL.LabelField("Object: ");
            EL.TextField($"({ar.room.gameObject.name})");
            EL.EndHorizontal();
        }
    }

}

