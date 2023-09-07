using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update
    public int x = 1;
    public int y = 1;
    public int z = 1;
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
        }
            
        else if (z > x)
        {
            float zE = transform.position.z - (10 * z /2);
            float yE = transform.position.y / 2 + y;
            Vector3 pos = new Vector3(transform.position.x, yE, zE);
            World_Pathfinding.setNotwalkble(World_Pathfinding.worldToCoord(pos), x, y,z);
        }

        else
            World_Pathfinding.setNotwalkble(World_Pathfinding.worldToCoord(transform.position));

        //gameObject.SetActive(false);

    }

    
}
