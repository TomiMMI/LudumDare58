using System;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public event Action OnGemDestroyed = delegate { };
    public GemInfos gemInfos;

    public SpriteRenderer SpriteRenderer;
    private Rigidbody2D m_Rigidbody;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }
    public void InitializeGem(GemInfos gemInfos)
    {
        this.gemInfos = gemInfos;
        SpriteRenderer.sprite = gemInfos.gemData.GemSprite;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Contains("Background"))
        {
            //Put the sorting layer under the hands
            SpriteRenderer.sortingOrder = 9;
        }
    }

    private void OnDestroy()
    {
        OnGemDestroyed();
    }
}
