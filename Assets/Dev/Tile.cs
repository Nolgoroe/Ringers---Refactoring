using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : TileParentLogic
{
    public override void OnPlaceTile()
    {
        partOfBoard = true;
    }

    public override void SetTile(SubTileData subTile)
    {

    }
}
