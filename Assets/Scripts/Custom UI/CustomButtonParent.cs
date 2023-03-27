using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class CustomButtonParent : BasicUIElement/*, IPointerDownHandler*/, IPointerUpHandler, IPointerClickHandler
{
    public System.Action buttonEvents;
    public UnityEvent buttonEventsInspector;


    [SerializeField] protected bool isUseOnce;

    private void OnMouseDown()
    {
        if (isInteractable && !UIManager.IS_DURING_TRANSITION)
        {
            OnClickButton();
        }
    }

    public abstract void OnClickButton();

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isInteractable && !UIManager.IS_DURING_TRANSITION)
        {
            OnClickButton();
        }
    }

    public virtual void DeactivateSpecificButton(CustomButtonParent button)
    {
        button.isInteractable = false;
    }
}
