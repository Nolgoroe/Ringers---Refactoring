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
        buttonEvents?.Invoke();
    }

    //called from button
    public void ActionsOnClickLevel ()
    {
        GameManager.instance.ClickOnLevelIconMapSetData(connectedLevelSO);
        UIManager.instance.DisplayLevelMapPopUp(connectedLevelSO);
    }

    public override void SetMe(string[] texts, Sprite[] sprites)
    {

    }
}
