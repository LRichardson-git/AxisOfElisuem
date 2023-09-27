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

    public static InputManager Instance;

    private void Start()
    {
        Instance = this;
    }
    private void Update()
    {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        rightClick = Input.GetMouseButtonDown(1);
        leftClick = Input.GetMouseButtonUp(0);
        leftClickn = Input.GetMouseButtonDown(0);
        leftLetGo = Input.GetMouseButtonUp(0);
        rightLetGo = Input.GetMouseButtonUp(1);

        MousePos = Input.mousePosition;

    }
}
