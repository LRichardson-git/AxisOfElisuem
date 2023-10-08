using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
#pragma warning disable 0414
    public int minDmg = 2;
    public int maxDmg = 5;
    public int penetration;
    public int minRange = 10;
    public int maxRange = 20;
    public int Critc = 10;
    public float punishment = 2;
    GunType type = GunType.sniper;

    public int getMin() { return minDmg; }

    public int getMax() { return maxDmg; }

    public int getCrit() { return Critc; }

    public void plusDmg(int dmg) { minDmg += dmg; maxDmg += dmg; }




}





enum GunType
{
    rifle,
    shotgun,
    sniper,
    machineGun,
    Laser
}
