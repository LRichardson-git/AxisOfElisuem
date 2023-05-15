using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Tile_Object
{
    [SerializeField]
    bool selected = false;
    public int movementPoints = 2;

    Unit_Movement _unit_Movement;

    public int width = 1;
    public int height = 1;

    private void Awake()
    {
        _unit_Movement = GetComponent<Unit_Movement>();
        
        Setup(x, y,width);
    }


    internal void Deselect()
    {
        selected = false;
    }

    internal void Select()
    {
        selected = true;
    }


    



}
