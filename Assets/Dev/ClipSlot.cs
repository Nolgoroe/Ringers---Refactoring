using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipSlot : TileHolder
{
    public override void OnRemoveTile()
    {
        heldTile = null;
    }
}
