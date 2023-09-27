using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAbility : Ability
{
    public int maxDamage = 5;
    public int minDamage = 2;
    public int radius = 5;

    public int pen = 2;

    Vector3 direction;
    public GrenadeAbility(int maxdamage, int mindamage, int radius) : base("Grenade", "Throws a grenade that explodes on impact", 1, 10, null)
    {
        maxDamage = maxdamage;
        minDamage = mindamage;
        this.radius = radius;
        direction = Vector3.zero;
    }


    //click the abiltity button, shows info like where goes and stuff and you click
    //after animation ends do exectue


    public override void Target(Vector3 worldSpace)
    {
        //maybe have sphere idk
        foreach (Unit unit in UnitManager.Instance.GetUnitList())
        {
            if ((Vector3.Distance(worldSpace, unit.transform.position) / 10) < 1.4)
            {
                //unit.highlight
                continue;
            }


            else if ((Vector3.Distance(worldSpace, unit.transform.position) / 10) < radius)
            {
                direction = unit.transform.position - worldSpace;
                if (Physics.Raycast(worldSpace, direction, out RaycastHit hitInfoC, radius * 10))
                {
                    if (hitInfoC.collider.gameObject.CompareTag("Unit"))
                    {
                        //unit.highlight
                        continue;
                    }
                }
            }

        }
    }



    

    public override void Execute(Vector3 worldSpace)
    {
        List<Unit> list = new List<Unit>();
        //raycast from worldspace on units
        float distance;
        foreach (Unit unit in UnitManager.Instance.GetUnitList())
        {
            distance = Vector3.Distance(worldSpace, unit.transform.position) /10 ;
            
            if ( distance < 1.4)
            {

                list.Add(unit);
                continue;
            }


            else if (distance < radius )
            {
                
                direction = unit.transform.position - worldSpace;

                if (Physics.Raycast(worldSpace, direction, out RaycastHit hitInfoC, radius * 10))
                {
                    if (hitInfoC.collider.gameObject.CompareTag("Unit"))
                    {
                        list.Add(unit);
                        continue;
                    }
                }
            }

        }
        
        damageUnits(list);
    }

    private void damageUnits(List<Unit> unitList)
    {
        foreach (Unit unit in unitList)
            Shooting.Instance.CmdDmgUnit(unit.getID(), minDamage, maxDamage, pen);
    }

}
