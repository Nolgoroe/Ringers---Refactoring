using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class sliceToSpawnDataStruct
{
    public SliceConditionsEnums sliceToSpawn;
    public UnityEvent onConnectionEvents;
    public int specificSliceIndex;
    public SubTileColor specificSlicesColor;
    public SubTileSymbol specificSlicesShape;
    public LootPacks RewardBag;

    public bool isLock;
    public bool RandomSlicePositions;
    public bool RandomSliceValues;

}


public enum PowerUp
{
    Switch,
    Joker,
    TileBomb,
    SliceBomb,
}

[System.Serializable]
public enum LootPacks
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
public class tileDataStruct
{
    public SubTileColor rightTileColor;
    public SubTileSymbol rightTileSymbol;
    public SubTileColor leftTileColor;
    public SubTileSymbol leftTileSymbol;
}

[CreateAssetMenu(fileName = "Level", menuName ="ScriptableObjects/Create Level")]
public class LevelSO : ScriptableObject
{
    [Header("Level Setup Settings")]
    public WorldEnum worldName;
    public int levelNumInZone;
    //public int numOfCells;
    //public int numOfSlices;
    public Ringtype ringType;

    public GameObject clipPrefab;
    public GameObject levelSpecificUserControls;

    public UnityEvent beforeRingSpawnActions; // each function that will be called here will "subscribe" to it's relavent stage in the gamemanger action
    public UnityEvent ringSpawnActions;
    public UnityEvent afterRingSpawnActions; 

    public SubTileColor[] levelAvailableColors;
    public SubTileSymbol[] levelAvailablesymbols;

    [Header("Statue")]
    public bool isUsingSpecificStatue;
    public GameObject specificStatueForLevel;

    [Header("Slices")]
    public bool allowRepeatSlices;
    public sliceToSpawnDataStruct[] slicesToSpawn;

    [Header("PowerUps")]
    public PowerUp[] powerupsForLevel;

    [Header("Stone Tiles")]
    public stoneTileDataStruct[] stoneTiles;

    [Header("Percise Position Settings")]
    public List<tileDataStruct> arrayOfSpecificTilesInClip;
}
