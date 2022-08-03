using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileHolder : MonoBehaviour
{
    public Tile heldTile;

    public abstract void OnRemoveTile();
}
