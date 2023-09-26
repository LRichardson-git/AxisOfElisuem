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

    private Vector3Int previousCell = new Vector3Int();
    Vector3Int CellLocation;
    private bool buttonDown = false;
    public static ObjectSelector Instance { get; private set; }


    private void Awake()
    {
        input = GetComponent<InputManager>();
        //Map_Generation.Instance.Generate_Map();
        visual = GetComponent<VisualPathfinding>();
        script = GetComponent<shaderscript>();
        CellLocation = new Vector3Int();
        shooting = GetComponent<Shooting>();
    }

    void Update()
    {
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
        
        else if (input.rightLetGo){ buttonDown = false;    moveUnit();
            //script.DeHighLight(); script.DeHightLightTile(); //visual.notShowMovement();
        }

        CellLocation = World_Pathfinding.worldToCoord(Camera.main.ScreenToWorldPoint(input.MousePos),selectedUnit.width, selectedUnit.depth);

        if (!CellLocation.Equals(previousCell) && buttonDown == true )
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

            var tempor = World_Pathfinding.worldToCoord(hit.point, selectedUnit.width,1);
            selectedUnit.MoveUnit((int)tempor.x, (int)tempor.y, tempor.z);

            //selectedUnit.GetComponent<Unit_Movement>().moveToTarget((int)tempor.x, (int)tempor.y, tempor.z);
        }
    }


    private void SelectUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            Unit unit = hit.collider.GetComponent<Unit>();
            
            if (unit != null && unit.isOwned)
            {
      
                if ( selectedUnit != null)
                {

                    // Deselect the previously selected unit
                    selectedUnit.Deselect();
                    selectedUnit = null;
                }

                // Select the new unit
                selectedUnit = unit;
                selectedUnit.Select();
                //shooting.CheckSight(selectedUnit);
                //Debug.Log("Can see :" +  selectedUnit.getList().Count);
               // Debug.Log("covers : " + selectedUnit.covers.Count);
               // Debug.Log("pos : " + selectedUnit.transform.position);
                // for (int i = 0; i < selectedUnit.getList().Count; i++)
                // {
                //selectedUnit.getList()[
                // }
            }
            //else { Debug.Log("not owned"); }
        }
    }
}
