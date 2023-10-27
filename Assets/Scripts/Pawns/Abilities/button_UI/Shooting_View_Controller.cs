using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Collections;

public class Shooting_View_Controller : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ChanceToHit;
    [SerializeField]
    private TextMeshProUGUI ChanceToCrit;
    [SerializeField]
    private TextMeshProUGUI ChanceToDmg;
    [SerializeField]
    private TextMeshProUGUI Name;
    [SerializeField]
    private TextMeshProUGUI Description;

    ObjectSelector _selector;

    public  GameObject manager;
    public Button firebutton;
    //dont see why not
    public static Shooting_View_Controller Instance;
    private CameraControler _controler;
    private int unitID;
    private TargetData Tdata;
    AudioManager _audio;
    Ability CurrentAbility;
    bool AbilityA = false;
    Unit CurrrentUnit;
    private void Start()
    {
        Instance = this;
        manager.SetActive(false);
        _selector = ObjectSelector.Instance;
        _controler = CameraControler.LocalInstance;
        _audio = AudioManager.instance;
    }

    public void Activate()
    {
        AbilityA = false;
        manager.SetActive(true);
        firebutton.gameObject.SetActive(true);
        _selector.canAction = false;
    }

    public void UpdateInfo(TargetData Data)
    {
        
        if (CurrrentUnit != null)
            CurrrentUnit.DeHighlight();
            

        CurrrentUnit = Data.getUnit();
        _selector.playAnimation("Aiming", Data.getUnit().transform.position);
        ChanceToCrit.gameObject.SetActive(true);
        Name.text = "fire";
        Description.text = "Fire at the target unit and end this units turn";
        ChanceToHit.text = Data.getHit() + "% to hit";
        ChanceToCrit.text = Data.getCrit() + "% to crit";
        ChanceToDmg.text = Data.minDmg + "-" + Data.maxDmg + " Dmg";
        unitID = Data.getUnit().getID();
        Tdata = Data;
        _controler.SetCameraUnit(Data.getUnit().transform.position);
        CurrrentUnit.highlight();
    }

    public void UpdateInfo(Ability ability) {
        firebutton.gameObject.SetActive(false);
        AbilityA = false;
        ChanceToCrit.gameObject.SetActive(false);
        Name.text = ability.Name;
        Description.text = ability.Description;
        ChanceToDmg.text = "Range: " + ability.Range ;
        ChanceToHit.text = ability.Damage;
        
    }


    public void UpdateInfo(Ability ability, bool trueing)
    {

        ChanceToCrit.gameObject.SetActive(false);
        Name.text = ability.Name;
        Description.text = ability.Description;
        ChanceToDmg.text = "";
        ChanceToHit.text = ability.Damage;
        CurrentAbility = ability;
        AbilityA = true;
    }

    //REMEMBER TO KEEP UNIT IDS UNIQUE SO THAT THE CORRECT UNIT IS HIT
    public void Fire()
    {
        if (!AbilityA)
        {
            int Dmg = Random.Range(Tdata.minDmg, Tdata.maxDmg);
            Shooting.Instance.CmdHitUnit(unitID, Tdata.getHit(), Dmg, Tdata.getUnit().gun.penetration, Tdata.getCrit());
            StartCoroutine(ShootSound(_selector.getSelectedUnit()));
            _selector.getSelectedUnit().doAction(2,3);
            
        }
        else if (CurrentAbility != null)
        {
            CurrentAbility.Execute(transform.position);
        }
        Deactivate();
    }

    IEnumerator ShootSound(Unit unit)
    {


        yield return new WaitForSeconds(1);
        _audio.cmDPlaySound(unit.gun.sound);

    }


    public bool On()
    {
        if (manager.activeSelf)
            return true;

        return false;
    }
    public void Deactivate()
    {
        if (CurrrentUnit != null)
            CurrrentUnit.DeHighlight();
        manager.SetActive(false);
        _selector.canAction = true;
        _selector.resetUnit();
    }
}
