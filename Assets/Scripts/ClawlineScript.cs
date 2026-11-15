using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.EventSystems;

public class ClawlineScript : MonoBehaviour
{

    private GameObject Hornet;    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float hornetSpeed = 10.0f;
    HornetScript hornetScript;
    private Rigidbody2D clawlineRb;

    private Rigidbody2D hornetRb;

    private SpriteRenderer childSprite;

    bool bouncyThingHit = false;



    float maxDistance = 4f;

    bool hitWall = false;
    bool hitHook;

    Vector2 MoveDir;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckWallCollision(collision);
        CheckHookCollision(collision, true);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckWallCollision(collision);
        
    }

    private void CheckHookCollision(Collider2D collision, bool onHook)
    {
        if (collision.TryGetComponent<IHookable>(out var wasHit))
        {
            if (onHook) hornetScript.isHooked = true;
          
        }
       
    }

    

    private void CheckWallCollision(Collider2D collision)
    {


        if (collision.TryGetComponent<IHittable>(out var wasHit))
        {
            hitWall = true;
        }

        if (collision.TryGetComponent<IHookable>(out var thing))
        {
            hitHook = true;
        }

        if (collision.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(5f);


        }

        if (collision.TryGetComponent<IBouncable>(out var bouncy))
        {
            bouncyThingHit = true;


        }
    }


    private IEnumerator ClawlineTravel()
    {
        hornetRb.linearVelocity = Vector2.zero;
        hornetRb.gravityScale = 0f;



        Vector3 startingClawLinePos = transform.position;
        maxDistance = 4f;

        
            while (Vector3.Distance(startingClawLinePos, transform.position) < maxDistance)
        { 
                if (!hitWall && !hitHook)
                {
                    moveClawline();
                }
                else { break; }

            yield return new WaitForFixedUpdate();
            }

            Vector3 startingHornetPos = Hornet.transform.position;
        float currTime = 0f;
        float timeOut = 1f;

        if (hitWall)
        {
            while (Vector2.Distance(hornetRb.position, transform.position) > 0.2f && currTime < timeOut)
            {
                moveHornet();
                currTime += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }
        } else
        {
            while (Vector2.Distance(hornetRb.position, (Vector2)transform.position) > 0.2f && currTime < timeOut)
                        {
                            moveHornet();
                        currTime += Time.fixedDeltaTime;

                        yield return new WaitForFixedUpdate();
                        }
            if (hitHook)hornetScript.isHooked = true;
        }

        if (bouncyThingHit)
        {
            hornetRb.AddForceY(30f, ForceMode2D.Impulse);
        }

        hornetScript.clawExists = false;

      




        Destroy(gameObject);


    }

    void moveClawline()
    {
        
           transform.Translate(MoveDir * Time.fixedDeltaTime * speed);
       
        
    }

    void moveHornet()
    {
        hornetRb.gravityScale = 0;
        Vector2 displacement = (MoveDir * Time.fixedDeltaTime * hornetSpeed);
        hornetRb.MovePosition(hornetRb.position + displacement);
        
    }
    void Awake()
    {
        clawlineRb = GetComponent<Rigidbody2D>();
        childSprite = GetComponentInChildren<SpriteRenderer>();


    }

    void Start()
    {

        hitHook = false;
        hitWall = false;
        hornetScript = FindAnyObjectByType<HornetScript>();
        Hornet = hornetScript.gameObject;
        hornetRb = Hornet.GetComponent<Rigidbody2D>(); 
        transform.position = hornetRb.position;
            StartCoroutine(ClawlineTravel());

        if (hornetScript.isWalking)
        {
            MoveDir = hornetScript.moveInput;
        } else
        {
            MoveDir = hornetScript.lastMoveInput;
        }

        if (MoveDir.x == 1)
        {
            childSprite.flipX = true;
        }

        if (MoveDir.sqrMagnitude < 0.01f)
        {
            MoveDir = Vector2.left;
        }





    }

    private void FixedUpdate()
    {
            hornetRb.linearVelocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        


    }


    
}
