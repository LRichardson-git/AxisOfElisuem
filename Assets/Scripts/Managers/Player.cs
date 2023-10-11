using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEditor;
using UnityEngine.UI;
public class Player : NetworkBehaviour
{
    // Start is called before the first frame update

    [SyncVar]
    public bool turn = true;

    [SyncVar]
    public bool testing = false;

   

    public static Player LocalInstance { get; private set; }

    //(hook = nameof(CmdendTurn))
    private void Start()
    {
        
        if (!isOwned) { return; }
        Setup();

        LocalInstance = this;
        
        Ui_Manager.Instance.init();

      
    }


    [Command]
    void Setup()
    {

        GameManager.Instance.newPlayer(this,this.connectionToClient);
    }




    // Update is called once per frame
    void Update()
    {

        if (!isOwned || turn == false) { return; }

    }

  

 

    
    void endTurn()
    {
        
        if (!isOwned || turn == false) { return; }
        
        //CmdEndTurn();

    }

    [Command]
    void CmdEndTurn()
    {
        turn = false;

        GameManager.Instance.EndTurn();

    }



}
