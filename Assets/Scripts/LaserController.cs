using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float speed;
    public Sprite[] balls;
    public GameObject[] trails;

    private void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);

        if (transform.position.y > 5.2f)
        {
            transform.position = new Vector2(transform.position.x, -0.2f);
            speed = 0;
            gameObject.SetActive(false);
        }
    }

    public void SetNewSprite(int index)
    {
        GetComponent<SpriteRenderer>().sprite = balls[index];
    }

    public void EnableTrail(int index)
    {
        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].SetActive(false);
        }
        trails[index].SetActive(true);
    }

}
