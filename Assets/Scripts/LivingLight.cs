using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingLight : MonoBehaviour
{
    public float lightGranted;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out NewPlayerController player))
        {
            player.AddEnergy(lightGranted);
            Destroy(gameObject);
        }
    }

    public float StealLight() {
        return lightGranted;
    }
}
