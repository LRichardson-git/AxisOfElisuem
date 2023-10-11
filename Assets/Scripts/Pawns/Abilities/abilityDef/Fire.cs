using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Ability
{
    Shooting _shooting;
    InputManager _input;
    AudioManager _audio;
    ObjectSelector _selector;
    public Fire() : base("Fire", "Fire your gun at an opposing unit and end this Units turn", 20, 20, null,0)
    {
        _shooting = Shooting.Instance;
        _input = InputManager.Instance;
        _audio = AudioManager.instance;
        _selector = ObjectSelector.Instance;
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

        Debug.Log(_shooting.getButtons().Count);
        if (_shooting.getButtons().Count > 0)
        {
            _shooting.CheckSight(_selector.getSelectedUnit());
            _shooting.SpawnButtons(_selector.getSelectedUnit().getList());
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
        Debug.Log("lol");
       _input.leftClick = true;
    }
}
