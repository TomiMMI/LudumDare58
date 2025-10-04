using DG.Tweening;
using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class Main : MonoBehaviour
{
    public event Action OnHandDestroyed = delegate { };
    private Collider2D m_handCollider;
    private bool m_isActive = false;
    private bool m_hasGrabbed = false;
    private Vector3 m_startingPosition;
    private Vector3 m_destination;

    [SerializeField]
    private Sprite  m_spriteOpened;
    [SerializeField]
    private Sprite m_spriteClosed;

    [SerializeField]
    private SpriteRenderer m_spriteRenderer;
    private float m_speed;

    public float m_maxShakeStrength = 1f;
    [SerializeField]
    private float m_grabTimer = 2f;
    [SerializeField]
    private float m_waitBeforeLeaving = 1f;
    [SerializeField]
    private float m_minTimeHand, m_maxTimeHand;
    [SerializeField]
    private float m_grabRadius;
    void Start()
    {
    }
    public void Initialize(Vector3 destination)
    {
        m_speed = UnityEngine.Random.Range(m_minTimeHand, m_maxTimeHand);
        m_destination = destination;
        TravelToPosition(m_destination);
        StartCoroutine(UpdateLoop());
        m_spriteRenderer.sprite = m_spriteOpened;
    }

    void Update()
    {

    }
    public IEnumerator UpdateLoop()
    {
        while (true)
        {
            if (m_isActive)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_grabRadius);
                if(colliders.Any(x => x.name.Contains("Gem")))
                {
                    StartCoroutine(ActivateGrab());
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

    }
    public IEnumerator ActivateGrab()
    {
        m_isActive = false;
        //Shake 
        Tween ShakeTween = transform.DOShakePosition(0.1f,transform.right*0.3f).SetLoops(-1);

        yield return new WaitForSeconds(m_grabTimer);
        ShakeTween.Kill();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_grabRadius);
        if (!colliders.Any(x => x.name.Contains("Gem")))
        {
            m_isActive = true;
            yield break; // Cancel 
        }
        foreach (Collider2D collider in colliders)
        {
            if (collider.name.Contains("Gem"))
            {
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                rb.simulated = false;
                rb.bodyType = RigidbodyType2D.Kinematic;
                collider.transform.parent = transform;
            }
        }

        m_spriteRenderer.sprite = m_spriteClosed;
        m_hasGrabbed = true;
       
        yield return new WaitForSeconds(m_waitBeforeLeaving);
        TravelOutOfScreen();
    }
    public void TravelToPosition(Vector3 position)
    {
        m_startingPosition = transform.position;
        Tween tweenMovement = transform.DOLocalMove(position, m_speed).SetEase(Ease.OutQuart);

        tweenMovement.onComplete += () =>
        {
            m_isActive = true;
        };
    }

    public void TravelOutOfScreen()
    {
        Tween tweenOutMovement = transform.DOMove(m_startingPosition, m_speed).SetEase(Ease.OutQuart);
        tweenOutMovement.onComplete += () =>
        {
            //THIS IS WHEN WE WILL SPAWN GEMS ON SCREEN
            OnHandDestroyed();
            Destroy(gameObject);
        };
    }
}
