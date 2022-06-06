using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ConsumableItem")]
public class ConsumableItem : TradableItem
{
    public List<ItemStatCurrentChangeOverTime> statBuffs = new List<ItemStatCurrentChangeOverTime>();

    public override ItemTypeEnum GetItemType()
    {
        return ItemTypeEnum.Consumable;
    }
}
