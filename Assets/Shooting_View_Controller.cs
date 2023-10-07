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
    }

    public void Activate()
    {
        manager.SetActive(true);
        firebutton.gameObject.SetActive(true);
    }

    public void UpdateInfo(TargetData Data)
    {
        ChanceToHit.fontSize = 32;
        ChanceToHit.text = Data.getHit() + "% to hit";
        ChanceToCrit.text = Data.getCrit() + "% to crit";
        ChanceToDmg.text = Data.minDmg + "-" + Data.maxDmg + " Dmg";
        unitID = Data.getUnit().getID();
        Tdata = Data;
    }

    public void UpdateInfo(Ability ability) {
        ChanceToHit.fontSize = 16;
        ChanceToHit.text = ability.Name + "/n" + ability.Description;
        ChanceToCrit.text = "Range: " +ability.Range ;
        ChanceToDmg.text = ability.Damage ;

    }

    public void Fire()
    {
        int Dmg = Random.Range(Tdata.minDmg, Tdata.maxDmg);
        Shooting.Instance.CmdHitUnit(unitID,Tdata.getHit(), Dmg, Tdata.getUnit().gun.penetration,Tdata.getCrit());
        manager.SetActive(false);
    }
}
