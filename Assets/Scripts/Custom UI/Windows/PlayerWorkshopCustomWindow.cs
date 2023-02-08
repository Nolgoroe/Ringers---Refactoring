using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerWorkshopCustomWindow :  BasicCustomUIWindow
{
    [Header("Required refs inventory")]
    [SerializeField] private Transform materialsContent;
    [SerializeField] private CustomSpecificUIElementDisplayer materialDisplayPrefab;

    [Header("Required refs potion")]
    [SerializeField] private Transform[] potionsMaterialZones;

    [Header("Swappers")]
    [SerializeField] private ImageSwapHelper[] sortSwapHelpers;
    [SerializeField] private ImageSwapHelper[] catagoriesSwapHelpers;
    [SerializeField] private GameObject[] categoryTabs;

    private List<IngredientPlusMainTypeCombo> localCombos => GameManager.instance.GetPlayerCombos;
    private Dictionary<Ingredients, DictionairyLootEntry> localDict => GameManager.instance.GetIngredientDict;
    private Dictionary<Ingredientnames, Sprite> localDisplay => GameManager.instance.GetIngredientSpriteDict;

    public void InitPlayerWorkshop()
    {
        SortWorkshop(0);
        SwitchCategory(0);
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
    public void SwitchCategory(int index)
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

                    int amount = localDict[combo.typeIngredients[i]].amount;
                    Sprite sprite = localDisplay[combo.typeIngredients[i].ingredientName];

                    string[] texts = new string[] { amount.ToString() };
                    Sprite[] sprites = new Sprite[] { sprite };

                    displayer.SetMe(texts, sprites);
                }

                break;
            }
        }
    }
}
