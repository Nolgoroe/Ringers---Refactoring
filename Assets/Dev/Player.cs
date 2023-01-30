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
    public int amount;

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
    [SerializeField] private int ownedRubies;
    [SerializeField] private int ownedTears;

    private Dictionary<Ingredients, DictionairyLootEntry> ownedIngredients;

    [Header("Ingredient combos by type")]
    //we do this to sort the materials by their main types - build, herb, witch and gem
    [SerializeField] private List<IngredientPlusMainTypeCombo> ingredientsToMainTypes;
      
    private void Start()
    {
        ownedIngredients = new Dictionary<Ingredients, DictionairyLootEntry>();
    }

    [ContextMenu("Iterate in inventory")]
    private void IterateThroughInventory()
    {
        foreach (KeyValuePair<Ingredients, DictionairyLootEntry> ingredient in ownedIngredients)
        {
            Debug.Log(ingredient.Key + " amount: " + ingredient.Value.amount);

            if (ingredient.Value.hasChanged)
            {
                Debug.Log("Found change!");

                ingredient.Value.hasChanged = false;

            }
        }
    }

    public void AddIngredient(LootToRecieve ingredientToAdd)
    {
        Ingredients toAdd = ingredientToAdd.ingredient;
        if (ownedIngredients.ContainsKey(toAdd))
        {
            ownedIngredients[toAdd].hasChanged = true;
            ownedIngredients[toAdd].amount += ingredientToAdd.amount;
        }
        else
        {
            DictionairyLootEntry newLootEntry = new DictionairyLootEntry(toAdd, toAdd.ingredientType);
            ownedIngredients.Add(toAdd, newLootEntry);
            ownedIngredients[toAdd].hasChanged = true;
            ownedIngredients[toAdd].amount = ingredientToAdd.amount;

            AddToIngredientsComboByType(toAdd);
        }

        Debug.Log("Added: " + ingredientToAdd.amount + " " + "To: " + toAdd.ToString());
    }
    public void AddRubies(int amount)
    {
        ownedRubies += amount;
        Debug.Log("Added: " + amount + " " + "To gems!");
    }
    private void AddToIngredientsComboByType(Ingredients toAdd)
    {
        IngredientPlusMainTypeCombo Combo = ingredientsToMainTypes.Where(i => i.mainType == toAdd.ingredientType).SingleOrDefault();
        if (Combo != null)
        {
            Combo.typeIngredients.Add(toAdd);

        }
    }

    /**/
    // GETTERS!
    /**/
    public int GetOwnedRubies => ownedRubies;
    public int GetOwnedTears => ownedTears;
    public List<IngredientPlusMainTypeCombo> returnOwnedIngredientsByType => ingredientsToMainTypes;
    public Dictionary<Ingredients, DictionairyLootEntry> returnownedIngredients => ownedIngredients;
}
