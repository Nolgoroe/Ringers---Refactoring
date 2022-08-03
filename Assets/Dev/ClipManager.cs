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


    public void SummonTiles()
    {

    }
}
