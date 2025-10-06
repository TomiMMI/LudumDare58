using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collection : MonoBehaviour
{
    [SerializeField]
    private GameObject gemSpritePrefab;
    private List<GameObject> collectionAssets = new List<GameObject>();

    [SerializeField]
    private TMP_Text textContainer;
    public int foundGems = 0;

    private int numberofGems;

    void Start()
    {
        int temp = 0;
        foreach (GemSO gemSO in GeodesAndGemsUtilities.Instance.GemList)
        {
            gemSO.GemID = temp++;
            GameObject tempGO = Instantiate(gemSpritePrefab, this.transform);
            tempGO.GetComponent<GemImage>().SetVisual(gemSO);
            collectionAssets.Add(tempGO);
        }
        numberofGems = temp;
        textContainer.text = 0 + " / " + numberofGems;
    }
    private void Update()
    {
    }
    public void RevealGem(int gemIndex)
    {
        collectionAssets[gemIndex].GetComponent<GemImage>().Discover();
        foundGems++;
        textContainer.text = foundGems + " / " + numberofGems;
    }

}