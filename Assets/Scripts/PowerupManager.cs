using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class OwnedPowersAndAmounts
{
    public PowerupType powerType;
    public int amount;

    public OwnedPowersAndAmounts(PowerupType _InPower, int _InAmount)
    {
        powerType = _InPower;
        amount = _InAmount;
    }
}
public class PowerupManager : MonoBehaviour
{
    [Header("Owned")]
    [SerializeField] private List<PowerupScriptableObject> allPowerups;

    [Header("General")]
    public List<OwnedPowersAndAmounts> ownedPowerups;
    public List<PowerupType> unlockedPowerups;

    [Header("Live Crafting")]
    [SerializeField] private PowerupScriptableObject currentPotionSelected;
    [SerializeField] private int currentNeededRubies;
    [SerializeField] private List<PotionIngredientCustomSpecificDisplayer> spawnedDisplays;

    [Header("Crafting refrences")]
    [SerializeField] private BasicCustomUIWindow potionWindow;
    [SerializeField] private PotionCustomButton potionButtonPrefab;
    [SerializeField] private PotionIngredientCustomSpecificDisplayer potionMaterialPrefab;
    [SerializeField] private Transform[] potionsMaterialZones;
    [SerializeField] private Transform potionButtonsParent;
    //[SerializeField] private BasicCustomUIWindow buyPotionScreen; // go over with lior
    [SerializeField] private CustomSpecificUIElementDisplayer buyPotionScreenMaterialPrefab; // go over with lior
    [SerializeField] private Transform buyPotionScreenMaterialParent; // go over with lior

    [Header("General refrences")]
    [SerializeField] private Player player; //go over with Lior, do we need the whole player?
    
    public void InstantiatePowerButton(PowerupType powerType) // is it okay to take care of display and data of buttons here?
    {
        //summon and set button data
        PotionCustomButton createdButton = Instantiate(potionButtonPrefab, potionButtonsParent);
        createdButton.connecetdScriptableObjectType = powerType;

        //set button display and logic when it's summoned
        PowerupScriptableObject tempVar = allPowerups.Where(i => i.powerType == powerType).SingleOrDefault();
        if (currentPotionSelected == null)
        {
            Debug.LogError("Couldn't find potion!");
        }

        Sprite[] sprites = new Sprite[] { tempVar.potionSprite };
        System.Action[] actions = new System.Action[1];
        actions[0] += () => SetSelectedPotion(powerType);

        createdButton.OverrideSetMyElement(null, sprites, actions);
    }

    public void SetSelectedPotion(PowerupType powerType) // is it okay to take care of display and summon of dispay here?
    {
        PowerupScriptableObject tempSelected = allPowerups.Where(i => i.powerType == powerType).SingleOrDefault();

        if(currentPotionSelected == tempSelected)
        {
            return;
        }

        currentPotionSelected = tempSelected;
        if (currentPotionSelected == null)
        {
            Debug.LogError("Couldn't find potion!");
            return;
        }

        string[] texsts = new string[] { currentPotionSelected.powerType.ToString(), currentPotionSelected.potionDescription };
        Sprite[] sprites = new Sprite[] { currentPotionSelected.potionSprite};
        potionWindow.OverrideSetMyElement(texsts, sprites, null);

        StartCoroutine(InstantiateNeededIngredients());
    }

    private IEnumerator InstantiateNeededIngredients() // go over with lior
    {
        SwitchPotion();

        yield return new WaitForEndOfFrame();

        SpawnIngredients();
    }

    private void SwitchPotion()
    {
        for (int i = 0; i < potionsMaterialZones.Length; i++) // go over with Lior
        {
            for (int k = 0; k < potionsMaterialZones[i].childCount; k++)
            {
                Destroy(potionsMaterialZones[i].GetChild(k).gameObject);
            }
        }

        currentNeededRubies = 0;
        spawnedDisplays.Clear();

    }

