using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class stoneTileDataStruct
{
    public int cellIndex;
    public bool randomValues;
    public SubTileSymbol rightTileSymbol;
    public SubTileSymbol leftTileSymbol;
}

[System.Serializable]
public class SubTileData
{
    public SubTileSymbol subTileSymbol;
    public SubTileColor subTileColor;
    public MeshRenderer subtileMesh;
}
public abstract class TileParentLogic : MonoBehaviour
{
    public SubTileData subTileLeft, subTileRight;
    public bool partOfBoard;

    public abstract void SetPlaceTileData(bool place);
    public abstract void SetSubTileSpawnData(SubTileData subTile, SubTileSymbol resultSymbol, SubTileColor resultColor);


    virtual public void SetSubtilesAsConnectedGFX(SubTileData ownSubTile, SubTileData contestedSubTile)
    {
        Material matToChangeOwn = ownSubTile.subtileMesh.material;
        Material matToChangeContested = contestedSubTile.subtileMesh.material;

        matToChangeOwn.SetInt("Is_Piece_Match", 1);
        matToChangeContested.SetInt("Is_Piece_Match", 1);
    }
    virtual public void SetSubtilesAsNOTConnectedGFX(SubTileData ownSubTile, SubTileData contestedSubTile)
    {
        Material matToChangeOwn = ownSubTile.subtileMesh.material;
        Material matToChangeContested = contestedSubTile.subtileMesh.material;

        matToChangeOwn.SetInt("Is_Piece_Match", 0);
        matToChangeContested.SetInt("Is_Piece_Match", 0);
    }

    /// set subtile display function (maybe materials)
}
