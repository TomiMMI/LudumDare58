using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeodesAndGemsUtilities : MonoBehaviour
{
    public static GeodesAndGemsUtilities Instance { get; private set; }

    [SerializeField]
    private Dictionary<GemRequest, int> pebbleLootTable = new Dictionary<GemRequest, int>
    {
        {new GemRequest(GemRarity.Shit),850 },
        {new GemRequest(GemRarity.Colorful),150},
    };

    //TO IMPROVE
    [SerializeField]
    private Dictionary<GemRequest, int> midLootTable = new Dictionary<GemRequest, int>
    {
        {new GemRequest(GemRarity.Shit),300 },
        {new GemRequest(GemRarity.Colorful),700},
    };


    private Dictionary<GemRequest, int> colorfulLootTable = new Dictionary<GemRequest, int>
    {
        {new GemRequest(GemRarity.Epic),480 },
        {new GemRequest(GemRarity.Colorful),490},
        {new GemRequest(GemRarity.Legendary),30}
    };

    private Dictionary<GemRequest, int> gigageodeLootTable = new Dictionary<GemRequest, int>
    {
        {new GemRequest(GemRarity.Epic),330 },
        {new GemRequest(GemRarity.Legendary),440},
        {new GemRequest(GemRarity.Colorful),130},
        {new GemRequest(GemRarity.Shit),50}
    };

    [SerializeField]
    private List<GemSO> gemList;

    public List<GemSO> GemList => gemList;


    void Awake()
    {
        GeodesAndGemsUtilities.Instance = this;
        gemList = Resources.LoadAll<GemSO>("Scriptables/Gems").ToList().OrderBy(x => x.name).ToList();

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
            foreach (GemSO gem in temp.ToList())
            {
                if (gem.gemType != gemRequest.type)
                {
                    temp.Add(gem);
                }
            }
        }

        if (temp.Count > 0)
        {
            return temp[UnityEngine.Random.Range(0, temp.Count)];
        }
        //HANDLE THIS
        return null;
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
    public Color RarityColor(GemRarity rarity)
    {
        switch (rarity)
        {
            case GemRarity.Shit:
                return Color.grey;
            case GemRarity.Colorful:
                return Color.blue;
            case GemRarity.Epic:
                return Color.magenta;
            case GemRarity.Legendary:
                return Color.yellow;
        }
        return Color.black;
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
