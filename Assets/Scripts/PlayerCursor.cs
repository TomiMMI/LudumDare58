using DG.Tweening;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    public static PlayerCursor Instance { get; private set; }

    void Awake()
    {
        PlayerCursor.Instance = this;
    }

    [SerializeField]
    private SpriteRenderer cursorSpriteRenderer;

    [SerializeField]
    private UnityEngine.Sprite handSprite;
    [SerializeField]
    private UnityEngine.Sprite hammerSprite;

    public Tween hammerRotateTween;
    public Tween hammerRotateBackTween;

    public Vector3 startEulerAngles;
    void Start()
    {
        Cursor.visible = false;
        this.cursorSpriteRenderer.sprite = handSprite;
        startEulerAngles = cursorSpriteRenderer.transform.eulerAngles;
    }

    void Update()
    {
        this.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector2(-transform.lossyScale.x, transform.lossyScale.y);
    }
    public void SetCursorToHammer()
    {
        this.cursorSpriteRenderer.sprite = hammerSprite;
    }
    public void SetCursorToHand()
    {
        this.cursorSpriteRenderer.sprite = handSprite;
    }

    public void HitWithHammer()
    {
        if(hammerRotateBackTween != null)
        {
            hammerRotateBackTween.Kill();
            cursorSpriteRenderer.transform.eulerAngles = startEulerAngles;
        }
        hammerRotateBackTween = cursorSpriteRenderer.transform.DORotate(cursorSpriteRenderer.transform.eulerAngles - new Vector3(0, 0, 45),0.2f);
        hammerRotateBackTween.onComplete += () =>
        {
            cursorSpriteRenderer.transform.DORotate(startEulerAngles, 0.1f);
        };

    }



}