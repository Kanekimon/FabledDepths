using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


public enum PlaceableType
{
    Tile,
    Obstacle
}

[CreateAssetMenu(menuName = "Create Placeable")]
public class Placeable : ScriptableObject
{
    public GameObject PlaceablePrefab;
    public RoomType RoomType;
    public PlaceableType PlaceableType;


    public static Placeable CreatePlaceable()
    {
        Placeable asset = ScriptableObject.CreateInstance<Placeable>();


        string name = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/ScriptableObject/RoomEditor/Placeable_{Guid.NewGuid()}.asset");
        AssetDatabase.CreateAsset(asset, name);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;

        return asset;
    }
}

