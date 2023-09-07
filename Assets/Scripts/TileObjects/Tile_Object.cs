using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Object :MonoBehaviour
{

    public int x, y ,z, width, depth;

    public virtual void Setup(int x, int y, int z,int width, int depth)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        init();
    }

    private void init()
    {
        transform.transform.position = World_Pathfinding.coordToWorld(x, y, z, width, depth);
        World_Pathfinding.setNotwalkble(x, y, z);
    }


}
