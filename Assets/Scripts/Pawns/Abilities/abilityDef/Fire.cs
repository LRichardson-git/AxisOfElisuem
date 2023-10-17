using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Ability
{
    Shooting _shooting;
    InputManager _input;
    AudioManager _audio;
    ObjectSelector _selector;
    public Fire() : base("Fire", "Fire your gun at an opposing unit and end this Units turn", 2, 20, "Fire")
    {
        _shooting = Shooting.Instance;
        _input = InputManager.Instance;
        _audio = AudioManager.instance;
        _selector = ObjectSelector.Instance;
        uses = 10000;
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

        
        if (_shooting.getButtons().Count > 0)
        {
            _shooting.CheckSight(_selector.getSelectedUnit());
            _shooting.SpawnButtons(_selector.getSelectedUnit().getList());
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
