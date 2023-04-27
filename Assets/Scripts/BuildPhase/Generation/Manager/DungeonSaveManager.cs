using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DungeonSaveManager : Singleton<DungeonSaveManager>
{

    #region Save/Load Room

    /// <summary>
    /// Generates savedata for a single room and saves it as json
    /// </summary>
    /// <param name="r">The room to be saved</param>
    public void SaveRoomAsJson(Room r)
    {

        using (StreamWriter streamWriter = new StreamWriter(@"D:\Unity Workspace\FabledDepths\TempJson\room.json"))
        {

            streamWriter.Write(JsonConvert.SerializeObject(RoomSaveData.SaveRoom(r), Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

    }

    /// <summary>
    /// Loads a savedata for a single room from json and generates the room
    /// </summary>
    /// <param name="fileLocation">Location of the saved json file</param>
    /// <returns></returns>
    public Room LoadRoomFromJson(string fileLocation)
    {
        Room r = null;
        using (StreamReader streamReader = new StreamReader(@"D:\Unity Workspace\FabledDepths\TempJson\room.json"))
        {
            r = RoomSaveData.LoadRoom(JsonConvert.DeserializeObject<RoomSaveData>(streamReader.ReadToEnd()));
        }
        return r;
    }

    /// <summary>
    /// Saves all rooms inside the dungeonmap
    /// Creates savedata for all rooms and then saves it as json
    /// </summary>
    /// <param name="roomMap">Map of the dungeon</param>
    public void SaveRooms(Dictionary<(int xIndex, int yIndex), BaseRoom> roomMap)
    {
        List<RoomSaveData> rooms = new List<RoomSaveData>();

        foreach (KeyValuePair<(int xIndex, int yIndex), BaseRoom> room in roomMap)
        {
            if (room.Value is PlaceholderRoom)
                continue;

            rooms.Add(RoomSaveData.SaveRoom((Room)room.Value));
        }

        string json =  JsonConvert.SerializeObject(rooms, Formatting.Indented, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        //TODO: getCurrentPlayer ID
        int id = 1;

        DatabaseManager.Instance.SaveDungeon(id, json);

    }


    /// <summary>
    /// Loads a list of savedata from json and generates the saved dungeon
    /// </summary>
    /// <param name="fileLocation">Location of the json file</param>
    public void LoadRooms(string fileLocation)
    {
        List<RoomSaveData> rooms = new List<RoomSaveData>();
        using (StreamReader streamReader = new StreamReader(@"D:\Unity Workspace\FabledDepths\TempJson\room.json"))
        {
            rooms = JsonConvert.DeserializeObject<List<RoomSaveData>>(streamReader.ReadToEnd());
        }

        foreach (RoomSaveData room in rooms)
        {
            BuildManager.Instance.RegisterRoom(RoomSaveData.LoadRoom(room), false);
        }
        BuildManager.Instance.RegeneratePlaceholder();
    }



    #endregion
}

