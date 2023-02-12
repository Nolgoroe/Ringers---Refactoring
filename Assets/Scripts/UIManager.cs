using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;

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

public enum MainScreens
{
    InLevel,
    Map,
}
public class UIManager : MonoBehaviour
{
    public static UIManager instance; //TEMP - LEARN DEPENDENCY INJECTION

    public static bool IS_USING_UI;
    public static bool IS_DURING_TRANSITION;

    [Header("General refrences")]
    [SerializeField] private Player player;
    [SerializeField] private AnimalsManager animalManager;

    [Header("Active screens")]
    [SerializeField] private BasicUIElement currentlyOpenSoloElement;
    [SerializeField] private List<BasicUIElement> currentAdditiveScreens;
    [SerializeField] private List<BasicUIElement> currentPermanentScreens;

    [Header("Map Screen")]
    [SerializeField] private BasicCustomUIWindow levelScrollRect;
    [SerializeField] private PlayerWorkshopCustomWindow playerWorkshopWindow;
    [SerializeField] private BasicCustomUIWindow levelMapPopUp;
    [SerializeField] private BasicCustomUIWindow generalSettings;
    [SerializeField] private BasicCustomUIWindow generalMapUI;
    [SerializeField] private AnimalAlbumCustonWindow animalAlbumWindow;
    [SerializeField] private BasicCustomUIWindow animalAlbumRewardWidnow;

    [Header("In level Screen")]
    [SerializeField] private BasicCustomUIWindow inLevelUI;
    [SerializeField] private BasicCustomUIWindow inLevelSettingsWindow;
    [SerializeField] private BasicCustomUIWindow inLevelNonMatchTilesMessage;
    [SerializeField] private BasicCustomUIWindow inLevelLostLevelMessage;
    [SerializeField] private BasicCustomUIWindow inLevelLastDealWarning;
    [SerializeField] private BasicCustomUIWindow inLevelExitToMapQuesiton;
    [SerializeField] private BasicCustomUIWindow inLevelRestartLevelQuesiton;
    [SerializeField] private WinLevelCustomWindow inLevelWinWindow;

    [Header("Fade object settings")] // might move to SO
    [SerializeField] private BasicCustomUIWindow fadeWindow;
    [SerializeField] private float fadeIntoLevelTime;
    [SerializeField] private float fadeOutLevelTime;
    [SerializeField] private float fadeIntoMapTime;
    [SerializeField] private float fadeOutMapTime;

    [Header("Map setup")] //might move to a different script
    [SerializeField] private WorldDisplayCombo[] orderOfWorlds;
    [SerializeField] private RefWorldDisplayCombo[] worldImageReferenceCombo;

    private void Start()
    {
        instance = this;

        StartCoroutine(DisplayLevelMap(false));
    }

    private void OnValidate()
    {
        if(orderOfWorlds.Length != worldImageReferenceCombo.Length)
        {
            Debug.LogError("World counts do not match!", gameObject);
        }
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
                // reverse for
                for (int i = currentAdditiveScreens.Count - 1; i >= 0; i--)
                {
                    if (!currentAdditiveScreens[i].isPermanent)
                    {
                        CloseElement(currentAdditiveScreens[i]);
                    }
                }
            }

