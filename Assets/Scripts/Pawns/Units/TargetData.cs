using Unity;
using UnityEngine;

public class TargetData 
{
    private Unit Target;
    private int hitPercentage;
    private int CritPercentage;
    private GameObject hitPoint;
    public int minDmg;
    public int maxDmg;
    public TargetData(Unit unit, int hitChance, int critChance, GameObject hitpoint)
    {
        Target = unit;
        hitPercentage = hitChance;
        CritPercentage = critChance;
        this.hitPoint = hitpoint;
    }

    public int getHit() { return hitPercentage; }

    public Unit getUnit() { return Target; }

    public int getCrit() { return CritPercentage; }

    public GameObject getHitPoint() { return hitPoint; }

    public void setDmg(int min, int max) { minDmg = min; maxDmg = max; }
}
