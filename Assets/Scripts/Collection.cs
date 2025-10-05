using UnityEngine;
using System;
using System.Collections.Generic;

public class Collection : MonoBehaviour
{
    [SerializeField]
    private GameObject gemSpritePrefab;
    private List<GameObject> collectionAssets = new List<GameObject>();

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
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RevealGem(1);
        }
    }
    public void RevealGem(int gemIndex)
    {
        collectionAssets[gemIndex].GetComponent<GemImage>().Discover();
    }

}