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
    public int aim = 50;
    public int crit = 0;
    public int aimModifer = 0;



    //Networked so client can know
    public int HP = 5;
    //maybe dir makes more sense with hitchanges with it?
    List<TargetData> InVision = new List<TargetData>();
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
        InVision = new List<TargetData>();
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
    public void addToList(TargetData unit) { InVision.Add(unit); }
    
    public void setList(List<TargetData> units) { InVision = units; }

    public void DeleteList() { InVision.Clear(); }

    public List<TargetData> getList() { return InVision; }

    public void CheckCover() { covers = Shooting.Instance.CalulateCover(this); }

    public void DeleteCover() { covers.Clear(); }



}
