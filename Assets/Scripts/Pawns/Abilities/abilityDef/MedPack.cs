using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedPack : Ability
{
    public int damage = 3;
    public Unit target;
    ObjectSelector _selector;
    AudioManager _audio;
    public MedPack() : base("Medpack", "Heal an allied unit", 2, 2, "MedPack")
    {
        Damage = "Heal: " + damage;
        _audio = AudioManager.instance;
    }

    //00
    public override void deActivate()
    {
       
    }
    //00
    public override void ExcuteAbility(Vector3 worldSpace)
    {
       
    }
    //execture 3
    public override bool Execute(Vector3 worldSpace)
    {
        if (target == null)
        {
            _audio.PlaySound("Error");
            return false;

        }

        _selector.playAnimation("Use", target.transform.position);
        if (Vector3.Distance(_selector.getSelectedUnit().transform.position, _selector.hoveredUnit().transform.position) < Range *10)
            {
                target.ApplyDmg(-damage, 10);
                _audio.cmDPlaySound("heal");
            return true;
        }
        else
            _audio.PlaySound("Error");

        dehighlight();
        return false;
    }
    //init 0
    public override void Init()
    {
        _selector = ObjectSelector.Instance; 
    }
    //setup 1
    public override void setup()
    {
    }
    //target 2
    public override void Target(Vector3 worldSpace)
    {
        target = _selector.hoveredUnit();


        
        
        if (target != null)
        {
            if (!target.highlighted)
            {
                _selector.hoveredUnit().highlight();
                AudioManager.instance.PlaySound("select");

            }      
        }
        else 
        {
            dehighlight();
        }
        


    }


    void dehighlight()
    {
        foreach (Unit unit in UnitManager.Instance.GetUnitList())
            unit.DeHighlight();
    }
}
