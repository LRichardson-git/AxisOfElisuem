using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solider : Unit
{
    [SerializeField]
    public List<GameObject> targetPoints;

    public GameObject gunModel;


    /*
    public Solider(int x, int y, int z, int width, int depth) : base(x, y, z, width, depth)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.width = width;
        this.depth = depth;
    }
    */
  
}
