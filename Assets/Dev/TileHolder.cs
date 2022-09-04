using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileHolder : MonoBehaviour
{
    public TileParentLogic heldTile;
    public Transform tileGFXParent;

    // think about creating an action system here aswell for "on remove" + "on recieve" - look at gamemanger as example.

    public abstract TileParentLogic OnRemoveTile();
    public abstract void RecieveTile(TileParentLogic tileToPlace);
    public abstract void AcceptTileToHolder(TileParentLogic recievedTile);
}
