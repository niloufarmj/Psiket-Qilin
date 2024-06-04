using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);

        if (transform.position.y < -5.5)
            Destroy(gameObject);
    }
}
