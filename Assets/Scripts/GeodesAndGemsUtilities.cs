using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GeodesAndGemsUtilities : MonoBehaviour
{
    public static GeodesAndGemsUtilities Instance { get; private set; }

    [SerializeField]
    private Dictionary<GemRequest, int> pebbleLootTable = new Dictionary<GemRequest, int>
    {
        {new GemRequest(GemRarity.Shit),939 },
        {new GemRequest(GemRarity.Colorful,GemType.Triangle),50},
        {new GemRequest(GemRarity.Epic),10},
        {new GemRequest(GemRarity.Legendary),1}
    };
    
        //TO IMPROVE
    [SerializeField]
    private Dictionary<GemRequest, int> midLootTable = new Dictionary<GemRequest, int>
    {
        {new GemRequest(GemRarity.Epic),939 },
        {new GemRequest(GemRarity.Colorful,GemType.Triangle),50},
    };


    private Dictionary<GemRequest, int> colorfulLootTable = new Dictionary<GemRequest, int>
    {
        {new GemRequest(GemRarity.Colorful,GemType.Triangle),400 },
        {new GemRequest(GemRarity.Colorful,GemType.Square),300},
        {new GemRequest(GemRarity.Colorful,GemType.Diamond),100},
        {new GemRequest(GemRarity.Shit),140},
        {new GemRequest(GemRarity.Legendary),10}
    };

    private Dictionary<GemRequest, int> gigageodeLootTable = new Dictionary<GemRequest, int>
    {
        {new GemRequest(GemRarity.Colorful,GemType.Diamond),430 },
        {new GemRequest(GemRarity.Epic),430},
        {new GemRequest(GemRarity.Legendary),140}
    };

    [SerializeField]
    private List<GemSO> gemList;

    public List<GemSO> GemList => gemList;


    void Awake()
    {
        GeodesAndGemsUtilities.Instance = this;
    }
    private void Start()
    {
        gemList = Resources.LoadAll<GemSO>("Scriptables/Gems").ToList();
    }

    public GemSO GetGemFromGeode(GeodeType geodeType)
    {
        return GetGemFromGemRequest(GetGemRequestFromLootTable(geodeType));
    }

    private GemSO GetGemFromGemRequest(GemRequest gemRequest)
    {

        List<GemSO> temp = new List<GemSO>();
        foreach (GemSO gem in this.gemList)
        {
            if (gem.gemRarity == gemRequest.rarity)
            {
                temp.Add(gem);
            }
        }
        if (gemRequest.type != null)
        {
            List<GemSO> temp2 = new List<GemSO>();
            foreach (GemSO gem in temp)
            {
                if (gem.gemType != gemRequest.type)
                {
                    temp2.Add(gem);
                }
            }
            if (temp2.Count > 0)
            {
                return temp2[UnityEngine.Random.Range(0, temp.Count)];
            }
        }
        return temp[UnityEngine.Random.Range(0, temp.Count)];
    }

    private GemRequest GetGemRequestFromLootTable(GeodeType type)
    {
        switch (type)
        {
            case GeodeType.Pebble:
                return GetGemRequest(pebbleLootTable);

            case GeodeType.Colorful:
                return GetGemRequest(colorfulLootTable);

            case GeodeType.Gigageode:
                return GetGemRequest(gigageodeLootTable);
            case GeodeType.Mid:
                return GetGemRequest(midLootTable);
        }
        return null;
    }

    private GemRequest GetGemRequest(Dictionary<GemRequest, int> lootTable)
    {
        int temp = UnityEngine.Random.Range(1, 1001);
        foreach (KeyValuePair<GemRequest, int> pair in lootTable)
        {
            if (temp <= pair.Value)
            {
                return pair.Key;
            }
            else
            {
                temp -= pair.Value;
            }
        }
        return null;
    }
}



public class GemRequest
{
    public GemRarity rarity;
    public GemType? type;

    public GemRequest(GemRarity rarity, GemType? type = null)
    {
        this.rarity = rarity;
        this.type = type;
    }
}
public enum GemRarity
{
    Shit,
    Colorful,
    Epic,
    Legendary

}
public enum GemType
{
    Square,
    Triangle,
    Diamond,

}
public enum GeodeType
{
    Pebble,
    Mid,
    Colorful,
    Gigageode
}
