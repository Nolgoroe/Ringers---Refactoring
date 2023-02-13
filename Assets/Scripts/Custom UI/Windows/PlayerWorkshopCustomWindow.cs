using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class PlayerWorkshopCustomWindow :  BasicCustomUIWindow
{
    [Header("General refrences")]
    [SerializeField] private CustomSpecificUIElementDisplayer materialDisplayPrefab;

    [Header("Required refrences inventory")]
    [SerializeField] private Transform materialsContent;

    [Header("Swappers")]
    [SerializeField] private ImageSwapHelper[] sortSwapHelpers;
    [SerializeField] private ImageSwapHelper[] catagoriesSwapHelpers;
    [SerializeField] private GameObject[] categoryTabs;

    [SerializeField] private int CurrentCategoryIndex = -1;

    private List<IngredientPlusMainTypeCombo> localCombos => GameManager.instance.GetPlayerCombos;
    private Dictionary<Ingredients, LootEntry> localownedIngredientsDict => GameManager.instance.GetIngredientDict;

    public void InitPlayerWorkshop()
    {
        SortWorkshop(0);
        TrySwitchCategory(0);
    }

    public void SortWorkshop(int index)
    {
        DestoryAllIngredientChildren();

        SetSortButtonsDisplay(index);

        SpawnAllOwnedIngredientsByType(index);
    }

    private void SetSortButtonsDisplay(int index)
    {
        for (int i = 0; i < sortSwapHelpers.Length; i++)
        {
            if (i == index)
            {
                sortSwapHelpers[i].SetActivatedChild();
            }
            else
            {
                sortSwapHelpers[i].SetDeActivatedChild();
            }
        }
    }
    private void SwitchCategory(int index)
    {
        SetCategoriesDisplay(index);
    }
    private void SetCategoriesDisplay(int index)
    {
        for (int i = 0; i < catagoriesSwapHelpers.Length; i++)
        {
            if (i == index)
            {
                catagoriesSwapHelpers[i].SetActivatedChild();
                categoryTabs[i].gameObject.SetActive(true);
            }
            else
            {
                catagoriesSwapHelpers[i].SetDeActivatedChild();
                categoryTabs[i].gameObject.SetActive(false);
            }
        }
    }

    private void DestoryAllIngredientChildren()
    {
        for (int i = 0; i < materialsContent.childCount; i++)
        {
            Destroy(materialsContent.GetChild(i).gameObject);
        }
    }

    private void SpawnAllOwnedIngredientsByType(int compareTypeIndex)
    {
        IngredientTypes requiredType = (IngredientTypes)compareTypeIndex;

        foreach (IngredientPlusMainTypeCombo combo in localCombos)
        {
            if(combo.mainType == requiredType)
            {
                for (int i = 0; i < combo.typeIngredients.Count; i++)
                {
                    CustomSpecificUIElementDisplayer displayer = Instantiate(materialDisplayPrefab, materialsContent);

                    int amount = localownedIngredientsDict[combo.typeIngredients[i]].amount;
                    Sprite sprite = combo.typeIngredients[i].ingredientSprite;

                    string[] texts = new string[] { amount.ToString() };
                    Sprite[] sprites = new Sprite[] { sprite };

                    displayer.SetMyElement(texts, sprites);
                }

                break;
            }
        }
    }

    public bool TrySwitchCategory(int index)
    {
        if(CurrentCategoryIndex != index)
        {
            CurrentCategoryIndex = index;
            SwitchCategory(index);
            return true;
        }

        Debug.LogError("This screnn is already open");
        return false;
    }
}
