using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    Ray ray;
    Camera cam;
    RaycastHit hit;
    private void Start()
    {
        cam = Camera.main; 
    }



    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        

        if (Physics.Raycast(ray, out hit))
        {
            transform.position = World_Pathfinding.coordToWorld(World_Pathfinding.worldToCoord(hit.point),1,1);
        }
    }
}
