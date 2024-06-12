using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI missedText, killedText;
    public static GameManager instance;

    public GameData data;

    public float[] xLocations;
    public float yLocation;

    private GameObject[] probs;

    public float centerScaleFactor;

    public float centerLength;

    public float enemySpeed;

    public CanvasScaler canvasScaler;
    public float big, small;
    public GameObject mobileInputs;

    public AudioSource deathSound, laserSound;

    int missed = 0, killed = 0;

    private void Start()
    {
        instance = this;

        enemySpeed = data.speed;

        if (Application.isMobilePlatform)
        {
            canvasScaler.matchWidthOrHeight = small;
            if (mobileInputs)
                mobileInputs.SetActive(true);
        }
        else
        {
            canvasScaler.matchWidthOrHeight = big;
            if (mobileInputs)
                mobileInputs.SetActive(false);
        }
    }

    public void InitLocations()
    {
        probs = GameObject.FindGameObjectsWithTag("Probs");

        
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
