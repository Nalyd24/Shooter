using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;  
    private float currentHealth;

        void Start()
    {
        currentHealth = maxHealth; 
    }

    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }
    void OnDeath()
        {
            Destroy(gameObject);
        }
}
