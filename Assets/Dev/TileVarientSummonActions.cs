using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Action", menuName = "ScriptableObjects/Create Level Action")]
public class TileVarientSummonActions : ScriptableObject
{
    [Header("Required refrences")]
    public TileCreator tileCreatorPreset;

    public void SummonStoneTiles()
    {
        Ring ring = FindObjectOfType<Ring>();

        SubTileColor[] availableColors = new SubTileColor[] { SubTileColor.Stone };

        foreach (stoneTileDataStruct stoneTile in GameManager.currentLevel.stoneTiles)
        {           
            Tile tile = tileCreatorPreset.CreateTile(Tiletype.Normal, GameManager.currentLevel.levelAvailablesymbols, availableColors);
            ring.ringCells[stoneTile.cellIndex].RecieveTile(tile);
        }
    }
}

