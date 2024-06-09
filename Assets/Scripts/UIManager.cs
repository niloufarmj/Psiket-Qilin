using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameData data;
    public GameObject qbitMenu, speedMenu;
    public GameObject twoSelected, threeSelected, slowSelected, normalSelected, fastSelected;
  
    public void PlayGame()
    {
        if (qbitMenu.activeSelf) DisableQbitMenu();
        if (speedMenu.activeSelf) DisableSpeedMenu();

        if (data.Qbits == 2)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(1);
            SceneManager.LoadScene(1);
        }
    }

    public void Controls()
    {

    }

    public void HandleQbitMenu()
    {
        if (qbitMenu.activeSelf) DisableQbitMenu();
        else EnableQbitMenu();
    }

    public void HandleSpeedMenu()
    {
        if (speedMenu.activeSelf) DisableSpeedMenu();
        else EnableSpeedMenu();
    }

    public void EnableQbitMenu()
    {
        DisableSpeedMenu();

        qbitMenu.SetActive(true);

        if (data.Qbits == 2)
        {
            twoSelected.SetActive(true);
            threeSelected.SetActive(false);
        }
        else
        {
            twoSelected.SetActive(false);
            threeSelected.SetActive(true);
        }
    }

    public void DisableQbitMenu()
    {
        if (twoSelected.activeSelf) data.Qbits = 2;
        else data.Qbits = 3;

        qbitMenu.SetActive(false);
    }

    public void EnableSpeedMenu()
    {
        DisableQbitMenu();
        speedMenu.SetActive(true);

        if (data.speed == 0.8f)
        {
            slowSelected.SetActive(true);
            normalSelected.SetActive(false);
            fastSelected.SetActive(false);
        }
        else if (data.speed == 1.1f)
        {
            slowSelected.SetActive(false);
            normalSelected.SetActive(true);
            fastSelected.SetActive(false);
        }
        else
        {
            slowSelected.SetActive(false);
            normalSelected.SetActive(false);
            fastSelected.SetActive(true);
        }
    }

    public void DisableSpeedMenu()
    {
        if (slowSelected.activeSelf) data.speed = 0.9f;
        else if (normalSelected.activeSelf) data.speed = 1.1f;
        else data.speed = 1.3f;

        speedMenu.SetActive(false);
    }

    public void SelectTwo()
    {
        twoSelected.SetActive(true);
        threeSelected.SetActive(false);
    }

    public void SelectThree()
    {
        twoSelected.SetActive(false);
        threeSelected.SetActive(true);
    }

    public void SelectSlow()
    {
        slowSelected.SetActive(true);
        normalSelected.SetActive(false);
        fastSelected.SetActive(false);
    }

    public void SelectNormal()
    {
        slowSelected.SetActive(false);
        normalSelected.SetActive(true);
        fastSelected.SetActive(false);
    }

    public void SelectFast()
    {
        slowSelected.SetActive(false);
        normalSelected.SetActive(false);
        fastSelected.SetActive(true);
    }
}
