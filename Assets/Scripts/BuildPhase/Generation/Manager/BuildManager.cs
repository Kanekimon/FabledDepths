using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public enum BuildMode
{
    Move, 
    Detail
}

public class BuildManager : Singleton<BuildManager>
{
    public BaseDungeonSettings Settings;

    int[] nDiscoveryX = new int[] { 0, 1, 0, -1 };
    int[] nDiscoveryY = new int[] { 1,0, -1, 0 };

    public Dictionary<(int xIndex, int yIndex), BaseRoom> RoomMap = new Dictionary<(int xIndex, int yIndex), BaseRoom>();
    public GameObject DungeonContainer;

    internal void SetBuildMode(BuildMode move)
    {
        BuildMode = move;

        if (BuildMode == BuildMode.Move)
            DragDropManager.Instance.enabled = true;
        else
            DragDropManager.Instance.enabled = false;
    }

    public BuildMode BuildMode { get; set; } = BuildMode.Detail;

    public virtual void Awake()
    {
        base.Awake();
        SetBuildMode(BuildMode.Move);
        DungeonContainer = GameObject.FindWithTag("dungeoncontainer");
    }


    /// <summary>
    /// Erases all rooms and creates the start room
    /// </summary>
    /// <param name="init">should a start room be generated after erasing</param>
    public void EraseAll(bool init = true)
    {
        Destroy(DungeonContainer);
        DungeonContainer = new GameObject();
        DungeonContainer.name = "DungeonContainer";

        RoomMap.Clear();
        if(init)
            InitBuildingPhase(0);
    }


