using UnityEngine;
using TMPro;
    public class UnitInformationUpdater : MonoBehaviour
    { 
        public TextMeshPro UnitInfoText;
        public GameObject FloatingPoint;
        public CameraControler cameraa;    
        public string UnitInfo;
        public void OnInfoChanged(int HP)
        {
            UnitInfo = "HP: " + HP;
            UnitInfoText.text = UnitInfo;
        }

    private void Start()
    {
        cameraa = CameraControler.LocalInstance;
    }
    private void FixedUpdate()
    {
          UnitInfoText.transform.LookAt(cameraa.transform.position);

        UnitInfoText.transform.rotation *= Quaternion.Euler(0, 180, 0);

    }

}

