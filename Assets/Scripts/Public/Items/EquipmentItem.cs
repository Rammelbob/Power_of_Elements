using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "EquipmentItem")]
public class EquipmentItem : TradableItem
{
    public ItemTypeEnum itemType;
    public List<ItemStatLevelChange> statBuffs = new List<ItemStatLevelChange>();

    public override ItemTypeEnum GetItemType()
    {
        return itemType;
    }
}
