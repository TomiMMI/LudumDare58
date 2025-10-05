using UnityEngine;

[CreateAssetMenu(fileName = "Gem", menuName = "Scriptable Objects/Gem")]
public class GemSO : ScriptableObject
{
    public string gemName;
    public Sprite gemSprite;
    public GemRarity gemRarity;
    public GemType gemType;
    
}
