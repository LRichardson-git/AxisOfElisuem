using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoke : MonoBehaviour
{

    public int life = 2;
    public int modifer = -20;
    public int radius = 5;

    public void updateLife(int i)
    {
        life -= 1;

        if (life <= 0)
            removeSelf(i);
    }


    void removeSelf(int i)
    {
       UnitManager.Instance.removeobject(i);
       
    }

    public bool Affected(Unit unit) {

        if ((Vector3.Distance(transform.position, unit.transform.position) / 10) < radius - 0.6)
            return true;

        return false;
                
                }
}
