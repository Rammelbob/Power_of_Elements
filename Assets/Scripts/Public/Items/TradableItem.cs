using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "TradableItem")]
public class TradableItem : BaseItem
{
    public int currencyValue;

    public override ItemTypeEnum GetItemType()
    {
        return ItemTypeEnum.Currency;
    }

}
