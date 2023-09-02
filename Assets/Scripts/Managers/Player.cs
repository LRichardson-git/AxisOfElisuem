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
    void Start()
    {
        if (!isOwned) { return; }
        testingg();


        LocalInstance = this;
        
        Ui_Manager.Instance.init();
    }

    


    [Command]
    void testingg()
    {
        GameManager.Instance.newPlayer(this);

        
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!isOwned || turn == false) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            CmdMove();
        }

        if (Input.GetMouseButtonDown(1))
        {
            endTurn();
        }


    }

    //tell server 
    [Command]
    private void CmdMove()
    {
        //validate logic

        RpcMove();
    }


    //run on all clients (on server)
    [ClientRpc]
    private void RpcMove()
    {

        gameObject.transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
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
