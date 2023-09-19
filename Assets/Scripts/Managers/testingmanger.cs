using UnityEngine;
using Mirror;
public class testingmanger : NetworkBehaviour
{
    public GameObject blocker;
    public GameObject Solider;
    // Update is called once per frame

    private void Start()
    {
        netIdentity.AssignClientAuthority(connectionToClient);
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

    [Command]
    void CmdServerSpawnSolider()
    {
        GameObject GameSolider = Instantiate(Solider);
        NetworkServer.Spawn(GameSolider, connectionToClient);
        UnitManager.Instance.AddUnit(GameSolider.GetComponent<Unit>());
    }

}












/*
            for (int x = 0; x < 30; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    for (int z = 0; z < 20; z++)
                    {
                        if (World_Pathfinding.getIndex()[x, y, z].type == Tile_Type.floor)
                            Instantiate(blocker,World_Pathfinding.coordToWorld(x,y,z,1,1),transform.rotation);
                    }
                }
            }*/