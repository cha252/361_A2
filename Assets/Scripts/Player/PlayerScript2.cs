using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

// 08/11/25 c - resolved 
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

    public Vector3 respawnPosition; 
    public float fallThreshold = -20f; //jy - de fualt respawn fall threshold 

    [SerializeField] TextMeshProUGUI numCoinsText, numLivesText; //TextMeshPro objects for UI
    public int level;

    
    private Vector3 externalForce = Vector3.zero; // jy - knockback related  for bomb obstacle

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
    }

    private void Jump()
    {
        // Set Velocity Up
        mVelocity.y = 7.0f;
        mAnimator.SetTrigger("Jump");
    }

    private void Respawn()
    {
        //If not in level 0
        if (level != 0)
        {
            //Decrement the lives value
            GameManagerScript.instance.RemoveLife();
        }
        
        //If the player still has lives
        if (GameManagerScript.instance.numLives > 0)
        {
            mController.enabled = false;
            transform.position = respawnPosition;
            mController.enabled = true;
            mVelocity = Vector3.zero;
            transform.parent = null;

            externalForce = Vector3.zero; // jy. reset knockback force on respawn
        }
        //If the player has run out of lives
        else
        {
            //Reset the player's number of coins and lives
            GameManagerScript.instance.Reset();
            //Load level 0
            SceneManager.LoadScene("Level0_Loader", LoadSceneMode.Single);
        }
    }


    public void GetKnockedBack(Vector3 direction, float force)
    {
        
        direction.y = 0.7f; // jy - added slight upward force for more dynamic effect
        direction = direction.normalized * force;
        externalForce = direction;
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
                targetSpeed = WalkSpeed; // a, d
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

        Vector3 finalVelocity = mVelocity + externalForce;  // jy - apply external force (knockback from bomb) to final velocity
        externalForce = Vector3.Lerp(externalForce, Vector3.zero, 2f * Time.deltaTime);  // jy - gradually decrease knockback force over time

        // Move Character
        mController.Move(finalVelocity * Time.deltaTime);

        //Update the coin and lives vaules
        numCoinsText.text = GameManagerScript.instance.numCoins.ToString();
        numLivesText.text = GameManagerScript.instance.numLives.ToString();
    }

    void LateUpdate()
    {
        mCharacterModel.localRotation = Quaternion.Euler(0, mTargetRotationY, 0); // ** rotating 'rig'. 
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
            GameManagerScript.instance.AddCoin();
        }
        else if (other.CompareTag("Laser"))
        {
            Respawn();
        }
    }
}