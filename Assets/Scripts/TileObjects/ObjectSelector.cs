using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private Unit selectedUnit;
    //have towncentre/building here
    InputManager input;
    VisualPathfinding visual;
    shaderscript script;

    private Vector3Int previousCell = new Vector3Int();
    Vector3Int CellLocation;
    private bool buttonDown = false;


    private void Awake()
    {
        input = GetComponent<InputManager>();
        //Map_Generation.Instance.Generate_Map();
        visual = GetComponent<VisualPathfinding>();
        script = GetComponent<shaderscript>();
        CellLocation = new Vector3Int();
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
        else if (input.rightLetGo){ buttonDown = false;  visual.notShowMovement();  moveUnit();  script.DeHighLight(); script.DeHightLightTile(); }
        CellLocation = World_Pathfinding.worldToCoord(Camera.main.ScreenToWorldPoint(input.MousePos),selectedUnit.width);

        if (!CellLocation.Equals(previousCell) && buttonDown == true )
        {
            
            previousCell = CellLocation;
            
            visual.showMovement(selectedUnit);
            
            
        }
        
    }





    private void moveUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var tempor = World_Pathfinding.worldToCoord(hit.point, selectedUnit.width);
            Debug.Log("moving to: " + tempor);
            selectedUnit.GetComponent<Unit_Movement>().moveToTarget((int)tempor.x, (int)tempor.y, tempor.z);
        }
    }


    private void SelectUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("1");
            Unit unit = hit.collider.GetComponent<Unit>();
            if (unit != null)
            {
                Debug.Log("2");
                if (selectedUnit != null)
                {
                    Debug.Log("3");
                    // Deselect the previously selected unit
                    selectedUnit.Deselect();
                }

                // Select the new unit
                selectedUnit = unit;
                selectedUnit.Select();
            }
        }
    }
}
