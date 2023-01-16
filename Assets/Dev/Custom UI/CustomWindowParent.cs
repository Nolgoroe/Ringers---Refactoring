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
        foreach (CustomButtonParent button in buttonRefs)
        {
            button.buttonEvents = null;
        }
    }

    public abstract void OverrideResetAllButtonEvents();
}
