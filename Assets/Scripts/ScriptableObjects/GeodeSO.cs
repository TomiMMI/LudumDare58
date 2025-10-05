using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GeodeSo", menuName = "Scriptable Objects/GeodeSo")]
public class GeodeSO : ScriptableObject
{
    public Sprite GeodeSprite;
    public GeodeType GeodeType;

}

public class GeodeInfos
{
    public GeodeSO GemSO;
    public Guid guid;

    public float GeodeSizeMultiplier = 1; //The bigger it is the more loot there is

    public GeodeInfos(GeodeSO GemSO)
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
        GeodeInfos go = (GeodeInfos)obj;
        return go.guid.Equals(guid);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    { 
        return guid.GetHashCode();
    }

}