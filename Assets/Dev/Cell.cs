using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : TileHolder
{
    public Cell leftCell, rightCell;

    public List<SliceConditionsEnums> sliceConditionsLeft, sliceConditionsRight;

    public bool goodConnectLeft, goodConnectRight;
    public bool isLocked;
    public void OnPlaceTileInCell()
    {

    }

    public override void OnRemoveTile()
    {
        throw new System.NotImplementedException();
    }

    private void CheckConnection()
    {

    }
}
