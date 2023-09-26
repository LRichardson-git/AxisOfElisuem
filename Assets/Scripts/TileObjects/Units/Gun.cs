using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private int minDmg = 2;
    private int maxDmg = 5;
    public int penetration;
    public int minRange = 10;
    public int maxRange = 20;
    private int Critc = 10;
    GunType type = GunType.rifle;

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
