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
    float tracker = 0;

    private void Update()
    {
        if (!begun || isOwned)
            return;
        if (movee == false)
        {
            seen = false;
            return;
        }

        if (Player.LocalInstance.turn)
        {
            seen = false; return;

        }
        float TimeSinceLastAction = Time.time - LastActionTime;


        tickTimer += Time.deltaTime;

        if (TimeSinceLastAction > 0.1)
        {
            Debug.Log("XD");
            //other units know to attack

            if (UnitManager.Instance.checkSightsmove(this))
            {
                
                if (!seen)
                    CameraControler.LocalInstance.FollowUnit(this);
                seen = true;

            }
            else
            {
                seen = false;
                Debug.Log("XD");

            }            LastActionTime = Time.time;
        }


    }


}
