using UnityEngine;

public class PlayerAnimOnlyRB : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;
    [Header("Ground check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundRadius = 0.18f;
    [SerializeField] LayerMask groundMask;

    [Header("Tuning")]
    [SerializeField] float maxGroundSpeed = 4f;
    [SerializeField] float speedDamp = 0.1f;

    bool grounded;

    static readonly int SpeedHash = Animator.StringToHash("Speed");
    static readonly int JumpHash = Animator.StringToHash("Jump");
    static readonly int GroundedHash = Animator.StringToHash("Grounded");
    static readonly int FreeFallHash = Animator.StringToHash("FreeFall");

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        if (!animator || !rb) return;

        Vector3 p = groundCheck ? groundCheck.position : rb.position;
        grounded = Physics.CheckSphere(p, groundRadius, groundMask, QueryTriggerInteraction.Ignore);

        animator.SetBool(GroundedHash, grounded);

        float vy = rb.linearVelocity.y;
        animator.SetBool(FreeFallHash, !grounded && vy < -0.5f);

        Vector3 v = rb.linearVelocity;
        v.y = 0f;
        float speed01 = Mathf.Clamp01(v.magnitude / Mathf.Max(0.01f, maxGroundSpeed));
        animator.SetFloat(SpeedHash, speed01, speedDamp, Time.deltaTime);

        if (grounded && Input.GetButtonDown("Jump"))
            animator.SetTrigger(JumpHash);
    }

    private void OnLand()
    {
        animator.ResetTrigger(JumpHash);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 p = groundCheck ? groundCheck.position : transform.position;
        Gizmos.DrawWireSphere(p, groundRadius);
    }
}

