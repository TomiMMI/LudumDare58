using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int money = 0;
    public event Action OnInventoryUpdated = delegate { };
    public GeodeSO basicGeodeSO;
    public static PlayerStats Instance { get; private set; }

    [SerializeField]
    private Collection collection;

    [SerializeField]
    private GameObject suicididalTextPrefab;

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
    public List<GeodeType> DiscoveredGeodeTypes = new()
    {
        GeodeType.Pebble
    };

    [SerializeField]
    public int hammerForce = 1;

    public int HammerForce => hammerForce;


    public void AddMoney(int moneyCount)
    {
        money += moneyCount;
        OnInventoryUpdated();
    }

    public bool RemoveMoney(int moneyCount)
    {
        if (money < moneyCount)
        {
            return false;
        }
        money -= moneyCount;
        OnInventoryUpdated();
        return true;
    }

    public float GrinderGrindSpeed => grinderGrindSpeed;
    private float grinderGrindSpeed = 1;

    private List<GemSO> gemCollection = new List<GemSO>();


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

        foreach (GeodeInfos geode in Geodes)
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
            //Todo : Drag to left of the screen
        }
    }
    public GeodeInfos GetGeodeFromType(GeodeType type)
    {
        return Geodes.FirstOrDefault(x => x.GemSO.GeodeType == type);
    }
    public void SpawnSuicidalTextAtLocation(Vector3 position, String text, Vector2 movementVector, Vector2 sizeVector, float time)
    {
        Instantiate(suicididalTextPrefab, position, suicididalTextPrefab.transform.rotation).GetComponent<SuicidalText>().SetupText(text, movementVector, sizeVector, time);
    }
    public void SpawnSuicidalTextAtLocation(Vector3 position, String text, Vector2 movementVector, float time)
    {
        Instantiate(suicididalTextPrefab, position, suicididalTextPrefab.transform.rotation).GetComponent<SuicidalText>().SetupText(text, movementVector, time);
    }

    void Awake()
    {
        AddGeodeInfo(new GeodeInfos(basicGeodeSO));
        CountGeodes();
        PlayerStats.Instance = this;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnSuicidalTextAtLocation((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), "Test", new Vector2(0,0.5f),new Vector2(0.05f,0.05f), 1f);
        }
    }
}