public class CellularAutomata : IGenerator
{
    public Room GenerateRoom(RoomConfiguration rC)
    {
        Room r = new Room((0, 0), rC.Width, rC.Height);

        for (int x = 0; x < r.BoundingBox.Width; x++)
        {
            for (int y = 0; y < r.BoundingBox.Height; y++)
            {
                if (r[x, y] == null)
                {
                    r[x, y] = new Tile(x, y, r.RoomObject.transform);
                }
            }
        }

        return r;

    }
}

