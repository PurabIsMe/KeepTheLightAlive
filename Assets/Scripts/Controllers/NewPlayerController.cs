using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class NewPlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float energy;
    public float maxEnergy;
    public float energyRegen;
    public float dashPower;
    public float dashCost;
    public float maxLightRadius;
    public float maxlightIntensity;
    public float shotCost;
    public float shotSpeed;

    bool handle_flash = true;

    public Light2D energyLight;
    public GameObject arm;
    public Image energybar;
    public Light2D flashlight;
    public GameObject bulletPrefab;

    Rigidbody2D rb;

    public SpriteRenderer face;
    public Sprite default_face;
    public Sprite ow_face;
    public Sprite star_face;
    public Sprite rage_face;

    public float face_time = 2f;

    string current_face = "Default";
    float t = 0;
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetFlashHandling(bool hf)
    {
        handle_flash = hf;
    }

    void Face_Logic()
    {
        if(current_face != "Default")
        {
            t += Time.deltaTime;
            if(t > face_time)
            {
                face.sprite = default_face;
                t = 0;
            }
        }
    }

    //game loop
    private void Update() {
        Face_Logic();

        float usedEnergy = 0;
        usedEnergy += HandleMovement(energy);
        usedEnergy += HandleOther(energy);
        HandleEnergy(usedEnergy);

        //arm look

        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Flashlight
        if (handle_flash)
        {
            flashlight.intensity = maxlightIntensity * energy / maxEnergy;
            flashlight.pointLightOuterRadius = maxLightRadius * energy / maxEnergy;
        }

        energyLight.intensity = maxlightIntensity/2 * energy / maxEnergy;
        energyLight.pointLightOuterRadius = maxLightRadius/2 * energy / maxEnergy;
    }

    //move, jump, all that good stuff, then return used energy
    private float HandleMovement(float givenEnergy) {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 to_mosue = -(transform.position - worldPosition);
        to_mosue.z = 0;
        to_mosue.Normalize();

        float angle = Mathf.Atan2(to_mosue.y, to_mosue.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle);

        float energyPercentage = energy / maxEnergy;//value between 0 and 1, to be multiplied with jump and walking, the less energy, the less speed player has.
        //This does not feel fun, scrapping
        energyPercentage = 1;
        float usedEnergy = 0;//amount of energy taken from actions (will be returned)

        //walk
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 mov = new Vector3(moveX, moveY, 0); 
        mov.Normalize();

        rb.AddForce(mov * walkSpeed * Time.deltaTime);



        //dash
        if (Input.GetKeyDown(KeyCode.LeftShift)) {       
            rb.AddForce(to_mosue * 100 * energyPercentage * dashPower);
            usedEnergy += dashCost;
        }
        return usedEnergy;
    }

    private void OnDamage()
    {
        current_face = "OWFace";
        face.sprite = ow_face;
    }
    //fire
    private float HandleOther(float givenEnergy) {
        float usedEnergy = 0;
        //fire
        if (Input.GetMouseButtonDown(0) && energy > shotCost) {
            current_face = "RageFace";
            face.sprite = rage_face;

            usedEnergy += shotCost;

            GameObject newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = arm.transform.position;

            //rotate bullet to face mousePos
            Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            newBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            newBullet.GetComponent<BulletScript>().SetDirection(arm.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), shotSpeed);
        }
        
        return usedEnergy;
    }

    //energy visuals/ keeping energy from going past max, or under 0
    private void HandleEnergy(float usedEnergy) {
        energy -= usedEnergy;
        if (energy > maxEnergy) { 
            energy = maxEnergy;
        }
        if (energy < 0) { 
            energy = 0;
        }
        energybar.fillAmount = energy/maxEnergy;// show energy
        energy += Time.deltaTime * energyRegen;
    }

    //enemy attacking
    public float StealEnergy(float amount) {
        energy -= amount;
        return amount;
    }
    //add energy and increase max by extra/2
    public void AddEnergy(float added) {
        energy += added;
        float extra = energy - maxEnergy;

        face.sprite = star_face;
        current_face = "StarFace";

        if (extra > 0){
            maxEnergy += extra / 2;
        }
    }
}
