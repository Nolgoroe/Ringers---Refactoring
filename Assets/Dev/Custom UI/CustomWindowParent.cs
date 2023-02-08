using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public abstract class CustomWindowParent : BasicUIElement
{
    //ui window is what we call the general parent of the UI prefab

    public virtual void ResetAllButtonEvents()
    {
        foreach (CustomButtonParent button in getButtonRefrences)
        {
            button.buttonEvents = null;
        }
    }
    public virtual void ActivateSpecificButton(CustomButtonParent button)
    {
        button.isInteractable = true;
    }

    public abstract void OverrideResetAllButtonEvents();
}
