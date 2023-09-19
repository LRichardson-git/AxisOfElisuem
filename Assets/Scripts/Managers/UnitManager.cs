using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<Unit> _units;

    private void Awake()
    {
        Instance = this;
        _units = new List<Unit>();
    }
    public void AddUnit(Unit unit)
    {
        _units.Add(unit);
    }

    public List<Unit> GetUnitList() { return _units; }

    public void RemoveUnit(Unit unit) { _units.Remove(unit); }
};
