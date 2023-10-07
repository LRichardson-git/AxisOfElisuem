using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Cover 
{
    public coverHeight height;
    public Vector3 Direction;

    public Cover(coverHeight height, Vector3 Direction)
    {
        this.height = height;
        this.Direction = Direction;
    }

    public Cover(int y,int y2, Vector3 Direction)
    {
        if (y == y2)
            height = coverHeight.Tall;
        else
            height = coverHeight.Short;

        this.Direction = Direction;
    }


}
