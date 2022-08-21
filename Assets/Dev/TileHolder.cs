using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileHolder : MonoBehaviour
{
    public TileParentLogic heldTile;
    public Transform tileGFXParent;

    public abstract void OnRemoveTile();
    public abstract void RecieveTile(TileParentLogic tileToPlace);
    public abstract void AcceptTileToHolder(TileParentLogic recievedTile);
}
