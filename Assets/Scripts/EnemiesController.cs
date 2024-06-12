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
            interval *= 0.995f;
        }
    }

    public void SpawnEnemy()
    {
        //Debug.Log(GameManager.instance.xLocations);
        GameObject enemyObj = Instantiate(enemyPrefab);
        enemyObj.transform.SetParent(GameObject.FindGameObjectWithTag("Enemies").transform, false);
    }
}
