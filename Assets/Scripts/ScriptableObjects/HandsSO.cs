using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HandsSO", menuName = "Scriptable Objects/HandsSO")]
public class HandsSO : ScriptableObject
{
    //Unused?
    public string Name;
    public Sprite SpriteClosed;
    public Sprite SpriteOpened;

    public List<GemSO> wantedGems;

    public GeodeSO DroppedGeode;

    public float richesMultiplier = 1;

    public float handRarity = 1;
    public AudioClip aClip;
}
