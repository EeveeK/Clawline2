using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ClawlineScript : MonoBehaviour
{

    private GameObject Hornet;    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float hornetSpeed = 10.0f;
    HornetScript hornetScript;
    private Rigidbody2D clawlineRb;

    private Rigidbody2D hornetRb;



    float maxDistance = 4f;

    bool hitWall = false;
    bool hitHook;

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
        float timeOut = 2f;

        if (hitWall)
        {
            while (Vector2.Distance(hornetRb.position, transform.position) > 0.6f && currTime < timeOut)
            {
                moveHornet();
                currTime += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }
        } else
        {
            Vector2 overtakeTarget = (Vector2)transform.position + (Vector2.left * 1.5f);
            while (Vector2.Distance(hornetRb.position, overtakeTarget) > 0.2f && currTime < timeOut)
                        {
                            moveHornet();
                        currTime += Time.fixedDeltaTime;

                        yield return new WaitForFixedUpdate();
                        }
            if (hitHook)hornetScript.isHooked = true;
        }

        hornetScript.clawExists = false;

        

        Destroy(gameObject);


    }

    void moveClawline()
    {
        
           transform.Translate(Vector3.left * Time.fixedDeltaTime * speed);
       
        
    }

    void moveHornet()
    {
        hornetRb.gravityScale = 0;

        //Hornet.transform.Translate(Vector3.left * Time.fixedDeltaTime * hornetSpeed);
        Vector2 displacement = (Vector3.left * Time.fixedDeltaTime * hornetSpeed);
        hornetRb.MovePosition(hornetRb.position + displacement);
        
    }
    void Awake()
    {
        clawlineRb = GetComponent<Rigidbody2D>();

        
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
