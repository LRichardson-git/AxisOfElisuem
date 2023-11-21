using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
public class GameManager : NetworkBehaviour
{
    // Start is called before the first frame update


#pragma warning disable
    public SyncList<int> currentPlayers = new SyncList<int>();

    
    public SyncList<int> playerScores = new SyncList<int>();

    public TextMeshProUGUI playersText;
    [SerializeField]
    private int Playerturn = 0;
    private List<NetworkConnectionToClient> connections;
    
    public SyncList<int> humanPlayers = new SyncList<int>();
    public static GameManager Instance { get; private set; }

    
    [SerializeField]
    private SyncList<Player> playerList = new SyncList<Player>();

    private bool simultaneous = false;

    [SyncVar]
    public int players;

    private int turnsEnded;
    private int playersTurns;

    int nextPlayer;


    


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




    private void Update()
    {
        //start
        if (!isServer) { return; }


        

    }






    //Player tells server they have ended turn
    [Command]
    public void EndTurn(int player)
    {
        turnsEnded++;
        Debug.Log("Ended player turn");
        

        


    }

    [ClientRpc]
    void playerChange(int player)
    {


        Debug.Log("playerChange");

       



    }
    
    void newTurn()
    {

        //list of players can now 

        

        if(simultaneous)
        {
            foreach (Player player in playerList) { player.turn = true;}
            currentPlayers = humanPlayers;
            playersTurns = humanPlayers.Count;
            return;
        }

        nextPlayer = 0;

        currentPlayers = new SyncList<int> { nextPlayer };
        playerList[nextPlayer].turn = true;


    }


    //what happens between turns
    void changePlayer() {

        int temp = currentPlayers[currentPlayers.Count - 1] + 1;
        playersTurns = 1;
        currentPlayers = new SyncList<int> { temp };
       
        turnsEnded = 0;
    }
}
