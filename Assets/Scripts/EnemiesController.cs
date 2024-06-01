using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float interval;

    private float currentTime;

    private void Start()
    {
        SpawnEnemy();
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (interval - currentTime < 0.05)
        {
            SpawnEnemy();
            currentTime = 0;
            interval *= 0.99f;
        }
    }

    public void SpawnEnemy()
    {
        GameObject enemyObj = Instantiate(enemyPrefab);
    }
}