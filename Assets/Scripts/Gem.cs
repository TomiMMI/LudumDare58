using System;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public event Action OnGemDestroyed = delegate { };
    public GemInfos gemInfos;
    public GemSO defaultGemSO;

    public SpriteRenderer SpriteRenderer;
    private Rigidbody2D m_Rigidbody;
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        //gemInfos = new GemInfos(defaultGemSO);
        //InitializeGem(gemInfos);
    }
    private void Start()
    {
        m_Rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }
    public void InitializeGem(GemInfos gemInfos)
    {
        this.gemInfos = gemInfos;
        SpriteRenderer.sprite = gemInfos.GemSO.gemSprite;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Contains("Bag"))
        {
            //Put the sorting layer under the hands
            SpriteRenderer.sortingOrder = 8;
        }
    }
    public void Disable()
    {
        m_Rigidbody.simulated = false;
    }

    public void Enable()
    {
        m_Rigidbody.simulated = true;
    }
    private void OnDestroy()
    {
        OnGemDestroyed();
    }
}
