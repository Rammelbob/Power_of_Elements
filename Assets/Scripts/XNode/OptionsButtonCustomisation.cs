using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OptionsButtonCustomisation : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    public UnityEvent OnClick;

    public event Action<RectTransform> OnUIStatSelect;

    public void PrintDebug()
    {
        Debug.Log("Click");
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 0.9f, 0, 255);
        OnUIStatSelect?.Invoke(GetComponent<RectTransform>());

    }

    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 255);

    }
}