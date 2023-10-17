using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAndGunAbility : Ability
{
    public RunAndGunAbility() : base("Run&Gun", "Gain 1 extra actionPoint this round", 0, 20, "RunGun")
    {
        uses = 2;


    }

    // Start is called before the first frame update
    public override void deActivate()
    {

    }

    public override void ExcuteAbility(Vector3 worldSpace)
    {

    }

    public override bool Execute(Vector3 worldSpace)
    {
        AbilityManager.Instance.deactivate();

        ObjectSelector.Instance.getSelectedUnit().ActionPoints += 1;
        ObjectSelector.Instance.refreshUnit();
        Final();
        return true;
    }

    public override void Init() { 

    }

    public override void setup()
    {

    }

    public override void Target(Vector3 worldSpace)
    {
    Shooting_View_Controller.Instance.Activate();
        Shooting_View_Controller.Instance.UpdateInfo(this, true);
    AbilityManager.Instance.disable = true;
    }
}
