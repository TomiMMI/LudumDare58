using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class LoadingBar : MonoBehaviour
{
    [SerializeField]
    private Image progressImage;
    [SerializeField]
    private TMP_Text loadingBarText;

    public void UpdateLoadingBar(float timeElapsed, float timeToGrind)
    {
        loadingBarText.text = ((int)timeElapsed / 60) + ":" + ((int)timeElapsed % 60) + " / " + ((int)timeToGrind / 60) + ":" + ((int)timeToGrind % 60);
        progressImage.fillAmount = timeElapsed / timeToGrind;
    }
}