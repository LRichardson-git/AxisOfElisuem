using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeTarget_Instance : MonoBehaviour
{
    public static GrenadeTarget_Instance Instance;

    private void Start()
    {
        Instance = this;
    }
}
