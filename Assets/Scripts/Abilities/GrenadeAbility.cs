using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAbility : Ability
{
    public int Damage { get; }
    public int radius { get; }

    public GrenadeAbility(int damage, int radius) : base("Grenade", "Throws a grenade that explodes on impact", 1, 10)
    {
        Damage = damage;
        this.radius = radius;
    }


    //click the abiltity button, shows info like where goes and stuff and you click
    //after animation ends do exectue


    public override void Target(Transform worldSpace)
    {
        //raycast from worldspace on units
        //maybe update call??

    }


    public override void Execute(Unit user, Unit target)
    {
        // Code to apply damage to target
    }
}
