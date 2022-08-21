using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class stoneTileDataStruct
{
    public int cellIndex;
    public bool randomValues;
    public bool noColors;
    public TileColor rightTileColor;
    public TileSymbol rightTileSymbol;
    public TileColor leftTileColor;
    public TileSymbol leftTileSymbol;
}

[System.Serializable]
public class sliceToSpawnDataStruct
{
    public SliceConditionsEnums sliceToSpawn;
    public bool isLock;
    public bool isLoot;
    public bool isLimiter;
    public LootPacks[] RewardBags;
}


public enum LevelActions
{
}




/// TEMPORARY HERE
public enum PowerUp
{
    Switch,
    Joker,
    TileBomb,
    SliceBomb,
}

public enum LootPacks
{
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
    public TileColor rightTileColor;
    public TileSymbol rightTileSymbol;
    public TileColor leftTileColor;
    public TileSymbol leftTileSymbol;
}

[CreateAssetMenu(fileName = "Level", menuName ="ScriptableObjects/Create Level")]
public class LevelSO : ScriptableObject
{
    [Header("Level Setup Settings")]
    public WorldEnum worldName;
    public int levelNumInZone;
    public GameObject boardPrefab;
    public int numOfCells;
    public GameObject clipPrefab;
    public GameObject tilePrefab;
    public GameObject levelSpecificUserControls;
    public List<LevelActions> beforeRingSpawnActions;
    public List<LevelActions> afterRingSpawnActions;
    public TileColor[] levelAvailableColors;
    public TileSymbol[] levelAvailablesymbols;

    [Header("Statue")]
    public bool isUsingSpecificStatue;
    public GameObject specificStatueForLevel;

    [Header("Slices")]
    public bool RandomSlicePositions;
    public bool allowRepeatSlices;
    public sliceToSpawnDataStruct[] slicesToSpawn;

    [Header("PowerUps")]
    public PowerUp[] powerupsForLevel;

    [Header("Stone Tiles")]
    public stoneTileDataStruct[] stoneTiles;

    [Header("Percise Position Settings")]
    public List<tileDataStruct> arrayOfTiles;
    public int[] specificSliceSpots;
    public TileColor[] specificSlicesColors;
    public TileSymbol[] specificSlicesShapes;
}
