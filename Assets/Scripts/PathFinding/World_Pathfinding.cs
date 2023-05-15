using System.Collections.Generic;
using UnityEngine;

public static class World_Pathfinding
{


    private static int MOVEMENT_COST = 10;

    private static Coords[,] mapIndex;

    public static int width = 30, height = 30;

    public static int CellSize = 10;

    static World_Pathfinding()
    {

        mapIndex = new Coords[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapIndex[x, y] = new Coords(x, y);
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
        Vector3Int temp = new Vector3Int(coord.x,coord.y,0);
        newPos = TileMapper.Instance.tilemap.CellToWorld(temp);


        
        newPos.x += (width * 5);
       newPos.y += (width * 5);
  
       
        return newPos;
    }

    public static Vector3Int worldToCoord(Vector3 Pos, int width)
    {
        Vector3Int Grid;
        if (width > 1)
        {
            Pos.x -= ((width-1) *5);
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

    public static List<Vector3> findPath(int xEnd, int yEnd, int xSt, int ySt, int uWidth, int uHeight)
    {
        //Debug.Log("PATH");
        if (xEnd < 0 || xEnd > 198 || yEnd < 0 || yEnd > 198)
            return null;


        

        if (!isAreaWalkable(xEnd, yEnd, uWidth, uWidth))
        {
            Debug.Log("not walkable");
            return null;
        }

        Queue<Coords> visitQueue = new Queue<Coords>();
        bool[,] visited = new bool[width, height];


        Coords startCoord = mapIndex[xSt, ySt];
        Coords EndCoord = mapIndex[xEnd, yEnd];
        //  List<Coords> FinalPath;


        visitQueue.Enqueue(startCoord);
        visited[startCoord.x, startCoord.y] = true;

        //calulate offset for position in world
        


            for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Coords Cord = mapIndex[x, y];
                Cord.m_gCost = int.MaxValue;
                Cord.CalculateFCost();
                Cord.LastCoord = null;

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


            foreach (Coords neighbourN in getNeighbourList(CurrentCoord, uWidth, uWidth))
            {
                if (visited[neighbourN.x, neighbourN.y]) continue;

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

                    if (!visited[neighbourN.x, neighbourN.y])
                    {
                        visitQueue.Enqueue(neighbourN);
                        visited[neighbourN.x, neighbourN.y] = true;
                    }
                }
            }
        }

        //out of nodes on list (searched through whole map and cant find path

        Debug.Log(mapIndex[xEnd, yEnd].IsWalkable);
        mapIndex[xEnd, yEnd].IsWalkable = false;


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
        int distance = xDistance + yDistance;

        // returns the cost between the two distances
        return MOVEMENT_COST * distance;
    }



    private static List<Coords> getNeighbourList(Coords currentNode, int unitWidth, int unitHeight)
    {
        List<Coords> neighbourList = new List<Coords>();

        // Define possible neighbor offsets
        int[,] offsets = new int[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }, { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };

        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            int newX = currentNode.x + offsets[i, 0];
            int newY = currentNode.y + offsets[i, 1];
            if (newX >= 0 && newX < width && newY >= 0 && newY < height)
            {
                // Check if the new coordinates are within bounds
                if (isAreaWalkable(newX, newY, unitWidth, unitHeight))
                {
                    neighbourList.Add(mapIndex[newX, newY]);
                }
            }
        }

        return neighbourList;
    }


    private static bool isAreaWalkable(int x, int y, int unitwidth, int unitheight)
    {
        for (int i = x; i < x + unitwidth; i++)
        {
            for (int j = y; j < y + unitheight; j++)
            {
                if (i < 0 || i >= width || j < 0 || j >= height || !mapIndex[i, j].IsWalkable)
                {
                    return false;
                }
            }
        }
        return true;
    }



    public static List<Vector3> findAllPaths(int xSt, int ySt, int movement, int unitWidth, int unitHeight)
    {
        List<Vector3> allPaths = new List<Vector3>();

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
                    List<Vector3> path = findPath(i, j, xSt, ySt, unitWidth, unitHeight);

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
