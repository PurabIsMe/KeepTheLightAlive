using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede_Segment : MonoBehaviour
{

    public float speed = 3f;
    public Transform target;
    Rigidbody2D rb;

    public float stop_distance = 3f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        Vector3 dir = -(transform.position - target.position);
        float dis = dir.magnitude;

        dir.Normalize();
        float penalty = 1f;
        if(dis < stop_distance)
        {
            penalty = .03f;
        }
        rb.MovePosition(transform.position + dir * speed * Time.fixedDeltaTime * penalty);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle + 90);
        
    }
}
