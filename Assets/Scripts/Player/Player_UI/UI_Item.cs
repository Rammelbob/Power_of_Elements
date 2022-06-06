using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class UI_Item : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler,IDropHandler
{
    public Image itemImage;
    public TextMeshProUGUI itemCount;
    public GameObject counter;
    public BaseItem itemInfo;
    public UI_ItemSlot parentSlot;
    public bool isEquiped;
    int count;
    RectTransform rectTransform;
    Canvas canvas;
    CanvasGroup canvasGroup;

    public event Action<UI_Item> OnItemLeftClick, OnItemRightClick, OnItemBeginDrag;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.7f;
    }

    private void SetItemCounter(int counterValue)
    {
        count = counterValue;
        itemCount.text = count.ToString();
        ShowCounter();
    }

    public void IncreaseCounter(int amount)
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
        itemImage.sprite = itemToSet.itemIcone;
        SetItemCounter(1);
        itemInfo = itemToSet;
    }

    public void OnPointerEnter()
    {
        canvasGroup.alpha = 1f;
    }

    public void OnPointerExit()
    {
        canvasGroup.alpha = 0.6f;
    }
    public void OnPointerClick(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnItemRightClick?.Invoke(this);
        }
        else
        {
            OnItemLeftClick?.Invoke(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
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
}
