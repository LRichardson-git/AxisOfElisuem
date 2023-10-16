using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    [SerializeField]
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
        //_units = new List<Unit>();
        _highlighters = new List<GameObject>();
        smokeList = new List<smoke>();
    }

    private void Start()
    {
        floorMaterial = Floor.GetComponent<Renderer>().material;

        int tempId = 0;

        foreach (Unit unit in _units) {
            unit.SetID(tempId);
            tempId++;
                }
    }

    public void setPlayerID()
    {
        //dumb way of doing this
        foreach (Unit unit in _units)
        {
            unit.setPlayerID();
        }
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

        if (unit.movementPoints < 2)
            return;

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
                    // calculate distance from center
                    float distance = Mathf.Sqrt(Mathf.Pow(i - unit.x, 2) + Mathf.Pow(j - unit.y, 2) + Mathf.Pow(k - unit.z, 2));




                    if (unit.ActionPoints < 2)
                        spawnHighlighter(true, i, j, k, unit);
                    else if (unit.ActionPoints > 2)
                        spawnHighlighter(false, i, j, k, unit) ;

                    else 
                        spawnHighlighter(i, j, k, unit);
                }
            }
        }


    }

    void spawnHighlighter( int i, int j , int k, Unit unit)
    {
        List<Vector3> path = World_Pathfinding.findPath(i, j, k, unit.x, unit.y, unit.z, unit.width, unit.height, unit.depth, unit.flying);

        if (path != null && path.Count <= unit.movementPoints)
        {
            if (path.Count > unit.movementPoints / 2)
                _highlighters.Add(Instantiate(HighlightDash, World_Pathfinding.coordToWorld(i, j, k, 1, 1), Quaternion.identity));
            else
                _highlighters.Add(Instantiate(HighlightTile, World_Pathfinding.coordToWorld(i, j, k, 1, 1), Quaternion.identity));
        }
    }

    void spawnHighlighter(bool dash,int i, int j, int k, Unit unit)
    {
        List<Vector3> path = World_Pathfinding.findPath(i, j, k, unit.x, unit.y, unit.z, unit.width, unit.height, unit.depth, unit.flying);

        if (path != null && path.Count <= unit.movementPoints)
        {
            if (!dash)
                _highlighters.Add(Instantiate(HighlightTile, World_Pathfinding.coordToWorld(i, j, k, 1, 1), Quaternion.identity));
            else
                _highlighters.Add(Instantiate(HighlightDash, World_Pathfinding.coordToWorld(i, j, k, 1, 1), Quaternion.identity));
        }
    }



}
