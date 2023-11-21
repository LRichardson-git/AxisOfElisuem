using UnityEngine;

public class testfinding
{
    // Start is called before the first frame update
    int CellSize = 10;
    public Vector3Int coordToWorld(Coords coord) //y is height
    {
        Vector3Int newPos = new Vector3Int();
        newPos.x = coord.x * 10 + 5;
        newPos.y = coord.y * 10 + (5 * 2);
        newPos.z = coord.z * 10 + 5;
        return newPos;
    }

    public Vector3Int coordToWorld(Coords coord, int udepth) //y is height
    {
        Vector3Int newPos = new Vector3Int();
        newPos.x = coord.x * 10 + 5;
        newPos.y = coord.y * 10 + (5 * udepth);
        newPos.z = coord.z * 10 + 5;
        return newPos;
    }

    public Vector3 coordToWorld(int x, int y, int z, int uWidth, int uDepth)
    {
        Vector3Int newPos = new Vector3Int();
        newPos.x = x * 10 + (5 * uWidth);
        newPos.y = y * 10 + (5 * uDepth);
        newPos.z = z * 10 + (5 * uWidth);

        if (uDepth == 1)
            newPos.y -= 5;

        return newPos;
    }

    public Coords worldToCoord(Vector3 Pos)
    {
        Coords Grid = new Coords((int)Mathf.Floor((Pos.x) / CellSize),
                                 (int)Mathf.Floor((Pos.y) / CellSize),
                                 (int)Mathf.Floor((Pos.z) / CellSize));
        return Grid;
    }
    public Vector3Int worldToCoordT(Vector3 Pos)
    {
        Vector3Int Grid = new Vector3Int((int)Mathf.Floor((Pos.x) / CellSize),
                                 (int)Mathf.Floor((Pos.y) / CellSize),
                                 (int)Mathf.Floor((Pos.z) / CellSize));
        return Grid;
    }

    public Vector3Int worldToCoord(Vector3 Pos, int uWidth, int uDepth)
    {
        Vector3Int Grid = new Vector3Int();
        Grid.x = (int)Mathf.Floor((Pos.x) / CellSize);
        Grid.y = (int)Mathf.Floor((Pos.y) / CellSize);
        Grid.z = (int)Mathf.Floor((Pos.z) / CellSize);

        if (uDepth > 1)
            Grid.y -= (int)uDepth / 2;

        if (uWidth > 1)
        {
            Grid.x -= uWidth;
            Grid.z -= uWidth;
        }

        return Grid;
    }


    public int worldToCoord(Vector3 Pos, int uDepth)
    {

        int x = (int)Mathf.Floor((Pos.x) / CellSize);
        int y = (int)Mathf.Floor((Pos.y) / CellSize);
        int z = (int)Mathf.Floor((Pos.z) / CellSize);

        if (uDepth > 1)
            y -= (int)uDepth / 2;


        return x + y + z;
    }

}
