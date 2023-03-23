using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;
using System;

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


}

