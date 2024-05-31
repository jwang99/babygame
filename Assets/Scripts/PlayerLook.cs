using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    public Camera cam;
    private float xRot = 0f;

    public float xSens = 30f;
    public float ySens = 30f;
    // Start is called before the first frame update

    public void ProcessLook(Vector2 input)
    {
        float mousedX = input.x;
        float mousedY = input.y;
        // claculate camera rot
        xRot -= (mousedY * Time.deltaTime) * ySens;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        // rotate camera up and down, and player left and right (left/right affects walking direction, and we dont want player model
        // // to rotate up down)
        cam.transform.localRotation = Quaternion.Euler(xRot, 0,  0);
        // rotate player. Rotating positive in up direction is to the right bc of LHR kms
        transform.Rotate(Vector3.up * mousedX * Time.deltaTime * xSens);
    }
}