    /// <summary>
    /// Start of the Building Phase
    /// </summary>
    /// <param name="stage"></param>
    public void InitBuildingPhase(int stage)
    {
        if(DungeonContainer.transform.childCount > 0)
        {
            DestroyImmediate(DungeonContainer);
            DungeonContainer = new GameObject();
            DungeonContainer.tag = "dungeoncontainer";
            DungeonContainer.name = "DungeonContainer";
        }

        if(stage == 0)
            RegisterRoom((0, 0));
        else
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public KeyValuePair<(int x, int y), BaseRoom> GetRoom(int x, int y)
    {
        return RoomMap.Where(r => r.Key == (x,y)).FirstOrDefault();
    }


    /// <summary>
    /// Checks if the room needs to be linked to adjacend rooms
    /// </summary>
    /// <param name="r">Room to check adjacend fields</param>
    public void CheckAdjacentRooms(Room r )
    {

        if (r.Doors.HasFlag(DoorPlacement.north))
        {
            LinkRooms(r, (r.Index.X + nDiscoveryX[0], r.Index.Y + nDiscoveryY[0]));
        }
        if (r.Doors.HasFlag(DoorPlacement.east))
        {
            LinkRooms(r, (r.Index.X + nDiscoveryX[1], r.Index.Y + nDiscoveryY[1]));
        }
        if (r.Doors.HasFlag(DoorPlacement.south))
        {
            LinkRooms(r, (r.Index.X + nDiscoveryX[2], r.Index.Y + nDiscoveryY[2]));
        }
        if (r.Doors.HasFlag(DoorPlacement.west))
        {
            LinkRooms(r, (r.Index.X + nDiscoveryX[3], r.Index.Y + nDiscoveryY[3]));
        }
    }

    public void LinkRooms(BaseRoom r, (int X, int Y)Index)
    {
        BaseRoom linkTo = null;
        if (RoomMap.ContainsKey(Index) && RoomMap[Index] != null)
        {
            linkTo = RoomMap[Index];
        }
        else if(!RoomMap.ContainsKey(Index))
        {
             linkTo = RegisterPlaceholder(Index);
        }

        if (linkTo == null)
            return;

        linkTo.Adjacend.Add(new AdjacendRoom() { Index = r.Index, room = r });
        r.Adjacend.Add(new AdjacendRoom() { Index = Index, room = linkTo });
    }

    /// <summary>
    /// Removes all links to the given room
    /// </summary>
    /// <param name="r">The room you want to unlike from all his adjacend ones</param>
    public void UnlinkRooms(BaseRoom r)
    {
        foreach(AdjacendRoom ar in r.Adjacend)
        {
            ar.room.Adjacend.RemoveAll(x => x.Index == r.Index);
        }
    }


    internal void RegeneratePlaceholder()
    {
        foreach (Transform child in DungeonContainer.transform)
        {
            if (child.name.Contains("Placeholder"))
            {
                RoomMap.Remove( RoomMap.Where(x => x.Value.RoomObject.transform == child).FirstOrDefault().Key);
                Destroy(child.gameObject);
               
            }
            else
            {
                RoomMap.Where(x => x.Value.RoomObject.transform == child).FirstOrDefault().Value.Adjacend.Clear();
            }
        }

        List<BaseRoom> tmpList = RoomMap.Values.ToList();

        foreach(BaseRoom room in tmpList)
        {
            CheckAdjacentRooms((Room)room);
        }


    }

    /// <summary>
    /// Triggers the generation of a new room
    /// </summary>
    /// <param name="Index">Position of room inside map</param>
    /// <returns>The created room</returns>
    public Room CreateRoom((int X, int Y) Index)
    {
        return RoomGenerationManager.Instance.GenerateRoom(Index, Settings.RoomWidth, Settings.RoomHeight, DungeonContainer);
    }


    /// <summary>
    /// Register loaded/existing room to room map
    /// </summary>
    /// <param name="r">Instance of a room</param>
    /// <returns>The registered room</returns>
    public Room RegisterRoom(Room r, bool linkRooms = true)
    {
        r.RoomObject.transform.parent = DungeonContainer.transform;
        RoomMap[r.Index] = r;

        if (linkRooms)
            CheckAdjacentRooms(r);
        return r;
    }

    /// <summary>
    /// Generates and registeres a new room with at a given index
    /// </summary>
    /// <param name="Index">Index in Map</param>
    /// <returns>The registered room</returns>
    public Room RegisterRoom((int X, int Y) Index)
    {
        Room r = CreateRoom(Index);
        RoomMap[Index] = r;

        CheckAdjacentRooms(r);

        return r;
    }

    /// <summary>
    /// Creates and registeres a new room and replaces the given Placeholder for it
    /// </summary>
    /// <param name="plh">Placeholder that was interacted with</param>
    public void RegisterRoom(PlaceholderClickHandler pch) 
    {
        PlaceholderRoom plh = (PlaceholderRoom) RoomMap.Values.Where(x => x.RoomObject == pch.gameObject).First();
        Room r = CreateRoom(plh.Index);
        RoomMap[plh.Index] = r;
        plh.CopyRoomRefs(r);

        CheckAdjacentRooms(r);

        Destroy(plh.RoomObject);
    }

    /// <summary>
    /// Creates and registeres a new room and replaces the given Placeholder for it
    /// </summary>
    /// <param name="plh">Placeholder that was interacted with</param>
    public void RegisterRoom(PlaceholderRoom plh, Room r)
    { 
        RoomMap.Remove(r.Index);
        r.ChangeIndex(plh.Index);

        Vector3 plhPos = plh.RoomObject.transform.position;
        r.RoomObject.transform.position = new Vector3((Settings.RoomWidth * plh.Index.X) - (Settings.RoomWidth / 2), (Settings.RoomHeight * plh.Index.Y) - (Settings.RoomHeight/2), 0);  //CurrentlyOver.transform.position - new Vector3(BuildManager.Instance.Settings.RoomWidth / 2, BuildManager.Instance.Settings.RoomHeight / 2, 0);

        RoomMap[plh.Index] = r;
        DestroyImmediate(plh.RoomObject);
        RegeneratePlaceholder();
    }


    /// <summary>
    /// Creates and registeres a new room and replaces the given Placeholder for it
    /// </summary>
    /// <param name="plh">Placeholder that was interacted with</param>
    public void RegisterRoom(PlaceholderRoom plh, RoomSaveData rsd)
    {
        Room r = RoomSaveData.LoadRoom(rsd);
        r.ChangeIndex(plh.Index);

        Vector3 plhPos = plh.RoomObject.transform.position;
        r.RoomObject.transform.position = new Vector3((Settings.RoomWidth * plh.Index.X) - (Settings.RoomWidth / 2), (Settings.RoomHeight * plh.Index.Y) - (Settings.RoomHeight / 2), 0);  //CurrentlyOver.transform.position - new Vector3(BuildManager.Instance.Settings.RoomWidth / 2, BuildManager.Instance.Settings.RoomHeight / 2, 0);
        r.RoomObject.transform.parent = DungeonContainer.transform;
        RoomMap[plh.Index] = r;
        DestroyImmediate(plh.RoomObject);
        RegeneratePlaceholder();
    }



    /// <summary>
    /// Creates and registeres a new placeholder at a given Index
    /// </summary>
    /// <param name="Index">Position inside map</param>
    /// <returns>The created and registered placeholder</returns>
    public PlaceholderRoom RegisterPlaceholder ((int x, int y) Index)
    {
        PlaceholderRoom plRoom = RoomGenerationManager.Instance.GeneratePlaceholder(Index, Settings.RoomWidth, Settings.RoomHeight, DungeonContainer);

        RoomMap[Index] = plRoom;
        return plRoom;
    }

    public PlaceholderRoom GetPlaceholder(GameObject plcObject)
    {
        try
        {
            KeyValuePair<(int X, int Y), BaseRoom> placeholder = RoomMap.Where(x => x.Value.RoomObject != null && x.Value.RoomObject == plcObject).FirstOrDefault();

            return (PlaceholderRoom)placeholder.Value;
        }
        catch(Exception ex)
        {
            return null;
        }
    }
}


