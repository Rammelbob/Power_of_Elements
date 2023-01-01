using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class BaseItem : ScriptableObject
{
    public BaseItemValues values;
    
    public abstract ItemTypeEnum GetItemType();
}


[Serializable]
public class BaseItemValues
{
    public string itemName;
    public string description;
    public Sprite itemIcone;
    public bool isEquipped;
}

[Serializable]
public class ItemStatLevelChange
{
    public StatEnum statType;
    public int amount;
}

[Serializable]
public class ItemStatCurrentChangeOverTime
{
    public StatEnum statType;
    public float amount;
    public float actionTime;
}
