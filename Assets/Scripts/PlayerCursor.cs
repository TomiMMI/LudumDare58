using DG.Tweening;
using UnityEngine;

public enum CursorMode
{
    Hammer,
    Classic
}
public class PlayerCursor : MonoBehaviour
{
    CursorMode cursorMode = CursorMode.Classic;
    public static PlayerCursor Instance { get; private set; }

    void Awake()
    {
        PlayerCursor.Instance = this;
    }

    [SerializeField]
    private SpriteRenderer cursorSpriteRenderer;
    [SerializeField]
    private SpriteRenderer handSpriteRenderer;

    [SerializeField]
    private UnityEngine.Sprite handClosedSprite;
    [SerializeField]
    private UnityEngine.Sprite handOpenedSprite;
    [SerializeField]
    private UnityEngine.Sprite hammerSprite;

    public Tween hammerRotateTween;
    public Tween hammerRotateBackTween;

    public Vector3 startEulerAngles;
    public Vector2 offset;
    void Start()
    {
        Cursor.visible = false;
        startEulerAngles = cursorSpriteRenderer.transform.eulerAngles;
        SetCursorToHand();
    }

    void LateUpdate()
    {
        this.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
        if (cursorMode == CursorMode.Classic)
        {
            if (Input.GetMouseButtonDown(0))
            {
                handSpriteRenderer.sprite = handClosedSprite;
            }
            if (Input.GetMouseButtonUp(0))
            {
                handSpriteRenderer.sprite = handOpenedSprite;
            }
        }
    }
    public void SetCursorToHammer()
    {
        handSpriteRenderer.gameObject.SetActive(false);
        this.cursorSpriteRenderer.gameObject.SetActive(true);

        cursorMode = CursorMode.Hammer;
        this.cursorSpriteRenderer.sprite = hammerSprite;
    }

    public void SetCursorToHand()
    {
        cursorMode = CursorMode.Classic;
        handSpriteRenderer.gameObject.SetActive(true);
        handSpriteRenderer.sprite = handOpenedSprite;

        this.cursorSpriteRenderer.gameObject.SetActive(false);
    }


    public void HitWithHammer()
    {
        if (hammerRotateBackTween != null)
        {
            hammerRotateBackTween.Kill();
            cursorSpriteRenderer.transform.eulerAngles = startEulerAngles;
        }
        hammerRotateBackTween = cursorSpriteRenderer.transform.DORotate(cursorSpriteRenderer.transform.eulerAngles + new Vector3(0, 0, 75), 0.1f);
        hammerRotateBackTween.onComplete += () =>
        {
            cursorSpriteRenderer.transform.DORotate(startEulerAngles, 0.15f);
        };

    }



}