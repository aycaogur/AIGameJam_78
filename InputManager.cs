using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerInfo playerInfo;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool shiftInput;
    public bool jumpinput;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerInfo = GetComponent<PlayerInfo>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.B.performed += i => shiftInput = true;
            playerControls.PlayerActions.B.canceled += i => shiftInput = false;
            playerControls.PlayerActions.Jump.performed += i => jumpinput = true;
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInput()
    {
        MovementInput();
        RunningInput();
        JumpingInput();
    }

    private void MovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerInfo.isFastRunning);
    }

    private void RunningInput()
    {
        if (shiftInput && moveAmount > 0.5f)
        {
            playerInfo.isFastRunning = true;
        }
        else
        {
            playerInfo.isFastRunning = false;
        }
    }

    private void JumpingInput()
    {
        if(jumpinput)
        {
            jumpinput = false;
            playerInfo.Jumping();
        }
    }
}


