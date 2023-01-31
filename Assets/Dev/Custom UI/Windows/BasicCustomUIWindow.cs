using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicCustomUIWindow : CustomWindowParent
{
    public override void OverrideSetMe(string[] texts, Sprite[] sprites, Action[] actions)
    {
        base.SetMe(texts, sprites);

        if (buttonRefs.Length > 0 && actions != null)
        {
            ResetAllButtonEvents();

            for (int i = 0; i < buttonRefs.Length; i++)
            {
                buttonRefs[i].buttonEvents += actions[i];
                buttonRefs[i].isInteractable = true;
            }
        }
    }

    public override void OverrideResetAllButtonEvents()
    {
        base.ResetAllButtonEvents();
    }
}
