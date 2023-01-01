using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_Item : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerClickHandler ,ISelectHandler
{
    public Image itemImage;
    public TextMeshProUGUI itemCount;
    public GameObject counter;
    public BaseItem itemInfo;
    public UI_ItemSlot parentSlot;
    int count;
    RectTransform rectTransform;
    Canvas canvas;
    CanvasGroup canvasGroup;
    

    public event Action<UI_Item> OnItemLeftClick, OnItemRightClick, OnItemBeginDrag;
    public event Action<RectTransform> OnUIItemSelect;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void SetItemCounter(int counterValue)
    {
        count = counterValue;
        itemCount.text = count.ToString();
        ShowCounter();
    }

    public void ChangeCounter(int amount)
    {
        count += amount;
        itemCount.text = count.ToString();
        ShowCounter();
    }

    private void ShowCounter()
    {
        if (count <= 1)
            counter.SetActive(false);
        else
            counter.SetActive(true);
    }

    public void SetItem(BaseItem itemToSet)
    {
        itemImage.sprite = itemToSet.values.itemIcone;
        SetItemCounter(1);
        itemInfo = itemToSet;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        parentSlot.OnDrop(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnItemRightClick?.Invoke(this);
        }
        else
        {
            OnItemLeftClick?.Invoke(this);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!itemInfo.values.isEquipped)
            OnUIItemSelect?.Invoke(GetComponent<RectTransform>());
    }
}
