using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour
{
    public Transform followTransform;
    public float speed;
    public Vector3 offset;

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, followTransform.position + offset, speed * Time.deltaTime);
    }
}
