using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public GameObject leftChild, center, rightChild;
    public float[] xLocations;

    public float childLength, centerScaleFactor;

    public int leftIndex, rightIndex;

    private GameObject[] lasers;

    private void Start()
    {
        leftIndex = Random.Range(0, xLocations.Length);
        rightIndex = Random.Range(leftIndex, xLocations.Length);

        if (leftIndex == 0 && rightIndex == 7) rightIndex = 6;

        transform.position = new Vector2(xLocations[Random.Range(0, xLocations.Length)], 6);

        leftChild.transform.position = new Vector2(xLocations[leftIndex] - childLength / 2, 6);
        rightChild.transform.position = new Vector2(xLocations[rightIndex] + childLength / 2, 6);

        center.transform.localScale = new Vector2((rightIndex - leftIndex) * centerScaleFactor, center.transform.localScale.y);
        center.transform.position = new Vector2(leftChild.transform.position.x + childLength / 2 + (rightIndex - leftIndex) * 0.46f, 6);

        if (leftIndex == rightIndex)
        {
            center.SetActive(false);
        }
    }

    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);

        // Check for collision with "Laser" tag
        CheckLaserCollision();

        if (transform.position.y < -0.5f)
            Destroy(gameObject);
    }

    private void CheckLaserCollision()
    {
        // Find all the "Laser" tagged gameObjects
        lasers = GameObject.FindGameObjectsWithTag("Laser");

        bool isLeftChildHit = false;
        bool isRightChildHit = false;

        // Check if the left and right child collide with any of the lasers
        foreach (GameObject laser in lasers)
        {
            if (leftChild.GetComponent<BoxCollider2D>().IsTouching(laser.GetComponent<BoxCollider2D>()))
            {
                isLeftChildHit = true;
            }

            if (rightChild.GetComponent<BoxCollider2D>().IsTouching(laser.GetComponent<BoxCollider2D>()))
            {
                isRightChildHit = true;
            }
        }

        // If both left and right child are hit by lasers, destroy the enemy
        if (isLeftChildHit && isRightChildHit)
        {
            Debug.Log("Ha Ha Killed You!");
            Destroy(gameObject);
        }
    }

}
