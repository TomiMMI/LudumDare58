using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class Geode : MonoBehaviour
{

    [SerializeField]
    private GeodeType geodeType;
    [SerializeField]
    private int hardness = 100;

    [SerializeField]
    private List<Sprite> geodeSprites;

    private int[] hardnessThresholds;
     private Color[] colors = new Color[] { Color.magenta, Color.yellow, Color.red };
    private int hardnessState = 0;
    void Start()
    {
        hardnessThresholds = new int[] { (int)hardness / 3 * 2, (int)hardness / 3, 0 };
            //this.GetComponent<SpriteRenderer>().sprite = geodeSprites[0];
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
        GeodeUpdateDEBUG();
    }

    private void GeodeUpdate()
    {
        if (hardness <= 0)
        {
            this.GetComponent<SpriteRenderer>().sprite = geodeSprites[geodeSprites.Count - 1];
            this.GetComponent<Collider2D>().enabled = false;
            //TO DO : Spawn Gem
        }
        if (hardness <= hardnessThresholds[hardnessState])
        {
            hardnessState++;
            this.GetComponent<SpriteRenderer>().sprite = geodeSprites[hardnessState];
            //this.transform.DOShakePosition(0.1f,transform.right*0.3f, 1 * hardnessState);
        }
    }
        private void GeodeUpdateDEBUG()
    {
        Debug.Log(hardness);
        if (hardness <= 0)
        {
            this.GetComponent<SpriteRenderer>().color = Color.red;
            this.GetComponent<Collider2D>().enabled = false;
            //TO DO : Spawn Gem
            this.StartCoroutine(WaitAndDestroy());
            return;
        }
        if (hardness <= hardnessThresholds[hardnessState])
        {
            hardnessState++;
            this.GetComponent<SpriteRenderer>().color = colors[hardnessState];
            //this.transform.DOShakePosition(0.1f,transform.right*0.3f, 1 * hardnessState);
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

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
    
}
