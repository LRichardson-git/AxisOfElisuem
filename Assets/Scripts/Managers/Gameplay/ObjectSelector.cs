using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private Unit selectedUnit;
    InputManager input;
    VisualPathfinding visual;
    shaderscript script;
    Shooting shooting;
    AbilityManager abilityManager;
    private Vector3Int previousCell = new Vector3Int();
    Vector3Int CellLocation;
    private bool buttonDown = false;
    public bool canmove = false;
    public static ObjectSelector Instance { get; private set; }
    public GameObject smoke;
    [SerializeField]
    Grenade grenade;
    Camera _cam;

    private void Start()
    {
        Instance = this;
        input = GetComponent<InputManager>();
        //Map_Generation.Instance.Generate_Map();
        visual = GetComponent<VisualPathfinding>();
        script = GetComponent<shaderscript>();
        CellLocation = new Vector3Int();
        shooting = GetComponent<Shooting>();
        abilityManager = AbilityManager.Instance;
        _cam = Camera.main;
    }

    void Update()
    {
        if (abilityManager.active == true)
            return;


        if (input.leftClick)
        {
            SelectUnit();
        }

        //unity selected
        if (selectedUnit == null)
            return;
        if (input.rightClick)
        {
            buttonDown = true;
            //script.HighlightTiles(World_Pathfinding.findAllPaths(selectedUnit.x, selectedUnit.y, selectedUnit.movementPoints, selectedUnit.width, selectedUnit.height));

        }

        else if (input.rightLetGo && abilityManager.active == false)
        {
            buttonDown = false; moveUnit();
            //script.DeHighLight(); script.DeHightLightTile(); //visual.notShowMovement();
        }

        CellLocation = World_Pathfinding.worldToCoord(Camera.main.ScreenToWorldPoint(input.MousePos), selectedUnit.width, selectedUnit.depth);

        if (!CellLocation.Equals(previousCell) && buttonDown == true)
        {
            previousCell = CellLocation;
            //visual.showMovement(selectedUnit);
        }

        if (Input.GetKeyUp(KeyCode.T))
        {

            //Shooting.Instance.CmdHitUnit(1,50,2,2);
            selectedUnit.HP -= 1;
        }

    }

    public Unit getSelectedUnit()
    {
        return selectedUnit;
    }

    private void moveUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            var tempor = World_Pathfinding.worldToCoord(hit.point, selectedUnit.width, 1);
            selectedUnit.MoveUnit((int)tempor.x, (int)tempor.y, tempor.z);

            //selectedUnit.GetComponent<Unit_Movement>().moveToTarget((int)tempor.x, (int)tempor.y, tempor.z);
        }
    }


    private void SelectUnit()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            Unit unit = hit.collider.GetComponent<Unit>();

            if (unit != null && unit.isOwned)
            {

                if (selectedUnit != null)
                {

                    // Deselect the previously selected unit
                    selectedUnit.Deselect();
                    selectedUnit = null;
                }

                // Select the new unit
                selectedUnit = unit;
                selectedUnit.Select();
                UnitManager.Instance.ShowPaths(selectedUnit);
                //    Debug.Log(abilityManager);
                AbilityManager.Instance.createButtons(selectedUnit);

            }
        }
    }


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

        Vector3 direction = (End - selectedUnit.transform.position);
        float distance = Vector3.Distance(selectedUnit.transform.position, End) - 1;
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
        Debug.Log("false");
        ability.deActivate();

        return false;
    }

    public void DeactiveGrenade()
    {
        grenade.stopAim();
    }

    public Unit hoveredUnit()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            Unit unit = hit.collider.GetComponent<Unit>();
            return unit;
        }

        return null;

    }
}










