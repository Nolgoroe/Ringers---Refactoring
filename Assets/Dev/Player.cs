using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class DictionairyLootEntry
{
    public Ingredients ingredient;
    public IngredientTypes ingredientType;
    public bool hasChanged;
    public int value;

    public DictionairyLootEntry(Ingredients _ingredient, IngredientTypes _ingredientType)
    {
        ingredient = _ingredient;
        ingredientType = _ingredientType;
    }
}

[System.Serializable]
public class IngredientPlusMainTypeCombo
{
    public IngredientTypes mainType;
    public List<Ingredients> typeIngredients;
}


public class Player : MonoBehaviour
{


    private Dictionary<Ingredients, DictionairyLootEntry> ownedIngredients;

    [Header("Ingredient combos by type")]
    [SerializeField] private List<IngredientPlusMainTypeCombo> ingredientsToMainTypes;

    [Header("TEMP")]
    [SerializeField] private Ingredients testToAdd;
   
    
    
    private void Start()
    {
        ownedIngredients = new Dictionary<Ingredients, DictionairyLootEntry>();
    }

    [ContextMenu("Iterate in inventory")]
    private void IterateThroughInventory()
    {
        foreach (KeyValuePair<Ingredients, DictionairyLootEntry> ingredient in ownedIngredients)
        {
            Debug.Log(ingredient.Key);
            Debug.Log(ingredient.Value.value);

            if (ingredient.Value.hasChanged)
            {
                Debug.Log("Found change!");

                ingredient.Value.hasChanged = false;

            }
        }
    }

    [ContextMenu("Add ingredient")]
    public void AddIngredient()
    {
        int amountToAdd = Random.Range(1, 11); // 1-10

        if (ownedIngredients.ContainsKey(testToAdd))
        {
            ownedIngredients[testToAdd].hasChanged = true;
            ownedIngredients[testToAdd].value += amountToAdd;
        }
        else
        {
            DictionairyLootEntry newLootEntry = new DictionairyLootEntry(testToAdd, testToAdd.ingredientType);
            ownedIngredients.Add(testToAdd, newLootEntry);
            ownedIngredients[testToAdd].hasChanged = true;
            ownedIngredients[testToAdd].value = amountToAdd;

            AddToIngredientsComboByType();
        }

        Debug.Log("Added: " + amountToAdd + " " + "To: " + testToAdd.ToString());
    }

    private void AddToIngredientsComboByType()
    {
        IngredientPlusMainTypeCombo Combo = ingredientsToMainTypes.Where(i => i.mainType == testToAdd.ingredientType).SingleOrDefault();
        if (Combo != null)
        {
            Combo.typeIngredients.Add(testToAdd);

        }
    }
}
