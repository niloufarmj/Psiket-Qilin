using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    bool collided = false;
    GameObject laser;
    public bool isCollided()
    {
        return collided;
    }

    public GameObject getCollidedLaser()
    {
        return laser;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            collided = true;
            laser = collision.gameObject;
        }
    }
}
