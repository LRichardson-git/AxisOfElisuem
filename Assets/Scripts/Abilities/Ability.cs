using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability 
{
    public string Name { get; }
    public string Description { get; }
    public int Cost { get; }
    public int Range { get; }

    public Ability(string name, string description, int cost, int range)
    {
        Name = name;
        Description = description;
        Cost = cost;
        Range = range;
    }

    public abstract void Execute(Unit user, Unit target);

    public abstract void Target(Transform worldSpace);
}
