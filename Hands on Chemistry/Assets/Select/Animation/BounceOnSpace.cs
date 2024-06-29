using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOnSpace : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("Animator attached: " + (animator != null));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("BounceTrigger");
            Debug.Log("Space key pressed: BounceTrigger set");
        }
    }
}


