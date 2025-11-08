using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerScript2 : MonoBehaviour
{
    private CharacterController mController;
    private Animator mAnimator;
    public Vector3 mVelocity;
    public float WalkSpeed = 2f;
    public float RunSpeed = 4f;
    public float SprintSpeed = 6f;
    private bool mWasGrounded = false;

    private Transform mCharacterModel;

    public Vector3 respawnPosition; // jy - respawn location. 
    public float fallThreshold = -20f; //jy - respawn fall threshold 
    [SerializeField] TextMeshProUGUI numCoinsText, numLivesText; //TextMeshPro objects for UI
    private int numCoins, numLives; //Int values for coins and lives

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

        mCharacterModel = transform.Find("HumanM_Model/Rig"); // have to rotate rotate "rig" not the mesh to work. 

        // Get Input Actions
        mMoveAction = InputSystem.actions.FindAction("Move");
        mWalkAction = InputSystem.actions.FindAction("Walk");
        mSprintAction = InputSystem.actions.FindAction("Sprint");
        mJumpAction = InputSystem.actions.FindAction("Jump");

        //Initialise the coins and lives values
        numCoins = 0;
        numLives = 5;
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
        //Decrement the lives value
        numLives--;
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
        
        //Update the coin and lives vaules
        numCoinsText.text = numCoins.ToString();
        numLivesText.text = numLives.ToString();
    }

    void LateUpdate()
    {
        mCharacterModel.localRotation = Quaternion.Euler(0, mTargetRotationY, 0);
    }

    //Added the below method to collect the coins
    void OnTriggerEnter(Collider other)
    {
        //If the player is the collision
        if (other.CompareTag("Coin"))
        {
            //Hide the coin
            other.gameObject.SetActive(false);
            //Update the player's coin count
            numCoins++;
        }
    }
}