using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmouse : MonoBehaviour
{
    Ray ray;
    public Camera cam;
    RaycastHit hit;
    World_Pathfinding path;
    InputManager inputManager;
    private void Start()
    {
       // path = World_Pathfinding.Instance;
      //  inputManager = InputManager.Instance;
       // Debug.Log("starting");
    }



    void FixedUpdate()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
      //  Debug.Log("Mouse position: " + inputManager.MousePos);

        if (Physics.Raycast(ray, out hit))
        {
           // Debug.Log("Raycast hit: " + hit.point);

            if (hit.collider.tag != "Finish")
           {
           
               Vector3 worldPos = World_Pathfinding.Instance.coordToWorld(World_Pathfinding.Instance.worldToCoord(hit.point), 1);
              //  Debug.Log("World position: " + worldPos);
               transform.position = worldPos;
            }
        }
    }
}