using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float projectileVelocity = 50f;
    Rigidbody rb;
    public float damage = 25f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(BulletLife());
        rb.linearVelocity = transform.forward * projectileVelocity;
    }

   
    void Update()
    {
        
    }

    IEnumerator BulletLife()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {

        GameObject hitObject = collision.gameObject;

        Health targetHealth = hitObject.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }

    if (hitObject.CompareTag("Environment") || hitObject.CompareTag("Enemy"))
        {
            rb.linearVelocity = transform.forward * 0f;
        }
    }
}
