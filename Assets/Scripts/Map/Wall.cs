using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{


    public void init() {




       int x = (int)(transform.position.x - 5) / 10; //81
       int y = (int)(transform.position.y - 10) / 10;
       float epsilon = 0.1f;
       int z = (int)(transform.position.z - 5 + epsilon); //floating point messing up conversion maybe need to add too all
       z /= 10;


        World_Pathfinding.Instance.setType(x, y, z, Tile_Type.Wall);
        World_Pathfinding.Instance.setType(x, y + 1, z, Tile_Type.Wall);
    }
    
}
