using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.UI;
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
    private int unitID;
    private TargetData Tdata;

    private void Start()
    {
        Instance = this;
        manager.SetActive(false);
        _selector = ObjectSelector.Instance;
    }

    public void Activate()
    {
        manager.SetActive(true);
        firebutton.gameObject.SetActive(true);
        _selector.canAction = false;
    }

    public void UpdateInfo(TargetData Data)
    {
        ChanceToCrit.gameObject.SetActive(true);
        Name.text = "fire";
        Description.text = "Fire at the target unit and end this units turn";
        ChanceToHit.text = Data.getHit() + "% to hit";
        ChanceToCrit.text = Data.getCrit() + "% to crit";
        ChanceToDmg.text = Data.minDmg + "-" + Data.maxDmg + " Dmg";
        unitID = Data.getUnit().getID();
        Tdata = Data;
    }

    public void UpdateInfo(Ability ability) {

        ChanceToCrit.gameObject.SetActive(false);
        Name.text = ability.Name;
        Description.text = ability.Description;
        ChanceToDmg.text = "Range: " + ability.Range ;
        ChanceToHit.text = ability.Damage;

    }

    //REMEMBER TO KEEP UNIT IDS UNIQUE SO THAT THE CORRECT UNIT IS HIT
    public void Fire()
    {
        int Dmg = Random.Range(Tdata.minDmg, Tdata.maxDmg);
        Shooting.Instance.CmdHitUnit(unitID,Tdata.getHit(), Dmg, Tdata.getUnit().gun.penetration,Tdata.getCrit());
        
        Deactivate();
    }

    public void Deactivate()
    {
        manager.SetActive(false);
        _selector.canAction = true;
        _selector.resetUnit();
    }
}
