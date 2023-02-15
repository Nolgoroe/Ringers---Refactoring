using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMapPopupCustomWindow : CustomWindowParent
{
    public ChestBarLogic connectedChestBar;

    private void OnEnable()
    {
        if(GameManager.instance.currentCluster.isChestCluster)
        {
            connectedChestBar. gameObject.SetActive(true);
        }
        else
        {
            connectedChestBar.gameObject.SetActive(false);
        }
    }
    public override void OverrideResetAllButtonEvents()
    {
        base.ResetAllButtonEvents();
    }

    public override void OverrideSetMyElement(string[] texts, Sprite[] sprites, Action[] actions)
    {
        base.SetMyElement(texts, sprites);

        if (getButtonRefrences.Length > 0 && actions != null)
        {
            ResetAllButtonEvents();

            for (int i = 0; i < getButtonRefrences.Length; i++)
            {
                getButtonRefrences[i].buttonEvents += actions[i];
                getButtonRefrences[i].isInteractable = true;
            }
        }
    }
}