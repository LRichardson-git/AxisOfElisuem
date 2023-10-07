using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class testingmanger : NetworkBehaviour
{
    public GameObject blocker;
    public GameObject Solider;
    public int id = 0;
    public static testingmanger Instance;
    // Update is called once per frame

    private void Awake()
    {
        testingmanger.Instance = this;
        
    }

   

    void Update()
    {
        if (!isOwned)
            return;



        if (Input.GetKeyUp(KeyCode.H))
        {

            //Debug.Log("spawning");
           
            CmdServerSpawnSolider();

            
            
                //Debug.Log("Units: " + UnitManager.Instance.GetUnitList().Count);
            
        }
        
    }

    

    [Command(requiresAuthority = false)]
    void CmdServerSpawnSolider()
    {

        GameObject GameSolider = Instantiate(Solider, Vector3.zero, Quaternion.identity);
        
        NetworkServer.Spawn(GameSolider, connectionToClient);
        GameSolider.GetComponent<Unit>().SetID(id);
        id++;
        
        CnrAddUnitToManager(GameSolider);
        
    }
    
    public void testPath(Unit unit)
    {

        List<Vector3> allPaths = new List<Vector3>();

        //tempoirary
        int temp = 0;
        int movement = unit.movementPoints;
        Vector3 last;
        // calculate paths to all walkable end points
        for (int i = unit.x - (movement); i < unit.x + movement; i++)
        {
            for (int j = unit.y - (movement); j < unit.y + movement; j++)
            {
                if (j < 0)
                    continue;

                for (int k = unit.z - (movement); k < unit.z + movement; k++)
                {
                    
                    

                    if (World_Pathfinding.findPath(i, j, k, unit.x, unit.y, unit.z, unit.width, unit.height, unit.depth, unit.flying) != null)
                        allPaths.Add(World_Pathfinding.coordToWorld(i,j,k,1,1));

                }
 
            }
        }

        //so lets a go
        for(int i = 0 ; i < allPaths.Count; i++) 
        {

            Instantiate(blocker, allPaths[i], Quaternion.identity);
        }



    }


    [ClientRpc]
    void CnrAddUnitToManager(GameObject unit)
    {
        UnitManager.Instance.AddUnit(unit.GetComponent<Solider>());
        unit.GetComponent<Unit>().Abilities.Add(new GrenadeAbility(5, 2, 3));
        unit.GetComponent<Unit>().Abilities.Add(new SmokeAbility(5, 2, 5));
        unit.GetComponent<Unit>().Abilities.Add(new MedPack());
        unit.GetComponent<Unit>().init();
    }
    
}
