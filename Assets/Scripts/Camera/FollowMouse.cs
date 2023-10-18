using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    Ray ray;
    Camera cam;
    RaycastHit hit;
    World_Pathfinding path;
    private void Start()
    {
        cam = Camera.main;
        path = World_Pathfinding.Instance;
    }



    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        

        if (Physics.Raycast(ray, out hit))
        {
            transform.position = path.coordToWorld(path.worldToCoord(hit.point),1);
        }
    }
}
