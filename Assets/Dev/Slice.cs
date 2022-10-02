using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SliceConditionsEnums
{
    None,
    GeneralColor,
    GeneralSymbol,
    SpecificColor,
    SpecificSymbol,
}

public class Slice : MonoBehaviour
{
    public int index;

    [Header("Debug data, delete later")]
    public SliceConditionsEnums connectionType;
    public SubTileSymbol requiredSymbol;
    public SubTileColor requiredColor;
    public bool isLocking;

    [Header("permanent data")]
    public ConditonsData sliceData;

    //TEMP - will maybe change to lock sprite animation.
    public SpriteRenderer midIcon;

    public void InitSlice(ConditonsData data, SliceConditionsEnums type, SubTileSymbol symbol, SubTileColor color, bool isLock)
    {
        sliceData = data;
        connectionType = type;
        requiredSymbol = symbol;
        requiredColor = color;
        isLocking = isLock;
    }

    public void SetSprite(Sprite sprite)
    {
        midIcon.sprite = sprite;
        midIcon.gameObject.SetActive(true);
    }
}
