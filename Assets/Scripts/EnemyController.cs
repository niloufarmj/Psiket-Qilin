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

    public GameObject deathParticle;


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
        GameManager.instance.InitLocations();
        leftIndex = Random.Range(0, xLocations.Length);
        rightIndex = Random.Range(leftIndex, xLocations.Length);
        int spriteIndex = Random.Range(0, childs.Length);


        transform.position = new Vector2(xLocations[Random.Range(0, xLocations.Length)], 6);

        leftChild.transform.position = new Vector2(xLocations[leftIndex], 6);
        rightChild.transform.position = new Vector2(xLocations[rightIndex], 6);
        leftChild.GetComponent<Image>().sprite = childs[spriteIndex];
        rightChild.GetComponent<Image>().sprite = childs[spriteIndex];


        center.transform.position = new Vector2(leftChild.transform.position.x, 5.93f);
        center.GetComponent<Image>().sprite = centers[spriteIndex];

        RectTransform centerRect = center.GetComponent<RectTransform>();

        centerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 
            rightChild.GetComponent<RectTransform>().anchoredPosition.x - leftChild.GetComponent<RectTransform>().anchoredPosition.x);
        

        if (leftIndex == rightIndex)
        {
            center.SetActive(false);
        }
    }

    private IEnumerator WaitForSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    private void CheckLaserCollision()
    {
        leftCollided = leftChild.GetComponent<EnemyCollisionHandler>().isCollided();
        rightCollided = rightChild.GetComponent<EnemyCollisionHandler>().isCollided();

        
        if (leftCollided && rightCollided)
        {
            GameManager.instance.deathSound.Play();
            GameObject[] lasers = { leftChild.GetComponent<EnemyCollisionHandler>().getCollidedLaser(), rightChild.GetComponent<EnemyCollisionHandler>().getCollidedLaser() };
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].transform.position = new Vector2(lasers[i].transform.position.x, -0.6f);
                lasers[i].GetComponent<LaserController>().speed = 0;
                lasers[i].SetActive(false);
            }

            Instantiate(deathParticle, leftChild.gameObject.transform.position, leftChild.gameObject.transform.rotation);
            Instantiate(deathParticle, rightChild.gameObject.transform.position, rightChild.gameObject.transform.rotation);

            GameManager.instance.AddKilled();
            Destroy(gameObject);
        }
    }

}
