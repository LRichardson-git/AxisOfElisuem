using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    Ray ray;
    public Camera cam;
    RaycastHit hit;

    //IDK why but if I used inputmanager or a refernce to worldpathdinng it didnt work in build dosent make sense
    void FixedUpdate()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {

            if (hit.collider.tag != "Finish")
            {

                Vector3 worldPos = World_Pathfinding.Instance.coordToWorld(World_Pathfinding.Instance.worldToCoord(hit.point), 1);
                transform.position = worldPos;
            }
        }
    }
}
