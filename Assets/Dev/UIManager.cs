using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public static bool ISUSINGUI;
    public static UIManager instance; //TEMP - LEARN DEPENDENCY INJECTION

    [SerializeField] private BasicUIElement currentlyOpenSoloElement;
    [SerializeField] private List<BasicUIElement> currentAdditiveScreens;


    [Header("Map setup")]
    [SerializeField] private WorldDisplayCombo[] orderOfWorlds;
    [SerializeField] private RefWorldDisplayCombo[] worldReferebceCombo;


    [Header("Map Screen")]
    public BasicUIElement levelMapPopUp;

    private void Start()
    {
        instance = this;
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
                    element.gameObject.SetActive(false); //or destory!!!
                    currentAdditiveScreens.Remove(element);
                }
            }

            UIElement.gameObject.SetActive(true);
        }
        else
        {
            if(UIElement.isOverrideSolo)
            {
                currentlyOpenSoloElement.gameObject.SetActive(false); // or derstroy!!!

                UIElement.gameObject.SetActive(true);
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
            currentAdditiveScreens.Add(UIElement);
        }
    }
    public void CloseElement(BasicUIElement UIElement)
    {
        if (UIElement.isSolo)
        {
            currentlyOpenSoloElement = null;

            ISUSINGUI = false;
        }

        if (currentAdditiveScreens.Contains(UIElement))
        {
            currentAdditiveScreens.Remove(UIElement);

            if(currentAdditiveScreens.Count == 0)
            {
                ISUSINGUI = false;
            }
        }


        UIElement.gameObject.SetActive(false); //(OR destory it!!)


    }

    /**/
    //Level map related actions
    /**/
    public void DisplayLevelMapPopUp(LevelSO levelSO)
    {
        OpenSolo(levelMapPopUp);
        string[] texts = new string[] { levelSO.levelNumInZone.ToString(), levelSO.worldName.ToString() };

        levelMapPopUp.SetMe(texts, null);
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
