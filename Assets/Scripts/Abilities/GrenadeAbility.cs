using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAbility : Ability
{
    public int maxDamage = 5;
    public int minDamage = 2;
    public int radius = 4;
    public int pen = 2;

    private GrenadeTarget_Instance target;
    private List<Unit> highLight;
    private Camera _cam;
    Vector3 direction;
    public GrenadeAbility(int maxdamage, int mindamage, int radius) : base("Grenade", "Throws a grenade that explodes on impact", 1, 15, null)
    {
        maxDamage = maxdamage;
        minDamage = mindamage;
        this.radius = radius;
        direction = Vector3.zero;

        radius = radius * 20;
        //setanimationname
        Animation = "Throw_Grenade";
    }


    public override void Init()
    {
        Debug.Log("2");
        target = GrenadeTarget_Instance.Instance;
        highLight = new List<Unit>();
        _cam = Camera.main;
    }


    //click the abiltity button, shows info like where goes and stuff and you click
    //after animation ends do exectue


    //set sphere to true and set size to radius
    public override void setup() {
        target.gameObject.SetActive(true);  
        //*20 becuase it is a sphere
        target.transform.localScale = new Vector3(radius *20, radius *20, radius *20);
        
        //setup animation
    }


    public override void Target(Vector3 worldSpace )
    {
        //dehighlight incase of movement
        if (highLight.Count >0)
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
            if ((Vector3.Distance(worldSpace, unit.transform.position) / 10) < 1.4)
            {
                highLight.Add(unit);
                continue;
            }


            else if ((Vector3.Distance(worldSpace, unit.transform.position) / 10) < radius)
            {
                direction = unit.transform.position - worldSpace;
                if (Physics.Raycast(worldSpace, direction, out RaycastHit hitInfoC, radius * 10))
                {
                    if (hitInfoC.collider.gameObject.CompareTag("Unit"))
                    {
                        highLight.Add(unit);
                        continue;
                    }
                }
            }

        }
        if (highLight.Count > 0)
            highLightunits();


    }

    private void highLightunits()
    {
        foreach (Unit unit in highLight)  
            unit.highlight();
    }


    private void deHighLight()
    {
        foreach (Unit unit in highLight)
            unit.DeHighlight();
    }


    public override void deActivate() { target.gameObject.SetActive(false);  if(highLight.Count > 0) deHighLight(); }


    public void ExcuteAbility(Vector3 worldSpace) {


        List<Unit> list = new List<Unit>();
        //raycast from worldspace on units
        float distance;
        worldSpace = worldSpace - _cam.transform.forward;
        foreach (Unit unit in UnitManager.Instance.GetUnitList())
        {
            distance = Vector3.Distance(worldSpace, unit.transform.position) / 10;

            if (distance < 1.4)
            {

                list.Add(unit);
                continue;
            }


            else if (distance < radius)
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
        //play animation
        
        deActivate();
        damageUnits(list);
    }

    public override void Execute(Vector3 worldSpace)
    {
        ObjectSelector.Instance.playAnimation(Animation, worldSpace);
        
        ObjectSelector.Instance.FireGrenade(worldSpace, this);
    }



    private void damageUnits(List<Unit> unitList)
    {
        foreach (Unit unit in unitList)
            Shooting.Instance.CmdDmgUnit(unit.getID(), minDamage, maxDamage, pen);
    }

}
