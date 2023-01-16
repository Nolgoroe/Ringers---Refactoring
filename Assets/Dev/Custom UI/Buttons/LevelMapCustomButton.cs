using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEditor;

public class LevelMapCustomButton : CustomButtonParent
{
    [SerializeField] private LevelSO connectedLevelSO;

    public override void OnClickButton()
    {
        buttonEventsInspector?.Invoke();
    }

    //called from button
    public void ActionsOnClickLevel ()
    {
        GameManager.instance.ClickOnLevelIconMapSetData(connectedLevelSO);
        UIManager.instance.DisplayLaunchLevelPopUp(connectedLevelSO);
    }

    public override void OverrideSetMe(string[] texts, Sprite[] sprites, System.Action[] actions)
    {
        base.SetMe(texts, sprites);
    }
}
