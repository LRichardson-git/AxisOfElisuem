using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNew : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Coords lol = World_Pathfinding.Instance.worldToCoord(transform.position);
        World_Pathfinding.Instance.setNotwalkble(lol.x, lol.y, lol.z) ;
        World_Pathfinding.Instance.setType(lol.x, lol.y +1, lol.z,Tile_Type.floor);
    }

}
