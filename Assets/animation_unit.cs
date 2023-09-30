using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_unit : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        animator.SetFloat("Speed", 5);
    }
}
