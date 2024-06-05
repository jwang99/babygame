using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : CharacterMotor
{
    private CharacterController controller;
    public Camera cam;
    private Vector3 playerVelocity;
    public float baseSpeed = 3f;

    private bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 1.5f;
    public float crawlHeightOffset = 1f;
    private bool isSprinting;
    private bool isCrawling;

    private enum PlayerActionState
    {
        WALK,
        SPRINT,
        CRAWL,

    }

    // Start is called before the first frame update
    void Start()
    {
        base.animationStateController = GetComponent<AnimationStateController>();
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
        // Physics outside of player's control
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -0.2f; //negative to trigger grounded?
        }
        controller.Move(playerVelocity * Time.deltaTime);

        // after this, everything is only executed if there is input
        if (input == Vector2.zero)
        {
            base.animationStateController.animationState = AnimationStateController.CharacterAnimationState.IDLE;
            return;
        }

        float speed = baseSpeed;
        base.animationStateController.animationState = AnimationStateController.CharacterAnimationState.WALK;
        if (isSprinting)
        {
            Debug.Log("sprinting");
            base.animationStateController.animationState = AnimationStateController.CharacterAnimationState.RUN;
            speed *= 1.5f;
        }
        if (isCrawling)
        {
            Debug.Log("crawling");
            base.animationStateController.animationState = AnimationStateController.CharacterAnimationState.CRAWL;
            speed *= 0.6f;
        }
        Vector3 moveDirection = Vector3.zero;

        Vector3 inputDirection = new Vector3(input.x, 0, input.y);

        // TODO: figure out what this does lol
        Vector3 cameraDirection = cam.transform.forward;
        cameraDirection = Vector3.ProjectOnPlane(cameraDirection, Vector3.up);
        Quaternion rotation = Quaternion.LookRotation(cameraDirection);

        moveDirection = rotation * inputDirection;


        controller.Move(moveDirection * speed * Time.deltaTime);


        transform.LookAt(transform.position + moveDirection);

    }

    // TODO: repurposing this for climbing, clean this up later
    public void Jump()
    {
        Debug.Log("jumping");
        if (isGrounded)
        {

            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        base.animationStateController.TriggerClimb();
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
