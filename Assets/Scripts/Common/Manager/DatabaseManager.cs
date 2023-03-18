using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class DatabaseManager : Singleton<DatabaseManager>
{



    protected virtual void Awake()
    {
        base.Awake();

    }



    public bool SaveCard(RoomCard rc)
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/FabledDephts.db";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = $"INSERT INTO fb_cards (card_type,card_difficulty,card_room_save_data) VALUES (\"{rc.Type.ToString()}\",\"{rc.Difficulty}\",\'{rc.Room.ToJson()}\')";
        cmnd.ExecuteNonQuery();

        dbcon.Close();

        return true;
    }



}

