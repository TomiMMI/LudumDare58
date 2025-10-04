using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    //Gems that are already in the inventory 
    public Dictionary<GemInfos, Gem> CurrentGems = new Dictionary<GemInfos, Gem>();
    //Add gemInfos in here 
    public List<GemInfos> GemsInInventory = new List<GemInfos>();




}

public class GemInfos
{
    public GemData gemData;
    public Guid guid;

    public GemInfos(GemData gemData)
    {
        this.gemData = gemData;
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
[CreateAssetMenu(menuName ="Data/GemData")]
public class GemData: ScriptableObject
{

    public Sprite GemSprite;
    public int GemValue;





}