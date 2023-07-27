using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Laser : MonoBehaviour
{
    //In percent
    public float energy_cost = .5f;
    public float charge_time = 2f;
    public float lazing_time = .1f;
    public float ease_time = .5f;
    public Light2D flash_laser;
    public Transform raycast_point;
    public float damage = 5f;

    public NewPlayerController controller;
    LineRenderer line_renderer;

    public float init_out_angle = 60;
    public float init_in_angle = 50;
    public float min_angle = 2;
    public float init_intensity = 5;
    public float max_intensity = 20;

    float charge_dt = 0;
    float shoot_dt = 0;
    float ease_dt = 0;

    bool charging = false;
    bool shooting = false;
    bool easing = false;
    // Start is called before the first frame update
    void Start()
    {
        line_renderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && controller.energy > controller.maxEnergy * energy_cost && !charging && !easing && !shooting)
        {
            charging = true;
            controller.SetFlashHandling(false);
            Debug.Log("Charging!");
        }
        if (charging)
        {
            float t = charge_dt / charge_time;
            flash_laser.pointLightOuterAngle = Mathf.Lerp(init_out_angle, min_angle, t);
            flash_laser.pointLightInnerAngle = Mathf.Lerp(init_in_angle, min_angle, t);
            flash_laser.intensity = Mathf.Lerp(init_intensity, max_intensity, t);

            controller.energy -= controller.maxEnergy * energy_cost * Time.deltaTime / charge_time;

            charge_dt += Time.deltaTime;

            if (charge_dt > charge_time)
            {
                Debug.Log("Shooting!");
                charging = false;
                shooting = true;
                charge_dt = 0;
                line_renderer.enabled = true;
            }
        }
        if (shooting) {
            line_renderer.SetPosition(0, raycast_point.position);
            shoot_dt += Time.deltaTime;
            RaycastHit2D hit;
            Vector2 mouse_point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 ray_dir =  -(new Vector2(raycast_point.position.x, raycast_point.position.y) - mouse_point);
            ray_dir.Normalize();
            hit = Physics2D.Raycast(raycast_point.position, ray_dir);

            if(hit.collider != null)
            {
                if(hit.collider.gameObject.tag == "Enemy")
                {
                    Debug.Log("Enemy Hit!");
                    Centipede c = hit.collider.gameObject.GetComponent<Centipede>();
                    c.Damage(damage * Time.deltaTime);
                }
                else
                {
                    Debug.Log(hit.collider.gameObject);
                }
                line_renderer.SetPosition(1, hit.point);
            }
            else
            {
                line_renderer.SetPosition(1, new Vector2(raycast_point.position.x, raycast_point.position.y)  + ray_dir * 50);
            }
            if (shoot_dt > lazing_time)
            {
                line_renderer.enabled = false;
                Debug.Log("Easing!");
                shooting = false;
                easing = true;
                shoot_dt = 0;
            }
        }
        if (easing)
        {
            float t = ease_dt / ease_time;
            flash_laser.pointLightOuterAngle = Mathf.Lerp(min_angle, init_out_angle, t);
            flash_laser.pointLightInnerAngle = Mathf.Lerp(min_angle, init_in_angle, t);
            flash_laser.intensity = Mathf.Lerp(max_intensity, init_intensity, t);
            ease_dt += Time.deltaTime;
            if (ease_dt > ease_time)
            {
                Debug.Log("Finished Easing!");
                controller.SetFlashHandling(true);
                easing = false;
                ease_dt = 0;

            }
        }
    }
}
