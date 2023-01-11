using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MainlootContainerTypwa
{
    None,
    R1,
    R2,
    R3,
    R4,
    R5,
    LT1,
    LT2,
    LT3,
    LT4,
    LT5,
    LT6,
    LT7,
    LT8,
    LT9,
    LT10,
    LT11,
    LT12,
    LT13,
    LT14,
    LT15,
    LT16,
    LT17,
    LT18,
    LT19,
    LT20,
}

[System.Serializable]
public class LootBagsChances
{
    public LootBags lootBag;
    public int chance;
}

[CreateAssetMenu(fileName = "Main Loot Container", menuName = "ScriptableObjects/Create Main Loot Container")]
public class LootTables : ScriptableObject
{
    public MainlootContainerTypwa containerType;
    public LootBagsChances[] lootBags;
    public int minRubies, maxRubies;
}
