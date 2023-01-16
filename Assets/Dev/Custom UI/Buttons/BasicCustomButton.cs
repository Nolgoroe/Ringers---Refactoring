using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicCustomButton : CustomButtonParent
{
    public override void OnClickButton()
    {
        buttonEventsInspector?.Invoke();
        buttonEvents?.Invoke();
    }

    public override void OverrideSetMe(string[] texts, Sprite[] sprites, System.Action[] actions)
    {
        base.SetMe(texts, sprites);
    }
}
