using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    // Start is called before the first frame update


#pragma warning disable
    public SyncList<int> currentPlayers = new SyncList<int>();

    
    public SyncList<int> playerScores = new SyncList<int>();

    
    [SerializeField]
    private int turn;

    
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
        
        

        
    }
    
    void CmdGetUnitAuthority(NetworkIdentity unit , NetworkConnectionToClient connection)
    {

        unit.AssignClientAuthority(connection);
    }


    

    public int newPlayer(Player player, NetworkConnectionToClient connection)
    {
        
        playerList.Add(player);
        players++;


        foreach(Unit unit in UnitManager.Instance.GetUnitList())
        {
            if (unit.ownedBy == players)
            {
                CmdGetUnitAuthority(unit.netIdentity, connection);
                unit.ApplyDmg(2, 2);
            }
        }



        return players;
    }


    private void Update()
    {
        //start
        if (!isServer) { return; }
        if (Input.GetKeyDown("d"))
        {
            Debug.Log(players);
        }


            if (Input.GetKeyDown("p"))
        {
            for (int i = 0; i < players; i++)
            {
                playerScores.Add(0);
            }

            if (simultaneous)
            {
                //need changing
                currentPlayers = humanPlayers;
                playersTurns = humanPlayers.Count;
                
                return;
            }
            
            playersTurns = 1;
            currentPlayers.Add(0);
            playerList[0].turn = true;
            
            //currentPlayers[0] = 0;
            Debug.Log("started");
        }

    }


  



    //Player tells server they have ended turn
    
    public void EndTurn()
    {
        turnsEnded++;
        Debug.Log("Ended player turn");
        if (turnsEnded >= playersTurns)
        {
            foreach (int playerCurrent in currentPlayers)
            {
                //if all players done new turn
                if (playerCurrent >= players -1)
                {
                    
                    
                    newTurn();
                    turn++;
                }

                //go to next player (takes into account simltinous first :D)
                else if (playerCurrent >= currentPlayers.Count -1) {
                    nextPlayer = playerCurrent + 1;
                    
                    playerChange();


                }
            }

            turnsEnded = 0;

        }
            


    }

    
    void playerChange()
    {


        Debug.Log("playerChange");

        //current palyers == false

       // foreach (int ints in currentPlayers)
          //  playerList[ints].turn = false;


        currentPlayers = new SyncList<int> { nextPlayer };
        Debug.Log(currentPlayers[0]);
        Debug.Log(nextPlayer);
        playerList[nextPlayer].turn = true;



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
