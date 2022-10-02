using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring12Cell : Cell, IDroppedTileOn
{
    public bool DroopedOn(TileParentLogic tile)
    {
        Tile tile12Ring = GameManager.gameClip.tileCreatorPreset.CreateTile(Tiletype.Normal12, tile.subTileLeft.subTileSymbol, tile.subTileRight.subTileSymbol, tile.subTileLeft.subTileColor, tile.subTileRight.subTileColor);
        tile12Ring.transform.position = tile.transform.position;
        tile12Ring.transform.rotation = tile.transform.rotation;

        if (base.DroopedOn(tile12Ring))
        {
            Destroy(tile.gameObject);
            return true;
        }

        return false;
    }
}
