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
        gemName = patron.gemName;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (found && collision.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            GameObject.FindWithTag("SelectedGem").GetComponent<TMP_Text>().text = gemName;
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