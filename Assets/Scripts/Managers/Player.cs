
using UnityEngine;
using Mirror;


public class Player : NetworkBehaviour
{
    // Start is called before the first frame update
    [SyncVar]
    public bool turn = true;

    bool setup = false;

    public int playerID = 0;

    public Grenade gren;
    public static Player LocalInstance { get; private set; }
    ObjectSelector _selector;
    //haoppens once at start
    public void SetIDSetup(int ID)
    {
        playerID = ID;

        //player 1 starts with first turn
        if (ID != 1 && setup == false)
        {
            setup = true;
            turn = false;
            _selector.canAction = false;
            Shooting.Instance.Team = ID;
            
        }


        

    }


    

  


    //(hook = nameof(CmdendTurn))
    private void Start()
    {
        
        if (!isOwned) { return; }

        gren = Grenade.Instance;
        LocalInstance = this;
        Setup();
        _selector = ObjectSelector.Instance;
      
    }

    


    [Command]
    void Setup()
    {

        GameManager.Instance.newPlayer(this,this.connectionToClient);
    }




    // Update is called once per frame
    void Update()
    {

        if (!isOwned) { return; }

        if (Input.GetKeyUp(KeyCode.P))
        {
            endTurn();
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            CameraControler.LocalInstance.transform.rotation = CameraControler.LocalInstance.defaultRotation;
        }
    }

  

 

    
    public void endTurn()
    {
        
        if (!isOwned || turn == false) { return; }

        CmdEndTurn(playerID);
    }

    [Command]
    void CmdEndTurn(int ID)
    {
        checkTurn(ID);

    }
    [ClientRpc]
    void checkTurn(int ID)
    {
        //so that it happens on the local player not the whichever one called it
        Player.LocalInstance.checkTurnLocal(ID);

        
        
    }

    //make it so u can do stuff on your turn
    void checkTurnLocal(int ID)
    {
        if (!isOwned) { return; }

        

        if (ID != playerID)
        {
            turn = true;
            _selector.canAction = true;
            _selector.nextUnit();
        }
        else
        {
            turn = false;
            _selector.canAction = false;
            _selector.Deselectt();
        }

        UnitManager.Instance.newTurn();
    }




}
