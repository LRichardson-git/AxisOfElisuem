using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class VisualPathfinding : MonoBehaviour
{

    public LineRenderer lineOne;
    public LineRenderer lineTwo;

    
    Vector3 temp;


    private void Start()
    {

        if (lineOne == null )
        {
            lineOne = new LineRenderer();

        }

        setupLines();
        
    }

    private void setupLines()
    {
        if (lineOne != null && lineTwo != null)
        {

            lineOne.startColor = Color.green;
            lineOne.endColor = Color.green;
            lineTwo.startColor = Color.red;
            lineTwo.startColor = Color.red;

            lineOne.startWidth = 1f;
            lineOne.endWidth = 1f;
            lineTwo.startWidth = 1f;
            lineTwo.endWidth = 1f;
            lineOne.material = new Material(Shader.Find("Sprites/Default"));
            lineTwo.material = new Material(Shader.Find("Sprites/Default"));
        }
    }


    public void showMovement(Unit _unit)
    {

        notShowMovement();
        RaycastHit2D hit = World_Pathfinding.getWorldMouse();

        

        var endCoord = World_Pathfinding.worldToCoord(hit.point,_unit.width);
         // assuming the current object's position is the starting point

        // Call the findPath function to get the path
        var path = World_Pathfinding.findPath(endCoord.x, endCoord.y,endCoord.z, _unit.x, _unit.y,_unit.z, _unit.width, _unit.height,1);

        if (path != null)
        {
            // Convert the Coords returned by the findPath function to Vector3 path

            // Use the path to visually display the path on your grid

            


            int movePoints = _unit.movementPoints;

            if (movePoints < 1)
            {
                lineTwo.positionCount = path.Count;
                for (int i = 0; i < path.Count; i++)
                {
                    
                    lineTwo.SetPosition(i, path[i]);
                }
                return;
            }

            lineOne.positionCount = path.Count;
            bool lol = false;
            if (lineOne.positionCount > movePoints)
            {
                lineOne.positionCount = movePoints;
                
                lineTwo.positionCount = path.Count - (movePoints - 1);
                lol = true;

            }
            for (int i = 0; i < path.Count ; i++)
            {
                
                if(i < movePoints)
                {  lineOne.SetPosition(i, path[i]); }

                if (i >= movePoints -1 && lol == true )
                {
                    lineTwo.SetPosition((i  + 1)- (movePoints), path[i]);
                }

            }
            //shaderscript.Instance.HightLightTile(endCoord, _unit.width,_unit.height);
        }
    }



    public void notShowMovement()
    {
        lineTwo.positionCount = 0;
        lineOne.positionCount = 0;
    }

    public void highlightTiles(int x, int y)
    {



    }
}
