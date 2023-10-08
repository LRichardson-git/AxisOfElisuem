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




    private void Start()
    {
        Instance = this;

        _input = GetComponent<InputManager>();
        _abilityManager = AbilityManager.Instance;
        _unitManager = UnitManager.Instance;

        CellLocation = new Vector3Int();
        _cam = Camera.main;
    }

    void Update()
    {
        if (_abilityManager.active == true)
            return;


        if (_input.leftClick)
        {
            SelectUnit();
        }

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

        CellLocation = World_Pathfinding.worldToCoord(Camera.main.ScreenToWorldPoint(_input.MousePos), selectedUnit.width, selectedUnit.depth);

        if (!CellLocation.Equals(previousCell))
        {
            previousCell = CellLocation;
            //pre calculate cover and show
        }

        //Debug.Log(_input.Hotbar);
        //check abilities()
        if (_input.Hotbar != 0)
            _abilityManager.pressButton(_input.Hotbar);






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
            var tempor = World_Pathfinding.worldToCoord(hit.point, selectedUnit.width, 1);
            selectedUnit.MoveUnit((int)tempor.x, (int)tempor.y, tempor.z);
        }
    }


    private void SelectUnit()
    {
        Ray ray = _cam.ScreenPointToRay(_input.MousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            Unit unit = hit.collider.GetComponent<Unit>();

            if (unit != null && unit.isOwned)
            {

                if (selectedUnit != null)
                {
                    selectedUnit.Deselect();
                    selectedUnit = null;
                }

                // Select the new unit
                selectedUnit = unit;
                selectedUnit.Select();

                //call
                _unitManager.ShowPaths(selectedUnit);
                _abilityManager.createButtons(selectedUnit);

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










