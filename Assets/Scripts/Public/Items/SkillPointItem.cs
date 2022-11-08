using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillPointItem")]
public class SkillPointItem : BaseItem
{
    public override ItemTypeEnum GetItemType()
    {
        return ItemTypeEnum.SkillPoint;
    }
}
