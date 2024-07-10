using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ParentMotor : CharacterMotor
{
    public GameObject objectToFollow;
    private PlayerMotor playerMotor;
    public float targetMaxDistance = 3f;
    public float targetMinDistance = 1f;

    public NavMeshAgent agent;
    private FieldOfView fov;

    public float defaultSpeed = 3f;
    public float runningSpeed = 5f;


    // retrieval variables
    public Vector3 safeLocation;
    public Vector3 defaultLocation;
    public Transform defaultTransform;
    public enum ParentAction
    {
        IDLE,
        FOLLOW_WALK,
        FOLLOW_RUN,
        SAFE_ZONE,
        RETURN_TO_DEFAULT,
        //CLEANING,
        //CARRYING_OBJECT,
    }

    public int level;

    public ParentAction actionState;

    // Start is called before the first frame update
    void Start()
    {
        playerMotor = objectToFollow.GetComponent<PlayerMotor>();
        safeLocation = objectToFollow.transform.position;
        defaultLocation = transform.position;
        defaultTransform = transform;
        base.animationStateController = GetComponent<AnimationStateController>();
        actionState = ParentAction.IDLE;
        setAnimationState();
        agent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView>();
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
        // set action states

        // if parent can see player, player is not where they are supposed to be,
        // and parent is not actively retrieving player, have parent retrieve player

        if (fov.canSeePlayer && targetIsOutOfZone() && !(actionState == ParentAction.FOLLOW_RUN || actionState == ParentAction.SAFE_ZONE) ) // start following player to bring back
        {
            actionState = ParentAction.FOLLOW_RUN;   
          
        }
        if (actionState == ParentAction.FOLLOW_RUN) // start bringing back player
        {
            if (Vector3.Distance(transform.position, objectToFollow.transform.position) < 1f)
            {
                // TODO: trigger player to be picked up by parent
                Debug.Log("safe zone mode");
                actionState = ParentAction.SAFE_ZONE;
                playerMotor.GetCarriedBy(gameObject);

            }
        }
        if (actionState == ParentAction.SAFE_ZONE) {
            if (Vector3.Distance(transform.position, safeLocation) < 2) {
                actionState = ParentAction.RETURN_TO_DEFAULT;
                // TODO: move this elsewhere
                PlayerUI playerUI = objectToFollow.GetComponent<PlayerUI>();
                playerUI.UpdatePromptText("[ctrl] to crawl");
                playerMotor.GetPutDownBy(gameObject);
            } 
        }
        if (actionState == ParentAction.RETURN_TO_DEFAULT) {
            Debug.Log("parent return to default");
            if (Vector3.Distance(transform.position, defaultLocation) < 1) {
                actionState = ParentAction.IDLE;
                transform.position = defaultTransform.position;
                transform.rotation = defaultTransform.rotation;
            }
        }
        setAnimationState();

        // perform actions

        if (actionState == ParentAction.FOLLOW_RUN)
        {
            agent.SetDestination(objectToFollow.transform.position);
            agent.speed = runningSpeed;
        } else if (actionState == ParentAction.SAFE_ZONE)
        {
            agent.speed = defaultSpeed;
            agent.SetDestination(safeLocation);
        } else if (actionState == ParentAction.RETURN_TO_DEFAULT) {
            agent.SetDestination(defaultLocation);
        }


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
        if (actionState == ParentAction.FOLLOW_WALK || actionState == ParentAction.RETURN_TO_DEFAULT)
        {
            base.animationStateController.animationState = AnimationStateController.CharacterAnimationState.WALK;
        }
        else if (actionState == ParentAction.FOLLOW_RUN)
        {
            // Debug.Log("follow run");
            base.animationStateController.animationState = AnimationStateController.CharacterAnimationState.RUN;
        }
        else if (actionState == ParentAction.SAFE_ZONE)
        {
            base.animationStateController.animationState = AnimationStateController.CharacterAnimationState.CARRY_RUN;
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

    bool targetIsOutOfZone()
    {
        if (Vector3.Distance(objectToFollow.transform.position, safeLocation) > 1)
        {
            return true;
        }
        return false;
    }

}
