using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CoinStack : MonoBehaviour
{
    public GameObject CoinPrefab;
    public int CoinValue = 0;
    public int newCoinAt = 5;
    public int maxToSpawn = 10;
    public List<GameObject> coins = new();

    public void Initialize(int coinValue)
    {
        CoinValue = coinValue;
        int coinsToSpawn = coinValue / newCoinAt + 1;
        for(int i = 0; i < coinsToSpawn; i++)
        {
            GameObject coin = GameObject.Instantiate(CoinPrefab, transform);
            coin.transform.localScale = Vector3.one;
            coin.transform.position += new Vector3(0,(i+1) * 0.1f,0);
            coin.GetComponent<SpriteRenderer>().sortingOrder = 1 + i+1;
            coins.Add(coin);
        }

        MoveToCoinPile();
    }

    public void MoveToCoinPile()
    {
        Transform coinPileTransform = UIHandling.Instance.CoinLocation;

        transform.DOMove(coinPileTransform.position, 1).SetDelay(2).onComplete += ()=> {

            PlayerStats.Instance.AddMoney(CoinValue);    
            Destroy(gameObject); 
        };
        transform.DOScale(0.3f, 0.7f).SetDelay(2.5f);

    }
}
