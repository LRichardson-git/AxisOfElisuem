using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coords
{

    //Pos
    public int x;
    public int y;
    public int z;

    public int m_gCost; //distance from start
    public int m_hCost; //distance if not account for  blockers
    public int m_fCost; //fcost distance to node

    //coord data
    public Coords LastCoord;
    public bool IsWalkable;
    public Tile_Type type;

    public Coords(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;

        IsWalkable = false;
        LastCoord = null;
        type = Tile_Type.air;

        if (y < 1)
        {
            type = Tile_Type.floor;
            IsWalkable = true;
        }
    }
    public void settype(Tile_Type type) { 
        this.type = type; 
        if (type == Tile_Type.floor || type == Tile_Type.Ladder)
            IsWalkable=true;
        else
            IsWalkable=false;
        }
    public void CalculateFCost() //movement cost
    {
        m_fCost = m_gCost + m_hCost;
    }

    public Tile_Type getTypeof()
    {
        return type;
    }



}