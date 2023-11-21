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
    public GameObject activity;


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
            if (unit.gun.sound == "Rifle")
            {
                unit.addAbility(new GrenadeAbility(5, 2, 4));
                unit.addAbility(new SmokeAbility(2, 2, 5));
                unit.addAbility(new GrenadeAbility(5, 2, 4));
            }

            else if (unit.gun.sound == "Sniper")
            {
                unit.addAbility(new MedPack());
                unit.addAbility(new SmokeAbility(2, 2, 5));
            }

            else
            {
                unit.addAbility(new GrenadeAbility(5, 2, 4));
                unit.addAbility(new RunAndGunAbility());
                
            }

            unit.init();
            
        }

        SetupSight(Player.LocalInstance.playerID);

        
        init = true;
    }

    //called after everying is initlized
 
    //when a unit dies checks to see if the other team has won
    public void CheckWin(int team)
    {
        bool lost = true;
        int deaths = 0;

        Debug.Log("checking Win");
        foreach(Unit unit in _units)
        {
            if (unit.ownedBy == Player.LocalInstance.playerID && unit.alive)
                lost = false;
            else if (unit.ownedBy != Player.LocalInstance.playerID && !unit.alive)
            {
                deaths++;
            }
        }

        if (lost)
        {
            ObjectSelector.Instance.Win(false);

        }
        else if (deaths >= _units.Count / 2)
        {

            ObjectSelector.Instance.Win(true);
        }

    }

    public void newTurn()
    {
        
        if (smokeList.Count >0)
            for (int i = 0; i < smokeList.Count; i++)
                smokeList[i].updateLife(i);

       

        foreach (Unit unit in _units)
            if (Player.LocalInstance.turn == true && unit.alive)
            {
                unit.ActionPoints = 2;
                shooting.CheckSight(unit);
                unit.turn = true;
                unit.movementPoints = unit.maxMovementPoints;
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
        ObjectSelector.Instance.nextUnit();
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

    public void RemoveUnit(int ID) {
        Debug.Log(ID);
        foreach (Unit unit in _units)
            if (unit.getID() == ID)
            {
                _units.Remove(unit);
                break;
            }
            
       }

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

    public bool checkSightsmove(Unit target)
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
        {
            target.cantSee();
            return false;
        }
        return true;
    }


    

    void spawnHighlighter(int i, int j, int k, int distance, Unit unit)
    {


        Quaternion rot = new Quaternion(90, 0, 0, 90);
        Vector3 Pos = _path.coordToWorld(i, j, k, 1, 1);

        Pos.y += 0.6f;

        if (unit.ActionPoints > 2)
        {
            _highlighters.Add(Instantiate(HighlightTile, Pos, rot));
        }
        else
        {
            Pos.y += 0.6f;
            if (distance > unit.movementPoints / 2 || unit.ActionPoints < 2)
                _highlighters.Add(Instantiate(HighlightDash, Pos, rot));
            else
                _highlighters.Add(Instantiate(HighlightTile, Pos, rot));
        }
    }





    public void removeUnit(Unit unit)
    {
        _units.Remove(unit);
    }
}
