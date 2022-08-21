using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditonsData
{
    public bool conditionIsValidated;

    public virtual bool CheckCondition(SubTileData subTileCurrent, SubTileData subTileContested)
    {
        Debug.LogError("Coulden't find override for conditions");
        return false;
    }

    //public virtual bool CheckCondition(TileSymbol requiredSymbol, SubTileData subTileCurrent, SubTileData subTileContested)
    //{
    //    Debug.LogError("Coulden't find override for conditions");

    //    return false;
    //}

    //public virtual bool CheckCondition(TileColor requiredColor, SubTileData subTileCurrent, SubTileData subTileContested)
    //{
    //    Debug.LogError("Coulden't find override for conditions");

    //    return false;
    //}
}

[System.Serializable]
public class ColorAndShapeCondition : ConditonsData
{
    public override bool CheckCondition(SubTileData subTileCurrent, SubTileData subTileContested)
    {
        if(subTileCurrent.subTileColor == subTileContested.subTileColor)
        {
            return true;
        }

        if(subTileCurrent.subTileSymbol == subTileContested.subTileSymbol)
        {
            return true;
        }

        return false;
    }
}

[System.Serializable]
public class GeneralColorCondition : ConditonsData
{
    public override bool CheckCondition(SubTileData subTileCurrent, SubTileData subTileContested)
    {
        if (subTileCurrent.subTileColor == subTileContested.subTileColor)
        {
            return true;
        }

        return false;
    }
}

[System.Serializable]
public class GeneralSymbolCondition : ConditonsData
{
    public override bool CheckCondition(SubTileData subTileCurrent, SubTileData subTileContested)
    {
        if (subTileCurrent.subTileSymbol == subTileContested.subTileSymbol)
        {
            return true;
        }

        return false;
    }
}

[System.Serializable]
public class SpecificColorCondition : ConditonsData
{
    public TileColor requiredColor;

    public override bool CheckCondition(SubTileData subTileCurrent, SubTileData subTileContested)
    {
        if (subTileCurrent.subTileColor == requiredColor && subTileContested.subTileColor == requiredColor)
        {
            return true;
        }

        return false;
    }
}

[System.Serializable]
public class SpecificSymbolCondition : ConditonsData
{
    public TileSymbol requiredSymbol;

    public override bool CheckCondition(SubTileData subTileCurrent, SubTileData subTileContested)
    {
        if (subTileCurrent.subTileSymbol == requiredSymbol && subTileContested.subTileSymbol == requiredSymbol)
        {
            return true;
        }

        return false;
    }
}

