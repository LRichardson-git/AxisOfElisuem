using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAbility : GrenadeAbility
{
    public SmokeAbility(int maxdamage, int mindamage, int radius) : base(maxdamage, mindamage, radius) 
    {

        radius = radius * 20;
        //setanimationname
        Animation = "Throw_Grenade";
        Icon = "SmokeG";
    }


    public override void Init()
    {
        Name = "Smoke Grenade";
        Description = "throw a smoke greande to protect lower the chance to be hit";
        Damage = "25% protection";
        target = GrenadeTarget_Instance.Instance;
        highLight = new List<Unit>();
        _cam = Camera.main;
        Explode = false;
    }


    public override void Target(Vector3 worldSpace)
    {
        //dehighlight incase of movement
        if (highLight.Count > 0)
        {
            deHighLight();
        }
        ObjectSelector.Instance.AimGrenade();
        target.transform.position = worldSpace;
        highLight.Clear();
        //make worldspace slightly towards camera so it dosent ignore wall collisions by starting inside of the hitbox
        worldSpace = worldSpace - _cam.transform.forward;
        foreach (Unit unit in UnitManager.Instance.GetUnitList())
        {
            if ((Vector3.Distance(worldSpace, unit.transform.position) / 10) < radius - 0.6)
            {
                highLight.Add(unit);
                continue;
            }
        }
        if (highLight.Count > 0)
            highLightunits();
    }

        public override void ExcuteAbility(Vector3 worldSpace)
    {
        Grenade.Instance.cmdSpawnSmoke(radius);
        deActivate();
    }
}
