using UnityEngine;

public interface IGenerator
{
    public Room GenerateRoom((int X, int Y) Index, RoomConfiguration rC, int width, int height, GameObject container = null, bool generateGameObject = true);
}
