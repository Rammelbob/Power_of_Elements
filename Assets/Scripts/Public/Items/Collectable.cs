using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private BaseItem collectableItem;
    public GameObject text;
    public Renderer rendererForColorChange;
    public float intensity;

    private void Start()
    {
        text.SetActive(false);
        SetCollectableItem(collectableItem);
    }

    public BaseItem GetCollectableItem()
    {
        return collectableItem;
    }

    public void SetCollectableItem(BaseItem baseItem)
    {
        collectableItem = baseItem;
        ChangeColorebasedOnItemType();
    }

    public void ChangeColorebasedOnItemType()
    {
       
        switch (collectableItem.GetItemType())
        {
            case ItemTypeEnum.Consumable:
                rendererForColorChange.material.SetColor("_EmissionColor", Color.green * intensity);
                break;
            case ItemTypeEnum.Currency:
                rendererForColorChange.material.SetColor("_EmissionColor", Color.yellow * intensity);
                break;
            case ItemTypeEnum.SkillPoint:
                rendererForColorChange.material.SetColor("_EmissionColor", Color.blue * intensity);
                break;
            default:
                rendererForColorChange.material.SetColor("_EmissionColor", Color.white * intensity);
                break;
           
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.SetActive(false);
        }
    }
}
