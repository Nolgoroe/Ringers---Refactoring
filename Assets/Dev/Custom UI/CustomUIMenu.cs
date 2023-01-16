using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomUIMenu : BasicUIElement
{
    //ui menu is what we call the sub groups that build up the main parent "window"

    public override void OverrideSetMe(string[] texts, Sprite[] sprites, System.Action[] actions)
    {
        base.SetMe(texts, sprites);
    }
}
