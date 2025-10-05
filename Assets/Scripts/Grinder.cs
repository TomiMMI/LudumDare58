using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Grinder : MonoBehaviour
{
    [SerializeField]
    private float timeToGrind;
    private float timeElapsed;
    [SerializeField]
    private LoadingBar loadingBar;
    public int debug;

    [SerializeField]
    private Geode activeGeode;

    void Start()
    {
    }

    void Update()
    {

        HandleGrinder();
    }

    private void HandleGrinder()
    {
        if (activeGeode != null)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= timeToGrind)
            {
                EndGrinding();
            }
            else
            {
                loadingBar.UpdateLoadingBar(timeElapsed, timeToGrind);
            }
        }
    }
    private void StartGrinding(Geode geodeGrinded)
    {
        if (activeGeode == null)
        {
                   activeGeode = geodeGrinded;
        loadingBar.gameObject.SetActive(true); 
        }
    }

    private void EndGrinding()
    {
        Destroy(activeGeode);
        activeGeode = null;
        loadingBar.gameObject.SetActive(false);
    }


}