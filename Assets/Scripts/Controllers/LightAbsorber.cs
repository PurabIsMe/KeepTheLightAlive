using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAbsorber : EntityBase {
    public float moveSpeed;
    public float sight;
    public float damage;

    private void Update() {
        float closest = sight;
        Vector3 followPoint = transform.position;
        //move to closest light
        foreach(GameObject light in worldLight){   
            if((light.transform.position - transform.position).magnitude <= closest) {
                closest = (light.transform.position - transform.position).magnitude;
                followPoint = light.transform.position;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, followPoint, moveSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent(out NewPlayerController player)) {
            player.StealEnergy(damage);
            Destroy(gameObject);
        }
        if(collision.TryGetComponent(out LivingLight light)) {
            light.StealLight();
            Destroy(collision.gameObject);
        }
    }
}
