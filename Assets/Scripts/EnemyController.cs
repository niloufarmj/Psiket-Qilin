using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public GameObject leftChild, center, rightChild;
    public float[] xLocations;

    public Sprite[] childs, centers;

    public float childLength, centerScaleFactor;

    public int leftIndex, rightIndex;

    private GameObject[] lasers;

    bool leftCollided = false;
    bool rightCollided = false;

    private void Start()
    {
        initEnemy();
    }

    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);

        // Check for collision with "Laser" tag
        CheckLaserCollision();

        if (transform.position.y < -1.1f)
            Destroy(gameObject);
    }

    private void initEnemy()
    {
        leftIndex = Random.Range(0, xLocations.Length);
        rightIndex = Random.Range(leftIndex, xLocations.Length);
        int spriteIndex = Random.Range(0, childs.Length);

        if (leftIndex == 0 && rightIndex == 7) rightIndex = 6;


        transform.position = new Vector2(xLocations[Random.Range(0, xLocations.Length)], 6);

        leftChild.transform.position = new Vector2(xLocations[leftIndex] - childLength / 2, 6);
        rightChild.transform.position = new Vector2(xLocations[rightIndex] + childLength / 2, 6);
        leftChild.GetComponent<SpriteRenderer>().sprite = childs[spriteIndex];
        rightChild.GetComponent<SpriteRenderer>().sprite = childs[spriteIndex];

        center.transform.localScale = new Vector2((rightIndex - leftIndex) * centerScaleFactor, center.transform.localScale.y);
        center.transform.position = new Vector2(leftChild.transform.position.x + childLength / 2 + (rightIndex - leftIndex) * 0.46f, 5.93f);
        center.GetComponent<SpriteRenderer>().sprite = centers[spriteIndex];

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

            Destroy(gameObject);
        }
    }

}
