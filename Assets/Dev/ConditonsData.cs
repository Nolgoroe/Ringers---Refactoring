using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditonsData
{
    public bool conditionIsValidated;

    public System.Action onGoodConnectionActions;

    public virtual bool CheckCondition(SubTileData subTileCurrent, SubTileData subTileContested)
    {
        Debug.Log("Coulden't find override for conditions - Doing basic");

        ConditonsData sliceData = new ColorAndShapeCondition();

        return sliceData.CheckCondition(subTileCurrent, subTileContested);
    }
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
    public SubTileColor requiredColor;

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
    public SubTileSymbol requiredSymbol;

    public override bool CheckCondition(SubTileData subTileCurrent, SubTileData subTileContested)
    {
        if (subTileCurrent.subTileSymbol == requiredSymbol && subTileContested.subTileSymbol == requiredSymbol)
        {
            return true;
        }

        return false;
    }
}

