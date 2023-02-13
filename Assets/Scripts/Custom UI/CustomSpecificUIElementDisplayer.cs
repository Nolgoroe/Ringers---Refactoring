using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomSpecificUIElementDisplayer : BasicUIElement
{    //specific ui elements are what we call the specific elements

    public override void OverrideSetMyElement(string[] texts, Sprite[] sprites, System.Action[] actions)
    {
        base.SetMyElement(texts, sprites);
    }

}
