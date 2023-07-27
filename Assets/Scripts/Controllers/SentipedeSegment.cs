using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentipedeSegment : MonoBehaviour
{

    public SpriteRenderer sr { get; private set;}

    public GiantSentipede sentipede { get; set; }
    public GameObject is_head;
    public GameObject behind;

    private bool isHead => is_head == null;

    private void Awake(){
        sr = GetComponent<SpriteRenderer>();
    }
    
    public void Move() {
        if (is_head){
            transform.position = Vector3.MoveTowards(transform.position, is_head.transform.position, Time.deltaTime);
        } else{
            transform.position = Vector3.MoveTowards(transform.position, sentipede.playerTransform.position, Time.deltaTime);
        }
    }
}
