using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float velocity;
    public float damage;

    private float force;
    private Vector3 direction = Vector3.zero;

    public float lifetime = 5;
    float t;
    public void SetDirection(Vector3 startPoint, Vector3 targetPoint, float newForce) {
        force = newForce;
        direction = (targetPoint - startPoint).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * force);
    }

    private void Update()
    {
        t += Time.deltaTime;
        if(t > lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent(out Centipede entity)) { 
            entity.Damage(damage);
        }

    }
}
