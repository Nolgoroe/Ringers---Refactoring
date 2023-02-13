using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCustomButton : CustomButtonParent
{
    public PowerupType connecetdScriptableObjectType; //why public?


    public override void OnClickButton()
    {
        buttonEvents?.Invoke();

        buttonEventsInspector?.Invoke();

        if (isUseOnce)
        {
            DeactivateSpecificButton(this);
        }
    }

    public override void OverrideSetMyElement(string[] texts, Sprite[] sprites, Action[] actions)
    {
        base.SetMyElement(texts, sprites);

        if (getButtonRefrences.Length > 0 && actions != null)
        {
            for (int i = 0; i < getButtonRefrences.Length; i++)
            {
                getButtonRefrences[i].buttonEvents += actions[i];
                getButtonRefrences[i].isInteractable = true;
            }
        }
    }
}