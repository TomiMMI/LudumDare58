using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public GeodeSO basicGeodeSO;
    public static PlayerStats Instance { get; private set; }


    //Gems that are already in the inventory 
    public Dictionary<GemInfos, Gem> CurrentGems = new Dictionary<GemInfos, Gem>();
    //Add gemInfos in here 
    public List<GemInfos> GemsInInventory = new List<GemInfos>();


    public List<GeodeInfos> Geodes = new List<GeodeInfos>();
    public Dictionary<GeodeType, int> GeodeCounts = new()
    {
        { GeodeType.Pebble,0 },
        { GeodeType.Mid,0 },
        { GeodeType.Colorful,0 },
        { GeodeType.Gigageode,0 },
    };
    [SerializeField]
    private int hammerForce = 1;

    public int HammerForce => hammerForce;

   

    public void AddGeodeInfo(GeodeInfos geodeInfos)
    {
        Geodes.Add(geodeInfos);
        CountGeodes();
    }

    public void RemoveGeodeInfo(GeodeInfos geodeInfos)
    {
        if (Geodes.Contains(geodeInfos))
        {
            Geodes.Remove(geodeInfos);
        }
        CountGeodes();
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
    void Awake()
    {
        AddGeodeInfo(new GeodeInfos(basicGeodeSO));
        CountGeodes();
        PlayerStats.Instance = this;
    }
}