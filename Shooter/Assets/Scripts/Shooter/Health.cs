using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;  
    private float currentHealth;

        void Awake()
    {
        currentHealth = maxHealth; 
    }

    
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

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
