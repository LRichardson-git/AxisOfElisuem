using System.Collections.Generic;
using UnityEngine;

public static class World_Pathfinding
{
    //Z is up y IS DEPTH

    private static int MOVEMENT_COST = 10;
    private static int MOVEMENT_DIAGONAL_COST = 14;

    private static Coords[,,] mapIndex;

    public static int width = 30, height = 20, depth = 20;

    public static int CellSize = 10;

    static World_Pathfinding()
    {
        mapIndex = new Coords[width, depth, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                for (int z = 0; z < height; z++)
                {
                    mapIndex[x, y, z] = new Coords(x, y, z);
                }
            }
        }

        TestingSetup();

        setNotwalkble(9, 1, 9);
    }


    //Helper Functions

    //getters
    public static Coords[,,] getIndex()
    {
        return mapIndex;
    }

    //Setters

    public static void setWidthHeight(int x, int y, int z)
    {
        width = x; depth = y; height = z;
    }

    //SetWalkables
    //------------
    public static void setNotwalkble (int x, int y, int z)
    {
        mapIndex[x, y, z].settype(Tile_Type.Wall);
    }
    public static void setNotwalkble(Coords coord)
    {
        mapIndex[coord.x, coord.y, coord.z].settype(Tile_Type.Wall);
    }

    public static void setNotwalkble(Coords cord, int x, int y,int z) //fix this make it do all or not dosent mastter
    {
        for (int i = 0; i < x ; i++)
        {
            for (int j = 0; j < y ; j++)

                for (int k = 0; k < z ; k++)
                {
                    mapIndex[cord.x + i, cord.y + j, cord.z + k].settype(Tile_Type.Wall);
                }
        }
    }

    //Coords
    
    public static Vector3Int coordToWorld(Coords coord, int uWidth, int uDepth) //y is height
    {
        Vector3Int newPos = new Vector3Int();
        newPos.x = coord.x * 10 + (5 * uWidth);
        newPos.y = coord.y * 10 + (5 * uDepth) ;
        newPos.z = coord.z * 10 + (5 * uWidth);
        return newPos;
    }

    public static Vector3 coordToWorld(int x, int y, int z, int uWidth, int uDepth)
    {
        Vector3Int newPos = new Vector3Int();
        newPos.x = x * 10 + (5 * uWidth);
        newPos.y = y * 10 + (5 * uDepth);
        newPos.z = z * 10 + (5 * uWidth);
        return newPos;
    }

    public static Coords worldToCoord(Vector3 Pos)
    {
        Coords Grid = new Coords((int)Mathf.Floor((Pos.x) / CellSize),
                                 (int)Mathf.Floor((Pos.y) / CellSize),
                                 (int)Mathf.Floor((Pos.z) / CellSize));
        return Grid;
    }

    public static Vector3Int worldToCoord(Vector3 Pos, int uWidth, int uDepth)
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

    //Find Path

    public static List<Vector3> findPath(int xEnd, int yEnd, int zEnd, int xSt, int ySt, int zSt, int uWidth, int uHeight, int uDepth, bool flight)
    {
        if (xEnd < 0 || xEnd > width || yEnd < 0 || yEnd > depth || zEnd <0 || zEnd > height) { Debug.Log("invalid point"); return null; }

        if (!isAreaWalkable(xEnd, yEnd, zEnd, uWidth, uHeight, uDepth,flight))
        {
            Debug.Log("not walkable");
            return null;
        }

        Coords startCoord = mapIndex[xSt, ySt, zSt];
        Coords EndCoord = mapIndex[xEnd, yEnd, zEnd];

        //depth for hegiht
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                for (int z = 0; z < height; z++)
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

        //open closed list
        List<Coords> openList = new List<Coords>();
        List<Coords> closedList = new List<Coords>();

        // Add the start node to the open list
        openList.Add(startCoord);

        while (openList.Count > 0)
        {
            // Find the node with the lowest fCost in the open list
            Coords currentNode = FindLowestFCostNode(openList);

            // Move the current node from open to closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // Check if the current node is the target node
            if (currentNode == EndCoord)
            {
                // Path found, construct the path
                List<Vector3> path = calcuatePath(currentNode,uWidth,uDepth);
                return path;
            }

            // Get the neighboring nodes of the current node
            List<Coords> neighbors = getNeighbourList(currentNode, uWidth, uHeight, uDepth, flight);

            foreach (Coords neighbor in neighbors)
            {
                if (closedList.Contains(neighbor))
                    continue;

                int tentativeGCost = currentNode.m_gCost + calculateDistanceCost(currentNode, neighbor);

                if (!openList.Contains(neighbor) || tentativeGCost < neighbor.m_gCost)
                {
                    neighbor.m_gCost = tentativeGCost;
                    neighbor.m_hCost = calculateDistanceCost(neighbor, EndCoord);
                    neighbor.CalculateFCost();
                    neighbor.LastCoord = currentNode;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        // No path found
        return null;

    }

    private static List<Vector3> calcuatePath(Coords EndNode, int uwidth, int uDepth)
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
                vectorPath.Add(coordToWorld(coord, uwidth, uDepth));
            }

            return vectorPath;
        }

    }
    public static int calculateDistanceCost(Coords a, Coords b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int zDistance = Mathf.Abs(a.z - b.z);


        int diagonalDistance = Mathf.Min(xDistance, zDistance);
        int straightDistance = Mathf.Max(xDistance, zDistance) - diagonalDistance;

        int cost = MOVEMENT_DIAGONAL_COST * diagonalDistance + MOVEMENT_COST  * straightDistance + MOVEMENT_COST * yDistance;

        return cost;
    }

    private static Coords FindLowestFCostNode(List<Coords> nodeList)
    {
        Coords lowestFCostNode = nodeList[0];

        for (int i = 1; i < nodeList.Count; i++)
        {
            if (nodeList[i].m_fCost < lowestFCostNode.m_fCost)
            {
                lowestFCostNode = nodeList[i];
            }
        }

        return lowestFCostNode;
    }

    private static List<Coords> getNeighbourList(Coords currentNode, int unitWidth, int unitHeight, int unitDepth, bool flight)
    {
        List<Coords> neighbourList = new List<Coords>();

        // Define possible neighbor offsets


       for(int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                for (int k = -1; k < 2; k++)
                {
                    if (i == 0 && j == 0 && k == 0)
                        continue;

                    int newX = currentNode.x + i;
                    int newY = currentNode.y + j;
                    int newZ = currentNode.z + k;

                    if (newX >= 0 && newX < width && newY >= 0 && newY < depth && newZ >= 0 && newZ < height)
                    {
                        // Check if the new coordinates are within bounds
                        if (isAreaWalkable(newX, newY, newZ, unitWidth, unitHeight, unitDepth,flight))
                        {
                            neighbourList.Add(mapIndex[newX, newY, newZ]);
                        }
                    }

                }
            }
            
        }
        return neighbourList;

    }


    private static bool isAreaWalkable(int x, int y, int z, int unitwidth, int unitheight, int unitDepth, bool flying)
    {

        if (mapIndex[x, y, z].type == Tile_Type.air && flying == false)
            return false;


        for (int i = x; i < x + unitwidth; i++)
        {
            for (int j = y; j < y + unitDepth; j++)
            {
                for (int k = z; k < z + unitheight; k++)
                {
                    
                    if (i < 0 || i >= width || j < 0 || j >= depth || k < 0 || k >= height || mapIndex[i, j, k].type == Tile_Type.Wall)
                    {
                        return false;
                    }
                    
                }
            }
        }
        return true;
    }


    /*
    public static List<Vector3> findAllPaths(int xSt, int ySt, int movement, int unitWidth, int unitHeight, int unitDepth)
    {
        List<Vector3> allPaths = new List<Vector3>();
        return null;
        //tempoirary
        int temp = 0;
        if (unitWidth < 2)
            temp = 1;

        
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
    }

*/

