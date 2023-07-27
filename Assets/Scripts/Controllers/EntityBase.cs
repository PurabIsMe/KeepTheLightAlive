using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    public float health;
    public List<GameObject> worldLight;
    public Transform playerTransform;
    public GameObject[] lightDrops;

    //centipede overrides this function
    public virtual void DamageEntity(float damage) {
        health -= damage;
        if (health <= 0){
            //spawn new light
            if (lightDrops.Length > 0) Instantiate(lightDrops[Random.Range(0, lightDrops.Length)], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
