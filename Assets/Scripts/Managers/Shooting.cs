using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using Random = UnityEngine.Random;

public class Shooting : NetworkBehaviour
{
    public GameObject buttonPrefab;
    private List<GameObject> spawnedButtons = new List<GameObject>();
    public static Shooting Instance;
    UnitManager _unitManager;

    Vector3 east = Vector3.back;// east
    Vector3 south = Vector3.left; //south
    Vector3 north = Vector3.right; //north
    Vector3 west = Vector3.forward; //west

    private static Vector3 targetP = new Vector3();

    List<Vector3> directions;
    List<Cover> coverList;
    List<Vector3> Visited;
    public GameObject Smoke;

    public int Team = 0;

    public ShowHitDmg HitChance;
    public ShowHitDmg DmgChance;
    public fol bullet;
    public GameObject LOBBY;
    public GameObject activity;
    private void Awake()
    {
        Instance = this;
        //left to right
        directions = new List<Vector3> { west, north, east, south };
        coverList = new List<Cover>();
        Visited = new List<Vector3>();

    }

    private void Start()
    {
        _unitManager = UnitManager.Instance;

        if (isServer)
            LOBBY.SetActive(true);
    }

    public void CheckSight(Unit unit)
    {
        unit.DeleteList();
        // Iterate through all units in the scene
        
        foreach (Unit targetUnit in UnitManager.Instance.GetUnitList())
        {
            if (targetUnit == unit || targetUnit.ownedBy == Player.LocalInstance.playerID || !targetUnit.alive)
                continue;

            //Debug.Log(unit.name);
            //if cant see unit it is invisible
            if (CanSeeUnit(unit, targetUnit))
                targetUnit.canSee();


            

        }
    }

    

