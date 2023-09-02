using System.Collections.Generic;
using UnityEngine;

public static class World_Pathfinding
{


    private static int MOVEMENT_COST = 10;

    private static Coords[,,] mapIndex;

    public static int width = 30, height = 30, depth = 10;

    public static int CellSize = 10;

    static World_Pathfinding()
    {

        mapIndex = new Coords[width, height, depth];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    mapIndex[x, y, z] = new Coords(x, y, z);
                }
            }
        }


    }


    public static RaycastHit2D getWorldMouse()
    {
        Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);
        return hit;
    }
    public static void setWidthHeight(int x, int y)
    {
        width = x; height = y;
    }




    public static Vector3 coordToWorld(Coords coord, int width)
    {
        Vector3 newPos;
        Vector3Int temp = new Vector3Int(coord.x, coord.y, coord.z);
        newPos = TileMapper.Instance.tilemap.CellToWorld(temp);
        Debug.Log("cell to world :" + newPos);
        //5,0,0 = 

        newPos.x += (width * 5);
        newPos.z = (newPos.y += (width * 5));
        newPos.y = coord.z + (coord.z * 10);

        return newPos;
    }

    public static Vector3Int worldToCoord(Vector3 Pos, int width)
    {
        Vector3Int Grid;
        if (width > 1)
        {
            Pos.x -= ((width - 1) * 5);
            Pos.y -= ((width - 1) * 5);
            //Debug.Log(Pos);
            Grid = TileMapper.Instance.tilemap.WorldToCell(Pos);
        }
        else
            Grid = TileMapper.Instance.tilemap.WorldToCell(Pos);


        //Debug.Log((Pos.x - (5 * (width - 1))) / CellSize);
        return Grid;
    }





    public static Vector3 coordToWorld(int x, int y, int width)
    {
        Vector3 newPos;
        Vector3Int temp = new Vector3Int(x, y, 0);
        newPos = TileMapper.Instance.tilemap.CellToWorld(temp);



        newPos.x += (width * 5);
        newPos.y += (width * 5);
        return newPos;
    }

    public static List<Vector3> findPath(int xEnd, int yEnd, int zEnd, int xSt, int ySt, int zSt, int uWidth, int uHeight, int uDepth)
    {
        //Debug.Log("PATH");
        if (xEnd < 0 || xEnd > 198 || yEnd < 0 || yEnd > 198 || zEnd <0 || zEnd >= uDepth)
            return null;




        if (!isAreaWalkable(xEnd, yEnd, zEnd, uWidth, uWidth, uDepth))
        {
            Debug.Log("not walkable");
            return null;
        }

        Queue<Coords> visitQueue = new Queue<Coords>();
        bool[,,] visited = new bool[width, height, depth];


        Coords startCoord = mapIndex[xSt, ySt, zSt];
        Coords EndCoord = mapIndex[xEnd, yEnd, zEnd];
        //  List<Coords> FinalPath;


        visitQueue.Enqueue(startCoord);
        visited[startCoord.x, startCoord.y, startCoord.z] = true;

        //calulate offset for position in world



        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    Coords Cord = mapIndex[x, y, z];
                    Cord.m_gCost = int.MaxValue;
                    Cord.CalculateFCost();
                    Cord.LastCoord = null;
                }

            }
        }

        //now start calucalting the value of the startnode 
        //calulates cost between start node and node the object is trying to reach
        startCoord.m_gCost = 0;
        startCoord.m_hCost = calculateDistanceCost(startCoord, EndCoord);
        startCoord.CalculateFCost();

        //Algorthim cycle---- A*


        //while we still have nodes on the open list
        while (visitQueue.Count > 0)
        {
            Coords CurrentCoord = visitQueue.Dequeue();

            if (EndCoord == CurrentCoord)
            {
                //got to final node
                return calcuatePath(EndCoord, uWidth);
            }


            foreach (Coords neighbourN in getNeighbourList(CurrentCoord, uWidth, uWidth, uDepth))
            {
                if (visited[neighbourN.x, neighbourN.y,neighbourN.z]) continue;

                //checks if can walk past terrain
                if (neighbourN.IsWalkable == false)
                {
                    continue;
                }

                //check to see if have faster path on the current node than the neighbouring nodes
                int tentativeGCost = CurrentCoord.m_gCost + calculateDistanceCost(CurrentCoord, neighbourN);

                if (tentativeGCost < neighbourN.m_gCost)
                {

                    //update values of neigbour and make sure on open list since it is a faster path towards endnode
                    neighbourN.LastCoord = CurrentCoord;
                    neighbourN.m_gCost = tentativeGCost;
                    neighbourN.m_hCost = calculateDistanceCost(neighbourN, EndCoord);
                    neighbourN.CalculateFCost();

                    if (!visited[neighbourN.x, neighbourN.y,neighbourN.z])
                    {
                        visitQueue.Enqueue(neighbourN);
                        visited[neighbourN.x, neighbourN.y,neighbourN.z] = true;
                    }
                }
            }
        }

        //out of nodes on list (searched through whole map and cant find path

        Debug.Log(mapIndex[xEnd, yEnd, zEnd].IsWalkable);
        mapIndex[xEnd, yEnd, zEnd].IsWalkable = false;


        Debug.Log("null");
        return null;

    }


    private static List<Vector3> calcuatePath(Coords EndNode, int uwidth)
    {
        List<Coords> path = new List<Coords>();
        path.Add(EndNode);
        Coords currentNode = EndNode;

        while (currentNode.LastCoord != null)
        {
            path.Add(currentNode.LastCoord);
            currentNode = currentNode.LastCoord;

        }

        path.Reverse();


        if (path == null)
        {
            return null;
        }

        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (Coords coord in path)
            {
                vectorPath.Add(coordToWorld(coord, uwidth));
            }
            return vectorPath;
        }

    }
    private static int calculateDistanceCost(Coords a, Coords b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int zDistance = Mathf.Abs(a.z - b.z);
        int distance = xDistance + yDistance + zDistance;

        // returns the cost between the two distances
        return MOVEMENT_COST * distance;
    }



    private static List<Coords> getNeighbourList(Coords currentNode, int unitWidth, int unitHeight, int unitDepth)
    {
        List<Coords> neighbourList = new List<Coords>();

        // Define possible neighbor offsets


        int[,,] offsets = new int[,,] {
        { {-1, 0, 0}, {1, 0, 0}, {0, -1, 0}, {0, 1, 0}, {-1, -1, 0}, {-1, 1, 0},{1, -1, 0},{1, 1, 0},{0, 0, 0} },
        { {-1, 0, 1}, {1, 0, 1}, {0, -1, 1}, {0, 1, 1}, {-1, -1, 1}, {-1, 1, 1},{1, -1, 1},{1, 1, 1},{0, 0, 1} },
        { {-1, 0, -1}, {1, 0, -1}, {0, -1, -1}, {0, 1, -1}, {-1, -1, -1}, {-1, 1, -1},{1, -1, -1},{1, 1, -1},{0, 0, -1} }
    };


        //test the K
        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            for (int j = 0; j < offsets.GetLength(1); j++)
            {
                for (int k = 0; k < offsets.GetLength(2); k++)
                {
                    int newX = currentNode.x + offsets[i, j, 0];
                    int newY = currentNode.y + offsets[i, j, 1];
                    int newZ = currentNode.z + offsets[i, j, 2];

                    if (newX >= 0 && newX < width && newY >= 0 && newY < height && newZ >= 0 && newZ < depth)
                    {
                        // Check if the new coordinates are within bounds
                        if (isAreaWalkable(newX, newY, newZ, unitWidth, unitHeight, unitDepth))
                        {
                            neighbourList.Add(mapIndex[newX, newY, newZ]);
                        }
                    }
                }
            }

            return neighbourList;
        }

        return null;
    }


    private static bool isAreaWalkable(int x, int y, int z, int unitwidth, int unitheight, int unitDepth)
    {
        for (int i = x; i < x + unitwidth; i++)
        {
            for (int j = y; j < y + unitheight; j++)
            {
                for (int k = z; k < z + unitDepth; k++)
                {
                    if (i < 0 || i >= width || j < 0 || j >= height || k < 0 || k >= depth || !mapIndex[i, j, z].IsWalkable)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }



    public static List<Vector3> findAllPaths(int xSt, int ySt, int movement, int unitWidth, int unitHeight, int unitDepth)
    {
        List<Vector3> allPaths = new List<Vector3>();
        return null;
        //tempoirary
        int temp = 0;
        if (unitWidth < 2)
            temp = 1;

        /*
        Debug.Log(xSt + "." + ySt + "." + movement);
        // calculate paths to all walkable end points
        for (int i = xSt - (movement - temp); i < xSt + movement; i++)
        {
            for (int j = ySt - (movement - temp); j < ySt + movement; j++)
            {
                
                if (i>-1 && j > -1 &&  mapIndex[i, j].IsWalkable)
                {
                    List<Vector3> path = findPath(i, j, xSt, ySt, unitWidth, unitHeight,unitDepth);

                    if (path != null)
                    {
                        foreach (Vector3 coords in path)
                        {
                            allPaths.Add(coords);
                        }

                    }
                }
            }
        }

        return allPaths;
    }
*/

    }
}