            UIElement.gameObject.SetActive(true);
            currentlyOpenSoloElement = UIElement;
        }
        else
        {
            if(UIElement.isOverrideSolo)
            {
                currentlyOpenSoloElement.gameObject.SetActive(false);

                UIElement.gameObject.SetActive(true);
                currentlyOpenSoloElement = UIElement;
            }
            else
            {
                Debug.Log("Tried to open a solo screen on top of another solo screen, but is not overriding.");
                // do nothing
            }
        }

        IS_USING_UI = true;
    }
    private void AddAdditiveElement(BasicUIElement UIElement)
    {
        if(!currentAdditiveScreens.Contains(UIElement) && !currentPermanentScreens.Contains(UIElement))
        {
            if(UIElement.isPermanent)
            {
                currentPermanentScreens.Add(UIElement);
            }
            else
            {
                currentAdditiveScreens.Add(UIElement);
            }

            UIElement.gameObject.SetActive(true);
        }
    }
    private void AddUIElement(BasicUIElement UIElement)
    {
        if(UIElement.isSolo)
        {
            OpenSolo(UIElement);
        }
        else
        {
            AddAdditiveElement(UIElement);
        }
    }
    public void CloseElement(BasicUIElement UIElement)
    {
        if (UIElement.isSolo)
        {
            currentlyOpenSoloElement = null;
            StartCoroutine(ResetUsingUI());
        }

        if (currentAdditiveScreens.Contains(UIElement))
        {
            currentAdditiveScreens.Remove(UIElement);
        }

        if (currentPermanentScreens.Contains(UIElement))
        {
            currentPermanentScreens.Remove(UIElement);
        }

        UIElement.gameObject.SetActive(false);

        CheckResetUsingUI();
    }

    private void CheckResetUsingUI()
    {
        if (currentlyOpenSoloElement != null)
            return;

        if (currentAdditiveScreens.Count > 0)
            return;

        if (currentPermanentScreens.Count > 0)
            return;

        ResetUsingUI();
    }
    private IEnumerator ResetUsingUI()
    {
        yield return new WaitForEndOfFrame();
        IS_USING_UI = false;
    }
    private void CloseAllCurrentScreens()
    {
        // this function restarts ALL currently activated windows
        // including any "permanent" windows.

        if (currentlyOpenSoloElement)
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

        ResetUsingUI();
    }

    public void ActivateSingleButton(BasicCustomButton button)
    {
        DeactiavteAllCustomButtons();
        button.isInteractable = true;
    }
    private void DeactiavteAllCustomButtons()
    {
        if (currentlyOpenSoloElement)
        {
            for (int i = 0; i < currentlyOpenSoloElement.getButtonRefrences.Length; i++)
            {
                currentlyOpenSoloElement.getButtonRefrences[i].isInteractable = false;
            }
        }

        if (currentAdditiveScreens.Count > 0)
        {
            foreach (var screen in currentAdditiveScreens)
            {
                for (int i = 0; i < screen.getButtonRefrences.Length; i++)
                {
                    screen.getButtonRefrences[i].isInteractable = false;
                }
            }
        }

        if (currentPermanentScreens.Count > 0)
        {
            foreach (var screen in currentPermanentScreens)
            {
                for (int i = 0; i < screen.getButtonRefrences.Length; i++)
                {
                    screen.getButtonRefrences[i].isInteractable = false;
                }
            }
        }
    }

    /**/
    // Inside Level related actions
    /**/
    public void DisplayInLevelUI()
    {
        CloseAllCurrentScreens(); // close all screens open before level launch

        AddUIElement(inLevelUI);

        System.Action[] actions = new System.Action[inLevelUI.getButtonRefrences.Length];
        actions[0] += GameManager.gameClip.CallDealAction; //deal action
        actions[1] += GameManager.TestButtonDelegationWorks; //potion 1 action
        actions[2] += GameManager.TestButtonDelegationWorks; //potion 2 action
        actions[3] += GameManager.TestButtonDelegationWorks; //potion 3 action
        actions[4] += GameManager.TestButtonDelegationWorks; //potion 4 action
        actions[5] += DisplayInLevelSettings; //Options button
        actions[6] += SoundManager.instance.MuteMusic; //Music icon level
        actions[7] += SoundManager.instance.MuteSFX; //SFX icon level
        actions[8] += DisplayInLevelExitToMapQuestion; // to level map icon
        actions[9] += DisplayInLevelRestartLevelQuestion; //restart level icon

        inLevelUI.OverrideSetMyElement(null, null, actions);
    }
    private void DisplayInLevelSettings()
    {
        if(inLevelSettingsWindow.gameObject.activeInHierarchy)
        {
            CloseElement(inLevelSettingsWindow);
        }
        else
        {
            AddUIElement(inLevelSettingsWindow);
        }
    }
    public void DisplayInLevelRingHasNonMatchingMessage()
    {
        AddUIElement(inLevelNonMatchTilesMessage);
        System.Action[] actions = new System.Action[inLevelNonMatchTilesMessage.getButtonRefrences.Length];

        actions[0] += GameManager.gameControls.ReturnHomeBadRingConnections;
        actions[0] += () => CloseElement(inLevelNonMatchTilesMessage);
        actions[1] += DisplayInLevelLostMessage;

        inLevelNonMatchTilesMessage.OverrideSetMyElement(null, null, actions);
    }
    private void DisplayInLevelLostMessage()
    {
        AddUIElement(inLevelLostLevelMessage);
        System.Action[] actions = new System.Action[inLevelLostLevelMessage.getButtonRefrences.Length];

        actions[0] += () => FadeInFadeWindow(true, MainScreens.InLevel);
        actions[0] += GameManager.instance.CallRestartLevel;

        actions[1] += () => StartCoroutine(DisplayLevelMap(true));
        actions[1] += () => StartCoroutine(GameManager.instance.InitiateDestrucionOfLevel());

        inLevelLostLevelMessage.OverrideSetMyElement(null, null, actions);
    }
    public void DisplayInLevelLastDealWarning()
    {
        AddUIElement(inLevelLastDealWarning);
        System.Action[] actions = new System.Action[inLevelLastDealWarning.getButtonRefrences.Length];

        actions[0] += () => CloseElement(inLevelLastDealWarning);
        actions[1] += () => FadeInFadeWindow(true, MainScreens.InLevel);
        actions[1] += GameManager.instance.CallRestartLevel;

        inLevelLastDealWarning.OverrideSetMyElement(null, null, actions);
    }
    private void DisplayInLevelExitToMapQuestion()
    {
        AddUIElement(inLevelExitToMapQuesiton);
        System.Action[] actions = new System.Action[inLevelExitToMapQuesiton.getButtonRefrences.Length];

        actions[0] += () => StartCoroutine(DisplayLevelMap(true));
        actions[0] += () => StartCoroutine(GameManager.instance.InitiateDestrucionOfLevel());
        actions[1] += () => CloseElement(inLevelExitToMapQuesiton);


        inLevelExitToMapQuesiton.OverrideSetMyElement(null, null, actions);
    }
    private void DisplayInLevelRestartLevelQuestion()
    {
        AddUIElement(inLevelRestartLevelQuesiton);
        System.Action[] actions = new System.Action[inLevelRestartLevelQuesiton.getButtonRefrences.Length];

        actions[0] += () => FadeInFadeWindow(true, MainScreens.InLevel);
        actions[0] += GameManager.instance.CallRestartLevel;
        actions[1] += () => CloseElement(inLevelRestartLevelQuesiton);

        inLevelRestartLevelQuesiton.OverrideSetMyElement(null, null, actions);
    }
    public void DisplayInLevelWinWindow()
    {
        DeactiavteAllCustomButtons();

        System.Action[] actions = new System.Action[inLevelWinWindow.getButtonRefrences.Length];

        actions[0] += () => FadeInFadeWindow(true, MainScreens.InLevel);
        actions[0] += GameManager.instance.CallNextLevel;

        actions[1] += () => StartCoroutine(DisplayLevelMap(true));
        actions[1] += () => StartCoroutine(GameManager.instance.InitiateDestrucionOfLevel());

        inLevelWinWindow.OverrideSetMyElement(GameManager.instance.ReturnStatueName(), null, actions);

        AddUIElement(inLevelWinWindow);
    }



    //why is this here?
    public void ContinueAfterChest()
    {
        inLevelWinWindow.ManuallyShowToHudButton();
    }

    /**/
    // Level map related actions
    /**/
    public void DisplayLaunchLevelPopUp(LevelSO levelSO)
    {
        string[] texts = new string[] { "Level " + levelSO.levelNumInZone.ToString(), levelSO.worldName.ToString() };

        System.Action[] actions = new System.Action[levelMapPopUp.getButtonRefrences.Length];

        actions[0] += () => FadeInFadeWindow(true, MainScreens.InLevel);
        actions[0] += GameManager.instance.SetLevel;

        AddUIElement(levelMapPopUp);

        levelMapPopUp.OverrideSetMyElement(texts, null, actions);
    }
    private IEnumerator DisplayLevelMap(bool isFade)
    {
        if(isFade)
        {
            FadeInFadeWindow(true, MainScreens.Map);
            yield return new WaitForSeconds(ReturnFadeTime(true, MainScreens.Map) + 0.1f);
        }

        CloseAllCurrentScreens(); // close all screens open before going to map

        System.Action[] actions = new System.Action[generalMapUI.getButtonRefrences.Length];
        actions[0] += DisplayAnimalAlbum; // animal album
        actions[1] += DisplayPlayerWorkshop; // player workshop
        actions[2] += DisplayMapSettings; // open settings

        string tearsText = player.GetOwnedTears.ToString();
        string rubiesText = player.GetOwnedRubies.ToString();

        string[] texts = new string[] { tearsText, rubiesText };

        AddUIElement(levelScrollRect);
        AddUIElement(generalMapUI);

        generalMapUI.OverrideSetMyElement(texts, null, actions);
    }

    public void RefreshRubyAndTearsTexts(int tearsAmount, int rubiesAmount)
    {
        generalMapUI.getTextRefrences[0].text = tearsAmount.ToString(); // dew drops text
        generalMapUI.getTextRefrences[1].text = rubiesAmount.ToString(); // rubies text
    }

    private void DisplayMapSettings()
    {
        //called from button

        AddUIElement(generalSettings);

        System.Action[] actions = new System.Action[generalSettings.getButtonRefrences.Length];
        actions[0] += SoundManager.instance.MuteMusic; //Music icon level
        actions[1] += SoundManager.instance.MuteSFX; //SFX icon level

        string[] texts = new string[] { "Name of player: Avishy" };
        generalSettings.OverrideSetMyElement(texts, null, actions);
    }

    private void DisplayPlayerWorkshop()
    {
        System.Action[] actions = new System.Action[playerWorkshopWindow.getButtonRefrences.Length];
        actions[0] += () => playerWorkshopWindow.SwitchCategory(0); // inventory catagory
        actions[1] += () => playerWorkshopWindow.SwitchCategory(1); // potion catagory
        actions[2] += () => playerWorkshopWindow.SortWorkshop(0); // inventory build sort
        actions[3] += () => playerWorkshopWindow.SortWorkshop(1); // inventory gem sort
        actions[4] += () => playerWorkshopWindow.SortWorkshop(2); // inventory herb sort
        actions[5] += () => playerWorkshopWindow.SortWorkshop(3); // inventory witchcraft sort
        actions[6] += () => FadeInFadeWindow(true, MainScreens.InLevel); // potion brew button

        AddUIElement(playerWorkshopWindow);

        playerWorkshopWindow.OverrideSetMyElement(null, null, actions);
        //playerWorkshopWindow.InitPlayerWorkshop(player, lootManager);
        playerWorkshopWindow.InitPlayerWorkshop();
    }
    private void DisplayAnimalAlbum()
    {
        string tearsText = player.GetOwnedTears.ToString();
        string rubiesText = player.GetOwnedRubies.ToString();
        string[] texts = new string[] { tearsText, rubiesText };

        System.Action[] actions = new System.Action[animalAlbumWindow.getButtonRefrences.Length];
        actions[0] += () => animalAlbumWindow.SwitchAnimalCategory(0); // Fox type
        actions[1] += () => animalAlbumWindow.SwitchAnimalCategory(1); // Stag type
        actions[2] += () => animalAlbumWindow.SwitchAnimalCategory(2); // Owl type
        actions[3] += () => animalAlbumWindow.SwitchAnimalCategory(3); // Boar type
        actions[4] += () => animalAlbumWindow.GivePlayerRewardsFromAnimalAlbum(); // show animal reward window and give reward

        AddUIElement(animalAlbumWindow);

        animalAlbumWindow.OverrideSetMyElement(null, null, actions);
        animalAlbumWindow.InitAnimalAlbum(animalManager, player);
    }

    public void DisplayAnimalAlbumReward(int amountOfReward)
    {
        System.Action[] actions = new System.Action[animalAlbumRewardWidnow.getButtonRefrences.Length];
        actions[0] += () => CloseElement(animalAlbumRewardWidnow);

        AddUIElement(animalAlbumRewardWidnow);

        string[] texts = new string[] { amountOfReward.ToString() };

        animalAlbumRewardWidnow.OverrideSetMyElement(texts, null, actions);
    }

    /**/
    // general
    /**/
    [ContextMenu("Re-order Map")]
    private void ReOrderMapDisplay()
    {
        int id = 0;
        foreach (RefWorldDisplayCombo combo in worldImageReferenceCombo)
        {
            combo.mainImageRef.sprite = orderOfWorlds[id].mainSprite;
            combo.leftMargineImageRef.sprite = orderOfWorlds[id].leftMargineSprite;
            combo.rightMargineImageRef.sprite = orderOfWorlds[id].rightMargineSprite;
            id++;
        }
    }

    private void FadeInFadeWindow(bool fadeIn, MainScreens mainScreen)
    {
        IS_DURING_TRANSITION = true;

        float fadeInSpeed = ReturnFadeTime(fadeIn, mainScreen);

        CanvasGroup group = fadeWindow.group;

        float from = 0, to = 0;


        group.alpha = fadeIn == true ? 0 : 1;
        from = fadeIn == true ? 0 : 1;
        to = fadeIn == true ? 1 : 0;
        System.Action action = fadeIn == true ? () => StartCoroutine(ReverseFade(fadeIn, mainScreen, fadeInSpeed)) : OnEndFade;

        fadeWindow.gameObject.SetActive(true);
        
        fadeWindow.GeneralFloatValueTo(
            fadeWindow.gameObject,
            from,
            to,
            fadeInSpeed,
            LeanTweenType.linear,
            group,
            action);
    }

    private IEnumerator ReverseFade(bool fadeIn, MainScreens mainScreen, float fadeTime)
    {
        IS_DURING_TRANSITION = false; 
        // is this ok?
        // This is here for actions that want to happen on the transition between
        // fade in and out - so we for 0.5f seconds, allow actions to operate in "fade time"

        yield return new WaitForSeconds(fadeTime);

        FadeInFadeWindow(!fadeIn, mainScreen);
    }

    private void OnEndFade()
    {
        IS_DURING_TRANSITION = false;
        CloseElement(fadeWindow);
    }
    private float ReturnFadeTime(bool fadeIn, MainScreens mainScreen)
    {
        if(fadeIn)
        {
            switch (mainScreen)
            {
                case MainScreens.InLevel:
                    return fadeIntoLevelTime;
                case MainScreens.Map:
                    return fadeIntoMapTime;
                default:
                    break;
            }
        }
        else
        {
            switch (mainScreen)
            {
                case MainScreens.InLevel:
                    return fadeOutLevelTime;
                case MainScreens.Map:
                    return fadeOutMapTime;
                default:
                    break;
            }
        }
        Debug.LogError("Some problem here");
        return -1;
    }

#if UNITY_EDITOR

    [MenuItem("Build Preperation/Prepare UI for build")]

    static void DeactivateAllWindows()
    {
        CustomWindowParent[] allGameWindows = FindObjectsOfType<CustomWindowParent>();
        foreach (BasicUIElement window in allGameWindows)
        {
            window.gameObject.SetActive(false);
        }
    }

#endif
}
