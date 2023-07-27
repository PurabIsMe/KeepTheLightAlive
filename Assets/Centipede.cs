using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : MonoBehaviour
{
    public Transform target;
    public float HP = 5;
    public float speed = 5;
    public float turn_speed = 5f;
    Rigidbody2D rb;

    internal void Damage(float damage)
    {
        HP -= damage;
        if(HP < 0)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 dir = target.position - transform.position;
        dir.Normalize();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        rb.AddForce(dir * speed * Time.fixedDeltaTime);  
        rb.MoveRotation(angle + 90);
    }
    

}
