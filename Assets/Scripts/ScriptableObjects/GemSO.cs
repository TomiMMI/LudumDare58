using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Gem", menuName = "Scriptable Objects/Gem")]
public class GemSO : ScriptableObject
{
    public int GemID; 
    public string gemName;
    public Sprite gemSprite;
    public GemRarity gemRarity;
    public GemType gemType;
    
}

public class GemInfos
{
    public GemSO GemSO;
    public Guid guid;

    public GemInfos(GemSO GemSO)
    {
        this.GemSO = GemSO;
        guid = Guid.NewGuid();
        //Todo : play with size and shape a little bit!
    }

    // override object.Equals
    public override bool Equals(object obj)
    {

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return guid.Equals(guid);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return guid.GetHashCode();
    }

}
