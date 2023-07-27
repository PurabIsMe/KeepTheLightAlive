using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Centipede_Spawner : MonoBehaviour
{
    public float Max_spawn_distance = 50f;
    public int[] spawn_count = { 1, 2, 3, 6, 6, 5 };
    public float spawn_delay = 5;
    public Transform target;
    int index = 0;

    float t = 0;

    public GameObject centipede;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if (t > spawn_delay)
        {
            t = 0;
            for (int i = 0; i < spawn_count[index]; i++)
            {
                Spawn();
            }
            index++;

            if(index > spawn_count.Length)
                index = 0;

        }
    }

    void Spawn()
    {
        GameObject s = Instantiate(centipede);
        s.transform.position = new Vector3(Random.Range(-Max_spawn_distance, Max_spawn_distance), Random.Range(-Max_spawn_distance, Max_spawn_distance), 0) + transform.position;
        Centipede c = s.GetComponent<Centipede>();
        c.target = target;
    }
}
