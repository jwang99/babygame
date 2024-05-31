using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentController : MonoBehaviour
{
    public GameObject objectToFollow;
    public float targetMaxDistance = 3f;
    public float targetMinDistance = 1f;
    public enum ParentAction
    {
        IDLE,
        FOLLOW_WALK,
        //FOLLOW_RUN,
        //CLEANING,
        //CARRYING_OBJECT,
    }

    public ParentAction state;
    // Start is called before the first frame update
    void Start()
    {
        state = ParentAction.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        //define state

        // block for follow mode:
        if (state == ParentAction.IDLE)
        {
            if (Vector3.Distance(transform.position, objectToFollow.transform.position) > targetMaxDistance)
            {
                state = ParentAction.FOLLOW_WALK;
            }
        }
        else if (state == ParentAction.FOLLOW_WALK)
        {
            if (Vector3.Distance(transform.position, objectToFollow.transform.position) < targetMinDistance)
            {
                state = ParentAction.IDLE;
            }
        }

    }

}
