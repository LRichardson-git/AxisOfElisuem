using System.Collections.Generic;
using UnityEngine;

public static class World_Pathfinding
{
    //Z is up y IS DEPTH

    private static int MOVEMENT_COST = 10;
    private static int MOVEMENT_DIAGONAL_COST = 14;

    private static Coords[,,] mapIndex;

    public static int width = 30, height = 30, depth = 20;

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


    }

    public static void setNotwalkble (int x, int y, int z)
    {
        mapIndex[x, y, z].IsWalkable = false;
    }
    public static RaycastHit2D getWorldMouse()
    {
        Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);
        return hit;
    }
    public static void setWidthHeight(int x, int y, int z)
    {
        width = x;  depth = y; height = z;
    }



    public static Vector3 coordToWorld(Coords coord, int uWidth) //y is height
    {
        Vector3 newPos;
        newPos.x = coord.x * 10 + 5;
        newPos.y = coord.y * 10 + 5;
        newPos.z = coord.z * 10 + 5;
        return newPos;
    }

    public static Vector3Int worldToCoord(Vector3 Pos, int uWidth)
    {
        Vector3Int Grid = new Vector3Int();
        Grid.x = (int)Mathf.Floor((Pos.x) / CellSize);
        Grid.y = (int)Mathf.Floor((Pos.y) / CellSize);
        Grid.z = (int)Mathf.Floor((Pos.z) / CellSize);
        return Grid;
    }

    public static Coords worldToCoord(Vector3 Pos)
    {
        Coords Grid = new Coords((int)Mathf.Floor((Pos.x) / CellSize),
                                 (int)Mathf.Floor((Pos.y) / CellSize),
                                 (int)Mathf.Floor((Pos.z) / CellSize));
        return Grid;
    }



    //init  in units
    public static Vector3 coordToWorld(int x, int y,int z, int width)
    {
        Vector3 newPos;
        newPos.x = x * 10 + 5;
        newPos.y = y * 10 + 5;
        newPos.z = z * 10 + 5;
        return newPos;
    }

    public static List<Vector3> findPath(int xEnd, int yEnd, int zEnd, int xSt, int ySt, int zSt, int uWidth, int uHeight, int uDepth)
    {
        //Debug.Log("PATH");
        if (xEnd < 0 || xEnd > width || yEnd < 0 || yEnd > depth || zEnd <0 || zEnd > height) { Debug.Log("invalid point"); return null; }


        if (!isAreaWalkable(xEnd, yEnd, zEnd, uWidth, uHeight, uDepth))
        {
            Debug.Log("not walkable");
            return null;
        }

        Coords startCoord = mapIndex[xSt, ySt, zSt];
        Coords EndCoord = mapIndex[xEnd, yEnd, zEnd];
        //  List<Coords> FinalPath;


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
                List<Vector3> path = calcuatePath(currentNode,uWidth);
                return path;
            }

            // Get the neighboring nodes of the current node
            List<Coords> neighbors = getNeighbourList(currentNode, uWidth, uHeight, uDepth);

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
            //for (int i = 0; i < vectorPath.Count; i++)
               // Debug.Log(vectorPath[i]);
            return vectorPath;
        }

    }
    public static int calculateDistanceCost(Coords a, Coords b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int zDistance = Mathf.Abs(a.z - b.z);

        //if on straight land


        int diagonalDistance = Mathf.Min(xDistance, zDistance);
        //int straightDistance = Mathf.Max(xDistance, yDistance, zDistance) - diagonalDistance;
        int straightDistance = Mathf.Max(xDistance, zDistance) - diagonalDistance;
        int remaining = Mathf.Max(xDistance, yDistance, zDistance) - straightDistance - diagonalDistance;

        int cost = MOVEMENT_DIAGONAL_COST * diagonalDistance + MOVEMENT_COST  * straightDistance + MOVEMENT_COST * yDistance;

        return cost;
    }



    private static List<Coords> getNeighbourList(Coords currentNode, int unitWidth, int unitHeight, int unitDepth)
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
                        if (isAreaWalkable(newX, newY, newZ, unitWidth, unitHeight, unitDepth))
                        {
                            neighbourList.Add(mapIndex[newX, newY, newZ]);
                        }
                    }

                }
            }
            
        }
        return neighbourList;

    }


    private static bool isAreaWalkable(int x, int y, int z, int unitwidth, int unitheight, int unitDepth)
    {
        for (int i = x; i < x + unitwidth; i++)
        {
            for (int j = y; j < y + unitDepth; j++)
            {
                for (int k = z; k < z + unitheight; k++)
                {
                    if (i < 0 || i >= width || j < 0 || j >= depth || k < 0 || k >= height || !mapIndex[i, j, k].IsWalkable)
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
