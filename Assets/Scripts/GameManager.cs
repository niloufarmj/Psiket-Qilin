using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI missedText, killedText;
    public static GameManager instance;

    public GameData data;

    public float[] xLocations;
    public float yLocation;

    public GameObject[] probs;

    public float centerScaleFactor;

    public float centerLength;

    public float enemySpeed;

    int missed = 0, killed = 0;

    private void Start()
    {
        instance = this;
        enemySpeed = data.speed;

        for (int i = 0; i < probs.Length; i++)
        {
            xLocations[i] = probs[i].transform.position.x;
        }
        yLocation = probs[0].transform.position.y;
    }

    public void AddMissed()
    {
        missed++;
        missedText.text = missed.ToString();
    }

    public void AddKilled()
    {
        killed++;
        killedText.text = killed.ToString();
    }
}
