using UnityEngine;

public class PaperCanvas : MonoBehaviour
{
    public GameObject childObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        childObject.SetActive(false);
    }

    public void OnPaperCanvasShow()
    {
        childObject.SetActive(true);
    }
    public void OnPaperCanvasHide()
    {
        childObject.SetActive(false);
    }
}
