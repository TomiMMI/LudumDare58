using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class Main : MonoBehaviour
{
    public event Action OnHandDestroyed = delegate { };
    private Collider2D m_handCollider;
    private bool m_isActive = false;
    private bool m_hasGrabbed = false;
    private Vector3 m_startingPosition;
    private Vector3 m_destination;


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

    public HandsSO HandsSO;
    public List<Gem> GrabbedGems = new List<Gem>();
    public Geode ActiveGeode = null;

    public AudioSource audioSource;
    public AudioClip EatClip;
    void Start()
    {
    }
    public void Initialize(Vector3 destination, HandsSO handsSO)
    {
        m_speed = UnityEngine.Random.Range(m_minTimeHand, m_maxTimeHand);
        m_destination = destination;

        TravelToPosition(m_destination);
        StartCoroutine(UpdateLoop());
        m_spriteRenderer.sprite = handsSO.SpriteOpened;
        this.HandsSO = handsSO;
        audioSource.PlayOneShot(HandsSO.aClip);

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
                if (colliders.Any(x => x.name.Contains("Gem")))
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
        Tween ShakeTween = transform.DOShakePosition(0.1f, transform.right * 0.3f).SetLoops(-1);

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
            if (collider.GetComponent<Gem>() is Gem gem && gem != null)
            {
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                rb.simulated = false;
                rb.bodyType = RigidbodyType2D.Kinematic;
                collider.transform.parent = transform;
                GrabbedGems.Add(gem);
            }
        }
        audioSource.PlayOneShot(EatClip);
        //SCREEN SHAKE
        Camera.main.DOShakePosition(0.2f, 0.05f);
        m_spriteRenderer.transform.DOScale(Vector3.one * 1.15f, 0.2f);
        m_spriteRenderer.sprite = HandsSO.SpriteClosed;
        m_hasGrabbed = true;

        yield return new WaitForSeconds(m_waitBeforeLeaving);
        TravelOutOfScreen();
    }

    public bool HasCorrectGemType()
    {
        return GrabbedGems.Any(x => HandsSO.wantedGems.Contains(x.gemInfos.GemSO));
    }

    public float CalculateHandValue()
    {
        float value = 0;
        foreach (Gem gem in GrabbedGems)
        {
            value += gem.gemInfos.GemSO.gemValue * HandsSO.richesMultiplier;
        }
        return value;
    }

    public void SpawnLoot()
    {
        float riches = CalculateHandValue();
        if (HasCorrectGemType())
        {
            //Increase the geode size along with the riches multiplier

            Debug.Log("WE GOT A HUGE GEODE OF VALUE " + riches + " $$$");
            // INSTANTIATE THE GEODE
            //DROP IT ON THE TABLE 
            //DISABLE HAMMER 
            //SEND IT TO THE RIGHT
            GeodeInfos geodeInfos = new GeodeInfos(HandsSO.DroppedGeode);


            PlayerStats.Instance.AddGeodeInfo(geodeInfos);
            Geode geode = GameObject.Instantiate<Geode>(GeodeSpawner.Instance.GeodePrefab, transform);
            geode.GetComponent<Collider2D>().enabled = false;
            geode.InitializeGeode(geodeInfos);
            geode.transform.localScale = Vector3.one * 0.5f;
            geode.transform.DOScale(0.8f,0.1f);
            ActiveGeode = geode;

        }
        else
        {
            Debug.Log("WE GOT " + riches + " $$$");
            UIHandling.Instance.SpawnCoinPrefabAtPosition(transform.position, (int)riches);
        }

    }

    public Tween TravelToPosition(Vector3 position)
    {
        m_startingPosition = transform.position;
        Tween tweenMovement = transform.DOLocalMove(position, m_speed).SetEase(Ease.OutQuart);

        tweenMovement.onComplete += () =>
        {
            m_isActive = true;
        };
        return tweenMovement;
    }

    public void TravelOutOfScreen()
    {
        Tween tweenOutMovement = transform.DOMove(m_startingPosition, 0.2f).SetEase(Ease.InQuart);
        tweenOutMovement.onComplete += () =>
        {
            //THIS IS WHEN WE WILL SPAWN GEMS ON SCREEN
            StartCoroutine(DestroyHand());
        };
    }
    public IEnumerator DestroyHand()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (Gem gem in GrabbedGems)
        {
            gem.gameObject.SetActive(false);
        }

        yield return transform.DOLocalMove(m_destination, 0.2f).SetEase(Ease.OutQuart).WaitForCompletion();
        SpawnLoot();
        if (ActiveGeode != null)
        {
            
            ActiveGeode.transform.parent = null;
            //MOVE THE GEODE

            ActiveGeode.transform.DOMove(GeodeSpawner.Instance.geodeTypesTab.First(x => x.Value == ActiveGeode.GeodeInfos.GemSO.GeodeType).Key.transform.position, 1).SetDelay(1).onComplete += () => { Destroy(ActiveGeode.gameObject); };
            ActiveGeode.transform.DOScale(0.1f, 1).SetDelay(1);
        }
        m_isActive = true;
        yield return transform.DOMove(m_startingPosition, 0.2f).SetEase(Ease.InQuart).WaitForCompletion();

        OnHandDestroyed();
        Destroy(gameObject);
    }
}
