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

public class Slice : MonoBehaviour
{
    public SliceConditionsEnums connectionType;
    public TileSymbol requiredSymbol;
    public TileColor requiredColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
