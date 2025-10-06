using DG.Tweening;
using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Tutoriel : MonoBehaviour
{
    public Transform BreakGeodes;
    public Transform CollectGems;
    public Transform SellGems;
    public Transform GiveGems;
    public Transform Wishlist;
    public static Tutoriel Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Tutoriel.Instance = this;
    
        HideAll();
    }

    public void HideAll()
    {
        HideTransform(BreakGeodes);
        HideTransform(CollectGems);
        HideTransform(SellGems);
        HideTransform(Wishlist);
        HideTransform(GiveGems);
    }
    public void HideTransform(Transform t)
    {
        foreach (Transform t2 in t)
        {
            t2.gameObject.SetActive(false);
        }
    }

    IEnumerator ShowLinkedMessages(Transform t)
    {

        foreach (Transform text in t)
        {
            text.gameObject.SetActive(true);
            text.DOShakePosition(0.1f, new Vector3(0.1f, 0.1f));
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2);
        
        foreach (Transform text in t)
        {
            text.gameObject.SetActive(false);
            text.DOShakePosition(0.1f, new Vector3(0.1f, 0.1f));
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void TutoBreakGeodes()
    {
        StartCoroutine(ShowLinkedMessages(BreakGeodes));
    }

    public void TutoCollectGems()
    {
        StartCoroutine(ShowLinkedMessages(CollectGems));

    }

    public void TutoSellGems()
    {
        StartCoroutine(ShowLinkedMessages(SellGems));

    }

    public void TutoGiveGems()
    {
        StartCoroutine(ShowLinkedMessages(GiveGems));

    }

    public void TutoWishlist()
    {
        StartCoroutine(ShowLinkedMessages(Wishlist));

    }

}
