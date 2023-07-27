using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public float Max_spawn_distance = 50f;
    public float spawn_cd = .5f;

    public float max_star_size = 3f;
    public float min_star_size = .2f;
    public GameObject star;

    float t;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if(t > spawn_cd)
        {
            Spawn();
            t = 0;
        }
    }

    void Spawn()
    {
        GameObject s = Instantiate(star);
        s.transform.position = new Vector3(Random.RandomRange(-Max_spawn_distance, Max_spawn_distance), Random.RandomRange(-Max_spawn_distance, Max_spawn_distance), 0) + transform.position;
        Star sr = s.GetComponent<Star>();
        sr.max_scale = Random.RandomRange(min_star_size, max_star_size);
        LivingLight light = s.GetComponent<LivingLight>();
        light.lightGranted = sr.max_scale * 5;
    }
}
