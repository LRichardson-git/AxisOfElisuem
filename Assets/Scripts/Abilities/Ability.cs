using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability 
{
    public string Name { get; }
    public string Description { get; }
    public int Cost { get; }
    public int Range { get; }

    public Sprite icon { get; }

    public Ability(string name, string description, int cost, int range, Sprite icon)
    {
        Name = name;
        Description = description;
        Cost = cost;
        Range = range;
        this.icon = icon;
    }

    public abstract void Execute(Vector3 worldSpace);

    public abstract void Target(Vector3 worldSpace);
}
