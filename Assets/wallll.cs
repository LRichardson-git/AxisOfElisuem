using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is terrible 
//just to do it quickl;y
public class wallll : MonoBehaviour
{

    public Vector3Int pos;
    public Vector3Int dimension;
    public bool setfloor = true;
    // Start is called before the first frame update
    void Start()
    {
        
        }

    public void init()
    {
        if (setfloor)
        {

            for (int i = pos.x; i < pos.x + dimension.x; i++)
            {
                for (int j = pos.y; j < pos.y + dimension.y; j++)
                {

                    for (int k = pos.z; k < pos.z + dimension.z; k++)
                    {
                        World_Pathfinding.Instance.setType(i, j, k, Tile_Type.Wall);
                        World_Pathfinding.Instance.setType(i, j + 1, k, Tile_Type.floor);
                    }
                }
            }
        }

        else
        {
            for (int i = pos.x; i < pos.x + dimension.x; i++)
            {
                for (int k = pos.z; k < pos.x + dimension.z; k++)
                {

                    for (int j = pos.y; j < pos.x + dimension.y; j++)
                    {
                        World_Pathfinding.Instance.setType(i, j, k, Tile_Type.Wall);
                    }
                }
            }
        }

    }
    }
