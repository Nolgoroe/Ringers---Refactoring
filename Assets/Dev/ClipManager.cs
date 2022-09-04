using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorsAndMats
{
    public TileColor matColor;
    public Texture[] colorTex;
}

[System.Serializable]
public class SymbolToMat
{
    public TileSymbol mat;
    public Texture symbolTex;
}

public class ClipManager : MonoBehaviour
{
    [Header("Slots Zone")]
    public ClipSlot[] slots;

    [Header("Textures and Emission Maps")]
    public ColorsAndMats[] colorsToMats;
    public SymbolToMat[] symbolToMat;


    public void InitClipManager()
    {
        SummonTiles();
    }

    [ContextMenu("Summon tiles")]
    public void SummonTiles()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            SpawnRandomTileInSlot(slots[i]);
        }
    }

    public void RePopulateFirstEmpty()
    {
        foreach (ClipSlot slot in slots)
        {
            if(slot.heldTile == null)
            {
                SpawnRandomTileInSlot(slot);
                return;
            }
        }
    }
    public void RePopulateSpecificSlot(ClipSlot slot)
    {
        if (slot.heldTile == null)
        {
            SpawnRandomTileInSlot(slot);
            return;
        }
    }

    private void SpawnRandomTileInSlot(ClipSlot slot)
    {
        Tile tile = Instantiate(GameManager.instance.currentLevel.tilePrefab).GetComponent<Tile>();
        slot.heldTile = tile;
        SetSpawnDataAndDisplay(tile);

        slot.AcceptTileToHolder(tile);
    }


    private void SetSpawnDataAndDisplay(Tile tile)
    {
        //this is the data
        tile.SetTileSpawnData(tile.subTileLeft);
        tile.SetTileSpawnData(tile.subTileRight);

        //this is the display
        SetTileSpawnDisplayByData(tile.subTileLeft);
        SetTileSpawnDisplayByData(tile.subTileRight);
    }

    private void SetTileSpawnDisplayByData(SubTileData subTile)
    {
        int colorIndex = (int)subTile.subTileColor;
        int symbolIndex = (int)subTile.subTileSymbol;

        Material matToChange = subTile.subtileMesh.material;

        Texture colorSymbolTexture = colorsToMats[colorIndex].colorTex[symbolIndex];
        Texture connectionTex = symbolToMat[symbolIndex].symbolTex;

        matToChange.SetTexture("Tile_Albedo_Map", colorSymbolTexture);
        matToChange.SetTexture("MatchedSymbolTex", connectionTex);
    }
}
