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
            if (targetUnit == unit)
                continue;

            //add condition for team


            // Check if the unit can see the target unit
            if (CanSeeUnit(unit, targetUnit))
            {
                result.Add(targetUnit);
            }
        }
        unit.setList(result);
    }

    private bool CanSeeUnit(Unit unit, Unit target)
    {
        Vector3 direction = target.targetPoint.transform.position - unit.targetPoint.transform.position;
        float distance = Vector3.Distance(unit.transform.position, target.transform.position);

        if (distance > unit.Vision)
        {
            return false;
        }

        //normal enemy in open
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(unit.targetPoint.transform.position, direction, out hitInfo, distance);
        if (hit && hitInfo.collider.gameObject.CompareTag("Unit"))
        {
            return true;
        }

        //unit in cover, covers all possibilities
        if (unit.covers.Count > 0)
        {
            foreach (GameObject unitPoint in unit.GetComponent<Solider>().targetPoints)
            {
                RaycastHit hitInfo2;
                if (Physics.Raycast(unitPoint.transform.position, direction, out hitInfo2, distance))
                {
                    if (hitInfo2.collider.gameObject.CompareTag("Unit"))
                    {
                        return true;
                    }
                    else if (target.covers.Count > 0)
                    {
                        foreach (GameObject targetPoint in target.GetComponent<Solider>().targetPoints)
                        {
                            if (!Physics.Raycast(targetPoint.transform.position, direction, out RaycastHit hitInfoC, distance))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        //unit not in cover, enemy in cover
        else 
             {
              foreach (GameObject targetPoint in target.GetComponent<Solider>().targetPoints)
                    {
                   if (!Physics.Raycast(targetPoint.transform.position, direction, out RaycastHit hitInfoC, distance))
                        {
                            return true;
                        }
                    }
                }
            
        
        return false;
    }






    public float CalulateHitPercentage(Unit unit,GameObject Hitpoint ,Unit Target )
    {
        float Modifers = 0;

        int result;

        if (Target.covers.Count > 0)
            Modifers += EnemyCover(Hitpoint, Target);

        //add negative for gun range etc.. here

        result = (int)unit.aim + (int)Modifers;

        Debug.Log("Chance to hit: " + result);
        return result;
    }

    private float EnemyCover(GameObject HitPoint, Unit Target)
    {

        HitPoint.transform.LookAt(Target.transform);
        coverHeight coverType = coverHeight.none;
        //1 == no cover
        float closestCover = 1;

        foreach (Cover cover in Target.covers)
        {
            float result = Vector3.Dot(cover.Direction, HitPoint.transform.forward);
            //make sure unit is looking at target before
            if (result > -0.1)
            {
                Debug.Log("Not Covered : " + result);
                coverType = coverHeight.none;
            }
            else
            {
                Debug.Log("covered: " + cover.height + " : " + Vector3.Dot(cover.Direction, HitPoint.transform.forward));
                if (result <= closestCover)
                    coverType = cover.height;
            }
        }

        if (coverType == coverHeight.Tall)
            return -40;

        else if (coverType == coverHeight.Short)
            return -20;

        else if (coverType == coverHeight.none)
            return 30;

        return 0;
    }




    public List<Cover> CalulateCover(Unit unit)
    {
        //dont call this if not type to
        //cover only exist to normal units
        //depth 2 only for now

        
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

        return coverList;
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
