using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update
    public int x = 0;
    public int y = 0;
    public int z = 0;
    public int size = 0;
    void Start()
    {





        x = (int)(transform.position.x - 5) / 10;
        y = (int)(transform.position.y - 10) / 10;
        z = (int)(transform.position.z) / 10;


        World_Pathfinding.Instance.setType(x, y, z, Tile_Type.Wall);
        World_Pathfinding.Instance.setType(x, y + 1, z, Tile_Type.Wall);

    }

    
}
