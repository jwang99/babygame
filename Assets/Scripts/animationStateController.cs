using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    ParentController controller;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<ParentController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.state == ParentController.ParentAction.FOLLOW_WALK)
        {
            animator.SetBool("isWalking", true);
        }
    }
}
