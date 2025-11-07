using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript2 : MonoBehaviour
{
    private CharacterController mController;
    private Animator mAnimator;
    public Vector3 mVelocity;
    public float WalkSpeed = 2f;
    public float RunSpeed = 4f;
    public float SprintSpeed = 6f;
    private Transform mCameraPivot;
    private bool mWasGrounded = false;

    private Transform mCharacterModel;

    public Vector3 respawnPosition = new Vector3(0.34f, 1.35f, -63f); // jy - respawn location. 
    public float fallThreshold = -20f; //jy - respawn fall threshold 

    // Input Actions
    private InputAction mMoveAction, mWalkAction, mSprintAction, mJumpAction;

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

        // Get Camera Pivot
        mCameraPivot = transform.Find("CameraPivot");
        mCharacterModel = transform.Find("HumanM_Model/Rig"); // have to rotate rotate "rig" not the mesh to work. 

        // Get Input Actions
        mMoveAction = InputSystem.actions.FindAction("Move");
        mWalkAction = InputSystem.actions.FindAction("Walk");
        mSprintAction = InputSystem.actions.FindAction("Sprint");
        mJumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void Jump()
    {
        // Set Velocity Up
        mVelocity.y = 7.0f;
        mAnimator.SetTrigger("Jump");
    }
    private void Respawn()
    {
        mController.enabled = false; 
        transform.position = respawnPosition;
        mController.enabled = true;
        mVelocity = Vector3.zero;
    }

    private float mTargetRotationY = 0f;

    void Update()
    {
        // Get Input
        Vector2 move = mMoveAction.ReadValue<Vector2>();

        // Player Speed
        float targetSpeed = 0.0f;
        float moveDirection = 0.0f;

        if (move.x != 0.0f)
        {
            moveDirection = Mathf.Sign(move.x);

            if (moveDirection > 0)
            {
                mTargetRotationY = 0f;
            }
            else
            {
                mTargetRotationY = 180f;
            }

            if (mWalkAction.IsPressed())
            {
                targetSpeed = WalkSpeed;
            }
            else if (mSprintAction.IsPressed())
            {
                targetSpeed = SprintSpeed;
            }
            else
            {
                targetSpeed = RunSpeed;
            }
        }

        // Update Velocity
        mVelocity.z = Mathf.MoveTowards(mVelocity.z, targetSpeed * moveDirection, 20.0f * Time.deltaTime);

        // Apply Gravity
        if (mController.isGrounded == false)
        {
            mVelocity.y -= 7.8f * Time.deltaTime;
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
        mAnimator.SetFloat("Speed", Mathf.Abs(mVelocity.z));
        mAnimator.SetBool("isGrounded", mController.isGrounded);

        if (mJumpAction.WasPerformedThisFrame() && mController.isGrounded)
        {
            Jump();
        }

        
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }

        // Update whether Character was grounded last frame
        mWasGrounded = mController.isGrounded;

        // Move Character
        mController.Move(mVelocity * Time.deltaTime);
    }

    void LateUpdate()
    {
            mCharacterModel.localRotation = Quaternion.Euler(0, mTargetRotationY, 0);
    }
}