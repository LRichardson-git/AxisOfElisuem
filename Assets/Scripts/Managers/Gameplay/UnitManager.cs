using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<Unit> _units;
    private List<GameObject> _highlighters;
    public GameObject HighlightTile;
    public GameObject HighlightDash;

    public GameObject Floor;
    Material floorMaterial;
    private List<smoke> smokeList;
    private void Awake()
    {
        Instance = this;
        _units = new List<Unit>();
        _highlighters = new List<GameObject>();
        smokeList = new List<smoke>();
    }

    private void Start()
    {
        floorMaterial = Floor.GetComponent<Renderer>().material;
    }


    public void newTurn()
    {

        for (int i = 0; i < smokeList.Count; i++)
            smokeList[i].updateLife(i);


    }

    //Change in future
    public int GetModifers(Unit unit)
    {
        int modifers = 0;

        foreach (smoke smoke in smokeList)
            if (smoke.Affected(unit))
            {
                modifers += smoke.modifer;
                return modifers;
            }
        return modifers;
    }


    public void AddUnit(Unit unit)
    {
        _units.Add(unit);
    }



    public int getObjectnum(smoke objec)
    {
        for (int i = 0; i < smokeList.Count; i++)
        {
            if (smokeList[i] = objec)
                return i;
        }
     
        return -1;
    }

    public void removeobject(int i)
    {
        smoke temp = smokeList[i];
        smokeList.RemoveAt(i);
        Destroy(temp.gameObject);
    }

    public void addObject(smoke Smoke)
    {
        smokeList.Add(Smoke);
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
        int notDash = movement / 2;
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
                        if (i >notDash + unit.x || j >notDash +unit.y|| k > notDash +unit.z)
                            _highlighters.Add(Instantiate(HighlightDash, World_Pathfinding.coordToWorld(i, j, k, 1, 1), Quaternion.identity));
                        else
                            _highlighters.Add(Instantiate(HighlightTile, World_Pathfinding.coordToWorld(i, j, k, 1, 1), Quaternion.identity));


                    }

                }

            }
        }


    }
}