    private void SpawnIngredients()
    {
        int summonIndex = 0;
        foreach (IngredientsNeeded ingredientNeeded in currentPotionSelected.ingredientsNeeded)
        {
            PotionIngredientCustomSpecificDisplayer displayer = Instantiate(potionMaterialPrefab, potionsMaterialZones[summonIndex]);
            spawnedDisplays.Add(displayer);

            int ownedAmountOfIngredient = 0;
            int neededAmount = ingredientNeeded.amountNeeded;

            if (player.returnownedIngredients.ContainsKey(ingredientNeeded.ingredient))
            {
                ownedAmountOfIngredient = player.returnownedIngredients[ingredientNeeded.ingredient].amount;
            }

            string amountRepresentation = ownedAmountOfIngredient.ToString() + "/" + neededAmount.ToString();

            string[] texsts = new string[] { amountRepresentation };
            Sprite[] sprites = new Sprite[] { ingredientNeeded.ingredient.ingredientSprite };
            displayer.OverrideSetMyElement(texsts, sprites, null);

            displayer.SetColorMissingIngredients(ownedAmountOfIngredient < neededAmount);

            if (ownedAmountOfIngredient < neededAmount) // can collapse this to funciton?
            {
                int needed = neededAmount - ownedAmountOfIngredient;

                int priceOfIngredient = ingredientNeeded.ingredient.amountToPrice.priceToPay;

                currentNeededRubies += needed * priceOfIngredient;
            }

            summonIndex++;
        }
    }

    public void CallClearPowerupScreenDataCoroutine() // go over with Lior
    {
        //called from DarkBackground under workshop
        StartCoroutine(ClearPowerupScreenData());
    }
    public IEnumerator ClearPowerupScreenData()
    {
        for (int i = 0; i < potionButtonsParent.childCount; i++)
        {
            Destroy(potionButtonsParent.GetChild(i).gameObject);         
        }


        for (int i = 0; i < potionsMaterialZones.Length; i++) // go over with Lior
        {
            for (int k = 0; k < potionsMaterialZones[i].childCount; k++)
            {
                Destroy(potionsMaterialZones[i].GetChild(k).gameObject);
            }
        }

        currentPotionSelected = null;
        currentNeededRubies = 0;
        spawnedDisplays.Clear();
        yield return new WaitForEndOfFrame();
    }

    public void TryBrewPotion()
    {
        if(currentNeededRubies == 0)
        {
            BrewPotion();
        }
        else
        {
            CleanBuyPotionWindow();
            UIManager.instance.DisplayBuyPotionWindow(currentNeededRubies);
            PopulateBuyPotionWindow();
            //ask if want buy potion
        }
    }

    private void PopulateBuyPotionWindow()
    {
        foreach (IngredientsNeeded ingredientNeeded in currentPotionSelected.ingredientsNeeded) // this action replicates several times in neveral other places in this script - how to minimize?
        {
            int ownedAmountOfIngredient = 0;
            int neededAmount = ingredientNeeded.amountNeeded;

            if (player.returnownedIngredients.ContainsKey(ingredientNeeded.ingredient))
            {
                ownedAmountOfIngredient = player.returnownedIngredients[ingredientNeeded.ingredient].amount;
            }

            //spawn the display - is it ok in powerup manager?
            if(neededAmount > ownedAmountOfIngredient)
            {
                int deltaAmount = neededAmount - ownedAmountOfIngredient;

                CustomSpecificUIElementDisplayer displayer = Instantiate(buyPotionScreenMaterialPrefab, buyPotionScreenMaterialParent);
                Sprite[] sprites = new Sprite[] { ingredientNeeded.ingredient.ingredientSprite };
                string[] texsts = new string[] { deltaAmount.ToString()};

                displayer.OverrideSetMyElement(texsts, sprites, null);
            }
        }

    }

