using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GDirection
{
    UP, DOWN, LEFT, RIGHT
}
public class SpawnLoop : MonoBehaviour
{
    /// <summary>
    /// Where we will instantiate all of our hands
    /// </summary>

    [SerializeField]
    private Transform m_HandsTransform;

    [SerializeField]
    public GameObject HandPrefab;

    /// <summary>
    /// We keep track of all of our hands in case there are too many on screen
    /// </summary>
    [SerializeField]
    private List<Main> m_Hands;

    [SerializeField]
    private SpriteRenderer m_TableSpriteRenderer;

    /// <summary>
    /// This is used to determine if the hand should be rotated to the left.
    /// </summary>
    [SerializeField]
    private Transform m_LeftSideTranform;
    public float TableMargin = 2;
    public float HandSize = 5;

    public int MaxHandsAtOnce = 10;
    private int currentHands = 0;

    private List<Vector3> m_HandPositions = new List<Vector3>();
    [SerializeField]
    private int maxPointCountX = 3;
    [SerializeField]
    private int maxPointCountY = 3;


    /// <summary>
    /// Time values 
    /// </summary>
    public float StartOfDay = 6;
    public float EndOfDay = 18;
    public float CurrentTimeLerped;
    private float m_timer;
    private float m_lerpValue;
    [SerializeField]
    private float GameDuration;
    [SerializeField]
    public float m_MaxHandSpawnSpeed = 5f;
    [SerializeField]
    public float m_MinHandSpawnSpeed = 0.2f;
    //This handles the hands spawn rate. Low at first, then ramps up to be very fast !
    public AnimationCurve m_SpawnCurve;

    public List<HandsSO> hands;
    public List<float> handWeights;
    public bool shouldReset = false;
    public float maxRarity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        float sum = 0;
        hands = Resources.LoadAll<HandsSO>("Scriptables/Hands").ToList();
        handWeights = new List<float>();
        maxRarity = 0;
        foreach (HandsSO hand in hands)
        {
            maxRarity += hand.handRarity;
            handWeights.Add(maxRarity);
        }
        Debug.Log("MAX RARITY IS " + maxRarity);
        MakePointList();
        StartCoroutine(corSpawnPiecesDebug());
        ResetDay();
    }

    // Update is called once per frame
    void Update()
    {
        DayUpdate();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Time.timeScale = 5;
            Debug.Log("Time scale is " + Time.timeScale);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Time.timeScale = 0.2f;
            Debug.Log("Time scale is " + Time.timeScale);
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Time.timeScale = 0;
            Debug.Log("Time scale is " + Time.timeScale);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Time.timeScale = 1;
            Debug.Log("Time scale is " + Time.timeScale);
        }
#endif
    }
    public void ResetDay()
    {
        m_timer = 0;
        m_lerpValue = 0;
        shouldReset = false;
    }

    public void DayUpdate()
    {
        m_timer += Time.deltaTime;
        m_lerpValue = m_timer / GameDuration;
        CurrentTimeLerped = Mathf.Lerp(StartOfDay, EndOfDay, m_lerpValue);
        if (m_lerpValue >= 1)
        {
            shouldReset = true;
        }
    }
    public float GetLerpedTime()
    {
        return m_lerpValue;
    }

    /// <summary>
    /// Get a point in the table, with a gap to not have them right on the edge
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="margin"></param>
    /// <returns></returns>
    public Vector3 randomPointInTable(Bounds bounds, float margin)
    {
        return new Vector3(UnityEngine.Random.Range(bounds.min.x + margin, bounds.max.x - margin), UnityEngine.Random.Range(bounds.min.y + margin, bounds.max.y - margin), 0);
    }
    public void MakePointList()
    {
        float intervalY = (m_TableSpriteRenderer.bounds.size.y - TableMargin / 2) / maxPointCountY;
        float intervalX = (m_TableSpriteRenderer.bounds.size.x - TableMargin / 2) / maxPointCountX;
        for (int y = 0; y < maxPointCountY; y++)
        {
            for (int x = 0; x < maxPointCountX; x++)
            {
                if (y == 0 || y == maxPointCountY - 1 || x == 0)
                {
                    m_HandPositions.Add(new Vector3(m_TableSpriteRenderer.bounds.min.x + x * intervalX + TableMargin, m_TableSpriteRenderer.bounds.min.y + intervalY * y + TableMargin));
                }
            }
        }
    }

    public IEnumerator corSpawnPiecesDebug()
    {
        int i = UnityEngine.Random.Range(0, m_HandPositions.Count - 1);
        while (true)
        {
            float spawnSpeed = Mathf.Lerp(m_MaxHandSpawnSpeed, m_MinHandSpawnSpeed, m_SpawnCurve.Evaluate(m_lerpValue));
            Debug.Log(spawnSpeed + " " + m_SpawnCurve.Evaluate(m_lerpValue));
            yield return new WaitForSeconds(spawnSpeed);
            if (currentHands >= MaxHandsAtOnce)
            {
                Debug.Log("Too much hands !");
                continue;
            }

            //Vector3 point = randomPointInTable(m_TableSpriteRenderer.bounds, TableMargin);
            Vector3 point = m_HandPositions[i];
            //Get a point in the sprite;
            Main hand = GameObject.Instantiate(HandPrefab, m_HandsTransform).GetComponent<Main>();
            if (point.y > 0)
            {
                hand.transform.localEulerAngles = new Vector3(0, 0, 180);
            }
            if (point.x < m_LeftSideTranform.localPosition.x)
            {
                hand.transform.localEulerAngles = new Vector3(0, 0, 270);
            }
            hand.transform.position = point;
            hand.transform.localPosition -= hand.transform.up * HandSize;
            hand.transform.localScale = Vector3.one;
            float speed = UnityEngine.Random.Range(0.2f, 1);

            //CHOOSE THE HAND SCRIPTABLE OBJECT
            float randomRarity = UnityEngine.Random.Range(0, maxRarity);
            HandsSO handsSO = null;
            for (int j = 0; j < hands.Count - 1; j++)
            {
                if (handWeights[j] <= randomRarity && handWeights[j + 1] >= randomRarity)
                {
                    handsSO = hands[j];
                    break;
                }
            }
            if (handsSO == null)
            {
                Debug.LogError("COULDN'T FIND A VALID HAND YOUR RANDOM IS FUCKED UP");
                handsSO = hands[0];
            }
            hand.Initialize(point, handsSO);
            currentHands++;

            Action func = null;
            func = () =>
            {
                currentHands--;
                hand.OnHandDestroyed -= func;
            };
            hand.OnHandDestroyed += () =>
            {
                func();
            };
            i = (i + 3) % m_HandPositions.Count;
            //Destroy(hand.gameObject);

        }
    }
}