private static void TestingSetup()
        {
            for (int x = 0; x < 7; x++)
            {
                for (int z = 14; z < 20; z++)
                {
                    mapIndex[x, 3, z].settype(Tile_Type.floor);
                }
            }


            for (int y = 0; y < 5; y++)
            {
                mapIndex[2, y, 14].settype(Tile_Type.floor);
            }

        }



    private static Vector3 calcDirection(int x, int y, int z, int i , int j, int k)
    {
        Vector3Int baseP = coordToWorld(mapIndex[x, y, z], 1, 2);
        Vector3Int TargetP = coordToWorld(mapIndex[i, j, k], 1, 2);

        Vector3 direction = TargetP - baseP;
        direction.Normalize();

        Debug.Log(direction);
        return direction;

    }



    public static List<Cover> CheckCover(int x, int y, int z)
    {

        List<Cover> covers = new List<Cover>();

        foreach (Coords coords in mapIndex)
        {
            if (coords.type == Tile_Type.Wall)
                Debug.Log(coords.x + " : " + coords.y + " : " + coords.z);
        }


        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y + 1; j > y; j--)
            {
                for (int k = z - 1; k <= z + 1; k++)
                {
                    
                    if (mapIndex[x, y, z].getTypeof() == Tile_Type.Wall)
                    {
                        Vector3 direction = calcDirection(x, y, z, i, j, k);
                        if (covers == null)
                            covers.Add(new Cover(y, j, direction));

                        else
                        {

                            foreach (Cover cover in covers)
                            {

                                if (cover.Direction != direction)
                                    covers.Add(new Cover(y, j, direction));
                            }
                        }
                    }
                    


                }
            }
        }

        return covers;
    }




    
}
