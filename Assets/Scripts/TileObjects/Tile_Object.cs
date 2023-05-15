using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Object :MonoBehaviour
{

    public int x, y;

    public virtual void Setup(int x, int y, int width)
    {
        this.x = x;
        this.y = y;

        init(width);
    }

    private void init(int width)
    {
        transform.transform.position = World_Pathfinding.coordToWorld(x, y, width);
    }


}
