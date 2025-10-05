using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [SerializeField]
    private int hammerForce = 1;

    public int HammerForce => hammerForce;

    void Awake()
    {
        PlayerStats.Instance = this;
    }
}