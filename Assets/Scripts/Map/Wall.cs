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





        x = (int)transform.localScale.x / 10;
        y = (int)transform.localScale.y / 10;
        z = (int)transform.localScale.z / 10;

        if (x > z)
        {
            
            float xE = transform.position.x / 2 - x;
            float yE = transform.position.y / 2 + y;
            Vector3 pos = new Vector3(xE,yE, transform.position.z);
            World_Pathfinding.setNotwalkble(World_Pathfinding.worldToCoord(pos),x,y,z);
            Debug.Log(World_Pathfinding.worldToCoord(pos));

        }
            
        else if (z > x)
        {
            float zE = transform.position.z - (10 * z /2);
            float yE = transform.position.y / 2 + y;
            Vector3 pos = new Vector3(transform.position.x, yE, zE);
            World_Pathfinding.setNotwalkble(World_Pathfinding.worldToCoord(pos), x, y,z);
            Debug.Log(World_Pathfinding.worldToCoord(pos));

        }

        else
            World_Pathfinding.setNotwalkble(World_Pathfinding.worldToCoord(transform.position));


    }

    
}
