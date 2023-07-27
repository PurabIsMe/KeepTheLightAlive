using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Star : MonoBehaviour
{
    // Start is called before the first frame update
    public float blink_time = .5f;
    public float max_scale = 1f;
    public float min_scale = 0f;
    public Light2D l;
    float t;
    bool reversed = false;
    public Vector2 fall_dir;
    public float max_fall_time = 2f;
    public float min_fall_time = .5f;
    float fall_time = 0f;
    bool falling = true;
    LivingLight livingLight;
    float ft = 0;

    public float lifetime = 120;
    float lt = 0;
    void Awake()
    {
        l.pointLightOuterRadius = max_scale * 5;
        livingLight = GetComponent<LivingLight>();
        fall_time = Random.Range(min_fall_time, max_fall_time);
        transform.localScale = Vector3.one * max_scale;
        //transform.position = transform.position - (new Vector3(fall_dir.x, fall_dir.y,0) * fall_time);
    }

    // Update is called once per frame
    void Update()
    {
        if(lt > lifetime)
        {
            Destroy(gameObject);
        }
        if (falling)
        {
            transform.position = transform.position + new Vector3(fall_dir.x, fall_dir.y) * Time.deltaTime;
            ft += Time.deltaTime;
            if(ft > fall_time ) {
                falling = false;
                livingLight.enabled = true; 
            }
        }
        else
        {
            lt += Time.deltaTime;
            if (reversed)
                t -= Time.deltaTime;
            else
                t += Time.deltaTime;
            if (t > blink_time)
                reversed = true;
            if (t < 0)
                reversed = false;

            float s = Mathf.Lerp(max_scale, min_scale, t);
            transform.localScale = Vector3.one * s;
            l.intensity = s / 2;
        }
    }
}
