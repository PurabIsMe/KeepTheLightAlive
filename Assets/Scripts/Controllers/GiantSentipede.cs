using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class GiantSentipede : EntityBase
{
    public List<SentipedeSegment> segments = new List<SentipedeSegment>(); 
    public SentipedeSegment segmentPrefab;
    public Sprite HeadSprite;
    public Sprite BodySprite;
    public int size = 12;
    public float segmentSize;
    public GameObject[] enemySpawns;
    public float enemySpawnTimes;

    private void Start() {
        Spawn();
    }

    private void Spawn() { 
        for (int i = 0; i < size; i++) {
            Vector2 position = GridPosisition(transform.position) + (Vector2.left * i);
            SentipedeSegment segment = Instantiate(segmentPrefab, position, Quaternion.identity);
            segment.transform.SetParent(transform, false);
            segment.transform.localScale = Vector2.one * segmentSize;
            segment.sr.sprite = i== 0 ? HeadSprite : BodySprite;
            segment.sentipede = this;
            segments.Add(segment);
        }

        for (int i = 0;i < segments.Count; i++) {
            SentipedeSegment segment = segments[i];
            if(GetSegment(i - 1))segment.is_head = GetSegment(i - 1).gameObject;
            if(GetSegment(i + 1))segment.behind = GetSegment(i + 1).gameObject;
        }
    }

    private SentipedeSegment GetSegment(int index) { 
        if(index >= 0 && index < segments.Count) {
            return segments[index];
        } else{ 
            return null;
        }
    }

    private Vector2 GridPosisition(Vector2 position) {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
    }

    void Update() {
        foreach (SentipedeSegment segment in segments) {
            segment.Move();
            GetComponent<CircleCollider2D>().offset = segments[0].transform.position - transform.position;
        }
    }

    public override void DamageEntity(float damage) {
        health -= damage;
        if (health <= 0) {
            //spawn new light
            for (int i = 0; i < enemySpawnTimes; i++) {
                if (enemySpawns.Length > 0) {
                    GameObject newEnemy = Instantiate(enemySpawns[UnityEngine.Random.Range(0, enemySpawns.Length)], transform.position, Quaternion.identity);
                    newEnemy.GetComponent<EntityBase>().worldLight = worldLight;
                }
            }
            Destroy(gameObject);
        }
    }
}
