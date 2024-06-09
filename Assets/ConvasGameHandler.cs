using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvasGameHandler : MonoBehaviour
{
    public float big, small;
    public void Start()
    {
        if (Application.isMobilePlatform)
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = small;
        }
        else
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = big;
        }
    }
}
