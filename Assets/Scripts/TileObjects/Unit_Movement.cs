using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Movement : MonoBehaviour
{
    private Unit _unit;
    public float speed = 5f;
    private float posOffset = 0f;


    private void Awake()
    {
        _unit = GetComponent<Unit>();
        //lineRenderer = InputManager.Instance.GetComponent<LineRenderer>();

    }


    public void moveToTarget(int endX, int endY)
    {

        //call this end of turn if set to auto go somewhere
        // Find the path using World_Pathfinding's findPath method
        List<Vector3> path = World_Pathfinding.findPath(endX, endY, _unit.x, _unit.y, _unit.width, _unit.height);
        if (path != null)
        {
            //Debug.Log(path[0] + " " + path[1]);
            //need to +1 for some reason
            //Debug.Log(endX + ", " + endY);
            int limitedPathLength = Mathf.Min(path.Count, _unit.movementPoints);
            List<Vector3> limitedPath = path.GetRange(0, limitedPathLength);

            // Move the Unit along the limited path
            StartCoroutine(moveAlongPath(limitedPath));

            // Update the unit's movement path
            _unit.movementPoints -= limitedPathLength;
        }
    }

    private IEnumerator moveAlongPath(List<Vector3> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 targetPosition = path[i];
            while (Vector3.Distance(transform.position, targetPosition) >= speed * Time.deltaTime)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
                yield return null;
            }
        }
        Vector3Int temportayChangethisplease = World_Pathfinding.worldToCoord(transform.position, _unit.width);
        _unit.x = temportayChangethisplease.x;
        _unit.y = temportayChangethisplease.y;
    }


}
