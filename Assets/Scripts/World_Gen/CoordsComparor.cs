using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordsComparor : IComparer<Coords>
{
    public int Compare(Coords a, Coords b)
    {
        int result = a.m_fCost.CompareTo(b.m_fCost);
        if (result == 0)
        {
            // If f-costs are equal, compare by coordinates to ensure uniqueness
            result = a.x.CompareTo(b.x);
            if (result == 0)
            {
                result = a.y.CompareTo(b.y);
            }
        }
        return result;
    }
}
