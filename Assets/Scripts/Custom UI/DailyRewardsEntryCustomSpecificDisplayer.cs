using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardsEntryCustomSpecificDisplayer : BasicUIElement
{
    [SerializeField] private GameObject CurrentRewardParent;
    [SerializeField] private float timeDeactivateTodayVFX;
    [SerializeField] private Image connectedImage;
    [SerializeField] private Animator connectedAnimator;

    public override void OverrideSetMyElement(string[] texts, Sprite[] sprites, Action[] actions)
    {
        base.SetMyElement(texts, sprites);
    }

    public void SetDisplayTodaysReward(bool isTodaysReward)
    {
        CurrentRewardParent.gameObject.SetActive(isTodaysReward);

        connectedAnimator.enabled = isTodaysReward;
    }

    public void SetAsAlreadyGiven()
    {
        connectedImage.color = new Color(connectedImage.color.r, connectedImage.color.g, connectedImage.color.b, 0.5f);
        connectedAnimator.enabled = false;

    }
    public IEnumerator AfterRecievedReward()
    {
        connectedAnimator.SetTrigger("Give Daily");
        yield return new WaitForSeconds(timeDeactivateTodayVFX);

        SetDisplayTodaysReward(false);
    }
}
