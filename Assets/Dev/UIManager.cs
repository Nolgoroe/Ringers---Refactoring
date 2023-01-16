using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class WorldDisplayCombo
{
    public WorldEnum world;
    public Sprite mainSprite;
    public Sprite leftMargineSprite;
    public Sprite rightMargineSprite;

}
[System.Serializable]
public class RefWorldDisplayCombo
{
    public Image mainImageRef;
    public Image leftMargineImageRef;
    public Image rightMargineImageRef;

}
public class UIManager : MonoBehaviour
{
    //public static bool ISUSINGUI;
    public static UIManager instance; //TEMP - LEARN DEPENDENCY INJECTION

    [SerializeField] private BasicUIElement currentlyOpenSoloElement;
    [SerializeField] private List<BasicUIElement> currentAdditiveScreens;
    [SerializeField] private List<BasicUIElement> currentPermanentScreens;


    [Header("Map setup")]
    [SerializeField] private WorldDisplayCombo[] orderOfWorlds;
    [SerializeField] private RefWorldDisplayCombo[] worldReferebceCombo;


    [Header("Map Screen")]
    public BasicUIElement levelScrollRect;
    public BasicUIElement levelMapPopUp;

    [Header("In level Screen")]
    public BasicUIElement inLevelUI;
    public BasicUIElement inLevelSettingsWindow;

    private void Start()
    {
        instance = this;

        AddAdditiveElement(levelScrollRect);
    }




    /**/
    // main ui manager actions.
    /**/
    private void OpenSolo(BasicUIElement UIElement)
    {
        if(currentlyOpenSoloElement == null)
        {
            // show solo screen - CLOSE ALL ADDITIVE SCREENS

            if(currentAdditiveScreens.Count > 0)
            {
                foreach (BasicUIElement element in currentAdditiveScreens)
                {
                    if(!element.isPermanent)
                    {
                        CloseElement(element);
                    }
                }
            }

            UIElement.gameObject.SetActive(true); // or Instantiate
            currentlyOpenSoloElement = UIElement;
        }
        else
        {
            if(UIElement.isOverrideSolo)
            {
                currentlyOpenSoloElement.gameObject.SetActive(false); // or derstroy!!!

                UIElement.gameObject.SetActive(true); // or Instantiate
                currentlyOpenSoloElement = UIElement;
            }
            else
            {
                Debug.Log("Tried to open a solo screen on top of another solo screen, but is not overriding.");
                // do nothing
            }
        }
    }
    private void AddAdditiveElement(BasicUIElement UIElement)
    {
        if(!currentAdditiveScreens.Contains(UIElement))
        {
            if(UIElement.isPermanent)
            {
                currentPermanentScreens.Add(UIElement);
            }
            else
            {
                currentAdditiveScreens.Add(UIElement);
            }

            UIElement.gameObject.SetActive(true); // or instantiate
        }
    }
    public void CloseElement(BasicUIElement UIElement)
    {
        //if (UIElement.isPermanent) return;

        if (UIElement.isSolo)
        {
            currentlyOpenSoloElement = null;
        }

        if (currentAdditiveScreens.Contains(UIElement))
        {
            currentAdditiveScreens.Remove(UIElement);
        }

        if (currentPermanentScreens.Contains(UIElement))
        {
            currentPermanentScreens.Remove(UIElement);
        }

        UIElement.gameObject.SetActive(false); //(OR destory it!!)
    }

    public void CloseAllCurrentScreens()
    {
        if(currentlyOpenSoloElement)
        {
            CloseElement(currentlyOpenSoloElement);
        }
        
        if(currentAdditiveScreens.Count > 0)
        {
            // reverse for
            for (int i = currentAdditiveScreens.Count - 1; i >= 0; i--)
            {
                CloseElement(currentAdditiveScreens[i]);
            }
        }

        if (currentPermanentScreens.Count > 0)
        {
            // reverse for
            for (int i = currentPermanentScreens.Count - 1; i >= 0; i--)
            {
                CloseElement(currentPermanentScreens[i]);
            }
        }
    }

    /**/
    // Inside Level related actions
    /**/
    public void DisplayInLevelUI()
    {
        CloseAllCurrentScreens(); // close all screens open before level launch

        AddAdditiveElement(inLevelUI);

        System.Action[] actions = new System.Action[10];
        actions[0] += GameManager.gameClip.CallDealAction; //deal action
        actions[1] += GameManager.TestButtonDelegationWorks; //potion 1 action
        actions[2] += GameManager.TestButtonDelegationWorks; //potion 2 action
        actions[3] += GameManager.TestButtonDelegationWorks; //potion 3 action
        actions[4] += GameManager.TestButtonDelegationWorks; //potion 4 action
        actions[5] += GameManager.TestButtonDelegationWorks; //Options button
        actions[6] += GameManager.TestButtonDelegationWorks; //Music icon level
        actions[7] += GameManager.TestButtonDelegationWorks; //SFX icon level
        actions[8] += GameManager.instance.InitiateDestrucionOfLevel; // to level map icon - delete all current level data
        actions[8] += OpenLevelMap; //to level map icon - display
        actions[9] += GameManager.instance.CallRestartLevel; //restart level icon

        inLevelUI.OverrideSetMe(null, null, actions);
    }

    public void DisplayInLevelSettings()
    {
        if(inLevelSettingsWindow.gameObject.activeInHierarchy)
        {
            CloseElement(inLevelSettingsWindow);
        }
        else
        {
            AddAdditiveElement(inLevelSettingsWindow);
        }
    }

    /**/
    // Level map related actions
    /**/
    public void DisplayLaunchLevelPopUp(LevelSO levelSO)
    {
        OpenSolo(levelMapPopUp);
        string[] texts = new string[] { "Level " + levelSO.levelNumInZone.ToString(), levelSO.worldName.ToString() };

        System.Action[] actions = new System.Action[1];
        actions[0] += GameManager.instance.SetLevel;
        actions[0] += DisplayInLevelUI;

        //actions += GameManager.instance.SetLevel;
        //actions += DisplayInLevelUI;

        levelMapPopUp.OverrideSetMe(texts, null, actions);
    }
    public void OpenLevelMap()
    {
        CloseAllCurrentScreens(); // close all screens open before going to map

        AddAdditiveElement(levelScrollRect);
    }


    /**/
    // general
    /**/
    [ContextMenu("Re-order Map")]
    private void ReOrderMapDisplay()
    {
        int id = 0;
        foreach (RefWorldDisplayCombo combo in worldReferebceCombo)
        {
            combo.mainImageRef.sprite = orderOfWorlds[id].mainSprite;
            combo.leftMargineImageRef.sprite = orderOfWorlds[id].leftMargineSprite;
            combo.rightMargineImageRef.sprite = orderOfWorlds[id].rightMargineSprite;
            id++;
        }
    }
}
