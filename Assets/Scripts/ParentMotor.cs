using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ParentMotor : CharacterMotor
{
    public GameObject objectToFollow;
    public float targetMaxDistance = 3f;
    public float targetMinDistance = 1f;

    public NavMeshAgent agent;
    public enum ParentAction
    {
        IDLE,
        FOLLOW_WALK,
        FOLLOW_RUN,
        //CLEANING,
        //CARRYING_OBJECT,
    }

    public int level;

    public ParentAction actionState;
    // Start is called before the first frame update
    void Start()
    {
        base.animationStateController = GetComponent<AnimationStateController>();
        actionState = ParentAction.IDLE;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //define state
        if (level == 0)
        {
            TutorialActions();
        }
        else if (level == 1)
        {
            Level1Actions();
        }
        else if (level == 2)
        {

        }
        else if (level == 3)
        {

        }
        

    }

    void TutorialActions()
    {

    }

    void Level1Actions()
    {
        // block for follow mode:
        if (actionState == ParentAction.IDLE)
        {

            if (Vector3.Distance(transform.position, objectToFollow.transform.position) > targetMaxDistance)
            {
                actionState = ParentAction.FOLLOW_WALK;

            }
        }
        else if (actionState == ParentAction.FOLLOW_WALK)
        {
            if (Vector3.Distance(transform.position, objectToFollow.transform.position) < targetMinDistance)
            {
                actionState = ParentAction.IDLE;
            }
        }

        setAnimationState();
        // block for actions
        if (actionState == ParentAction.FOLLOW_WALK)
        {
            agent.SetDestination(objectToFollow.transform.position);
        }
        else if (actionState == ParentAction.IDLE)
        {
            agent.SetDestination(transform.position); // set target to self to stop walking
        }
    }

    void setAnimationState()
    {
        if (actionState == ParentAction.FOLLOW_WALK)
        {
            base.animationStateController.animationState = AnimationStateController.CharacterAnimationState.WALK;
        }
        else if (actionState == ParentAction.IDLE) // needed?
        {
            base.animationStateController.animationState = AnimationStateController.CharacterAnimationState.IDLE;
        }
        else // default
        {
            base.animationStateController.animationState = AnimationStateController.CharacterAnimationState.IDLE;

        }
    }

}
