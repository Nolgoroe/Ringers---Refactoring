using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InLevelUICustomWindow : CustomWindowParent
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    public override void OverrideSetMe(string[] texts, Sprite[] sprites, System.Action[] actions)
    {
        base.SetMe(texts, sprites);


        if (buttonRefs.Length > 0)
        {
            ResetAllButtonEvents();

            for (int i = 0; i < buttonRefs.Length; i++)
            {
                if(i <= actions.Length - 1 && actions[i] != null)
                {
                    buttonRefs[i].buttonEvents += actions[i];
                }
            }
        }
    }

    public override void OverrideResetAllButtonEvents()
    {
        base.ResetAllButtonEvents();
    }

    public void OpenOptionsWindow()
    {
        // called from button
        UIManager.instance.DisplayInLevelSettings();
        Debug.Log("Open options window");
        
    }
}
