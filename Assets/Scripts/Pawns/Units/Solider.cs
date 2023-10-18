using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solider : Unit
{
    [SerializeField]
    public List<GameObject> targetPoints;

    public GameObject gunModel;
    float tickTimer = 0;
    float tickTimerMax = 0.6f;
    float LastActionTime = 0;
    World_Pathfinding path;
    UnitManager manager;
    int old = 0;
    public bool begun = false;

  

    private void Update()
    {
        if (!begun || isOwned)
            return;


        float TimeSinceLastAction = Time.time - LastActionTime;


        tickTimer += Time.deltaTime;

        if (TimeSinceLastAction > 0.1)
        {
            //other units know to attack
            if (old != World_Pathfinding.Instance.worldToCoord(transform.position, depth))
            {
                UnitManager.Instance.checkSightsmove(this);
                old = World_Pathfinding.Instance.worldToCoord(transform.position, depth);
            }
                LastActionTime = Time.time;

        }


    }


}
