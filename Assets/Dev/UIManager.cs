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

public enum MainScreens
{
    InLevel,
    Map,
}
public class UIManager : MonoBehaviour
{
    public static UIManager instance; //TEMP - LEARN DEPENDENCY INJECTION

    public static bool ISUSINGUI;
    public static bool ISDURINGTRANSITION;

    [Header("General refrences")]
    [SerializeField] private Player player;
    [SerializeField] private AnimalsManager animalManager;
    [SerializeField] private LootManager lootManager;

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

    [Header("Fade object settings")]
    [SerializeField] private BasicCustomUIWindow fadeWindow;
    [SerializeField] private float fadeIntoLevelTime;
    [SerializeField] private float fadeOutLevelTime;
    [SerializeField] private float fadeIntoMapTime;
    [SerializeField] private float fadeOutMapTime;

    [Header("Map setup")]
    [SerializeField] private WorldDisplayCombo[] orderOfWorlds;
    [SerializeField] private RefWorldDisplayCombo[] worldReferebceCombo;

    private void Start()
    {
        instance = this;

        StartCoroutine(DisplayLevelMap(false));
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

        ISUSINGUI = true;
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
        //if (UIElement.isPermanent) return;

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

        UIElement.gameObject.SetActive(false); //(OR destory it!!)

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
        ISUSINGUI = false;
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
    private void RestartCurrentScreenWindows()
    {
        // this function restarts the CURRENT screens windows
        // meaning any "permanent" windows should NOT get touched.
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
    }
    private void DeactiavteAllCustomButtons()
    {
        if (currentlyOpenSoloElement)
        {
            for (int i = 0; i < currentlyOpenSoloElement.buttonRefs.Length; i++)
            {
                currentlyOpenSoloElement.buttonRefs[i].isInteractable = false;
            }
        }

        if (currentAdditiveScreens.Count > 0)
        {
            foreach (var screen in currentAdditiveScreens)
            {
                for (int i = 0; i < screen.buttonRefs.Length; i++)
                {
                    screen.buttonRefs[i].isInteractable = false;
                }
            }
        }

        if (currentPermanentScreens.Count > 0)
        {
            foreach (var screen in currentPermanentScreens)
            {
                for (int i = 0; i < screen.buttonRefs.Length; i++)
                {
                    screen.buttonRefs[i].isInteractable = false;
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

        System.Action[] actions = new System.Action[10];
        actions[0] += GameManager.gameClip.CallDealAction; //deal action
        actions[1] += GameManager.TestButtonDelegationWorks; //potion 1 action
        actions[2] += GameManager.TestButtonDelegationWorks; //potion 2 action
        actions[3] += GameManager.TestButtonDelegationWorks; //potion 3 action
        actions[4] += GameManager.TestButtonDelegationWorks; //potion 4 action
        actions[5] += DisplayInLevelSettingsWindow; //Options button
        actions[6] += SoundManager.instance.MuteMusic; //Music icon level
        actions[7] += SoundManager.instance.MuteSFX; //SFX icon level
        actions[8] += DisplayInLevelExitToMapQuestion; // to level map icon
        actions[9] += DisplayInLevelRestartLevelQuestion; //restart level icon
        //actions[9] += DisplayInLevelSettings; //restart level icon

        inLevelUI.OverrideSetMe(null, null, actions);
    }
    private void DisplayInLevelSettingsWindow()
    {
        DisplayInLevelSettings();
        Debug.Log("Open options window");

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
        System.Action[] actions = new System.Action[2];

        actions[0] += GameManager.gameControls.ReturnHomeBadRingConnections;
        actions[0] += () => CloseElement(inLevelNonMatchTilesMessage);

        actions[1] += DisplayInLevelLostMessage;

        inLevelNonMatchTilesMessage.OverrideSetMe(null, null, actions);
    }
    private void DisplayInLevelLostMessage()
    {
        AddUIElement(inLevelLostLevelMessage);
        System.Action[] actions = new System.Action[2];
        //actions[0] += RestartCurrentScreenWindows;

        actions[0] += () => FadeInFadeWindow(true, MainScreens.InLevel);
        actions[0] += GameManager.instance.CallRestartLevel;

        actions[1] += () => StartCoroutine(DisplayLevelMap(true));
        actions[1] += () => StartCoroutine(GameManager.instance.InitiateDestrucionOfLevel());

        inLevelLostLevelMessage.OverrideSetMe(null, null, actions);
    }
    public void DisplayInLevelLastDealWarning()
    {
        AddUIElement(inLevelLastDealWarning);
        System.Action[] actions = new System.Action[2];

        actions[0] += () => CloseElement(inLevelLastDealWarning);

        actions[1] += () => FadeInFadeWindow(true, MainScreens.InLevel);
        actions[1] += GameManager.instance.CallRestartLevel;

        inLevelLastDealWarning.OverrideSetMe(null, null, actions);
    }
    private void DisplayInLevelExitToMapQuestion()
    {
        AddUIElement(inLevelExitToMapQuesiton);
        System.Action[] actions = new System.Action[2];

        actions[0] += () => CloseElement(inLevelExitToMapQuesiton);

        actions[1] += () => StartCoroutine(DisplayLevelMap(true));
        actions[1] += () => StartCoroutine(GameManager.instance.InitiateDestrucionOfLevel());

        inLevelExitToMapQuesiton.OverrideSetMe(null, null, actions);
    }
    private void DisplayInLevelRestartLevelQuestion()
    {
        AddUIElement(inLevelRestartLevelQuesiton);
        System.Action[] actions = new System.Action[2];

        actions[0] += () => CloseElement(inLevelRestartLevelQuesiton);

        actions[1] += () => FadeInFadeWindow(true, MainScreens.InLevel);
        actions[1] += GameManager.instance.CallRestartLevel;

        inLevelRestartLevelQuesiton.OverrideSetMe(null, null, actions);
    }
    public void DisplayInLevelWinWindow()
    {
        DeactiavteAllCustomButtons();

        System.Action[] actions = new System.Action[2];

        actions[0] += () => FadeInFadeWindow(true, MainScreens.InLevel);
        actions[0] += GameManager.instance.CallNextLevel;

        actions[1] += () => StartCoroutine(DisplayLevelMap(true));
        actions[1] += () => StartCoroutine(GameManager.instance.InitiateDestrucionOfLevel());

        inLevelWinWindow.OverrideSetMe(GameManager.instance.ReturnStatueName(), null, actions);

        AddUIElement(inLevelWinWindow);
    }
    public void ContinueAfterChest()
    {
        //ISDURINGCHEST = false;

        inLevelWinWindow.ManuallyShowToHudButton();
    }

    /**/
    // Level map related actions
    /**/
    public void DisplayLaunchLevelPopUp(LevelSO levelSO)
    {
        string[] texts = new string[] { "Level " + levelSO.levelNumInZone.ToString(), levelSO.worldName.ToString() };

        System.Action[] actions = new System.Action[1];

        actions[0] += () => FadeInFadeWindow(true, MainScreens.InLevel);
        actions[0] += GameManager.instance.SetLevel;

        AddUIElement(levelMapPopUp);

        levelMapPopUp.OverrideSetMe(texts, null, actions);
    }
    private IEnumerator DisplayLevelMap(bool isFade)
    {
        if(isFade)
        {
            FadeInFadeWindow(true, MainScreens.Map);
            yield return new WaitForSeconds(ReturnFadeTime(true, MainScreens.Map) + 0.1f);
        }

        CloseAllCurrentScreens(); // close all screens open before going to map

        System.Action[] actions = new System.Action[3];
        actions[0] += DisplayAnimalAlbum; // animal album
        actions[1] += DisplayPlayerWorkshop; // player workshop
        actions[2] += DisplayMapSettings; // open settings

        string tearsText = player.GetOwnedTears.ToString();
        string rubiesText = player.GetOwnedRubies.ToString();

        string[] texts = new string[] { tearsText, rubiesText };

        AddUIElement(levelScrollRect);
        AddUIElement(generalMapUI);

        generalMapUI.OverrideSetMe(texts, null, actions);
    }

    public void RefreshRubyAndTearsTexts(int tearsAmount, int rubiesAmount)
    {
        generalMapUI.textRefrences[0].text = tearsAmount.ToString(); // dew drops text
        generalMapUI.textRefrences[1].text = rubiesAmount.ToString(); // rubies text
    }

    private void DisplayMapSettings()
    {
        //called from button

        AddUIElement(generalSettings);

        string[] texts = new string[] { "Name of player: Avishy" };
        generalSettings.OverrideSetMe(texts, null, null);
    }

    private void DisplayPlayerWorkshop()
    {
        System.Action[] actions = new System.Action[7];
        actions[0] += () => playerWorkshopWindow.SwitchCategory(0); // inventory catagory
        actions[1] += () => playerWorkshopWindow.SwitchCategory(1); // potion catagory
        actions[2] += () => playerWorkshopWindow.SortWorkshop(0); // inventory build sort
        actions[3] += () => playerWorkshopWindow.SortWorkshop(1); // inventory gem sort
        actions[4] += () => playerWorkshopWindow.SortWorkshop(2); // inventory herb sort
        actions[5] += () => playerWorkshopWindow.SortWorkshop(3); // inventory witchcraft sort
        actions[6] += () => FadeInFadeWindow(true, MainScreens.InLevel); // potion brew button

        AddUIElement(playerWorkshopWindow);

        playerWorkshopWindow.OverrideSetMe(null, null, actions);
        //playerWorkshopWindow.InitPlayerWorkshop(player, lootManager);
        playerWorkshopWindow.InitPlayerWorkshop();
    }
    private void DisplayAnimalAlbum()
    {
        string tearsText = player.GetOwnedTears.ToString();
        string rubiesText = player.GetOwnedRubies.ToString();
        string[] texts = new string[] { tearsText, rubiesText };

        System.Action[] actions = new System.Action[5];
        actions[0] += () => animalAlbumWindow.SwitchAnimalCategory(0); // Fox type
        actions[1] += () => animalAlbumWindow.SwitchAnimalCategory(1); // Stag type
        actions[2] += () => animalAlbumWindow.SwitchAnimalCategory(2); // Owl type
        actions[3] += () => animalAlbumWindow.SwitchAnimalCategory(3); // Boar type
        actions[4] += () => animalAlbumWindow.GivePlayerRewardsFromAnimalAlbum(); // show animal reward window and give reward

        AddUIElement(animalAlbumWindow);

        animalAlbumWindow.OverrideSetMe(null, null, actions);
        animalAlbumWindow.InitAnimalAlbum(animalManager, player);
    }

    public void DisplayAnimalAlbumReward(int amountOfReward)
    {
        System.Action[] actions = new System.Action[1];
        actions[0] += () => CloseElement(animalAlbumRewardWidnow);

        AddUIElement(animalAlbumRewardWidnow);

        string[] texts = new string[] { amountOfReward.ToString() };

        animalAlbumRewardWidnow.OverrideSetMe(texts, null, actions);
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

    private void FadeInFadeWindow(bool fadeIn, MainScreens mainScreen)
    {
        ISDURINGTRANSITION = true;

        float fadeInSpeed = ReturnFadeTime(fadeIn, mainScreen);

        CanvasGroup group;
        fadeWindow.TryGetComponent<CanvasGroup>(out group);

        float from = 0, to = 0;

        if (group == null)
        {
            Debug.LogError("No canvas group!");
            return;
        }

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
        ISDURINGTRANSITION = false; 
        // is this ok?
        // This is here for actions that want to happen on the transition between
        // fade in and out - so we for 0.5f seconds, allow actions to operate in "fade time"

        yield return new WaitForSeconds(fadeTime);

        FadeInFadeWindow(!fadeIn, mainScreen);
    }

    private void OnEndFade()
    {
        ISDURINGTRANSITION = false;
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
}
