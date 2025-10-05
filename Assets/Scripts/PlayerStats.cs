using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public event Action OnInventoryUpdated = delegate { };
    public GeodeSO basicGeodeSO;
    public static PlayerStats Instance { get; private set; }

    [SerializeField]
    private Collection collection;

    //Gems that are already in the inventory 
    public Dictionary<GemInfos, Gem> CurrentGems = new Dictionary<GemInfos, Gem>();
    //Add gemInfos in here 
    public List<GemInfos> GemsInInventory = new List<GemInfos>();

    private List<GemSO> gemCollection = new List<GemSO>();

    public List<GeodeInfos> Geodes = new List<GeodeInfos>();
    public Dictionary<GeodeType, int> GeodeCounts = new()
    {
        { GeodeType.Pebble,0 },
        { GeodeType.Mid,0 },
        { GeodeType.Colorful,0 },
        { GeodeType.Gigageode,0 },
    };

    public List<GeodeType> DiscoveredGeodeTypes = new()
    {
        GeodeType.Pebble
    };
    
    [SerializeField]
    private int hammerForce = 1;

    public int HammerForce => hammerForce;

   

    public void AddGeodeInfo(GeodeInfos geodeInfos)
    {
        if (!DiscoveredGeodeTypes.Contains(geodeInfos.GemSO.GeodeType))
        {
            DiscoveredGeodeTypes.Add(geodeInfos.GemSO.GeodeType);
        }
        Geodes.Add(geodeInfos);
        CountGeodes();
        OnInventoryUpdated();
    }

    public void RemoveGeodeInfo(GeodeInfos geodeInfos)
    {
        if (Geodes.Contains(geodeInfos))
        {
            Geodes.Remove(geodeInfos);
        }
        CountGeodes();
        OnInventoryUpdated();
    }

    public void CountGeodes()
    {

        for (int i = 0; i < GeodeCounts.Count; i++)
        {
            GeodeCounts[GeodeCounts.Keys.ToList()[i]] = 0;
        }

        foreach(GeodeInfos geode in Geodes)
        {
            GeodeCounts[geode.GemSO.GeodeType]++;
        }
    }

    public void TryToAddToCollection(GemSO gemSO)
    {
        if (!gemCollection.Contains(gemSO))
        {
            gemCollection.Add(gemSO);
            collection.RevealGem(gemSO.GemID);
            Debug.Log("New gem : " + gemSO.gemName);
            //Todo : Drag to collection UI
        }
        else
        {
            Debug.Log("Gem already existing : " + gemSO.gemName);
            GemsInInventory.Add(new GemInfos(gemSO));
            //Todo : Drag to left of the screen
        }
    }
    public GeodeInfos GetGeodeFromType(GeodeType type)
    {
        return Geodes.FirstOrDefault(x => x.GemSO.GeodeType == type);
    }

    void Awake()
    {
        AddGeodeInfo(new GeodeInfos(basicGeodeSO));
        CountGeodes();
        PlayerStats.Instance = this;
    }
}