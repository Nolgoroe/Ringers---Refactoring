using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring12Cell : Cell, IDroppedTileOn
{
    public override bool DroopedOn(TileParentLogic tile)
    {
        Tile tile12Ring = GameManager.gameClip.tileCreatorPreset.CreateTile(Tiletype.Normal12, tile.subTileLeft.subTileSymbol, tile.subTileRight.subTileSymbol, tile.subTileLeft.subTileColor, tile.subTileRight.subTileColor);
        
        // Make sure that the new 12 ring tile spawned is located exactly where the 8 ring tile was released.
        // we do this to make sure the tile activates the drop in animation from the correct position.
        tile12Ring.transform.position = tile.transform.position;
        tile12Ring.transform.rotation = tile.transform.rotation;

        if (base.DroopedOn(tile12Ring))
        {
            Destroy(tile.gameObject);
            return true;
        }

        Destroy(tile12Ring.gameObject);
        return false;
    }
}
