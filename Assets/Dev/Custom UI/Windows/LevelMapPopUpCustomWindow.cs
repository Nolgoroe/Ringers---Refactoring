using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class LevelMapPopUpCustomWindow : CustomWindowParent
{
    //private void Start()
    //{
    //    gameObject.SetActive(false);
    //}

    public override void OverrideSetMe(string[] texts, Sprite[] sprites, System.Action[] actions)
    {
        base.SetMe(texts, sprites);

        if (buttonRefs.Length > 0)
        {
            ResetAllButtonEvents();

            for (int i = 0; i < buttonRefs.Length; i++)
            {
                buttonRefs[i].buttonEvents += actions[i];
            }
        }
    }

    public override void OverrideResetAllButtonEvents()
    {
        base.ResetAllButtonEvents();
    }
}
