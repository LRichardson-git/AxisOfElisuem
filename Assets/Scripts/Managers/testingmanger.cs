using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingmanger : MonoBehaviour
{
    public GameObject blocker;

    public static testingmanger Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
        {

            for (int x = 0; x < 30; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    for (int z = 0; z < 20; z++)
                    {
                        if (World_Pathfinding.getIndex()[x, y, z].type == Tile_Type.floor)
                            Instantiate(blocker,World_Pathfinding.coordToWorld(x,y,z,1,1),transform.rotation);
                    }
                }
            }
        }
    }
}
