using UnityEngine;
using UnityEngine.InputSystem;

public class HornetScript : MonoBehaviour
{
    public Vector2 moveInput;
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject ClawLine;

    ClawlineScript clawlineScript;

    public float gravity;

    private SpriteRenderer spriteRenderer;

    public Vector2 direction;

    Vector2 distance;

    public bool clawExists;

    float moveDirection;
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

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log("Input received: " + moveInput);
    }

    public void OnClawline(InputAction.CallbackContext context)
    {

        if (isHooked) isHooked = false;
            if (context.performed)
        {
            Instantiate(ClawLine);
            clawExists = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clawExists = false;
       gravity = 9.8f;
        isHooked = false;
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

        hornetRb.linearVelocity = new Vector2 (moveDirection * speed, hornetRb.linearVelocity.y);

    }

    // Update is called once per frame
    void Update()
    {

       


        moveDirection = moveInput.x;
        //distance = moveDirection * speed * Time.fixedDeltaTime;

    }
}
