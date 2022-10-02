using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour
{
    public UnityEvent buttonEvents;

    private void OnMouseDown()
    {
        Debug.Log("Clicked button");

        buttonEvents?.Invoke();
    }
}
