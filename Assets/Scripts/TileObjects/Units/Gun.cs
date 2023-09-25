using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int minDmg = 2;
    public int maxDmg = 5;
    public int penetration;
    public int minRange = 10;
    public int maxRange = 20;
    public int Critc = 10;
    GunType type = GunType.rifle;
}

enum GunType
{
    rifle,
    shotgun,
    sniper,
    machineGun,
    Laser
}
