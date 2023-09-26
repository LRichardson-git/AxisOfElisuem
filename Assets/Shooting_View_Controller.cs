using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
public class Shooting_View_Controller : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ChanceToHit;
    [SerializeField]
    private TextMeshProUGUI ChanceToCrit;
    [SerializeField]
    private TextMeshProUGUI ChanceToDmg;

    public  GameObject manager;
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
    }

    public void UpdateInfo(TargetData Data)
    {
        ChanceToHit.text = Data.getHit() + "% to hit";
        ChanceToCrit.text = Data.getCrit() + "% to crit";
        ChanceToDmg.text = Data.minDmg + "-" + Data.maxDmg + " Dmg";
        unitID = Data.getUnit().getID();
        Tdata = Data;
    }


    public void Fire()
    {
        int Dmg = Random.Range(Tdata.minDmg, Tdata.maxDmg);
        Shooting.Instance.CmdHitUnit(unitID,Tdata.getHit(), Dmg, Tdata.getUnit().gun.penetration,Tdata.getCrit());
        manager.SetActive(false);
    }
}
