using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class Unit : Tile_Object
{

    //only server needs to know
    public int movementPoints = 2;
    public int height = 1;
    public bool flying = false;
    public const int maxHP = 5;
    public int Vision = 20;
    public int range = 18;
    public int aim = 50;
    public int crit = 0;
    public int aimModifer = 0;
    public int amour = 0;
    private UnitInformationUpdater UnitInfo;
    private Unit_Movement _unit_Move;
    private AudioManager audioManager;
    [SyncVar]
    private int ID;
    Renderer[] renderers;
    public List<Ability> Abilities;
    public Gun gun;
    private Renderer _material;
    [SerializeField]
    private Animator animator;
    public Color DefaultColor;
    public bool highlighted = false;
    public int ownedBy = -1;
    //Networked so client can know
    [SyncVar(hook = nameof(OnHpChanged))]
    public int HP = 5;
    
    public int ActionPoints = 2;
    public bool turn = true;

    public GameObject Model;

    void OnHpChanged(int _Old, int _New)
    {
        if (HP > maxHP)
            HP = maxHP;

        UnitInfo.OnInfoChanged(HP);



        if (HP <= 0)
        {
           // UnitManager.Instance.RemoveUnit(this);


            //NetworkServer.Destroy(this.gameObject);

        }
        // -0 delete self
    }

   


    //maybe dir makes more sense with hitchanges with it?
    List<TargetData> InVision = new List<TargetData>();
    public GameObject targetPoint;
    [SerializeField]
    public List<Cover> covers;


    //client
    protected bool selected = false;
    private UnitInformationUpdater Info;



   

    private void Start()
    {
        Setup(x, y, z, width, depth);
        Info = GetComponent<UnitInformationUpdater>();
        covers = new List<Cover>();
        InVision = new List<TargetData>();
        UnitInfo = gameObject.GetComponent<UnitInformationUpdater>();
        OnHpChanged(HP, HP);
        _unit_Move = GetComponent<Unit_Movement>();
        _unit_Move = GetComponent<Unit_Movement>();
        Abilities = new List<Ability>();
        audioManager = AudioManager.instance;
        // _material = GetComponent<Renderer>();
        renderers = GetComponentsInChildren<Renderer>();

        if (ownedBy != -1)
        {
            foreach (NetworkConnection conn in NetworkServer.connections.Values)
            {
                
                   // NetworkIdentity.AssignClientAuthority(conn);
                    break;
                
            }
        }

    }

    public void doAction(int ACost)
    {
        ActionPoints -= ACost;

        if (ActionPoints < 1)
        {
            turn = false;
            ObjectSelector.Instance.Deselect();
        }
    }

    internal void Deselect()
    {
        selected = false;
    }

    internal void Select()
    {
        selected = true;
    }

    public bool getSelected() { return selected; }
    public void addToList(TargetData unit) { InVision.Add(unit); }

    public void setList(List<TargetData> units) { InVision = units; }

    public void DeleteList() { InVision.Clear(); }

    public void setPlayerID() { if (isOwned) { Player.LocalInstance.SetIDSetup(ownedBy); } }
    public List<TargetData> getList() { return InVision; }

    [Command]
    public void cmdCheckCover(Quaternion rot, int AC) { checkCover(rot,AC); }

    [ClientRpc]
    void checkCover(Quaternion rot, int AC)
    {
        Model.transform.rotation = rot;
        covers = Shooting.Instance.CalulateCover(this);

        if (isOwned)
        {

            Shooting.Instance.CheckSight(this);
            Shooting.Instance.SpawnButtons(getList());
            doAction(AC);
            if (turn) { UnitManager.Instance.ShowPaths(this); }
        }
    }

    public void DeleteCover() { covers.Clear(); }

    public int getID() { return ID; }

    public void ApplyDmg(int Dmg, int Pen)
    {
        if (Pen >= amour)
            HP -= Dmg;
        else
            HP -= (Dmg - amour);
    }

    //maybe but maybe not thin about it

    //[Command(requiresAuthority =false)]
    public void MoveUnit(int x, int y, int z)
    {
        _unit_Move.moveToTarget(x, y, z);
    }


    public void SetID(int id)
    {
        ID = id;
    }


    public void highlight()
    {
        foreach (Renderer renderer in renderers){
            highlighted = true;
            renderer.material.color = Color.green;
        }
        }

    public void DeHighlight() {
        foreach (Renderer renderer in renderers)
        {
            highlighted = false;
            renderer.material.color = DefaultColor;
        }
    }


    public void init()
    {
        foreach (Ability ability in Abilities)
        {
            ability.Init();
        }
    }

    public void playAnim(string anim, Vector3 direction) { direction.y = transform.position.y;   transform.LookAt(direction);  animator.speed = 2;  animator.Play(anim); audioManager.cmDPlaySound(anim); }
    
}


