using UnityEngine;
using Mirror;

public class testingmanger : NetworkBehaviour
{
    public GameObject blocker;
    public GameObject Solider;
    // Update is called once per frame

    private void Start()
    {
        //netIdentity.AssignClientAuthority(connectionToClient);
        
    }

   

    void Update()
    {
       // if (!isOwned)
          //  return;



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

        //CnrAddUnitToManager(unit);
        
    }
    

    [ClientRpc]
    void CnrAddUnitToManager(GameObject unit)
    {
        UnitManager.Instance.AddUnit(unit.GetComponent<Solider>()); 
    }
    
}
