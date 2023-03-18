using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class RoomGenerationManager : Singleton<RoomGenerationManager>
{
    IGenerator _generator;
    private Room r;
    public RoomConfiguration Configuration;



    public Room GenerateRoom((int X, int Y) Index, int width, int height, GameObject container)
    {
        if (_generator == null)
            _generator = new LayerGenerator();


        if (Index == (0, 0))
            r = _generator.GenerateRoom(Index, Configuration, width, height, container);
        else
            r = _generator.GenerateRoom(Index, new RoomConfiguration(Configuration), width, height, container);

        r.RoomObject.transform.position -= new Vector3(width / 2, height / 2, 0);
        return r;
    }

    public Room GenerateRoom(int index, bool generateGameObject = true)
    {
        if (_generator == null)
            _generator = new LayerGenerator();

        Room r = _generator.GenerateRoom((-100, -100), mockconfigs[index], BuildManager.Instance.Settings.RoomWidth, BuildManager.Instance.Settings.RoomHeight, null, generateGameObject);
        return r;
    }

    public PlaceholderRoom GeneratePlaceholder((int X, int Y) Index, int width, int height, GameObject container)
    {
        if (_generator == null)
            _generator = new LayerGenerator();

        PlaceholderRoom plhR = new PlaceholderRoom();

        GameObject plRoom = GameObject.Instantiate(Resources.Load<GameObject>("Room/PlaceholderRoom"));
        plRoom.transform.parent = container.transform;

        plhR.Index = Index;
        plRoom.name = $"Placeholder [{Index.X}|{Index.Y}]";
        plRoom.transform.localScale = new Vector3(width, height, 0);
        plRoom.transform.position = new Vector3((Index.X * width) - GlobalVariables.HaltUnit, (Index.Y * height) - GlobalVariables.HaltUnit, 0);

        plhR.RoomObject = plRoom;

        return plhR;
    }


    #region mockrooms
    List<RoomConfiguration> mockconfigs = new List<RoomConfiguration>() {
        new RoomConfiguration() { RoomType = RoomType.monster, DoorPlacement = DoorPlacement.north },
        new RoomConfiguration() { RoomType = RoomType.monster, DoorPlacement = DoorPlacement.east },
        new RoomConfiguration() { RoomType = RoomType.monster, DoorPlacement = DoorPlacement.south },
        new RoomConfiguration() { RoomType = RoomType.monster, DoorPlacement = DoorPlacement.west },
        new RoomConfiguration() { RoomType = RoomType.monster, DoorPlacement = DoorPlacement.north | DoorPlacement.east },
        new RoomConfiguration() { RoomType = RoomType.monster, DoorPlacement = DoorPlacement.east | DoorPlacement.south },
        new RoomConfiguration() { RoomType = RoomType.monster, DoorPlacement = DoorPlacement.south | DoorPlacement.west },
        new RoomConfiguration() { RoomType = RoomType.monster, DoorPlacement = DoorPlacement.north | DoorPlacement.west },
        new RoomConfiguration() { RoomType = RoomType.monster, DoorPlacement = DoorPlacement.north | DoorPlacement.south },
        new RoomConfiguration() { RoomType = RoomType.monster, DoorPlacement = DoorPlacement.west | DoorPlacement.east },
        new RoomConfiguration() { RoomType = RoomType.monster, DoorPlacement = DoorPlacement.north | DoorPlacement.east | DoorPlacement.south | DoorPlacement.west }
    };

    #endregion



}

