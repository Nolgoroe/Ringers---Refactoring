using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class CustomButtonParent : BasicUIElement/*, IPointerDownHandler*/, IPointerUpHandler, IPointerClickHandler
{
    public System.Action buttonEvents;
    public UnityEvent buttonEventsInspector;

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    //Debug.Log("Test");
    //    //if(isInteractable)
    //    //{
    //    //    OnClickButton();
    //    //}
    //}
    
    private void OnMouseDown()
    {
        if (isInteractable && !UIManager.ISDURINGFADE /*&& !UIManager.ISDURINGCHEST*/)
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
        //Debug.Log("Test");
        if (isInteractable && !UIManager.ISDURINGFADE /*&& !UIManager.ISDURINGCHEST*/)
        {
            OnClickButton();
        }

    }
}
