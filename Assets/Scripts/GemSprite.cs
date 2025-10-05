using UnityEngine;
using UnityEngine.UI;

public class GemImage : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private bool found = false;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Discover()
    {
        this.found = true;
        this.image.color = Color.white;
    }
    public void SetVisual(GemSO patron)
    {
        image.sprite = patron.gemSprite;
        image.color = Color.black;
    }



}