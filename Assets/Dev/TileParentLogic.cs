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
    public MeshRenderer subtileMesh;
}
public abstract class TileParentLogic : MonoBehaviour
{
    public SubTileData subTileLeft, subTileRight;
    public bool partOfBoard;

    public abstract void OnPlaceTile();
    public abstract void SetTileSpawnData(SubTileData subTile);

    virtual protected TileSymbol RollTileSymbol()
    {
        TileSymbol[] tempSymbolList = GameManager.instance.currentLevel.levelAvailablesymbols;

        int random = Random.Range(0, tempSymbolList.Length);

        TileSymbol randomSymbol = tempSymbolList[random];

        return randomSymbol;
    }
    virtual protected TileColor RollTileColor()
    {
        TileColor[] tempColorList = GameManager.instance.currentLevel.levelAvailableColors;

        int random = Random.Range(0, tempColorList.Length);

        TileColor randomColor = tempColorList[random];

        return randomColor;
    }

    virtual public void SetSubtilesAsConnectedGFX(SubTileData ownSubTile, SubTileData contestedSubTile)
    {
        Material matToChangeOwn = ownSubTile.subtileMesh.material;
        Material matToChangeContested = contestedSubTile.subtileMesh.material;
        matToChangeOwn.SetInt("Is_Piece_Match", 1);
        matToChangeContested.SetInt("Is_Piece_Match", 1);
    }
    virtual public void SetSubtilesAsNOTConnected(SubTileData ownSubTile, SubTileData contestedSubTile)
    {
        Material matToChangeOwn = ownSubTile.subtileMesh.material;
        Material matToChangeContested = contestedSubTile.subtileMesh.material;
        matToChangeOwn.SetInt("Is_Piece_Match", 0);
        matToChangeContested.SetInt("Is_Piece_Match", 0);
    }
}
