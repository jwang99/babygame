// Unlike FieldOfView, which i need to fix and is only for seeing the player,
// PlayerFieldOfView represents the FOV *of* the player and helps get objects within view
// This checks for distance from the player and whether or not it is in view of the player,
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFieldOfView : MonoBehaviour
{
    public float fieldOfView = 45f;  // Field of view in degrees
    public float viewDistance = 5f; // Maximum distance to check for targets



    public LayerMask targetMask;
    public LayerMask obstructionMask;

    private GameObject closestTarget;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FieldOfViewCheck()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewDistance, targetMask);
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        closestTarget = null;

        foreach (Collider targetCollider in targetsInViewRadius)
        {
            GameObject target = targetCollider.gameObject;
            Vector3 directionToTarget = target.transform.position - currentPosition;
            float distanceToTarget = directionToTarget.sqrMagnitude;

            if (distanceToTarget < closestDistanceSqr && IsTargetInFieldOfView(target))
            {
                closestDistanceSqr = distanceToTarget;
                closestTarget = target;
            }
        }

        if (closestTarget != null)
        {
            Debug.Log("Closest target: " + closestTarget.name);
        }
            
    }

    bool IsTargetInFieldOfView(GameObject target)
    {
        Vector3 directionToTarget = target.transform.position - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        if (angleToTarget < fieldOfView / 2f)
        {
            return true;
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position to show the view radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }

    public GameObject GetClosestObjectInLayer() {
        FieldOfViewCheck();
        return closestTarget;
    }

}
