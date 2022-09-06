using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorsAndMats
{
    public SubTileColor matColor;
    public Texture[] colorTex;
}

[System.Serializable]
public class SymbolToMat
{
    public SubTileSymbol mat;
    public Texture symbolTex;
}

public class ClipManager : MonoBehaviour
{
    [Header("Slots Zone")]
    public ClipSlot[] slots;

    [Header("Required refrences")]
    public TileCreator tileCreatorPreset;

    public void InitClipManager()
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
        Tile tile = tileCreatorPreset.CreateTile(Tiletype.Normal, GameManager.currentLevel.levelAvailablesymbols, GameManager.currentLevel.levelAvailableColors);
        slot.AcceptTileToHolder(tile);
    }

}
