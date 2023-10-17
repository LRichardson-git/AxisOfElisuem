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
    Dictionary<KeyCode, int> keyCodeToInt = new Dictionary<KeyCode, int>();
    public static InputManager Instance;
    private KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };
    void Awake()
    {
        Instance = this;
        Hotbar = 0;

        

  
    }
    private void Start()
    {
       
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



        Hotbar = 0;
        //get number down
        for (int i = (int)KeyCode.Alpha0; i < (int)KeyCode.Alpha9; ++i)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                Hotbar = (i - ((int)KeyCode.Alpha0));
            }
            
        }

       
        MousePos = Input.mousePosition;

    }
}
