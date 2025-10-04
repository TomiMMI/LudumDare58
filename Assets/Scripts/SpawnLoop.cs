using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GDirection
{
    UP,DOWN,LEFT,RIGHT
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
    private  int currentHands = 0;

    private List<Vector3> m_HandPositions = new List<Vector3>();
    [SerializeField]
    private int maxPointCountX = 3;
    [SerializeField]
    private int maxPointCountY = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        MakePointList();
        StartCoroutine(corSpawnPiecesDebug());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnHand()
    {
        ///
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
        float intervalY =( m_TableSpriteRenderer.bounds.size.y - TableMargin/2) / maxPointCountY;
        float intervalX  = (m_TableSpriteRenderer.bounds.size.x- TableMargin/2) / maxPointCountX;
        for (int y = 0; y < maxPointCountY; y++)
        {
            for(int x = 0; x < maxPointCountX; x++)
            {
                m_HandPositions.Add(new Vector3(m_TableSpriteRenderer.bounds.min.x + x * intervalX + TableMargin, m_TableSpriteRenderer.bounds.min.y + intervalY * y + TableMargin));
            }
        }
    }

    public IEnumerator corSpawnPiecesDebug()
    {
        int i = UnityEngine.Random.Range(0,m_HandPositions.Count -1);
        while (true)
        {
            if(currentHands >= MaxHandsAtOnce)
            {
                Debug.Log("Too much hands !");
                yield return new WaitForSeconds(2);
                continue;
            }

            //Vector3 point = randomPointInTable(m_TableSpriteRenderer.bounds, TableMargin);
            Vector3 point = m_HandPositions[i];
            //Get a point in the sprite;
            Main hand = GameObject.Instantiate(HandPrefab, m_HandsTransform).GetComponent<Main>();
            if(point.y > 0)
            {
                hand.transform.localEulerAngles = new Vector3(0, 0, 180);
            }
            if(point.x < m_LeftSideTranform.localPosition.x)
            {
                hand.transform.localEulerAngles = new Vector3(0, 0, 270);
            }
            hand.transform.position = point;
            hand.transform.localPosition -= hand.transform.up * HandSize;
            hand.transform.localScale = Vector3.one;
            float speed = UnityEngine.Random.Range(0.2f, 1);
            hand.Initialize(point);
            yield return new WaitForSeconds(5);
            currentHands++;

            Action func = null;
            func = () =>
            {
                currentHands -- ;
                hand.OnHandDestroyed -= func;
            };
            hand.OnHandDestroyed += () => {
                func();
            };
            i = (i + 3) % m_HandPositions.Count;
            //Destroy(hand.gameObject);

        }
    }
}
