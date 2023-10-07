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

    public static InputManager Instance;

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    
        rightClick = Input.GetMouseButtonDown(1);
        leftClick = Input.GetMouseButtonDown(0);
       // leftClickn = Input.GetMouseButtonDown(0);
        leftLetGo = Input.GetMouseButtonUp(0);
        rightLetGo = Input.GetMouseButtonUp(1);

        if (Input.GetKeyDown(KeyCode.Escape) || rightLetGo)
        {
            cancelAction = true;
        }
        else
            cancelAction = false;

        


        MousePos = Input.mousePosition;

    }
}
