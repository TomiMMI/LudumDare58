using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer cursorSpriteRenderer;

    [SerializeField]
    private UnityEngine.Sprite handSprite;
    [SerializeField]
    private UnityEngine.Sprite hammerSprite;

    void Start()
    {
        Cursor.visible = false;
        this.cursorSpriteRenderer.sprite = handSprite;
    }
    void Update()
    {
        this.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public void SetCursorToHammer()
    {
        this.cursorSpriteRenderer.sprite = hammerSprite;
    }
        public void SetCursorToHand()
    {
        this.cursorSpriteRenderer.sprite = handSprite;
    }


}