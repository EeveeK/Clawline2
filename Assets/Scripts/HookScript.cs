using UnityEngine;

public class HookScript : MonoBehaviour, IHookable
{

    [SerializeField] private GameObject ClawLine;
    [SerializeField] private GameObject Hornet;

    private ClawlineScript ClawScript;

    private Rigidbody2D hornetRb;

    private HornetScript HS;

    private bool grabbed;
    public bool canBeHooked()
    {
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null && collision == ClawLine)
        {
            grabbed = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HS = GameObject.Find("Hornet").GetComponent<HornetScript>();
        hornetRb = Hornet.GetComponent<Rigidbody2D>();
        grabbed = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (grabbed)
        {
            hornetRb.simulated = false;
            hornetRb.linearVelocity = Vector2.zero;

        }
    }
}
