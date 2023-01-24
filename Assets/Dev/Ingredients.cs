using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Ingredientnames
{
    DawnDew,
    Wood,
    TreeSap,
    Grassblades,
    Wax,
    MossFleck,
    ScarletPimpernels,
    PeppermintLeaves,
    RockClump,
    VervainBlossom,
    AmberShard,
    DandelionRoot,
    CloverSeeds,
    NettleTears,
    RedClay,
    Feverfew,
    JadeShard,
    SilverNugget,
    Coltsfoot,
    SapphireShard,
    MoonDust,
    Amethyst,
    BlueFelt,
    JuniperNeedles,
    Cloudberry,
    ElderStalks,
    BlueQuartzCluster,
    BirchBark,
    Limestone,
    SilkThread,
    PoppyPetals,
    CherryBark,
    Moonstone,
}

public enum IngredientTypes
{
    Build,
    Herb,
    Witchcraft,
    Gem,
    None
}

[System.Serializable]
public class PriceInGems
{
    public int priceToPay;
    public int amountToRecieve;
} 

[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Create Ingredient")]
public class Ingredients : ScriptableObject
{
    public Ingredientnames ingredientName;
    public IngredientTypes ingredientType;
    public PriceInGems amountToPrice;

    [MenuItem("AssetDatabase/Rename ingredients")]
    static void Renameingredients()
    {
        foreach (var item in AssetDatabase.FindAssets("t:Ingredients", new[] { "Assets/Resources/Loot related/Ingredients (I)" }))
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(item);
            Ingredients temp = AssetDatabase.LoadAssetAtPath<Ingredients>(assetPath);
            
            if(temp)
            {
                AssetDatabase.RenameAsset(assetPath, "I" + temp.ingredientName);

                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }
    }

    [MenuItem("AssetDatabase/Rename Loot Tables")]
    static void RenameLootIngredients()
    {
        foreach (var item in AssetDatabase.FindAssets("t:MainLootContainer", new[] { "Assets/Resources/Loot related/Loot Tables (LT)" }))
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(item);
            LootTables temp = AssetDatabase.LoadAssetAtPath<LootTables>(assetPath);
            
            if(temp)
            {
                AssetDatabase.RenameAsset(assetPath, temp.containerType.ToString());

                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }
    }

    [MenuItem("AssetDatabase/Rename Loot Bags")]
    static void RenameLootBags()
    {
        foreach (var item in AssetDatabase.FindAssets("t:LootBags", new[] { "Assets/Resources/Loot related/Loot Bags(LB)" }))
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(item);
            LootBags temp = AssetDatabase.LoadAssetAtPath<LootBags>(assetPath);
            
            if(temp)
            {
                AssetDatabase.RenameAsset(assetPath, temp.bagType.ToString());

                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }
    }

}
