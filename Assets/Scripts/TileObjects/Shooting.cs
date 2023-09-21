using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Shooting : NetworkBehaviour
{
    public GameObject buttonPrefab;
    private List<GameObject> spawnedButtons = new List<GameObject>();
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

        directions = new List<Vector3> { east, south, north, west };
        coverList = new List<Cover>();
        Visited = new List<Vector3>();
    }

    public void CheckSight(Unit unit)
    {
        unit.DeleteList();
        // Iterate through all units in the scene
        foreach (Unit targetUnit in UnitManager.Instance.GetUnitList())
        {
            if (targetUnit == unit)
                continue;

            //add condition for team
            // Check if the unit can see the target unit
            CanSeeUnit(unit, targetUnit);
            
        }
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
            TargetData Data = new TargetData(target,CalulateHitPercentage(unit,unit.targetPoint,target),unit.crit,target.targetPoint);
            unit.addToList(Data);
            //Debug.Log("straight line");
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
                        TargetData Data1 = new TargetData(target, CalulateHitPercentage(unit, unit.targetPoint, target), unit.crit, target.targetPoint);
                        unit.addToList(Data1);
                        //Debug.Log("in cover1");
                        return true;
                    }
                    else if (target.covers.Count > 0)
                    {
                        foreach (GameObject targetPoint in target.GetComponent<Solider>().targetPoints)
                        {
                            if (!Physics.Raycast(targetPoint.transform.position, direction, out RaycastHit hitInfoC, distance))
                            {
                                TargetData Data = new TargetData(target, CalulateHitPercentage(unit, unitPoint, target), unit.crit, targetPoint);
                                unit.addToList(Data);
                                //Debug.Log("in cover2");
                                return true;
                            }
                        }
                    }
                }
            }
        }

        //unit not in cover, enemy in cover
        else if (target.covers.Count > 0)
             {
              foreach (GameObject targetPoint in target.GetComponent<Solider>().targetPoints)
                    {
                   if (!Physics.Raycast(targetPoint.transform.position, direction, out RaycastHit hitInfoC, distance))
                        {
                        TargetData Data = new TargetData(target, CalulateHitPercentage(unit, unit.targetPoint, target), unit.crit, targetPoint);
                        unit.addToList(Data);
                    //Debug.Log("p[en enemy cover");
                    return true;
                        }
                    }
                }
            
        
        return false;
    }






    public int CalulateHitPercentage(Unit unit,GameObject Hitpoint ,Unit Target )
    {
        float Modifers = 0;

        int result;

        if (Target.covers.Count > 0)
        {
            Modifers += EnemyCover(Hitpoint, Target);

            if (Modifers > 0) 
                { unit.crit = 30; }
            else
                unit.crit = 0;
        }


            //add negative for gun range etc.. here

            result = (int)unit.aim + (int)Modifers;

        Debug.Log("Chance to hit: " + result);
        return result;
    }

    private float EnemyCover(GameObject HitPoint, Unit Target)
    {

        HitPoint.transform.LookAt(Target.transform.position);
        coverHeight coverType = coverHeight.none;
        //1 == no cover
        float closestCover = 1;
        Debug.Log(HitPoint.name);
        foreach (Cover cover in Target.covers)
        {
            Debug.Log(cover.Direction);
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
                {
                    closestCover = result;
                    coverType = cover.height;
                }
            }
        }

        if (coverType == coverHeight.Tall)
            return -40;

        else if (coverType == coverHeight.Short)
            return -20;

        else if (coverType == coverHeight.none && Target.covers.Count > 0)
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

    public void SpawnButtons(List<TargetData> targets)
    {

        // Remove any previously spawned buttons
        foreach (GameObject button in spawnedButtons)
        {
            Destroy(button);
        }
        spawnedButtons.Clear();

        if (targets.Count <= 0) return;

        // Calculate the position of the bottom right corner of the screen
        Vector2 spawnPosition = new Vector2(Screen.width, 50);

        // Get the width of the button prefab
        float buttonWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;

        // Calculate the spacing between buttons
        float buttonSpacing = buttonWidth * 0.1f; // Adjust this value as needed

        // Spawn new buttons based on the number of objects
        for (int i = 0; i < targets.Count; i++)
        {
            // Adjust the spawn position based on the button width and spacing
            spawnPosition -= new Vector2(buttonWidth + buttonSpacing, 0);

            GameObject button = Instantiate(buttonPrefab, spawnPosition, Quaternion.identity, transform);
            spawnedButtons.Add(button);
            button.GetComponent<ButtonScript>().init(targets[i]);
        }
    }






}
