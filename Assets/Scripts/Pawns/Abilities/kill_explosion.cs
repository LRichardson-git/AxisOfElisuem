using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kill_explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("die", 3.9f);
    }

    //disabling loop didnt work and dont wanna spend time on this
    void die()
    {
        Destroy(this.gameObject);
    }
}
