using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomUIMenu : BasicUIElement
{
    //ui menu is what we call the sub groups that build up the main parent "window"

    public override void OverrideSetMyElement(string[] texts, Sprite[] sprites, System.Action[] actions)
    {
        base.SetMyElement(texts, sprites);
    }
}
