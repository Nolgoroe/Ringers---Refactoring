using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SliceConditionsEnums
{
    None,
    GeneralColor,
    GeneralShape,
    SpecificColor,
    SpecificShape,
}
public abstract class ConditonsData : MonoBehaviour
{
    public bool conditionIsValidated;

    public virtual bool CheckCondition(SubTileData subTileCurrent, SubTileData subTileContested)
    {
        Debug.LogError("Coulden't find override for conditions");
        return false;
    }

    public virtual bool CheckCondition(TileSymbol requiredSymbol, SubTileData subTileCurrent, SubTileData subTileContested)
    {
        Debug.LogError("Coulden't find override for conditions");

        return false;
    }

    public virtual bool CheckCondition(TileColor requiredColor, SubTileData subTileCurrent, SubTileData subTileContested)
    {
        Debug.LogError("Coulden't find override for conditions");

        return false;
    }
}

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

public class SpecificColorCondition : ConditonsData
{
    public override bool CheckCondition(TileColor requiredColor, SubTileData subTileCurrent, SubTileData subTileContested)
    {
        if (subTileCurrent.subTileColor == requiredColor && subTileContested.subTileColor == requiredColor)
        {
            return true;
        }

        return false;
    }
}
public class SpecificSymbolCondition : ConditonsData
{
    public override bool CheckCondition(TileSymbol requiredSymbol, SubTileData subTileCurrent, SubTileData subTileContested)
    {
        if (subTileCurrent.subTileSymbol == requiredSymbol && subTileContested.subTileSymbol == requiredSymbol)
        {
            return true;
        }

        return false;
    }
}

