using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GemImage : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private bool found = false;
    private String gemName;

    private GemRarity gemRarity;

    private void Awake()
    {
        image = GetComponent<Image>();
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Discover()
    {
        this.found = true;
        this.image.color = Color.white;
        GetComponent<BoxCollider2D>().enabled = true;
    }
    public void SetVisual(GemSO patron)
    {
        image.sprite = patron.gemSprite;
        image.color = Color.black;
        gemName = patron.gemName;
        gemRarity = patron.gemRarity;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            GameObject.FindWithTag("SelectedGem").GetComponent<TMP_Text>().text = gemName;
            GameObject.FindWithTag("SelectedGem").GetComponent<TMP_Text>().color = GeodesAndGemsUtilities.Instance.RarityColor(gemRarity);
        }
    }
        void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            GameObject.FindWithTag("SelectedGem").GetComponent<TMP_Text>().text = "";
        }
    }



}