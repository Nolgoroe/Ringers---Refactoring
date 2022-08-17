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


    [ContextMenu("Summon tiles")]
    public void SummonTiles()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Tile tile = Instantiate(GameManager.instance.currentLevel.tilePrefab, slots[i].transform).GetComponent<Tile>();
            slots[i].heldTile = tile;
        }
    }

    public void RePopulateFirstEmpty()
    {
        foreach (ClipSlot slot in slots)
        {
            if(slot.heldTile == null)
            {
                Tile tile = Instantiate(GameManager.instance.currentLevel.tilePrefab, slot.transform).GetComponent<Tile>();
                slot.heldTile = tile;
                return;
            }
        }
    }
    public void RePopulateSpecificSlot(ClipSlot slot)
    {
        if (slot.heldTile == null)
        {
            Tile tile = Instantiate(GameManager.instance.currentLevel.tilePrefab, slot.transform).GetComponent<Tile>();
            slot.heldTile = tile;
            return;
        }
    }
}
