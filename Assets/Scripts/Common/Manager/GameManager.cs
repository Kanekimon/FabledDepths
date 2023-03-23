using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GameManager : Singleton<GameManager>
{
    private Player _player;

    protected virtual void Awake()
    {
        base.Awake();
        InitPlayer();
    }


    protected virtual void Start()
    {
        BuildManager.Instance.InitBuildingPhase(0);
        Dev_Card_Builder.Instance.DrawCards();
    }



    void InitPlayer()
    {
        var roamingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var filePath = Path.Combine(roamingDirectory, "FabledDepths\\player.json");

        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        if (!File.Exists(filePath))
        {
            _player = new Player(Guid.NewGuid().ToString());
            SerializePlayer(_player, filePath);
        }
        else
        {
            _player = DeserializePlayer(filePath);
        }

    }


    public Player GetPlayer()
    {
        return _player;
    }

    void SerializePlayer(Player p, string filePath)
    {
        string json = JsonConvert.SerializeObject(p);

        using(StreamWriter stw = new StreamWriter(filePath))
        {
            stw.WriteLine(json);
        }
    }


    Player DeserializePlayer(string filePath)
    {
        Player p = null;
        using (StreamReader str = new StreamReader(filePath))
        {
            p = JsonConvert.DeserializeObject<Player>(str.ReadToEnd());
        }

        return p;
    }
}



