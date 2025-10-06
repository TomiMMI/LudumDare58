using UnityEngine;
using UnityEngine.Audio;

public class PaperCanvas : MonoBehaviour
{
    public GameObject childObject;
    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        childObject.SetActive(false);
    }

    public void OnPaperCanvasShow()
    {
        audioSource.Play();
        childObject.SetActive(true);
    }
    public void OnPaperCanvasHide()
    {
        childObject.SetActive(false);
    }
}
