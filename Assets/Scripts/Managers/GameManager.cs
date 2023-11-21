using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
public class GameManager : NetworkBehaviour
{
    // Start is called before the first frame update


#pragma warning disable

    public TextMeshProUGUI playersText;

    private List<NetworkConnectionToClient> connections;
    public static GameManager Instance { get; private set; }

    
    [SerializeField]
    private SyncList<Player> playerList = new SyncList<Player>();

    [SyncVar]
    public int players;

    private void Awake()
    {

        Instance = this;
        connections = new List<NetworkConnectionToClient>();
       
        
    }
    
    void CmdGetUnitAuthority(NetworkIdentity unit , NetworkConnectionToClient connection)
    {
        unit.AssignClientAuthority(connection);
        
    }

    public int newPlayer(Player player, NetworkConnectionToClient connection)
    {
        
        playerList.Add(player);
        players++;
        
        connections.Add(connection);
   
        foreach(Unit unit in UnitManager.Instance.GetUnitList())
        {
            if (unit.ownedBy == players)
            {
                CmdGetUnitAuthority(unit.netIdentity, connection);
            }
        }

        setPlayerID();

        playersText.text = "Players: " + players + "/2"; 
        return players;
    }

    [ClientRpc]
    void setPlayerID() {

        UnitManager.Instance.setPlayerID();
    }




}
