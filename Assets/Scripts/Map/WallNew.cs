using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNew : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Coords lol = World_Pathfinding.worldToCoord(transform.position);
        World_Pathfinding.setNotwalkble(lol.x, lol.y, lol.z) ;
        World_Pathfinding.setType(lol.x, lol.y +1, lol.z,Tile_Type.floor);
    }

}
