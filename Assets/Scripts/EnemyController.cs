using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public GameObject leftChild, center, rightChild;

    private float[] xLocations;

    public Sprite[] childs, centers;

    private float centerScaleFactor;

    public int leftIndex, rightIndex;


    bool leftCollided = false;
    bool rightCollided = false;

    

    private void Start()
    {
        xLocations = GameManager.instance.xLocations;
        centerScaleFactor = GameManager.instance.centerScaleFactor;
        initEnemy();
    }

    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - GameManager.instance.enemySpeed * Time.deltaTime);

        // Check for collision with "Laser" tag
        CheckLaserCollision();

        if (transform.position.y < GameManager.instance.yLocation + 0.6f)
        {
            GameManager.instance.AddMissed();
            Destroy(gameObject);
        }

    }

    private void initEnemy()
    {
        leftIndex = Random.Range(0, xLocations.Length);
        rightIndex = Random.Range(leftIndex, xLocations.Length);
        int spriteIndex = Random.Range(0, childs.Length);


        transform.position = new Vector2(xLocations[Random.Range(0, xLocations.Length)], 6);

        leftChild.transform.position = new Vector2(xLocations[leftIndex], 6);
        rightChild.transform.position = new Vector2(xLocations[rightIndex], 6);
        leftChild.GetComponent<Image>().sprite = childs[spriteIndex];
        rightChild.GetComponent<Image>().sprite = childs[spriteIndex];

        center.transform.localScale = new Vector2((rightIndex - leftIndex) * centerScaleFactor, center.transform.localScale.y);
        center.transform.position = new Vector2(leftChild.transform.position.x + (rightIndex - leftIndex) * GameManager.instance.centerLength, 5.93f);
        center.GetComponent<Image>().sprite = centers[spriteIndex];

        if (leftIndex == rightIndex)
        {
            center.SetActive(false);
        }
    }

    private void CheckLaserCollision()
    {
        leftCollided = leftChild.GetComponent<EnemyCollisionHandler>().isCollided();
        rightCollided = rightChild.GetComponent<EnemyCollisionHandler>().isCollided();

        GameObject[] lasers = { leftChild.GetComponent<EnemyCollisionHandler>().getCollidedLaser(), rightChild.GetComponent<EnemyCollisionHandler>().getCollidedLaser() };
        if (leftCollided && rightCollided)
        {
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].transform.position = new Vector2(lasers[i].transform.position.x, -0.6f);
                lasers[i].GetComponent<LaserController>().speed = 0;
                lasers[i].SetActive(false);
            }

            GameManager.instance.AddKilled();
            Destroy(gameObject);
        }
    }

}
