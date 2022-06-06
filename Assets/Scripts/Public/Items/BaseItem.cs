using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseItem : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite itemIcone;
    
    public abstract ItemTypeEnum GetItemType();
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
