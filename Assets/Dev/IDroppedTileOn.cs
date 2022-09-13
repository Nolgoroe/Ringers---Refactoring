using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Everything that is on the Tile Inserting layer HAS to have this interface. Everything that has this interface HAS to have a tileinserting layer
/// </summary>
public interface IDroppedTileOn
{
    bool DroopedOn(TileParentLogic tile);
}
