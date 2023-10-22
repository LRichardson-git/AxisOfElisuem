using UnityEngine;
using Mirror;
using System.Collections.Generic;
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

       



        if (Input.GetKeyUp(KeyCode.O))
            UnitManager.Instance.newTurn();
        
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
    
 


    [ClientRpc]
    void CnrAddUnitToManager(GameObject unit)
    {
        UnitManager.Instance.AddUnit(unit.GetComponent<Solider>());
        unit.GetComponent<Unit>().Abilities.Add(new Fire());
        unit.GetComponent<Unit>().Abilities.Add(new GrenadeAbility(5, 2, 3));
        unit.GetComponent<Unit>().Abilities.Add(new SmokeAbility(5, 2, 5));
        unit.GetComponent<Unit>().Abilities.Add(new MedPack());
        unit.GetComponent<Unit>().Abilities.Add(new HeadshotAbility());
        unit.GetComponent<Unit>().init();
    }
    
}
