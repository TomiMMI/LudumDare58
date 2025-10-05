using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }


    //Gems that are already in the inventory 
    public Dictionary<GemInfos, Gem> CurrentGems = new Dictionary<GemInfos, Gem>();
    //Add gemInfos in here 
    public List<GemInfos> GemsInInventory = new List<GemInfos>();

    [SerializeField]
    private int hammerForce = 1;

    public int HammerForce => hammerForce;

    void Awake()
    {
        PlayerStats.Instance = this;
    }
}