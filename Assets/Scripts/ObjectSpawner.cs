using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public float spawnX, spawnY;
    public GameObject[] enemies;
    public GameObject[] lights;
    public List<GameObject> worldLights;
    public Transform followTransform;
    public float enemySpawnRate;
    public float lightSpawnRate;

    float timeBetweenEnemySpawns;
    float timeBetweenLightSpawns;

    private void Start() {
        Spawn(enemies);
    }

    private void Update() {
        timeBetweenEnemySpawns += Time.deltaTime;//update seconds since last spawn
        timeBetweenLightSpawns += Time.deltaTime;

        if (timeBetweenEnemySpawns >= enemySpawnRate) {
            Spawn(enemies);
            timeBetweenEnemySpawns = 0;
        }
        if (timeBetweenLightSpawns >= lightSpawnRate) {
            Spawn(lights);
            timeBetweenLightSpawns = 0;
        }
    }

    //spawn new object with random position
    void Spawn(GameObject[] objects) {
        GameObject newEnemy = Instantiate(objects[Random.Range(0, objects.Length)], transform); //spawn random enemy in spawner
        newEnemy.transform.position = transform.position + new Vector3(Random.Range(0, spawnX), Random.Range(0, spawnY), 0);
        
        //entity stats
        if (newEnemy.TryGetComponent(out EntityBase entity)) {
            entity.worldLight = worldLights;
            entity.playerTransform = followTransform;
        } if (newEnemy.GetComponent<LivingLight>()) {
            worldLights.Add(newEnemy);
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + new Vector3(spawnX/2, spawnY/2, 0), new Vector3(spawnX, spawnY, 0));
    }
}
