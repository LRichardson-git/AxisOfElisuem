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
    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    private void Start()
    {
        _audio = AudioManager.instance;
    }

    public void moveToTarget(int endX, int endY, int endZ)
    {

        if (_isMoving == true )
            return;


        //call this end of turn if set to auto go somewhere
        List<Vector3> path = World_Pathfinding.findPath(endX, endY,endZ, _unit.x, _unit.y,_unit.z, _unit.width, _unit.height, _unit.depth,_unit.flying);

        if (path.Count > _unit.movementPoints)
        {
            _audio.PlaySound("Error");
            return;

        }
        dash = false;

        if (path.Count > _unit.movementPoints / 2)
            dash = true;

        if (path != null)
        {
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
            
            while (Vector3.Distance(transform.position, targetPosition) >= speed * Time.deltaTime)
            {
                //rotate towards location and set run animtion
                animator.SetFloat("Speed", 2);
                directionToTarget = targetPosition - transform.position;
                targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotaionSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed );

                yield return null;
            }
            transform.position = targetPosition;
        }
        Vector3Int temportayChangethisplease = World_Pathfinding.worldToCoord(transform.position, _unit.width,_unit.depth);
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

        _unit.DeleteCover();
        _unit.cmdCheckCover(oldRotation, AC);
        _isMoving = false;

        

        
        

       

    }
    
}
