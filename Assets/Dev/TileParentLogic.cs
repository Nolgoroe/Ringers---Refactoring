using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileColor
{
    Purple,
    Blue,
    Yellow,
    Green,
    Pink,
    NoColor,
    Stone,
    Joker,
}
public enum TileSymbol
{
    DragonFly,
    Bear,
    Ram,
    Turtle,
    NoShape,
    Joker
}

[System.Serializable]
public class SubTileData
{
    public TileSymbol subTileSymbol;
    public TileColor subTileColor;
}
public abstract class TileParentLogic : MonoBehaviour
{
    public SubTileData subTileLeft, subTileRight;
    public bool partOfBoard;

    public abstract void OnPlaceTile();
    public abstract void SetTile(SubTileData subTile);
}
