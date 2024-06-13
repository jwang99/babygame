using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    public Camera cam;
    public GameObject player;
    private Vector3 offset;
    private float xRot = 0f;
    private float yRot = 0f;

    private float yOffset = 0.1f;
    private float zOffset = 0.1f;

    public float xSens = 30f;
    public float ySens = 30f;
    // Start is called before the first frame update
    private void Start()
    {
        Vector3 playerPosition = player.transform.position;
        offset = new Vector3(playerPosition.x, playerPosition.y + yOffset, playerPosition.z + zOffset);
        cam.transform.position = player.transform.position + offset;
    }

    public void ProcessLook(Vector2 input)
    {
        float mousedX = input.x;
        float mousedY = input.y;
        // claculate camera rot
        xRot -= (mousedY * Time.deltaTime) * ySens;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        yRot += (mousedX * Time.deltaTime) * xSens;

        offset = Quaternion.AngleAxis((mousedX * Time.deltaTime) * xSens, Vector3.up) * offset;
        // rotate camera up and down, and player left and right (left/right affects walking direction, and we dont want player model
        // // to rotate up down)
        cam.transform.position = player.transform.position + offset;
        cam.transform.LookAt(player.transform.position);
        //cam.transform.localRotation = Quaternion.Euler(xRot, 0,  0);

    }
}
