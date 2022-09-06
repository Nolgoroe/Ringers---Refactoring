using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public int filledCellsCount;
    public int unsuccessfulConnectionsCount;

    public Cell[] ringCells;

    public void OnAddTileToRing()
    {
        filledCellsCount++;

        if (filledCellsCount == GameManager.currentLevel.numOfCells && unsuccessfulConnectionsCount == 0)
        {
            Debug.Log("Win Level");
        }
    }

    public void OnRemoveTileFromRing()
    {
        filledCellsCount--;
    }

}
