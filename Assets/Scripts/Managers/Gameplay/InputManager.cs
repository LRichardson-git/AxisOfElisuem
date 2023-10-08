using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public Vector3 MousePos;
    public bool rightClick;
    public bool leftClick;
    public bool leftClickn;
    public bool leftLetGo;
    public bool rightLetGo;
    public bool cancelAction;
    public bool SwitchTarget;
    public int Hotbar;
    private KeyCode[] keyCodes;
    public static InputManager Instance;

    void Awake()
    {
        Instance = this;
        Hotbar = 0;

        KeyCode[] keyCodes = {
            KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5

	};
    }

    private void Update()
    {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    
        rightClick = Input.GetMouseButtonDown(1);
        leftClick = Input.GetMouseButtonDown(0);
        leftLetGo = Input.GetMouseButtonUp(0);
        rightLetGo = Input.GetMouseButtonUp(1);

        if (Input.GetKeyDown(KeyCode.Escape) || rightLetGo)
        {
            cancelAction = true;
        }
        else
            cancelAction = false;

        //get number down
        for (int i = 49; i < 54; i++)
        {
           // Debug.Log(i);
            if (Input.GetKeyUp((KeyCode)i))
            {
                Hotbar = i;
            }
            else
                Hotbar = 0;
        }


       MousePos = Input.mousePosition;

    }
}