    public void CleanBuyPotionWindow()
    {
        if(buyPotionScreenMaterialParent.childCount > 0)
        {
            for (int i = 0; i < buyPotionScreenMaterialParent.childCount; i++)
            {
                Destroy(buyPotionScreenMaterialParent.GetChild(i).gameObject);
            }
        }
    }
    private void RefreshIngredientDisplays() // go over with lior
    {
        currentNeededRubies = 0;

        int summonIndex = 0;
        foreach (IngredientsNeeded ingredientNeeded in currentPotionSelected.ingredientsNeeded)
        {
            int ownedAmountOfIngredient = 0;
            int neededAmount = ingredientNeeded.amountNeeded;

            if (player.returnownedIngredients.ContainsKey(ingredientNeeded.ingredient))
            {
                ownedAmountOfIngredient = player.returnownedIngredients[ingredientNeeded.ingredient].amount;
            }

            string amountRepresentation = ownedAmountOfIngredient.ToString() + "/" + neededAmount.ToString();
            spawnedDisplays[summonIndex].SetAmountsText(amountRepresentation);

            if (ownedAmountOfIngredient < neededAmount) // can collapse this to funciton?
            {
                int needed = neededAmount - ownedAmountOfIngredient;

                int priceOfIngredient = ingredientNeeded.ingredient.amountToPrice.priceToPay;

                currentNeededRubies += needed * priceOfIngredient;
            }

            summonIndex++;
        }

    }
    private void RemoveNeededIngredientsFromPlayerBrewAction() // go over with lior
    {
        foreach (IngredientsNeeded ingredientNeeded in currentPotionSelected.ingredientsNeeded)
        {
            player.RemoveIngredients(ingredientNeeded.ingredient, ingredientNeeded.amountNeeded);
        }
    }

    private void RemoveNeededIngredientsFromPlayerBuyAction() // go over with lior
    {
        foreach (IngredientsNeeded ingredientNeeded in currentPotionSelected.ingredientsNeeded)
        {
            int ownedAmountOfIngredient = 0;
            int neededAmount = ingredientNeeded.amountNeeded;
            if (player.returnownedIngredients.ContainsKey(ingredientNeeded.ingredient))
            {
                ownedAmountOfIngredient = player.returnownedIngredients[ingredientNeeded.ingredient].amount;
            }

            if(ownedAmountOfIngredient >= neededAmount)
            {
                //remove what we need
                player.RemoveIngredients(ingredientNeeded.ingredient, ingredientNeeded.amountNeeded);
            }
            else
            {
                //remove all we have
                player.RemoveIngredients(ingredientNeeded.ingredient, ownedAmountOfIngredient);
            }
        }
    }

    public void BuyPotion()
    {
        if (player.GetOwnedRubies < currentNeededRubies)
        {
            Debug.LogError("Not enough rubies!");
            return;
        }

        player.RemoveRubies(currentNeededRubies);

        RemoveNeededIngredientsFromPlayerBuyAction();

        RefreshIngredientDisplays();


        OwnedPowersAndAmounts temoVar = ownedPowerups.Where(i => i.powerType == currentPotionSelected.powerType).SingleOrDefault();

        if(temoVar == null)
        {
            OwnedPowersAndAmounts newPotion = new OwnedPowersAndAmounts(currentPotionSelected.powerType, 1);
            ownedPowerups.Add(newPotion);
        }
        else
        {
            temoVar.amount++;
        }

        CleanBuyPotionWindow();
    }
    public void BrewPotion()
    {
        RemoveNeededIngredientsFromPlayerBrewAction();

        RefreshIngredientDisplays();

        AddPotion(currentPotionSelected.powerType);
    }

    public void AddPotion(PowerupType powerType)
    {
        OwnedPowersAndAmounts temoVar = ownedPowerups.Where(i => i.powerType == powerType).SingleOrDefault();

        if (temoVar == null)
        {
            OwnedPowersAndAmounts newPotion = new OwnedPowersAndAmounts(powerType, 1);
            ownedPowerups.Add(newPotion);
        }
        else
        {
            temoVar.amount++;
        }
        Debug.Log("Added this power: " + powerType.ToString());
    }
}
