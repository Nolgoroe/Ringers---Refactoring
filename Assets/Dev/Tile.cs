using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : TileParentLogic
{
    public override void OnPlaceTile()
    {
        partOfBoard = true;
    }

    public override void SetTileSpawnData(SubTileData subTile)
    {
        TileSymbol symbol = RollTileSymbol();
        TileColor color = RollTileColor();

        subTile.subTileSymbol = symbol;
        subTile.subTileColor = color;
    }
}
