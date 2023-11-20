using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    Ray ray;
    public Camera cam;
    RaycastHit hit;
    testfinding path;
    //IDK why but if I used inputmanager or a refernce to worldpathdinng it didnt work in build dosent make sense

    private void Start()
    {
        path = new testfinding();
    }

    void FixedUpdate()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {

            if (hit.collider.tag != "Finish")
            {

                Vector3 worldPos = path.coordToWorld(path.worldToCoord(hit.point), 1);
                transform.position = worldPos;
            }
        }
    }
}
