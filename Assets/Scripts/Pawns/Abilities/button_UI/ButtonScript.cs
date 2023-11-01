using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ButtonScript : MonoBehaviour
{
    public TargetData data;
    [SerializeField]
    private TextMeshProUGUI ChanceToHit;
    private Image image;
    public void init(TargetData data)
    {
        this.data = data;
        ChanceToHit.text = data.getHit() + "%";
        image = GetComponent<Image>();
    }

    public void openShootScreen()
    {
        
        Shooting_View_Controller.Instance.UpdateInfo(data);
        Shooting_View_Controller.Instance.Activate();

    }

    public void highlightUnit()
    {
        CameraControler.LocalInstance.SetCameraUnit(data.getUnit().transform.position, 80);
        transform.localScale = new Vector3(1.2f, 1.2f, 1);
    }

    public void goBAck()
    {
        if(!Shooting_View_Controller.Instance.On())
            CameraControler.LocalInstance.SetCameraUnit(ObjectSelector.Instance.getSelectedUnit().transform.position, 250);

        StopCoroutine("CameraMoveSmooth");
        transform.localScale = new Vector3(1, 1, 1);
    }

}
