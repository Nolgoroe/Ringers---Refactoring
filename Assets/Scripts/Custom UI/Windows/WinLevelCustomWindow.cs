using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WinLevelCustomWindow : BasicCustomUIWindow
{
    [SerializeField] private RectTransform FlowerRect;

    [SerializeField] private float timeToReveaFlowers;
    [SerializeField] private float timeToReveaButtons;

    [SerializeField] private BasicCustomButton nextLevelButton;
    [SerializeField] private BasicCustomButton toMapButton;

    private CanvasGroup nextLevelCanvasGroup;
    private CanvasGroup toMapCanvasGroup;
    public override void OverrideSetMyElement(string[] texts, Sprite[] sprites, Action[] actions)
    {

        base.SetMyElement(texts, sprites);

        if (getButtonRefrences.Length > 0)
        {
            ResetAllButtonEvents();

            for (int i = 0; i < getButtonRefrences.Length; i++)
            {
                getButtonRefrences[i].buttonEvents += actions[i];
                getButtonRefrences[i].isInteractable = false;
            }
        }

        nextLevelCanvasGroup = nextLevelButton.GetComponent<CanvasGroup>();
        toMapCanvasGroup = toMapButton.GetComponent<CanvasGroup>();

        nextLevelCanvasGroup.alpha = 0;
        toMapCanvasGroup.alpha = 0;
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
            if (nextLevelButton != null)
            {
                GeneralFloatValueTo(
                nextLevelButton.gameObject,
                0,
                1,
                timeToReveaButtons,
                LeanTweenType.linear,
                nextLevelCanvasGroup,
                () => ActivateButton(nextLevelButton));
            }

            // to map button
            // always appears, though sometimes after chest. if we are a chest level, chest manages showing the hud button
            ManuallyShowToHudButton();
        }
    }

    private void ActivateButton(CustomButtonParent button)
    {
        button.isInteractable = true;
    }

    public void ManuallyShowToHudButton()
    {
        if (toMapButton != null )
        {
            GeneralFloatValueTo(
            toMapButton.gameObject,
            0,
            1,
            timeToReveaButtons,
            LeanTweenType.linear,
            toMapCanvasGroup,
            () => ActivateButton(toMapButton));
        }
    }
}
