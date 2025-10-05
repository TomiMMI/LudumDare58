using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeodeSpawner : MonoBehaviour
{

    public static GeodeSpawner Instance { get; private set; }

    private void Awake()
    {
        GeodeSpawner.Instance = this;
    }
    public GeodeSO GeodeBadSO;
    public GeodeSO GeodeMidSO;
    public GeodeSO GeodeColorSO;
    public GeodeSO GeodeLegendarySO;

    public Geode ActiveGeode;
    public Geode GeodePrefab;

    public PlayerStats player;

    public Toggle ActiveTab;

    public Toggle BasicTab;
    public Toggle MidTab;
    public Toggle ColorTab;
    public Toggle LegendaryTab;

    public TextMeshProUGUI BasicText;
    public TextMeshProUGUI MidText;
    public TextMeshProUGUI ColorText;
    public TextMeshProUGUI LegendaryText;

    public Dictionary<Toggle, GeodeType> geodeTypesTab;
    public List<Toggle> Tabs;

    public ToggleGroup TabGroup;
    public Transform GeodeBaseTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tabs = new()
        {
            BasicTab,
            MidTab,
            ColorTab,
            LegendaryTab,
        };
        foreach(Toggle tg in Tabs)
        {
            tg.onValueChanged.AddListener((state) =>
            {
                if (state)
                {
                    SwapTab(tg);
                }
            });
        }
        

        geodeTypesTab = new()
        {
            {BasicTab, GeodeType.Pebble },
            { MidTab, GeodeType.Mid},
            {ColorTab, GeodeType.Colorful },
            { LegendaryTab, GeodeType.Gigageode},

        };
        BasicTab.isOn = true;
        ActiveTab = BasicTab;
        SpawnGeode();
        UpdateTabInventory();
    }

    public void SwapTab(Toggle tg)
    {
        ActiveTab = tg;
        SpawnGeode();
        //PUT THAT AT A CORRECT PLACE
        UpdateTabInventory();
    }
    public void UpdateTabInventory()
    {
        foreach(var keyVar in player.GeodeCounts)
        {
            Toggle toggle = geodeTypesTab.First(x => x.Value == keyVar.Key).Key;
            if (keyVar.Value == 0)
            {
                toggle.interactable = false;
            }
            else
            {
                toggle.interactable = true;
            }
            GetText(keyVar.Key).text = keyVar.Value.ToString();
        }
    }

    public void SpawnGeode()
    {
        if(ActiveTab == null)
        {
            ActiveTab = BasicTab;
        }
        GeodeInfos geodeInfos =new GeodeInfos(GetGeodeSO(geodeTypesTab[ActiveTab]));

        if (ActiveGeode != null)
        {
            ActiveGeode.DestroyGeode();
        }
        ActiveGeode = GameObject.Instantiate<Geode>(GeodePrefab,GeodeBaseTransform);
        ActiveGeode.InitializeGeode(geodeInfos);

        Action func = null;
        func = () =>
        {
            UpdateTabInventory();
            ActiveGeode.OnGeodeDestroyed -= func;
        };
        ActiveGeode.OnGeodeDestroyed += func;
        ActiveGeode.transform.localScale = Vector3.one * geodeInfos.GeodeSizeMultiplier;
        ActiveGeode.transform.DOPunchScale(Vector3.one * 0.1f,0.5f,2); 

    }

    public GeodeSO GetGeodeSO(GeodeType type)
    {
        switch (type)
        {
            case GeodeType.Pebble:
                return GeodeBadSO;
            case GeodeType.Mid:
                return GeodeMidSO;
            case GeodeType.Colorful:
                return GeodeColorSO;
            case GeodeType.Gigageode:
                return GeodeLegendarySO;
        }
        return null;
    }

    public TextMeshProUGUI GetText(GeodeType geodeType)
    {
        if (geodeType == GeodeType.Pebble)
        {
            return BasicText;
        }
        if (geodeType == GeodeType.Mid)
        {
            return MidText;
        }
        if (geodeType == GeodeType.Colorful)
        {
            return ColorText;
        }
        if (geodeType == GeodeType.Gigageode)
        {
            return LegendaryText;
        }
        return null;
    }
}