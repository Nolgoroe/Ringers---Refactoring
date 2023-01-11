using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class CustomButtonParent : BasicUIElement, IPointerDownHandler
{
    public UnityEvent buttonEvents;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(isInteractable)
        {
            OnClickButton();
        }
    }

    public abstract void OnClickButton();
}
