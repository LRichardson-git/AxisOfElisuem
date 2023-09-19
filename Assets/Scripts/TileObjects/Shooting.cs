using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Shooting : NetworkBehaviour
{
    private ObjectSelector CurrentObj;
    public static Shooting Instance;

    Vector3 east = Vector3.back;// east
    Vector3 south = Vector3.left; //south
    Vector3 north = Vector3.right; //north
    Vector3 west = Vector3.forward; //west

    List<Vector3> directions;
    List<Cover> coverList;
    List<Vector3> Visited;
    private void Awake()
    {
        Instance = this;
        CurrentObj = GetComponent<ObjectSelector>();
        directions = new List<Vector3> { east, south, north, west };
        coverList = new List<Cover>();
        Visited = new List<Vector3>();
    }

    public void CheckSight(Unit unit)
    {
        List<Unit> result = new List<Unit>();

        // Iterate through all units in the scene
        foreach (Unit targetUnit in UnitManager.Instance.GetUnitList())
        {
            // Check if the unit can see the target unit
            Vector3 direction = targetUnit.targetPoint.transform.position - unit.targetPoint.transform.position;

            if (Physics.Raycast(unit.targetPoint.transform.position, direction, out RaycastHit hitInfo, unit.Vision * 10))
            {
                if (hitInfo.collider.gameObject.CompareTag("Unit"))
                {
                    result.Add(targetUnit);
                }
            }
            unit.setList(result);
            //NetworkIdentity CalledIdentity = unit.GetComponent<NetworkIdentity>();
            //TargetVisibleUnits(CalledIdentity.connectionToClient, unit, result);
            
        }
        
        //CalulateHitPercentage(unit);
    }

    
    public float CalulateHitPercentage(Unit Unit, Unit Target )
    {
        //Vector3 targetPos = Target.transform.position;

        //transform.forward = west

        //south, west , north , east
        
        Unit.transform.LookAt(Target.transform);

        foreach (Cover cover in Target.covers)
        {
            //make sure unit is looking at target before
            if (Vector3.Dot(cover.Direction, Unit.transform.forward) > -0.1)
                Debug.Log("Not Covered : " + Vector3.Dot(cover.Direction, Unit.transform.forward));
            else
                Debug.Log("covered: " + cover.height + " : "+ Vector3.Dot(cover.Direction, Unit.transform.forward));

        }

       



        return 1;
    }

    public List<Cover> CalulateCover(Unit unit)
    {
        //dont call this if not type to
        //cover only exist to normal units
        //depth 2 only for now

        /*
        coverList = new List<Cover>(); // Declare and initialize the coverList
        Visited = new List<Vector3>();


        foreach (Vector3 dir in directions)
        {
            CheckCoverSpot(dir, unit, coverHeight.Tall);
        }

        foreach (Vector3 dir in directions)
        {

            foreach (Vector3 dir2 in Visited)
            {
                if (dir2 != dir)
                    CheckCoverSpot(dir, unit, coverHeight.Short);
            }
        }
        */
        return World_Pathfinding.CheckCover(unit.x,unit.y,unit.z);
    }


    private void CheckCoverSpot(Vector3 direction, Unit unit, coverHeight height)
    { 
        RaycastHit hit;

        Vector3 pos = unit.transform.position;
        if (height == coverHeight.Tall)
            pos = unit.targetPoint.transform.position;
        else
            pos.y = 2.5f;

        if (Physics.Raycast(pos, direction, out hit, 10f))
        {
            if (hit.collider.CompareTag("wall"))
            {
                Cover cover = new Cover(height, direction);
                coverList.Add(cover);
                Visited.Add(direction);
                
            }
        }
       
    }










private void TargetVisibleUnits(NetworkConnectionToClient target, Unit unit, List<Unit> units)
    {
        unit.setList(units);
    }




}
