using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Movement : MonoBehaviour
{
    private Unit _unit;
    public float speed = 20f;
    private float posOffset = 0f;


    private void Awake()
    {
        _unit = GetComponent<Unit>();
        //lineRenderer = InputManager.Instance.GetComponent<LineRenderer>();

    }


    public void moveToTarget(int endX, int endY, int endZ)
    {

        //call this end of turn if set to auto go somewhere
        // Find the path using World_Pathfinding's findPath method

        List<Vector3> path = World_Pathfinding.findPath(endX, endY,endZ, _unit.x, _unit.y,_unit.z, _unit.width, _unit.height, _unit.depth,_unit.flying);
        
        if (path != null)
        {
            int limitedPathLength = Mathf.Min(path.Count, _unit.movementPoints);
            List<Vector3> limitedPath = path.GetRange(0, limitedPathLength);

            // Move the Unit along the limited path
            StartCoroutine(moveAlongPath(limitedPath));

            // Update the unit's movement path
            _unit.movementPoints -= limitedPathLength;
        }
        else { Debug.Log("path is null"); }
       
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
        _unit.z = temportayChangethisplease.z;
    }


}
