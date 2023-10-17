using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability 
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Cost { get; set; }
    public float Range { get; set; }

    public string Icon { get; set; }

    public int uses = 1;
    public string Animation;

    public string Damage;

    public List<TargetData> old;
    public bool activated = false;
    private int v;

    public Ability(string name, string description, int cost, int range, string icon)
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
    public abstract bool Execute(Vector3 worldSpace);

    public abstract void Target(Vector3 worldSpace);

    public abstract void ExcuteAbility(Vector3 worldSpace);

    //if execute returns true (ability has executed) then do this
    public void Final() {

        uses -= 1;


        if (uses < 1)
        {
            ObjectSelector.Instance.getSelectedUnit().DeleteAbility(this);
            
            

        }
    
        if (uses <100)
            ObjectSelector.Instance.getSelectedUnit().doAction(Cost);

    }
  
}
