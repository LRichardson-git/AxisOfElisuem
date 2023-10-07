using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<Unit> _units;

    private List<GameObject> _highlighters;
    public GameObject HighlightTile;
    private void Awake()
    {
        Instance = this;
        _units = new List<Unit>();
        _highlighters = new List<GameObject>();
    }
    public void AddUnit(Unit unit)
    {
        _units.Add(unit);
    }




    public List<Unit> GetUnitList() { return _units; }

    public void RemoveUnit(Unit unit) { _units.Remove(unit); }

    public void removePaths()
    {
        foreach (GameObject highlight in _highlighters)
            Destroy(highlight);
    }


    public void ShowPaths(Unit unit)
    {
        if (_highlighters.Count > 0)
            removePaths();


        int movement = unit.movementPoints;
        // calculate paths to all walkable end points
        for (int i = unit.x - (movement); i < unit.x + movement; i++)
        {
            for (int j = unit.y - (movement); j < unit.y + movement; j++)
            {
                if (j < 0)
                    continue;

                for (int k = unit.z - (movement); k < unit.z + movement; k++)
                {

                    if (World_Pathfinding.findPath(i, j, k, unit.x, unit.y, unit.z, unit.width, unit.height, unit.depth, unit.flying) != null)
                    {
                        _highlighters.Add(Instantiate(HighlightTile, World_Pathfinding.coordToWorld(i, j, k, 1, 1), Quaternion.identity));
                        
                    }

                }

            }
        }


    }
}
