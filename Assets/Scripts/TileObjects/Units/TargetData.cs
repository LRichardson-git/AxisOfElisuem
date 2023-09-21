using Unity;
using UnityEngine;

public class TargetData 
{
    Unit Target;
    int hitPercentage;
    int CritPercentage;
    GameObject hitPoint;
    public TargetData(Unit unit, int hitChance, int critChance, GameObject hitpoint)
    {
        Target = unit;
        hitPercentage = hitChance;
        CritPercentage = critChance;
        this.hitPoint = hitpoint;
    }

    public int getHit() { return hitPercentage; }
}
