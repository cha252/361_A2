using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private CharacterController mController;
    private Animator mAnimator;
    public Vector3 mVelocity;
    public float WalkSpeed   = 2f;
    public float RunSpeed    = 4f;
    public float SprintSpeed = 6f;

    private Transform mCameraPivot;
    private bool mWasGrounded = false;

    // Input Actions
    private InputAction mMoveAction, mWalkAction, mSprintAction, mJumpAction;

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        mController = GetComponent<CharacterController>();
        mAnimator = GetComponent<Animator>();

        // Lock/Hide the Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set Velocity to 0
        mVelocity = Vector3.zero;

        // Get Camera Pivotc
        mCameraPivot = transform.Find("CameraPivot");

        // Get Input Actions
        mMoveAction = InputSystem.actions.FindAction("Move");
        mWalkAction = InputSystem.actions.FindAction("Walk");
        mSprintAction = InputSystem.actions.FindAction("Sprint");
        mJumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void Jump() {
        // Set Velocity Up
        mVelocity.y = 8.0f;
        mAnimator.SetTrigger("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        // Get Input
        Vector2 move = mMoveAction.ReadValue<Vector2>();

        // Player Speed
        float targetSpeed = 0.0f;
        if (move.y > 0.0f)
        {
            if (mWalkAction.IsPressed())
            {
                // Player Walking
                targetSpeed = WalkSpeed;
            }
            else if (mSprintAction.IsPressed())
            {
                // Player Sprinting
                targetSpeed = SprintSpeed;
            }
            else
            {
                // Player Running
                targetSpeed = RunSpeed;
            }
        }
        // Update Velocity
        mVelocity.z = Mathf.MoveTowards(mVelocity.z, targetSpeed, 2.0f * Time.deltaTime);

        // Apply Gravity
        if (mController.isGrounded == false)
        {
            mVelocity.y -= 9.8f * Time.deltaTime;
        }
        else
        {
            mVelocity.y = -1.0f;
        }

        // Landing
        if (mWasGrounded == false && mController.isGrounded)
        {
            mVelocity.z = 0.0f;
        }

        // Update Animator Variables
        mAnimator.SetFloat("Speed", mVelocity.z);
        mAnimator.SetBool("isGrounded", mController.isGrounded);

        // Jumping
        if (mJumpAction.WasPerformedThisFrame())
        {
            Jump();
        }

        // Update whether Character was grounded last frame
        // (must be before Controller Move)
        mWasGrounded = mController.isGrounded;

        // Move Character
        mController.Move(mVelocity * Time.deltaTime);
    }
}
