using System.Collections.Generic;
using UnityEngine;

public class World_Pathfinding :MonoBehaviour
{
    //Z is up y IS DEPTH

    private static int MOVEMENT_COST = 10;
    private static int MOVEMENT_DIAGONAL_COST = 14;

    private  Coords[,,] mapIndex;

    int width = 60, height = 40, depth = 13;

    public int CellSize = 10;

    public  Dictionary<Coords, Neighbors> NeighbourList;

    public static World_Pathfinding Instance;


    public List<Vector3> EntryPoints;
    private void Awake()
    {
        Instance = this;

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


        // NeighbourList = preCalculateNeighbors();
    }

    private void Start()
    {
        TestingSetup();

        Wall[] walls = FindObjectsOfType<Wall>();

        // Call the init() function on each Wall object
        foreach (Wall wall in walls)
        {
            wall.init();
        }


        wallll[] wallll = FindObjectsOfType<wallll>();

        // Call the init() function on each Wall object
        foreach (wallll wall in wallll)
        {
            wall.init();
        }
    }
    public  void setType(int x, int y, int z, Tile_Type type)
    {
        

        mapIndex[x, y, z].settype(type);
    }


  
    //getters
    public  Coords[,,] getIndex()
    {
        return mapIndex;
    }

    //Setters

    public  void setWidthHeight(int x, int y, int z)
    {
        width = x; depth = y; height = z;
    }

    //SetWalkables
    //------------
    public  void setNotwalkble (int x, int y, int z)
    {
        mapIndex[x, y, z].settype(Tile_Type.Wall);
    }
    public  void setNotwalkble(Coords coord)
    {
        mapIndex[coord.x, coord.y, coord.z].settype(Tile_Type.Wall);
    }

    public  void setNotwalkble(Coords cord, int x, int y,int z) //fix this make it do all or not dosent mastter
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
    
    public  Vector3Int coordToWorld(Coords coord) //y is height
    {
        Vector3Int newPos = new Vector3Int();
        newPos.x = coord.x * 10 + 5;
        newPos.y = coord.y * 10 + (5 * 2) ;
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

    public  Vector3 coordToWorld(int x, int y, int z, int uWidth, int uDepth)
    {
        Vector3Int newPos = new Vector3Int();
        newPos.x = x * 10 + (5 * uWidth);
        newPos.y = y * 10 + (5 * uDepth);
        newPos.z = z * 10 + (5 * uWidth);

        if (uDepth == 1)
            newPos.y -=  5;

        return newPos;
    }

    public  Coords worldToCoord(Vector3 Pos)
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

    public  Vector3Int worldToCoord(Vector3 Pos, int uWidth, int uDepth)
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

    //Find Path

    public List<Vector3> findPath(int xEnd, int yEnd, int zEnd, int xSt, int ySt, int zSt)
    {
        if (xEnd < 0 || xEnd >= width || yEnd < 0 || yEnd >= depth || zEnd < 0 || zEnd >= height || mapIndex[xEnd, yEnd, zEnd].type == Tile_Type.Ladder) { return null; }

        if (!mapIndex[xEnd, yEnd, zEnd].IsWalkable)
        {
            return null;
        }

        foreach (Unit unit in UnitManager.Instance.GetUnitList())
            if (unit.x == xEnd && unit.y == yEnd && unit.z == zEnd)
                return null;


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
                List<Vector3> path = calcuatePath(currentNode);
                return path;
            }

            // Get the neighboring nodes of the current node
            List<Coords> neighbors = getNeighbourList(currentNode);

            foreach (Coords neighbor in neighbors)
            {
                if (closedList.Contains(neighbor) )
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

    public int findPathT(int xEnd, int yEnd, int zEnd, int xSt, int ySt, int zSt)
    {
        if (xEnd < 0 || xEnd >= width || yEnd < 0 || yEnd >= depth || zEnd < 0 || zEnd >= height || mapIndex[xEnd, yEnd, zEnd].type == Tile_Type.Ladder) { return 0; }

        if (!mapIndex[xEnd, yEnd, zEnd].IsWalkable)
        {
            return -1;
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
                List<Vector3> path = calcuatePath(currentNode);
                return path.Count;
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
        return -1;
    }


    private  List<Vector3> calcuatePath(Coords EndNode)
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
                vectorPath.Add(coordToWorld(coord));
            }

            return vectorPath;
        }

    }
    public  int calculateDistanceCost(Coords a, Coords b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int zDistance = Mathf.Abs(a.z - b.z);


        int diagonalDistance = Mathf.Min(xDistance, zDistance);
        int straightDistance = Mathf.Max(xDistance, zDistance) - diagonalDistance;

        int cost = MOVEMENT_DIAGONAL_COST * diagonalDistance + MOVEMENT_COST  * straightDistance + MOVEMENT_COST * yDistance;

        return cost;
    }