    public bool CanSeeUnit(Unit unit, Unit target)
    {

        

        Vector3 direction = target.targetPoint.transform.position - unit.targetPoint.transform.position;
        float distance = Vector3.Distance(unit.transform.position, target.transform.position);
        targetP = target.transform.position;
        if (distance > unit.Vision)
            return false;

        //normal enemy in open
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(unit.targetPoint.transform.position, direction, out hitInfo, distance);
        if (hit && hitInfo.collider.gameObject.CompareTag("Unit"))
        {
            TargetData Data = new TargetData(target,CalulateHitPercentage(unit,unit.targetPoint,target),unit.crit + unit.gun.getCrit(), target.targetPoint);
            Data.setDmg(unit.gun.getMin(), unit.gun.getMax());
            unit.addToList(Data);
           // Debug.Log("straight line");
            return true;
        }

        bool shouldbreak = false;

        //unit in cover, covers all possibilities
        if (unit.covers.Count > 0)
        {
            List<GameObject> byClosest = new List<GameObject>();

            

            foreach (GameObject Upoint in unit.GetComponent<Solider>().targetPoints)
            {
                byClosest.Add(Upoint);
            }

            byClosest.Sort(compareByDistance);


            foreach (GameObject unitPoint in byClosest) { 
                shouldbreak = false;

                Vector3 coverDir = unitPoint.transform.position - unit.targetPoint.transform.position;
                foreach (Cover cover in unit.covers)
                {

                    if (coverDir.normalized == cover.Direction)
                    {

//Debug.Log( unitPoint.name);
                        shouldbreak = true;
                        break;
                    }
                }

                if (shouldbreak)
                    continue;
                

                RaycastHit hitInfo2;
                direction = target.targetPoint.transform.position - unitPoint.transform.position;
                if (Physics.Raycast(unitPoint.transform.position, direction, out hitInfo2, distance))
                {
                    if (hitInfo2.collider.gameObject.CompareTag("Unit") && !hitInfo2.collider.gameObject.GetComponent<Unit>().isOwned)
                    {
                        
                        TargetData Data1 = new TargetData(target, CalulateHitPercentage(unit, unitPoint, target), unit.crit + unit.gun.getCrit(), target.targetPoint);
                        Data1.setDmg(unit.gun.getMin(), unit.gun.getMax());
                        unit.addToList(Data1);
                        //Debug.Log(unitPoint.name);
                        return true;
                    }
                    else if (target.covers.Count > 0)
                    {
                        foreach (GameObject targetPoint in target.GetComponent<Solider>().targetPoints)
                        {
                            direction = targetPoint.transform.position - unitPoint.transform.position;
                            if (!Physics.Raycast(targetPoint.transform.position, direction, out RaycastHit hitInfoC, distance))
                            {
                               // Debug.Log(unitPoint.transform.position);
                                
                                TargetData Data = new TargetData(target, CalulateHitPercentage(unit, unitPoint, target), unit.crit + unit.gun.getCrit(), targetPoint);
                                Data.setDmg(unit.gun.getMin(), unit.gun.getMax());
                                unit.addToList(Data);
                                //Debug.Log("in cover2");
                                //Debug.Log(unitPoint.name);
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
                direction = targetPoint.transform.position -unit.targetPoint.transform.position; 
                   if (Physics.Raycast(unit.targetPoint.transform.position, direction, out RaycastHit hitInfoC, distance))
                        {

                        if (hitInfoC.collider.gameObject.CompareTag("Unit") && !hitInfoC.collider.gameObject.GetComponent<Unit>().isOwned)
                        {
                            Debug.DrawLine(unit.targetPoint.transform.position, targetPoint.transform.position, Color.green, 10f);
                            TargetData Data = new TargetData(target, CalulateHitPercentage(unit, unit.targetPoint, target), unit.crit + unit.gun.getCrit(), targetPoint);
                            Data.setDmg(unit.gun.getMin(), unit.gun.getMax());
                            unit.addToList(Data);
                       // Debug.Log(targetPoint.name);
                        return true;
                        }

                       
                        }
                else 
                {
                    Debug.DrawLine(unit.targetPoint.transform.position, targetPoint.transform.position, Color.green, 10f);
                    TargetData Data = new TargetData(target, CalulateHitPercentage(unit, unit.targetPoint, target), unit.crit + unit.gun.getCrit(), targetPoint);
                    Data.setDmg(unit.gun.getMin(), unit.gun.getMax());
                    unit.addToList(Data);
                   // Debug.Log(targetPoint.name);
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
            { unit.crit += 30; Modifers += 15; }
            else
                unit.crit = 0;
        }
        else
            Modifers += 25;


        if (unit.y > Target.y + 1)
            Modifers += 20;
        else if (unit.y < Target.y - 1)
            Modifers -= 20;

        Modifers += _unitManager.GetModifers(Target);


        //add negative for gun range etc.. here

        float distance = Vector3.Distance(unit.transform.position, Target.transform.position);
        //float distance = (unit.transform.position - Target.transform.position).sqrMagnitude;
        distance = distance / 10;
        if (distance >  unit.gun.maxRange)
        {
            Modifers -= (unit.gun.punishment * (distance - unit.gun.maxRange));
        }
        else if (distance < unit.gun.minRange)
        {
            Modifers -= (unit.gun.punishment * ( unit.gun.minRange - distance));
        }
        


        

        result = (int)unit.aim + (int)Modifers;
        result = Math.Clamp(result,0, 100);
        return result;
    }

    private float EnemyCover(GameObject HitPoint, Unit Target)
    {

        HitPoint.transform.LookAt(Target.transform.position);
        coverHeight coverType = coverHeight.none;
        //1 == no cover
        float closestCover = 1;

        if (Vector3.Distance(HitPoint.transform.position, Target.transform.position) <= 18)
        {
            //Debug.Log("close");
            return 30;

        }

        foreach (Cover cover in Target.covers)
        {

            float result = Vector3.Dot(cover.Direction, HitPoint.transform.forward);
            //make sure unit is looking at target before
            if (result > -0.1)
            {
                //Debug.Log("Not Covered : " + result);
                coverType = coverHeight.none;
            }
            else
            {
                //Debug.Log("covered: " + cover.height + " : " + Vector3.Dot(cover.Direction, HitPoint.transform.forward));
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

        if (Visited.Count < 1)
        {
            foreach (Vector3 dir in directions)
            {


                CheckCoverSpot(dir, unit, coverHeight.Short);

            }
        }
        else
        {
            foreach (Vector3 dir in directions)
            {

                    if (!Visited.Contains(dir))
                        CheckCoverSpot(dir, unit, coverHeight.Short);
                
            }
        }
       // Debug.Log(coverList.Count);
        return coverList;
    }


    private void CheckCoverSpot(Vector3 direction, Unit unit, coverHeight height)
    { 
        RaycastHit hit;

        Vector3 pos = unit.transform.position;
  
            

        pos = unit.targetPoint.transform.position;

        if (height != coverHeight.Tall) {
            pos.y -= 15.5f;
 
        }

        if (Physics.Raycast(pos, direction, out hit, 10f))
        {
            if (hit.collider.CompareTag("wall"))
            {
                Cover cover = new Cover(height, direction);
                coverList.Add(cover);
                Visited.Add(direction);
                Vector3 lookpoint = hit.point;
                lookpoint.y = unit.Model.transform.position.y;
                unit.Model.transform.LookAt(lookpoint);
                unit.Model.transform.Rotate(0,45,0);
   
                
            }
        }
       
    }

  


    

   
        public static float CalculateDistance(GameObject point1, Vector3 point2)
        {
            return Vector3.Distance(point1.transform.position, point2);
        }

        Comparison<GameObject> compareByDistance = (point1, point2) =>
        {
            float distance1 = Vector3.Distance(point1.transform.position, targetP);
            float distance2 = Vector3.Distance(point2.transform.position, targetP);
            return distance1.CompareTo(distance2);
        };

    

    public void RemoveButtons()
    {
 
        foreach (GameObject button in spawnedButtons)
        {

            Destroy(button);
        }
        spawnedButtons.Clear();
    }

    public void SpawnButtons(List<TargetData> targets)
    {
        // Remove any previously spawned buttons
        RemoveButtons();

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

  
    [Command(requiresAuthority = false)]
    public void CmdHitUnit(int unitID, int HitChance, int Dmg, int Pen, int crit)
    {
        int Damage = Dmg;
        Debug.Log("Test");
        Showhit(HitChance, unitID);


        if (Random.Range(0, 100) <= HitChance)
        {
            if (Random.Range(0, 100) <= crit)
                Damage = Dmg * 2;
            StartCoroutine(DamageUnit(unitID, Damage, Pen));
            ShowDmg(Damage, unitID);
        }

    }

     IEnumerator  DamageUnit(int ID, int Damage, int Pen) {


        yield return new WaitForSeconds(2);
        
        dmgUnit(ID, Damage, Pen);

    }


    [Command(requiresAuthority = false)]
    public void CmdDmgUnit(int unitID, int minDmg, int maxDmg, int pen)
    {
        int damage = Random.Range(minDmg, maxDmg);

        dmgUnit(unitID, damage, pen);
    }

    [Command(requiresAuthority = false)]
    public void CmdDmgUnit(int unitID, int Dmg)
    {
        //add ignoreing pen to unit method if we have an abiltiy like that
        dmgUnit(unitID, Dmg, 10);

    }

    [Command(requiresAuthority = false)]
    public void CmDShootAtunit(int SpawnID, int ID)
    {
        spawnbullet(SpawnID, ID);
    }

    [ClientRpc]
    void spawnbullet(int SpawnID, int ID)
    {
        Vector3 spawn = new Vector3();
        Vector3 target = new Vector3();
        foreach (Unit unit in _unitManager.GetUnitList())
        {
            if (SpawnID == unit.getID()) {
                spawn = unit.transform.position;
                continue;
            }

            if (ID == unit.getID())
            {
                target = unit.transform.position;
                continue;    
            }
        }

        fol temp = Instantiate(bullet, spawn, Quaternion.identity);
        temp.initil(target);

    }

    [Command (requiresAuthority = false)]
    public void cmdLookat(Vector3 point) {

        PanTo(point);
    }


    [ClientRpc]
    void PanTo(Vector3 locaiton)
    {
        CameraControler.LocalInstance.SetCameraUnit(locaiton, 50);
    }
    [Command (requiresAuthority = false)]
    public void cmdStartGame()
    {
        startGame();
    }
    [ClientRpc]
    void startGame()
    {
        foreach (Unit unit in _unitManager.GetUnitList())
            unit.GetComponent<Solider>().begun = true;

        if (Player.LocalInstance.turn == false)
        {
            activity.SetActive(true);
            Quaternion temp = Quaternion.Euler(30, -45, 0);
            CameraControler.LocalInstance.transform.rotation = temp;
        }
        else
            ObjectSelector.Instance.canAction = true;

        ObjectSelector.Instance.nextUnit();
        ObjectSelector.Instance.Deselectt();
    }





    [ClientRpc]
    void Showhit(int Hit, int ID)
    {
        Debug.Log("showhit");
        HitChance.ShowDmghit(Hit, ID);
    }

    [ClientRpc]
    void ShowDmg(int Hit, int ID)
    {
        Debug.Log("showDmg");
        StartCoroutine(DamageUnit(ID, Hit));
    }


    IEnumerator DamageUnit(int ID, int Damage)
    {


        yield return new WaitForSeconds(1.8f);
        DmgChance.ShowDmghit(Damage, ID);
        

    }


    public void dmgUnit(int ID, int Dmg, int pen)
    {
        foreach (Unit unit in UnitManager.Instance.GetUnitList())
        {
            if (unit.getID() == ID)
            {
                unit.ApplyDmg(Dmg, pen);
            }
        }
    }



    public List<GameObject> getButtons() { if (spawnedButtons != null) return spawnedButtons; else return null; }




}
