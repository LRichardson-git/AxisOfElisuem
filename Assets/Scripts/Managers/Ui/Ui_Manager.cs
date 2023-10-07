using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Ui_Manager : MonoBehaviour
{
    public static Ui_Manager Instance { get; private set; }

    [SerializeField]
    private View[] views;

    private void Awake()
    {
        Instance = this;
    }

    public void init()
    {
        foreach (View view in views)
        {
            view.init(); //initilize the views
        }
    }

    public void show<T>() where T : View //try to pass anything that dosent inherit from view whill give you a error //T is generic class
    {
        foreach (View view in views)
        {
            view.gameObject.SetActive(view is T); //if view is an instance of the type T then enable the view, otherwise it will be disabled
            //eg if only wanna show lobby and pass in lobby it will only show lobby and hide rest of UI
        }
    }
}