    private  Coords FindLowestFCostNode(List<Coords> nodeList)
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


    //not the best way but will change in future
    int diffrence(int num1, int num2)
    {
        int cout;

        cout = Mathf.Max(num2, num1) - Mathf.Min(num1, num2);
        return cout;
    }
    public int testdistance(Unit unit, int x , int y , int z)
    {
        Vector3Int temp = new Vector3Int(unit.x, unit.y, unit.z);
        Vector3Int temp2 = new Vector3Int(x, y, z);
        if (x < 0 || x >= width || y < 0 || y >= depth || z < 0 || z >= height || mapIndex[x, y, z].type == Tile_Type.Ladder || !mapIndex[x, y, z].IsWalkable) { return -1; }

        int tempint = (int)Vector3.Distance(temp, temp2);
        if (y != unit.y)
        {
            


                tempint += diffrence(unit.y, y);
            
        }



        //Debug.Log(x + " " + y + " " + z + " : " + tempint);
        if (tempint < unit.movementPoints)
        {





            return (int)(Vector3.Distance(temp, temp2));
        }
        else
            return -1;
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



    public class Neighbors
    {
        public List<Coords> NeighborsList { get; set; }

        public Neighbors (List<Coords> neighbours)
        {

            NeighborsList = neighbours;
        }
    }

    // Pre-calculate the neighboring nodes for each node in the map
    public  Dictionary<Coords, Neighbors> preCalculateNeighbors()
    {
        Dictionary<Coords,Neighbors> neighborsDict = new Dictionary<Coords, Neighbors>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                for (int z = 0; z < height; z++)
                {
                    Neighbors list = new Neighbors(getNeighbourList(mapIndex[x, y, z])); //list of coords
                    neighborsDict.Add(mapIndex[x, y, z], list);
                }
            }
        }

        return neighborsDict;
    }








    //soo bad but wanna finsih it
    private void TestingSetup()
    {


        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < 25; z++)
            {
                mapIndex[x, 4, z].settype(Tile_Type.floor);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 25; z < height; z++)
            {
                mapIndex[x, 0, z].settype(Tile_Type.floor);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 1; y < 4; y++)
                mapIndex[x, y, 25].settype(Tile_Type.Ladder);

        }

        for (int x = 0; x < 29; x++)
        {
            for (int y = 1; y < 4; y++)
                mapIndex[x, y, 25].settype(Tile_Type.Ladder);

        }

        for (int x = 33; x < width; x++)
        {
            for (int y = 1; y < 4; y++)
                mapIndex[x, y, 25].settype(Tile_Type.Ladder);

        }
        //base

        for (int x = 25; x < 35; x++)
        {
            for (int y = 5; y < 8; y++)
            {
                //mapIndex[x, y, 8].settype(Tile_Type.Ladder);
                
                mapIndex[x, y, 19].settype(Tile_Type.Ladder);
                
                }
        }

        
            for (int y = 5; y < 8; y++)
            {
                
                for (int z = 9; z < 19; z++)
                {
                    mapIndex[24, y, z].settype(Tile_Type.Ladder);
                    mapIndex[35, y, z].settype(Tile_Type.Ladder);
                }
            }

        //top

        for (int y = 8; y < 12; y++)
        {

            for (int z = 9; z < 13; z++)
            {
                mapIndex[27, y, z].settype(Tile_Type.Ladder);
                mapIndex[32, y, z].settype(Tile_Type.Ladder);
            }
        }

    }




    
}
