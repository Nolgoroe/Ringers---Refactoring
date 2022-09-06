using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SubTileColor
{
    Purple,
    Blue,
    Yellow,
    Green,
    Pink,
    Stone,
    NoColor,
    Joker,
}
public enum SubTileSymbol
{
    DragonFly,
    Bear,
    Ram,
    Turtle,
    NoShape,
    Joker,
}
public enum Tiletype
{
    Normal,
    Stone,
    NoType
}

[CreateAssetMenu(fileName = "Tile creator preset", menuName = "ScriptableObjects/Create tile creator")]
public class TileCreator : ScriptableObject
{
    [Header("Textures and Emission Maps")]
    public ColorsAndMats[] colorsToMats;
    public SymbolToMat[] symbolToMat; //rename symbol to texture also class name
    public GameObject[] tilePrefabs;

    public Tile CreateTile(Tiletype tileType, SubTileSymbol[] availableSymbols, SubTileColor[] availableColors)
    {
        Tile tile = Instantiate(tilePrefabs[(int)tileType]).GetComponent<Tile>();

        //data set, then decide on textures, then display set
        tile.SetSubTileSpawnData(tile.subTileLeft, RollTileSymbol(availableSymbols), RollTileColor(availableColors));
        Texture[] tempArray = ReturnTexturesByData(tile.subTileLeft);
        tile.SetTileSpawnDisplayByTextures(tile.subTileLeft, tempArray[0], tempArray[1]);

        //data set, then decide on textures, then display set
        tile.SetSubTileSpawnData(tile.subTileRight, RollTileSymbol(availableSymbols), RollTileColor(availableColors));
        tempArray = ReturnTexturesByData(tile.subTileRight);
        tile.SetTileSpawnDisplayByTextures(tile.subTileRight, tempArray[0], tempArray[1]);

        return tile;
    }

    private SubTileSymbol RollTileSymbol(SubTileSymbol[] availableSymbols)
    {
        SubTileSymbol randomSymbol = SubTileSymbol.NoShape;

        if(availableSymbols != null && availableSymbols.Length > 0)
        {
            int random = Random.Range(0, availableSymbols.Length);

            randomSymbol = availableSymbols[random];
        }

        return randomSymbol;
    }
    private SubTileColor RollTileColor(SubTileColor[] availableColors)
    {
        SubTileColor randomColor = SubTileColor.NoColor;

        if (availableColors!= null && availableColors.Length > 0)
        {
            int random = Random.Range(0, availableColors.Length);

            randomColor = availableColors[random];
        }

        return randomColor;
    }

    private Texture[] ReturnTexturesByData(SubTileData tileData)
    {
        SubTileSymbol tileSymbol = tileData.subTileSymbol;
        SubTileColor tileColor = tileData.subTileColor;

        Texture colorSymbolTexture = colorsToMats[(int)tileColor].colorTex[(int)tileSymbol];
        Texture connectionTex = symbolToMat[(int)tileSymbol].symbolTex;

        return new Texture[] { colorSymbolTexture, connectionTex };
    }


}
