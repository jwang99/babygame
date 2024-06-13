using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    public GameObject avatar;
    private Animator animator;
 
    public enum CharacterAnimationState
    {
        IDLE,
        WALK,
        RUN,
        CARRY_RUN,
        CRAWL,
        CLIMB,
    }

    public CharacterAnimationState animationState;
    // Start is called before the first frame update
    void Start()
    {
        // animation state controller should be in main object. If avatar is
        // a separate child object, it can be set as such and animation can be in avatar
        if (avatar == null)
        {
            avatar = gameObject;
        }
        animator = avatar.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (animatorHasParameter("isWalking"))
        {
            animator.SetBool("isWalking", animationState == CharacterAnimationState.WALK);
        }
        if (animatorHasParameter("isRunning"))
        {
            animator.SetBool("isRunning", animationState == CharacterAnimationState.RUN);
        }
        if (animatorHasParameter("isCrawling"))
        {
            animator.SetBool("isCrawling", animationState == CharacterAnimationState.CRAWL);
        }
        if (animatorHasParameter("isCarryRunning"))
        {
            animator.SetBool("isCarryRunning", animationState == CharacterAnimationState.CARRY_RUN);
        }
    }

    public void TriggerClimb()
    {
        Debug.Log("trigger climb");
        animator.SetTrigger("climbTrigger");
    }

    private bool animatorHasParameter(string name) 
    {
        for (int i = 0; i < animator.parameters.Length; i++)
        {
            if (animator.parameters[i].name == name)
            {
                return true;
            }
        }
        return false;
    }
}
