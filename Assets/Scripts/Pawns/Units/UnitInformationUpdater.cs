using UnityEngine;
using TMPro;
    public class UnitInformationUpdater : MonoBehaviour
    { 
        public GameObject FloatingPoint;
        public CameraControler cameraa;    
        public string UnitInfo;

    private void Start()
    {
        cameraa = CameraControler.LocalInstance;
    }
    private void FixedUpdate()
    {
        FloatingPoint.transform.LookAt(cameraa.transform.position);

        FloatingPoint.transform.rotation *= Quaternion.Euler(0, 180, 0);

    }

}

