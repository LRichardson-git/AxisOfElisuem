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
    public bool begun = false;
    public bool seen = false;

    private void Update()
    {

        //removed !begun btw
        if (!begun || ownedBy == Player.LocalInstance.playerID)
        {

            return;

        }
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
            //other units know to attack
            if (UnitManager.Instance.checkSightsmove(this))
            {
               // Debug.Log("seeing");

                if (!seen)
                    CameraControler.LocalInstance.FollowUnit(this);
                seen = true;

            }
            else
            {
                seen = false;
                AudioManager.instance.stopSoundLocal();

            }            LastActionTime = Time.time;
        }


    }


}
