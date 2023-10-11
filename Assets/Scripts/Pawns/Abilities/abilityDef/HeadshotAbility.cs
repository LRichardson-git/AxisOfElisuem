using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadshotAbility : Ability
{
    Shooting _shooting;
    InputManager _input;
    AudioManager _audio;
    ObjectSelector _objectSelector;
    

    public HeadshotAbility() : base("Headshot", "Fire your gun with Increased Crit Chance at an opposing unit and end this Units turn", 20, 20, null,2)
    {
        _shooting = Shooting.Instance;
        _input = InputManager.Instance;
        _audio = AudioManager.instance;
        _objectSelector = ObjectSelector.Instance;
        old = new List<TargetData>();
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

    public override void Execute(Vector3 worldSpace)
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
        }
        else
            _audio.PlaySound("Error");
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
