using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WinLevelCustomWindow : BasicCustomUIWindow
{
    [SerializeField] private RectTransform FlowerRect;

    [SerializeField] private float timeToReveaFlowers;
    [SerializeField] private float timeToReveaButtons;

    [SerializeField] private CanvasGroup[] canvasGroups;
    public override void OverrideSetMe(string[] texts, Sprite[] sprites, Action[] actions)
    {
        foreach (var group in canvasGroups)
        {
            group.alpha = 0;
        }

        base.SetMe(texts, sprites);

        if (buttonRefs.Length > 0)
        {
            ResetAllButtonEvents();

            for (int i = 0; i < buttonRefs.Length; i++)
            {
                buttonRefs[i].buttonEvents += actions[i];
                buttonRefs[i].isInteractable = false;
            }
        }
    }


    private void OnEnable()
    {
        FlowerRect.sizeDelta = new Vector2(0, 0);

        LeanTween.value(FlowerRect.sizeDelta.y, 1000, timeToReveaFlowers).setOnUpdate((float val) =>
        {
            FlowerRect.sizeDelta = new Vector2(FlowerRect.sizeDelta.x, val);
        });

        LeanTween.value(FlowerRect.sizeDelta.x, 1000, timeToReveaFlowers).setOnUpdate((float val) =>
        {
            FlowerRect.sizeDelta = new Vector2(val, FlowerRect.sizeDelta.y);
        });

        // next level button
        // only appears if we have a next level to move to
        // we don't have a next level if we're a chest level - the hud button will appear after chest is shown
        if(GameManager.instance.nextLevel != null)
        {
            if (buttonRefs[0] != null && canvasGroups[0] != null)
            {
                GeneralFloatValueTo(
                buttonRefs[0].gameObject,
                0,
                1,
                timeToReveaButtons,
                LeanTweenType.linear,
                canvasGroups[0],
                () => ActivateButton(buttonRefs[0]));
            }

            // to map button
            // always appears, though sometimes after chest. if we are a chest level, chest manages showing the hud button
            if (buttonRefs[1] != null && canvasGroups[1] != null)
            {
                GeneralFloatValueTo(
                buttonRefs[1].gameObject,
                0,
                1,
                timeToReveaButtons,
                LeanTweenType.linear,
                canvasGroups[1],
                () => ActivateButton(buttonRefs[1]));
            }
        }

    }

    private void ActivateButton(CustomButtonParent button)
    {
        button.isInteractable = true;
    }

    public void ManuallyShowToHudButton()
    {
        if (buttonRefs[1] != null && canvasGroups[1] != null)
        {
            GeneralFloatValueTo(
            buttonRefs[1].gameObject,
            0,
            1,
            timeToReveaButtons,
            LeanTweenType.linear,
            canvasGroups[1],
            () => ActivateButton(buttonRefs[1]));
        }
    }
}
