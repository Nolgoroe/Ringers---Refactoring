using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicCustomButton : CustomButtonParent
{
    [SerializeField] private bool isUseOnce;
    public override void OnClickButton()
    {
        buttonEventsInspector?.Invoke();
        buttonEvents?.Invoke();

        if (isUseOnce)
        {
            DeactivateSpecificButton(this);
        }
    }

    public override void OverrideSetMe(string[] texts, Sprite[] sprites, System.Action[] actions)
    {
        base.SetMe(texts, sprites);
    }
}
