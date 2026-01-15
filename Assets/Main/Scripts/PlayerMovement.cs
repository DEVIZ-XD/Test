using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Rotation")]
    [SerializeField] private float turnSpeed = 900f;

    [Header("Ground check")]
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private float groundCheckHeight = 0.6f;

    [Header("Animator")]
    [SerializeField] private float inputDeadzone = 0.01f;

    private float horizontalMovement;
    private bool jumpPressedThisFrame;

    private static readonly int SpeedID = Animator.StringToHash("Speed");
    private static readonly int GroundedID = Animator.StringToHash("Grounded");
    private static readonly int JumpID = Animator.StringToHash("Jump");
    private static readonly int FreeFallID = Animator.StringToHash("FreeFall");
    private static readonly int MotionSpeedID = Animator.StringToHash("MotionSpeed");

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        bool grounded = IsGrounded();
        animator.SetBool(GroundedID, grounded);
        float inputAbs = Mathf.Abs(horizontalMovement);
        if (inputAbs < inputDeadzone) inputAbs = 0f;
        float animSpeed = inputAbs * moveSpeed;

        animator.SetFloat(SpeedID, animSpeed);

        animator.SetFloat(MotionSpeedID, inputAbs);

        animator.SetBool(JumpID, jumpPressedThisFrame && grounded);

        bool freeFall = !grounded && rb.linearVelocity.y < -0.1f;
        animator.SetBool(FreeFallID, freeFall);

        jumpPressedThisFrame = false;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(horizontalMovement * moveSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
        if (Mathf.Abs(horizontalMovement) > inputDeadzone)
        {
            float targetYaw = horizontalMovement > 0f ? 90f : -90f;
            Quaternion targetRot = Quaternion.Euler(0f, targetYaw, 0f);

            Quaternion newRot = Quaternion.RotateTowards(rb.rotation, targetRot, turnSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRot);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressedThisFrame = true;

            if (IsGrounded())
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        Vector3 p1 = groundCheck.position + Vector3.up * (groundCheckHeight * 0.5f);
        Vector3 p2 = groundCheck.position - Vector3.up * (groundCheckHeight * 0.5f);
        return Physics.CheckCapsule(p1, p2, groundCheckRadius, groundLayer);
    }

public void OnLand()
    {
        //затычка
    }
}

