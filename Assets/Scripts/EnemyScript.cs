using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour, IDamageable,IHittable, IDealDamage, IBouncable
{
    [SerializeField] float maxHealth = 10f;
    [SerializeField] float currentHealth;

    bool invulnerable = false;

    public void TakeDamage(float amount)
    {
        if (invulnerable)
        {
            return;
        }

        currentHealth -= amount;
        Debug.Log("Took Damage!");
        StartCoroutine(InvincibilityRoutine());

        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        } 
        
    }

    private IEnumerator InvincibilityRoutine()
    {
        invulnerable = true;
        yield return new WaitForSeconds(1);
        invulnerable = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool canItBeHit()
    {
        return true;
    }

    void Awake()
    {
        currentHealth = maxHealth;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DealDamage(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(5f);


        }
    }

    public void Bounce(Collider2D collision)
    {
        
    }
}
