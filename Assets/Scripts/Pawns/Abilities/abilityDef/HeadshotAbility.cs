using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadshotAbility : Ability
{
    Shooting _shooting;
    InputManager _input;
    AudioManager _audio;
    ObjectSelector _objectSelector;
    

    public HeadshotAbility() : base("Headshot", "Fire your gun with Increased Crit Chance at an opposing unit and end this Units turn", 0, 20, "HeadShot")
    {
        _shooting = Shooting.Instance;
        _input = InputManager.Instance;
        _audio = AudioManager.instance;
        _objectSelector = ObjectSelector.Instance;
        old = new List<TargetData>();
        uses = 2;
    }

    //setup
    //target
    //execute
    public override void deActivate()
    {

    }

    public override void ExcuteAbility(Vector3 worldSpace)
    {

    }

    public override bool Execute(Vector3 worldSpace)
    {

        List<GameObject> list = _shooting.getButtons();

        if (list.Count > 0)
        {
            activated = true;
            Unit Current = _objectSelector.getSelectedUnit();
            Current.crit += 40;
            old = Current.getList();
            _shooting.CheckSight(Current);
            _shooting.SpawnButtons(Current.getList());
            _shooting.getButtons()[0].GetComponent<ButtonScript>().openShootScreen();
            return true;
        }
        else
            _audio.PlaySound("Error");
        return false;
    }

    public override void Init()
    {

    }

    public override void setup()
    {

    }

    public override void Target(Vector3 worldSpace)
    {
        _input.leftClick = true;
    }
}
