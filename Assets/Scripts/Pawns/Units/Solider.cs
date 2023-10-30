using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solider : Unit
{
    [SerializeField]
    public List<GameObject> targetPoints;

    public GameObject gunModel;
    float tickTimer = 0;
    float LastActionTime = 0;
    int old = 0;
    public bool begun = false;
    public bool seen = false;
  

    private void Update()
    {
        if (!begun || isOwned)
            return;

        if (Player.LocalInstance.turn)
            seen = false; return;


        float TimeSinceLastAction = Time.time - LastActionTime;


        tickTimer += Time.deltaTime;

        if (TimeSinceLastAction > 0.1)
        {
            //other units know to attack
            if (old != World_Pathfinding.Instance.worldToCoord(transform.position, depth))
            {
                if (UnitManager.Instance.checkSightsmove(this))
                {
                    if (seen)
                        CameraControler.LocalInstance.FollowUnit(this);

                    seen = true;
                }
                else
                    seen = false;
                old = World_Pathfinding.Instance.worldToCoord(transform.position, depth);
            }
           
            
            LastActionTime = Time.time;
        }


    }


}
