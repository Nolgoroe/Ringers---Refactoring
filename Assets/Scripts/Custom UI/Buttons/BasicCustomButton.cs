using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicCustomButton : CustomButtonParent
{
    [SerializeField] private bool isUseOnce;
    public override void OnClickButton()
    {
        buttonEvents?.Invoke();

        buttonEventsInspector?.Invoke();

        if (isUseOnce)
        {
            DeactivateSpecificButton(this);
        }
    }

    public override void OverrideSetMyElement(string[] texts, Sprite[] sprites, System.Action[] actions)
    {
        base.SetMyElement(texts, sprites);
    }
}
