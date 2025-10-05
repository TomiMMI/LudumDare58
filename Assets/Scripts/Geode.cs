using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using DG.Tweening;
using System;

public class Geode : MonoBehaviour
{
    public event Action OnGeodeDestroyed = delegate { };
    [SerializeField]
    private GeodeType geodeType;
    [SerializeField]
    private int hardness = 100;

    [SerializeField]
    private List<Sprite> geodeSprites;

    private SpriteRenderer m_spriteRenderer;
    private int[] hardnessThresholds;
     private Color[] colors = new Color[] { Color.magenta, Color.yellow, Color.red };
    private int hardnessState = 0;
    public GeodeInfos GeodeInfos;
    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        hardnessThresholds = new int[] { (int)hardness / 3 * 2, (int)hardness / 3, 0 };
            //this.GetComponent<SpriteRenderer>().sprite = geodeSprites[0];
    }

    public void InitializeGeode(GeodeInfos geodeInfos)
    {
        this.GeodeInfos = geodeInfos;
        geodeType = geodeInfos.GemSO.GeodeType;
        m_spriteRenderer.sprite = geodeInfos.GemSO.GeodeSprite;
    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        //Cursor Hammer Animation
        //this.transform.DOShakePosition(0.1f,transform.right*0.3f);
        hardness -= PlayerStats.Instance.HammerForce;
        this.transform.DOShakePosition(0.05f, transform.right * 0.1f, 1 );
        PlayerCursor.Instance.HitWithHammer();
        GeodeUpdate();
    }


    private void GeodeUpdate()
    {
        Debug.Log(hardness);
        if (hardness <= 0)
        {
            this.GetComponent<SpriteRenderer>().color = Color.red;
            this.GetComponent<Collider2D>().enabled = false;
            //TO DO : Spawn Gem
            GemSO GemSO = GeodesAndGemsUtilities.Instance.GetGemFromGeode(geodeType);
            PlayerStats.Instance.TryToAddToCollection(GemSO);
            UIHandling.Instance.CreateAndAddGemToBag(transform, GemSO);
            this.StartCoroutine(WaitAndDestroy());
            return;
        }
        if (hardness <= hardnessThresholds[hardnessState])
        {
            hardnessState++;
            //m_spriteRenderer.color = colors[hardnessState];
            this.transform.DOShakePosition(0.1f, transform.right * 0.3f, 1 * hardnessState);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            collision.gameObject.GetComponent<PlayerCursor>().SetCursorToHammer();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            collision.gameObject.GetComponent<PlayerCursor>().SetCursorToHand();
        }
    }
    public void DestroyGeode()
    {
        OnGeodeDestroyed();
        Destroy(gameObject);
    }
    private IEnumerator WaitAndDestroy()
    {

        PlayerStats.Instance.RemoveGeodeInfo(GeodeInfos);
        OnGeodeDestroyed();
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
    
}
