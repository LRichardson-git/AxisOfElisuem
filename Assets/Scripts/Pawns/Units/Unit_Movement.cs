using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Unit_Movement : MonoBehaviour
{
    private Unit _unit;
    public float speed = 20f;
    private bool _isMoving = false;
    public Animator animator;
    private int rotaionSpeed = 1000;
    private AudioManager _audio;
    bool dash = false;
    World_Pathfinding _path;
    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    private void Start()
    {
        _audio = AudioManager.instance;
        _path = World_Pathfinding.Instance;
    }

    public void moveToTarget(int endX, int endY, int endZ)
    {

        if (_isMoving == true )
            return;





        //call this end of turn if set to auto go somewhere

        int distance = _path.testdistance(_unit, endX, endY, endZ);

        if (distance == -1)
        {
            _audio.PlaySound("Error");
            return;

        }

        List<Vector3> path = _path.findPath(endX, endY, endZ, _unit.x, _unit.y, _unit.z);
        dash = false;

        if (distance > _unit.movementPoints / 2)
            dash = true;

        if (path != null)
        {
            _unit.covers.Clear();
            animator.speed = 1;
            int limitedPathLength = Mathf.Min(path.Count, _unit.movementPoints);
            List<Vector3> limitedPath = path.GetRange(0, limitedPathLength);
            // Move the Unit along the limited path
            _audio.cmdLoopSound("running");
            StartCoroutine(moveAlongPath(limitedPath));

            // Update the unit's movement path
            _unit.movementPoints -= limitedPathLength;
        }
        else { Debug.Log("path is null"); }
       
    }

    private IEnumerator moveAlongPath(List<Vector3> path)
    {

        Vector3 directionToTarget;
        Quaternion targetRotation;
        _unit.Model.transform.rotation = Quaternion.identity;
        _isMoving = true;
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 targetPosition = path[i];
            //targetPosition.y = 5 * _unit.depth;
            animator.speed = 2;
            if (targetPosition.y  > transform.position.y )
            {
                //check like if next point on path if greater than + 10 to continue i tink
                speed = 15f;
                animator.SetFloat("Y", 2);
            }
            else if (targetPosition.y < transform.position.y)
            {
                speed = 15f;
                animator.SetFloat("Y", -2);
            }
            else
            {
                speed = 20f;
                animator.SetFloat("Y", 0);

            }


            while (Vector3.Distance(transform.position, targetPosition) >= speed * Time.deltaTime)
            {
                //rotate towards location and set run animtion
                animator.SetFloat("Speed", 2);
                Vector3 copy = targetPosition;
                copy.y = transform.position.y;
                directionToTarget = copy - transform.position;
                targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotaionSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed );

                
                
                yield return null;
            }
            transform.position = targetPosition;
        }
        Vector3Int temportayChangethisplease = _path.worldToCoord(transform.position, _unit.width, _unit.depth);
        _unit.x = temportayChangethisplease.x;
        _unit.y = temportayChangethisplease.y;
        _unit.z = temportayChangethisplease.z;

        animator.SetFloat("Speed", 0);
        _audio.cmdStopSound();

        Quaternion oldRotation = _unit.Model.transform.rotation;
        transform.rotation = Quaternion.identity;

        int AC = 1;

        if (dash)
            AC = 2;
        UnitManager.Instance.updateVision();
        _unit.DeleteCover();
        _unit.cmdCheckCover(oldRotation, AC);
        _isMoving = false;

        

        
        

       

    }
    
}
