using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCustomUIWindow : CustomWindowParent
{
    public override void OverrideSetMe(string[] texts, Sprite[] sprites, Action[] actions)
    {
    }

    public override void OverrideResetAllButtonEvents()
    {
        base.ResetAllButtonEvents();
    }
}
