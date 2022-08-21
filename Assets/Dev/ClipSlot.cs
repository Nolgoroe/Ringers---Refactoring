using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipSlot : TileHolder
{
    public override void AcceptTileToHolder(TileParentLogic recievedTile)
    {
        recievedTile.transform.SetParent(tileGFXParent);

        recievedTile.transform.localPosition = Vector3.zero;
        recievedTile.transform.localRotation = Quaternion.identity;
        recievedTile.transform.localScale = Vector3.one;

        heldTile = recievedTile;
    }

    public override void OnRemoveTile()
    {
        heldTile = null;
    }

    public override void RecieveTile(TileParentLogic recievedTile)
    {
        AcceptTileToHolder(recievedTile);
    }
}
