using Newtonsoft.Json;
using System.IO;


public class RoomGenerationManager : Singleton<RoomGenerationManager>
{
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

    public Room LoadRoomFromJson(string json)
    {
        Room r = null;
        using (StreamReader streamReader = new StreamReader(@"D:\Unity Workspace\FabledDepths\TempJson\room.json"))
        {
            r = RoomSaveData.LoadRoom(JsonConvert.DeserializeObject<RoomSaveData>(streamReader.ReadToEnd()));
        }
        return r;
    }


}

