using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Object :MonoBehaviour
{

    public int x, y ,z;

    public virtual void Setup(int x, int y, int z,int width)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        init(width);
    }

    private void init(int width)
    {
        transform.transform.position = World_Pathfinding.coordToWorld(x, y, z, width);
        World_Pathfinding.setNotwalkble(x, y, z);
    }


}
