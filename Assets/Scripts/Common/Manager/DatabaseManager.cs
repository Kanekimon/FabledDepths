using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Bson;

public class DatabaseManager : Singleton<DatabaseManager>
{



    protected virtual void Awake()
    {
        base.Awake();

    }

    public bool DeleteAllCards()
    {
        //delete from your_table;
        //delete from sqlite_sequence where name = 'your_table';

        try
        {
            string connection = "URI=file:" + Application.persistentDataPath + "/FabledDephts.db";
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();

            IDbCommand deleteALl = dbcon.CreateCommand();
            deleteALl.CommandText = $"delete from fb_cards";
            deleteALl.ExecuteNonQuery();

            IDbCommand resetAutoInc = dbcon.CreateCommand();
            resetAutoInc.CommandText = $"delete from sqlite_sequence where name = 'fb_cards'";
            resetAutoInc.ExecuteNonQuery();

            dbcon.Close();

            return true;
        }catch (Exception ex) { return false; }
    }


    public bool SaveCard(RoomCard rc)
    {
        try
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
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool SaveCards(List<RoomCard> rcs)
    {
        try
        {
                string connection = "URI=file:" + Application.persistentDataPath + "/FabledDephts.db";
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();

            string commandArgs = "";

            foreach (RoomCard rc in rcs)
            {
                IDbCommand cmnd = dbcon.CreateCommand();
                cmnd.CommandText = $"INSERT INTO fb_cards (card_type,card_difficulty,card_room_save_data, card_door_count, card_needs_cloning) VALUES (\"{rc.Type.ToString()}\",\"{rc.Difficulty}\",\'{rc.Room.ToJson()}\',\'{rc.DoorCount}\', \'1\')";
                cmnd.ExecuteNonQuery();

            }

            dbcon.Close();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool AddCardToPlayer(RoomCard rc, int userId)
    {
        try
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
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool SaveDungeon(int userId, string json)
    {
        try
        {
            IDbConnection dbConnect = CreateConnection();

            IDbCommand cmnd = dbConnect.CreateCommand();
            cmnd.CommandText = $"INSERT INTO fb_dungeons (player_id, dungeon_data) VALUES (\"{userId}\",\'{json}\')";
            cmnd.ExecuteNonQuery();
            dbConnect.Close();
            return true;
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }
    }

    public string GetDungeonJson(int userId)
    {
        try
        {
            IDbConnection con = CreateConnection();
            IDbCommand cmnd = con.CreateCommand();
            cmnd.CommandText = $"SELECT dungeon_data FROM fb_dungeons WHERE player_id = \"{userId}\"";
            IDataReader reader = cmnd.ExecuteReader();

            string dungeon = "";
            while (reader.Read())
            {
                byte[] json = (byte[])reader["dungeon_data"];
                dungeon = System.Text.Encoding.Default.GetString(json);
            }


            return dungeon;
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return "";
        }
    }



    public IDbConnection CreateConnection()
    {
        try
        {
            string connection = "URI=file:" + Application.persistentDataPath + "/FabledDephts.db";
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();
            return dbcon;
        }
        catch(Exception ex)
        {
            return null;
        }
    }



}

