using Mirror;
using System;
using UnityEngine;
using TMPro;
using Mirror.Authenticators;

    public class UnitInformationUpdater : NetworkBehaviour
    {
        public TextMeshPro UnitInfoText;
        public GameObject FloatingPoint;

        [SyncVar(hook = nameof(OnInfoChanged))]
        public string UnitInfo;

        private Unit _unit;

        void OnInfoChanged(string _Old, string _New)
        {
            UnitInfoText.text = UnitInfo;
        }

        private void Awake()
        {
            _unit = GetComponent<Unit>();
            UnitInfo = "HP: " + _unit.HP;

            if (!isOwned)
            {
                // Assign authority to the client
                GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.G)) 
                ChangeUnitHP(_unit.HP - 1);
        }


        [Server]
        public void ChangeUnitHP(int HP)
        {
            UnitInfo = "HP: " + HP;
        }
        
    }

