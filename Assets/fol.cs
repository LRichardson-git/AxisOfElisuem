using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fol : MonoBehaviour
{
    float speed = 300f; // Speed of the object
    public bool init = false;
    Vector3 target;
    public void initil(Vector3 target)
    {
        this.target = target;
        init = true;
        transform.LookAt(target);

    }
    void FixedUpdate()
    {
        if (!init)
            return;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(target, transform.position) < 10)
           Destroy(this.gameObject);
    }
}
