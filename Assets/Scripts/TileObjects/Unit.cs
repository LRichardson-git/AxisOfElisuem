using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class Unit : Tile_Object
{
    [SerializeField]

    //only server needs to know
    public int movementPoints = 2;
    public int height = 1;
    public bool flying = false;
    public const int maxHP = 5;
    public int Vision = 20;
    public int range = 18;

    //Networked so client can know
    public int HP = 5;
    List<Unit> InVision = new List<Unit>();
    public GameObject targetPoint;
    [SerializeField]
    public List<Cover> covers;

    //client
    protected bool selected = false;
    private UnitInformationUpdater Info;
    
    public Unit(int x, int y, int z, int width, int depth)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.width = width;
        this.depth = depth;
    }

    private void Start()
    {
        Setup(x, y,z,width,depth);
        Info = GetComponent<UnitInformationUpdater>();
        covers = new List<Cover>();
    }

    internal void Deselect()
    {
        selected = false;
    }

    internal void Select()
    {
        selected = true;
    }

    public bool getSelected() { return selected; }
    public void addToList(Unit unit) { InVision.Add(unit); }
    
    public void setList(List<Unit> units) { InVision = units; }

    public List<Unit> getList() { return InVision; }

    public void CheckCover() { covers = Shooting.Instance.CalulateCover(this); }

    public void DeleteCover() { covers.Clear(); }



}
