using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
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
    Thread thread;
    World_Pathfinding _path;
    Shooting shooting;

    testingpath _testingpath;
    testingpath _testingpath2;
    List<Unit> inVision;

    public bool testt = false;
    bool init = false;
    private void Awake()
    {
        Instance = this;
        //_units = new List<Unit>();
        _highlighters = new List<GameObject>();
        smokeList = new List<smoke>();
      
        inVision = new List<Unit>();
    }

    private void Start()
    {
        //floorMaterial = Floor.GetComponent<Renderer>().material;

        int tempId = 0;

        foreach (Unit unit in _units) {
            unit.SetID(tempId);
            tempId++;
        }

        _testingpath = new testingpath();
        _testingpath2 = new testingpath();

        _path = World_Pathfinding.Instance;
        shooting = Shooting.Instance;



    }

    public void SetupSight(int ID)
    {
        foreach (Unit unit in _units)
            if (unit.ownedBy != ID)
                unit.cantSee();
    }


    public void setPlayerID()
    {

        if (init != false)
            return;

        //dumb way of doing this
        foreach (Unit unit in _units)
        {
            
            unit.setPlayerID();
            unit.addAbility(new Fire());
            unit.addAbility(new GrenadeAbility(5, 2, 4));
            unit.addAbility(new SmokeAbility(2, 2, 5));
            unit.addAbility(new RunAndGunAbility());
            unit.addAbility(new MedPack());
            unit.init();

        }

        SetupSight(Player.LocalInstance.playerID);

        startt();
        init = true;
    }

    //called after everying is initlized
    public void startt()
    {
        foreach (Unit unit in _units)
            unit.GetComponent<Solider>().begun = true;

        
    }

    public void newTurn()
    {
        
        if (smokeList.Count >0)
            for (int i = 0; i < smokeList.Count; i++)
                smokeList[i].updateLife(i);

       

        foreach (Unit unit in _units)
            if (Player.LocalInstance.turn == true)
            {
                unit.ActionPoints = 2;
                shooting.CheckSight(unit);

            }
         
        inVision.Clear();
        foreach(Unit unit in _units)
        {
            foreach (TargetData data in unit.getList())
                if (!inVision.Contains(data.getUnit()) && unit.isOwned)
                    inVision.Add(data.getUnit());

        }
        
        foreach (Unit unit in _units)
            if (!unit.isOwned)
                unit.cantSee();
        
        foreach (Unit unit in inVision)
            unit.canSee();
        
        ObjectSelector.Instance.Deselect();
    }


    public void updateVision()
    {
        foreach (Unit unit in _units)
            if (!unit.isOwned)
                unit.cantSee();

        foreach (Unit unit in _units)
            foreach (TargetData data in unit.getList())
                data.getUnit().canSee();
                    
    }


    //Change in future

    //enemy moves or unit dies
    public void TeamCheckSight(int ID)
    {
        foreach (Unit unit in _units)
            if (unit.ownedBy == ID)
                Shooting.Instance.CheckSight(unit);
    }
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
        int notDash = 0;

        //check the 4 ys with floors we will go to, simple as

        if (testt)
        {
            for (int j = 0; j < 9; j += 4)
            {
                notDash++;
                for (int i = unit.x - (movement); i < unit.x + movement; i++)
                {



                    for (int k = unit.z - (movement); k < unit.z + movement; k++)
                    {

                        spawnHighlighter(i, j, k, unit);

                    }

                }
            }
        }
        else
        {

            for (int j = 0; j < 13; j += 4)
            {
                notDash++;

               

                    for (int i = unit.x - (movement); i < unit.x + movement; i++)
                {



                    for (int k = unit.z - (movement); k < unit.z + movement; k++)
                    {
                         




                        int temp = _path.testdistance(unit, i, j, k);
                        if (temp != -1 && temp < unit.movementPoints)
                            spawnHighlighter(i, j, k, temp, unit);

                    }

                }

            }

      }

    }

    public void checkSightsmove(Unit target)
    {
        bool seen = false;
        foreach(Unit unit in _units)
        {
            //owned by player
            if (unit != target && Player.LocalInstance.playerID == unit.ownedBy)
            {

                if (Shooting.Instance.CanSeeUnit(unit, target))
                {
                    seen = true;
                    target.canSee();
                    break;
                }

            }
        }

        if (!seen)
            target.cantSee();

    }



    public void test(Unit unit)
    {
        removePaths();
        Debug.Log("test"); 
        foreach (moveable move in unit.moveables)
            _highlighters.Add(Instantiate(HighlightDash, _path.coordToWorld(move.x, move.y, move.z, 1, 1), Quaternion.identity));
    }
    
    void addMoveable(int i, int j, int k, Unit unit)
    {
        int p = _testingpath.findPath(i, j, k, unit.x, unit.y, unit.z);
        if (p <= unit.movementPoints && p != 0)
        {
            unit.addmove(i, j, k);
        }
    }

    void addMoveable2(int i, int j, int k, Unit unit)
    {
        int p = _testingpath2.findPath(i, j, k, unit.x, unit.y, unit.z);
        if (p <= unit.movementPoints && p != 0)
        {
            unit.addmove(i, j, k);
        }
    }

    void spawnHighlighter(int i, int j, int k, int distance, Unit unit)
    {

            if (distance > unit.movementPoints / 2 || unit.ActionPoints < 2)
                _highlighters.Add(Instantiate(HighlightDash, _path.coordToWorld(i, j, k, 1, 1), Quaternion.identity));
            else
                _highlighters.Add(Instantiate(HighlightTile, _path.coordToWorld(i, j, k, 1, 1), Quaternion.identity));
        
    }

    void spawnHighlighter( int i, int j , int k, Unit unit)
    {
       // Debug.Log(unit);
        int p = _path.findPathT(i, j, k, unit.x, unit.y, unit.z);

        if (p <= unit.movementPoints && p != 0)
        {
            if (p > unit.movementPoints / 2)
                _highlighters.Add(Instantiate(HighlightDash, _path.coordToWorld(i, j, k, 1, 1), Quaternion.identity));
            else
                _highlighters.Add(Instantiate(HighlightTile, _path.coordToWorld(i, j, k, 1, 1), Quaternion.identity));
        }
    }

    void spawnHighlighter(bool dash,int i, int j, int k, Unit unit)
    {
        List<Vector3> path = _path.findPath(i, j, k, unit.x, unit.y, unit.z);

        if (path != null && path.Count <= unit.movementPoints)
        {
            if (!dash)
                _highlighters.Add(Instantiate(HighlightTile, _path.coordToWorld(i, j, k, 1, 1), Quaternion.identity));
            else
                _highlighters.Add(Instantiate(HighlightDash, _path.coordToWorld(i, j, k, 1, 1), Quaternion.identity));
        }
    }



}
