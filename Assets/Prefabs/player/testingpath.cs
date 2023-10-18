using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingpath 
{

    private static int MOVEMENT_COST = 10;
    private static int MOVEMENT_DIAGONAL_COST = 14;

    public Coords[,,] mapIndex;

    static int width = 40, height = 40, depth = 15;

    public static int CellSize = 10;




     public  testingpath()
    {
        mapIndex = new Coords[width, depth, height];

        mapIndex = World_Pathfinding.Instance.getIndex();

    }


    //Coords

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


    public int findPath(int xEnd, int yEnd, int zEnd, int xSt, int ySt, int zSt)
    {
        if (xEnd < 0 || xEnd >= width || yEnd < 0 || yEnd >= depth || zEnd < 0 || zEnd >= height || mapIndex[xEnd, yEnd, zEnd].type == Tile_Type.Ladder) { return 0; }

        if (!mapIndex[xEnd, yEnd, zEnd].IsWalkable)
        {
            return 0;
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
                return calcuatePath(currentNode);
            }

            // Get the neighboring nodes of the current node
            List<Coords> neighbors = getNeighbourList(currentNode);

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
        return 0;
    }


    private int calcuatePath(Coords EndNode)
    {
        List<Coords> path = new List<Coords>();
        path.Add(EndNode);
        Coords currentNode = EndNode;

        while (currentNode.LastCoord != null)
        {
            path.Add(currentNode.LastCoord);
            currentNode = currentNode.LastCoord;

        }
        return path.Count;

    }
    public static int calculateDistanceCost(Coords a, Coords b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int zDistance = Mathf.Abs(a.z - b.z);


        int diagonalDistance = Mathf.Min(xDistance, zDistance);
        int straightDistance = Mathf.Max(xDistance, zDistance) - diagonalDistance;

        int cost = MOVEMENT_DIAGONAL_COST * diagonalDistance + MOVEMENT_COST * straightDistance + MOVEMENT_COST * yDistance;

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

    private  List<Coords> getNeighbourList(Coords currentNode)
    {
        List<Coords> neighbourList = new List<Coords>();

        // Define possible neighbor offsets


        int x = currentNode.x;
        int y = currentNode.y;
        int z = currentNode.z;
        int newX = x;
        int newY = y;
        int newZ = z;


        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                for (int k = -1; k < 2; k++)
                {
                    if (i == 0 && j == 0 && k == 0)
                        continue;

                    newX = x + i;
                    newY = y + j;
                    newZ = z + k;

                    if (newX >= 0 && newX < width && newY >= 0 && newY < depth && newZ >= 0 && newZ < height)
                    {
                        // Check if the new coordinates are within bounds
                        if (isAreaWalkable(newX, newY, newZ))
                        {
                            neighbourList.Add(mapIndex[newX, newY, newZ]);
                        }
                    }

                }
            }

        }
        return neighbourList;

    }


    private  bool isAreaWalkable(int x, int y, int z)
    {
        if (!mapIndex[x, y, z].IsWalkable)
            return false;

        return true;
    }




}
