using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenerator
{
    public Room GenerateRoom(RoomConfiguration rC);
}
