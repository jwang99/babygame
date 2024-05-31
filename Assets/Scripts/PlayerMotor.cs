using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float baseSpeed = 3f;

    private bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 1.5f;
    public float crawlHeightOffset = 1f;
    private bool isSprinting;
    private bool isCrawling;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        isSprinting = false;
        isCrawling = false;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        
    }
    // receive inputs for input manager and apply to character controller
    public void ProcessMove(Vector2 input)
    {
        float speed = baseSpeed;
        if (isSprinting)
        {
            Debug.Log("sprinting");
            speed *= 1.5f;
        }
        if (isCrawling)
        {
            Debug.Log("crawling");
            speed *= 0.6f;
        }
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y; // kms
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -0.2f; //negative to trigger grounded?
        }  
        controller.Move(playerVelocity * Time.deltaTime);
        //Debug.Log(playerVelocity.y);

    }

    public void Jump()
    {
        //Debug.Log("jumping");
        if (isGrounded)
        {
            
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
    }

    public void CrawlStart()
    {
        // crawling animation, rendering

        isCrawling = true;
    }

    public void CrawlCancel()
    {
        // toggle crawl
        isCrawling = false;
    }

    public void ToggleSprint()
    {
        isSprinting = !isSprinting;
    }
}
