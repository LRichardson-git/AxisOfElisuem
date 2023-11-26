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
    private int rotaionSpeed = 800;
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
        animator.speed = 1.5f;
    }

    public void moveToTarget(int endX, int endY, int endZ)
    {

        if (_isMoving == true )
            return;





        //call this end of turn if set to auto go somewhere

        int distance = _path.testdistance(_unit, endX, endY, endZ);

        if (distance == -1 || _unit.ActionPoints < 1)
        {
            _audio.PlaySoundL("Error");
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
            _audio.soundLooplocal("running");
            StartCoroutine(moveAlongPath(limitedPath));

            // Update the unit's movement path
            _unit.movementPoints -= limitedPathLength;
            CameraControler.LocalInstance.FollowUnit(_unit.GetComponent<Solider>());
            _unit.GetComponent<Solider>().seen = true;

        }
        else {
            _audio.PlaySoundL("Error");
            return;
        }
       
    }

    private IEnumerator moveAlongPath(List<Vector3> path)
    {
        bool updown = false;
        Vector3 directionToTarget;
        Quaternion targetRotation;
        //_unit.Model.transform.rotation = Quaternion.identity;
        _isMoving = true;
        _unit.movee = true;
        _unit.moving(_unit.movee);
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 targetPosition = path[i];
            //targetPosition.y = 5 * _unit.depth;
            animator.speed = 1.3f;

            if (i + 1 < path.Count)
            {

                if (path[i+1].y > transform.position.y + 11)
                {
                    //check like if next point on path if greater than + 10 to continue i tink
                    speed = 20f;
                    animator.speed = 2.2f;
                    animator.SetFloat("Y", 2);
                    updown = true;
                }
                
                else if (i > 1 && path[i - 2].y > path[i].y + 11)
                {
                    speed = 20f;
                    animator.speed = 2.2f;
                    animator.SetFloat("Y", -2);

                    

                    if (updown == false) {
                        updown = true;
                        transform.Rotate(0,180,0);
                    }
                    
                }
                else
                {
                    speed = 50f;
                    
                    animator.SetFloat("Y", 0);
                    animator.SetFloat("Speed", 2);
                    updown = false;
                }

                

            }

            while (Vector3.Distance(transform.position, targetPosition) >= speed * Time.deltaTime)
            {
                //rotate towards location and set run animtion
                if (!updown)
                {
                    Vector3 copy = targetPosition;
                    copy.y = transform.position.y;
                    directionToTarget = copy - transform.position;
                    targetRotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotaionSpeed * Time.deltaTime);
                }
                
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed );

                
                
                yield return null;
            }
            transform.position = targetPosition;
        }

        Vector3Int temportayChangethisplease = _path.worldToCoord(transform.position, _unit.width, _unit.depth);
        _unit.x = temportayChangethisplease.x;
        _unit.y = temportayChangethisplease.y;
        _unit.z = temportayChangethisplease.z;

        animator.StopPlayback();
        animator.SetFloat("Y", 0);
        animator.SetFloat("Speed", 0);

        _audio.stopSoundLocal();

        Quaternion oldRotation = transform.rotation;
       // transform.rotation = Quaternion.identity;

        int AC = 1;

        if (dash)
            AC = 2;
        _unit.cmdCheckCover(oldRotation, AC);
        _isMoving = false;
        _unit.movee = false;
        _unit.moving(_unit.movee);
        GetComponent<Solider>().seen = false;




    }
    
}
