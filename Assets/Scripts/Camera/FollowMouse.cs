using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    Ray ray;
    Coords temp;
    
    // Update is called once per frame

    private void Start()
    {
        Coords temp = new Coords(0,0,0);
        
    }
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log(hit.point);
            //Debug.Log("position:" + World_Pathfinding.worldToCoord(hit.point,1));
            transform.position = World_Pathfinding.coordToWorld(World_Pathfinding.worldToCoord(hit.point),1);
        }
    }
}
