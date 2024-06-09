using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsController : MonoBehaviour
{
    public GameObject starPrefab;
    public Sprite[] starImages;

    public float leftLimit, rightLimit, topLimit, bottomLimit;

    public float interval;

    private float currentTime;

    void Start()
    {
        int starsCount = Random.Range(15, 40);

        for (int i = 0; i < starsCount; i++)
        {
            float x = Random.Range(leftLimit, rightLimit);
            float y = Random.Range(topLimit, bottomLimit);

            ShowStar(x, y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (interval - currentTime < 0.05)
        {
            currentTime = 0;
            ShowStar(Random.Range(leftLimit, rightLimit), Random.Range(topLimit, topLimit + 1));
        }
    }

    public void ShowStar(float x, float y)
    {
        GameObject newStar = Instantiate(starPrefab);
        newStar.transform.position = new Vector2(x, y);
        newStar.GetComponent<Image>().sprite = starImages[Random.Range(0, starImages.Length)];
        newStar.transform.SetParent(GameObject.FindGameObjectWithTag("Stars").transform, false);
    }
}
