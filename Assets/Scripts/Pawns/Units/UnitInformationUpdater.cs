using UnityEngine;
using TMPro;
    public class UnitInformationUpdater : MonoBehaviour
    { 
        public TextMeshPro UnitInfoText;
        public GameObject FloatingPoint;

        public string UnitInfo;
        public void OnInfoChanged(int HP)
        {
            UnitInfo = "HP: " + HP;
            UnitInfoText.text = UnitInfo;
        }
        
    }

