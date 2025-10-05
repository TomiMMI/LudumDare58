
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GemInventory : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_InventoryBackground;
    [SerializeField]
    private float m_margin;

    [SerializeField]
    private int m_maxPointCountY, m_maxPointCountX;

    [SerializeField]
    private Gem GemPrefab;

    private Dictionary<Gem, Vector3> takenIndexes = new Dictionary<Gem, Vector3>();
    private List<Vector3> m_InventoryPositions = new List<Vector3>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MakePointList();
    }

    public void MakePointList()
    {
        m_InventoryPositions.Clear();
        float intervalY = (m_InventoryBackground.bounds.size.y - m_margin / 2) / m_maxPointCountY;
        float intervalX = (m_InventoryBackground.bounds.size.x - m_margin / 2) / m_maxPointCountX;
        for (int y = 0; y < m_maxPointCountY; y++)
        {
            for (int x = 0; x < m_maxPointCountX; x++)
            {
                m_InventoryPositions.Add(new Vector3(m_InventoryBackground.bounds.min.x + x * intervalX + m_margin, m_InventoryBackground.bounds.min.y + intervalY * y + m_margin));
            }
        }
    }

    public Vector3? GetFreePosition()
    {
        List<Vector3> positions = m_InventoryPositions.Except(takenIndexes.Values).ToList();
        if (positions.Count() == 0)
        {
            return null;
        }
        return positions[UnityEngine.Random.Range(0, positions.Count - 1)];
    }


    public Gem AddToInventory(GemInfos GemSO)
    {
        Vector3? vec = GetFreePosition();
        if(vec == null )
        {
            Debug.LogError("Inventory is full not adding the gem");
            return null;
        }
        //Create a gem prefab
        //Add it into the right inventory 
        Gem Gem = GameObject.Instantiate(GemPrefab, transform);
        Gem.transform.localScale = Vector3.one;
        Gem.InitializeGem(GemSO);
        //Set it above the inventory background sorting layer
        Gem.SpriteRenderer.sortingOrder = 21;
        //Preferably in a place far spaced from each others
        Gem.transform.position = (Vector3)vec;

        Action func = null;
        func = () =>
        {
            Vector3 pos = takenIndexes[Gem];
            takenIndexes.Remove(Gem);
            m_InventoryPositions.Add(pos);
            Gem.OnGemDestroyed -= func;
        };

        Gem.OnGemDestroyed += func;
        takenIndexes[Gem] = (Vector3)vec;

        return Gem;
    }
}
