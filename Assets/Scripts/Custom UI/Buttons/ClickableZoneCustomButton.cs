using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickableZoneCustomButton : CustomButtonParent
{
    [SerializeField] private BasicUIElement connectedParent;

    private void OnValidate()
    {
        if (transform.parent == null) return;
        transform.parent.TryGetComponent(out connectedParent);

        if (connectedParent == null)
        {
            Debug.LogError("Parent isn't a basic ui element!!!");
            return;
        }
    }

    public override void OnClickButton()
    {
        buttonEvents?.Invoke();
        buttonEventsInspector?.Invoke();
    }

    // called from button
    public void CallUICloseElement()
    {
        if (connectedParent == null)
        {
            Debug.LogError("Parent isn't a basic ui element!!!");
            return;
        }

        UIManager.instance.CloseElement(connectedParent);
    }

    public override void OverrideSetMyElement(string[] texts, Sprite[] sprites, System.Action[] actions = null)
    {
        base.SetMyElement(texts, sprites);
    }
}
