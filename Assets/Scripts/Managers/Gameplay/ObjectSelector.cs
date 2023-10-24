using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public static ObjectSelector Instance { get; private set; }

    //Managers
    InputManager _input;
    AbilityManager _abilityManager;
    UnitManager _unitManager;

    private Unit selectedUnit;
    private Vector3Int previousCell = new Vector3Int();

    [SerializeField]
    Grenade grenade;
    Camera _cam;
    Vector3Int CellLocation;

    private bool buttonDown = false;
    public bool canmove = false;

    public bool canAction = true;
    Shooting _shooting;

    World_Pathfinding path;
    private void Start()
    {
        Instance = this;

        _input = GetComponent<InputManager>();
        _abilityManager = AbilityManager.Instance;
        _unitManager = UnitManager.Instance;
        _shooting = Shooting.Instance;
        CellLocation = new Vector3Int();
        _cam = Camera.main;
        path = World_Pathfinding.Instance;
    }

    void Update()
    {

        if (_input.Hotbar != 0 && selectedUnit != null)
            _abilityManager.pressButton(_input.Hotbar);




        if (_abilityManager.active == true || !canAction)
            return;


        if (_input.leftClick)
        {
            SelectUnit();
        }

        if (_input.SwitchTarget)
            nextUnit();


        //unity selected
        if (selectedUnit == null)
            return;



        if (_input.rightClick)
        {
            buttonDown = true;
            //show path?

        }

        else if (_input.rightLetGo && _abilityManager.active == false)
        {
            buttonDown = false; moveUnit();
            
        }

        CellLocation = path.worldToCoord(Camera.main.ScreenToWorldPoint(_input.MousePos), selectedUnit.width, selectedUnit.depth);

        if (!CellLocation.Equals(previousCell))
        {
            previousCell = CellLocation;
            //pre calculate cover and show
        }

        //Debug.Log(_input.Hotbar);
        //check abilities()

        
       






    }

    void nextUnit()
    {
        List<Unit> TempList = _unitManager.GetUnitList();
        int point = 0;



        if (selectedUnit == null)
        {
            foreach (Unit unit in _unitManager.GetUnitList())
                if (unit.ownedBy == Player.LocalInstance.playerID && unit.ActionPoints > 0)
                    SelectUnit(unit);
        }






        for (int i = 0; i < TempList.Count; i++)
            if (TempList[i] == selectedUnit)
            {
                point = i + 1;
                while (point < TempList.Count) {

                        if (TempList[point].ActionPoints > 0 && TempList[point].ownedBy == Player.LocalInstance.playerID)
                        {
                            SelectUnit(TempList[point]);
                            return;
                        }
                    point++;

                  }

                        for (int j = 0; j < TempList.Count; j++)
                        {
                            if (TempList[j].ActionPoints > 0 && TempList[j].ownedBy == Player.LocalInstance.playerID) 
                            {
                                SelectUnit(TempList[j]);
                                return;


                            }

                    }

                }

            }

        



        


    public void resetUnit()
    {
        if (selectedUnit == null)
            return;

        selectedUnit.crit = 10;
        //done to make sure anything effecting shooting is reset
        _shooting.CheckSight(selectedUnit);
        _shooting.SpawnButtons(selectedUnit.getList());

    }

    public Unit getSelectedUnit()
    {
        return selectedUnit;
    }

    private void moveUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(_input.MousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var tempor = path.worldToCoord(hit.point, selectedUnit.width, 1);
            selectedUnit.MoveUnit((int)tempor.x, (int)tempor.y, tempor.z);

            
        }
    }


    public void Deselect()
    {

        if (selectedUnit == null)
            return;

        selectedUnit.Deselect();
        selectedUnit = null;

        //Select next avaiable unity
        foreach (Unit unit in _unitManager.GetUnitList())
            if (unit.turn == true && unit.isOwned == true)
                SelectUnit(unit);

        if (selectedUnit == null) {

            _unitManager.removePaths();
            _shooting.RemoveButtons();
            _abilityManager.RemoveButtons();
            Player.LocalInstance.endTurn();
        }
        
        //select next avaiable unit

    }
    public void refreshUnit()
    {
        initiation();
    }
    private void SelectUnit(Unit unit)
    {
        if (selectedUnit != null)
        {
            selectedUnit.Deselect();
            selectedUnit = null;
        }

        selectedUnit = unit;
        initiation();
    }

    private void initiation()
    {
        
        Vector3 unitTransform = selectedUnit.transform.position;
        
        CameraControler.LocalInstance.SetCameraUnit(unitTransform);
        // Select the new unit
        selectedUnit.Select();
        _shooting.CheckSight(selectedUnit);
        _shooting.SpawnButtons(selectedUnit.getList());
        _unitManager.ShowPaths(selectedUnit);
        _abilityManager.createButtons(selectedUnit);
    }

    private void SelectUnit()
    {
        Ray ray = _cam.ScreenPointToRay(_input.MousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            Unit unit = hit.collider.GetComponent<Unit>();

            if (unit != null && unit.isOwned && unit.turn)
            {
                SelectUnit(unit);

            }
        }
    }





    //Grenade Related (move)
    public void playAnimation(string anim, Vector3 direction)
    {

        if (anim == null || anim == "")
            return;

        selectedUnit.playAnim(anim, direction);
    }

    public void returnGun() { selectedUnit.GetComponent<Solider>().gunModel.gameObject.SetActive(true); }
    public void AimGrenade() { grenade.gameObject.SetActive(true); grenade.Aim(selectedUnit.transform.position); }
    public bool FireGrenade(Vector3 End, GrenadeAbility ability)
    {
        float animspeed = 1.6f;

        //nothing in way fire
        if (grenade.CheckFire(selectedUnit.transform.position, End))
        {
            grenade.gameObject.SetActive(true); grenade.fireF(End, ability, animspeed, ability.Explode);
            selectedUnit.GetComponent<Solider>().gunModel.gameObject.SetActive(false);
            return true;
        }
        else
            foreach (GameObject targetPoint in selectedUnit.GetComponent<Solider>().targetPoints)
            {
                if (grenade.CheckFire(targetPoint.transform.position, End))
                {
                    grenade.Aim(targetPoint.transform.position);
                    grenade.gameObject.SetActive(true); grenade.fireF(End, ability, animspeed, ability.Explode);
                    selectedUnit.GetComponent<Solider>().gunModel.gameObject.SetActive(false);
                    return true;
                }
            }
        ability.deActivate();
        AudioManager.instance.PlaySound("Error");
        return false;
    }

    public void DeactiveGrenade()
    {
        grenade.stopAim();
    }

    public Unit hoveredUnit()
    {
        Ray ray = _cam.ScreenPointToRay(_input.MousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            Unit unit = hit.collider.GetComponent<Unit>();
            return unit;
        }

        return null;

    }
}










