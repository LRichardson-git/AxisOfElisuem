using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability 
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Cost { get; set; }
    public float Range { get; set; }

    public Sprite Icon { get; set; }


    public string Animation;

    public string Damage;
    public Ability(string name, string description, int cost, int range, Sprite icon)
    {
        Name = name;
        Description = description;
        Cost = cost;
        Range = range;
        Icon = icon;
    }


    public abstract void Init();

    public abstract void setup();
    public abstract void deActivate();
    public abstract void Execute(Vector3 worldSpace);

    public abstract void Target(Vector3 worldSpace);

    public abstract void ExcuteAbility(Vector3 worldSpace);
  
}
