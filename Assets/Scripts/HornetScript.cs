using UnityEngine;
using UnityEngine.InputSystem;

public class HornetScript : MonoBehaviour
{
    public Vector2 moveInput = new Vector2(1,0);
    public Vector2 lastMoveInput = new Vector2(1, 0);

    public bool isWalking;
    [SerializeField] private float speed = 8f;
    [SerializeField] private GameObject ClawLine;

    ClawlineScript clawlineScript;

    public float gravity;

    private SpriteRenderer spriteRenderer;

    private Animator animator;

    Vector2 distance;

    public bool clawExists;
    public Vector3 prevPos;

    public bool isFlipped;

    public bool isHooked;

    Rigidbody2D hornetRb;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (hornetRb.linearVelocityY != 0) return;

        if (clawExists)
        {
            Debug.LogWarning("Movement blocked because ClawLine exists!");
            return;
        }
        if (context.performed)
        {
            isHooked = false;
            hornetRb.AddForceY(30f, ForceMode2D.Impulse);
            Debug.Log("Input received: " + "Jump!");
        }
        
    }

    public void OnClawline(InputAction.CallbackContext context)
    {

        if (isHooked) isHooked = false;
            if (context.performed)
        {
            if(clawExists == false)
            {
                Instantiate(ClawLine);
                clawExists = true;
                animator.SetBool("Clawline", true);
                if (isWalking) 
                { animator.SetFloat("ClawDir", moveInput.x); } else { animator.SetFloat("ClawDir", lastMoveInput.x); }

            }
            
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clawExists = false;
       gravity = 9.8f;
        isHooked = false;
        animator = GetComponent<Animator>();
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hornetRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //hornetRb.MovePosition(hornetRb.position + distance);
        if (clawExists)
        {
            Debug.LogWarning("Movement blocked because ClawLine exists!");
            return;
        }
        if (isHooked)
        {
            Debug.LogWarning("Movement blocked because ISHOOKED is TRUE!");
            hornetRb.linearVelocity = Vector2.zero;
            hornetRb.gravityScale = 0f;
            return;
        } else
        {
            hornetRb.gravityScale = gravity;
        }

        hornetRb.linearVelocity = new Vector2(moveInput.x * speed, hornetRb.linearVelocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!clawExists)
        {
            animator.SetBool("Clawline", false);

        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {        animator.SetBool("isWalking", true);
        isWalking = true; ;

        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            lastMoveInput = moveInput;
            isWalking = false;
        }

        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);

    }
}
