using UnityEngine;
using TMPro;
public class ButtonScript : MonoBehaviour
{
    private TargetData data;
    [SerializeField]
    private TextMeshProUGUI ChanceToHit;
    public void init(TargetData data)
    {
        this.data = data;
        ChanceToHit.text = data.getHit() + "%";
    }

    public void openShootScreen()
    {
        Shooting_View_Controller.Instance.UpdateInfo(data);
        Shooting_View_Controller.Instance.Activate();

    }

}
