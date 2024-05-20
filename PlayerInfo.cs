using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;
    public Button switchButton;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody rb;

    public float interactDistance = 1;
    bool isGameConsole = false;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffSet = 0.5f;
    public LayerMask groundLayer;    

    [Header("Movement Bools")]
    public bool isFastRunning;
    public bool isGrounded;
    public bool isJumping;    

    [Header("Movement Speed")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 5;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 15;

    [Header("Jump Speeds")]
    public float jumpHeight;
    public float gravityIntensity = -15;


    private void Awake()
    {
        isGrounded = true;
        isGameConsole = false;
        
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        animatorManager.OnAnimationEnd += OnAnimationEnd;
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;

        if (switchButton != null)
        {
            switchButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Switch button is not assigned in the inspector.");
        }
        
        animatorManager.OnAnimationEnd += OnAnimationEnd;
    }

    private void OnDestroy()
    {
        animatorManager.OnAnimationEnd -= OnAnimationEnd; 
    }

    private void OnAnimationEnd(string animationName)
    {
        if (animationName == "electric" && switchButton != null)
        {
            switchButton.gameObject.SetActive(true);
        }
    }

    public void AllMovement()
    {
        FallingAndLanding();

        if (!isGrounded && playerManager.isInteracting)
            return;      

        HandleMovement();
        Rotation();
    }

    private void HandleMovement()
    {
        if (isJumping)
            return;

        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;        

        if (isFastRunning)
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection = moveDirection * runSpeed;
            }
            else
            {
                moveDirection = moveDirection * walkSpeed;
            }
        }

        if (isGameConsole && !IsElectricAnimationPlaying())
        {
            animatorManager.PlayTargetAnimation("electric", true);
            
            isGameConsole = false; 
        }

        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    bool IsElectricAnimationPlaying()
    {
        Animator animator = animatorManager.anim;

        int electricHash = Animator.StringToHash("electric");

        return animator.GetCurrentAnimatorStateInfo(0).shortNameHash == electricHash;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gameconsole") && !isGameConsole)
        {
            isGameConsole = true;
        }
    }

    void OnTriggerExit(Collider other)
    {        
        if (other.CompareTag("gameconsole"))
        {
            isGameConsole = false;
        }
    }

    private void Rotation()
    {
        if (isJumping)
            return;

        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void FallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffSet;
        targetPosition = transform.position;

        if (!isGrounded&&!isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("fall", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out hit, groundLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("land", true);
            }
            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y=rayCastHitPoint.y;
            inAirTimer = 0;
            isGrounded = true;
            playerManager.isInteracting = false;
        }
        else
        {
            isGrounded = false;
        }

        if(isGrounded&& !isJumping)
        {
            if(playerManager.isInteracting||inputManager.moveAmount>0)
            {
                transform.position=Vector3.Lerp(transform.position,targetPosition,Time.deltaTime);
            }
            else
            {
                transform.position=targetPosition;
            }
        }
    }

    public void Jumping()
    {
        if(isGrounded)
        {
            animatorManager.anim.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y=jumpingVelocity;
            rb.velocity=playerVelocity;
        }
    }
}
