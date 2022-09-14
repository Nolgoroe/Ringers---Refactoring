using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public int filledCellsCount;
    public int unsuccessfulConnectionsCount;

    public Cell[] ringCells;

    public System.Action onAddTile;
    public System.Action onRemoveTile; // not yet implemented

    public void InitRing()
    {
        onAddTile += OnAddTileToRing;
        onAddTile += ChangeCellCountAndConnectionDataOnRemove;

        onRemoveTile += UpdateFilledAndConnectDataCount;
    }

    private void OnAddTileToRing()
    {
        filledCellsCount++;

        if (filledCellsCount == GameManager.currentLevel.numOfCells && unsuccessfulConnectionsCount == 0)
        {
            Debug.Log("Win Level");
        }
    }

    public void InsertTileToCell(int cellIndex, TileParentLogic tile, bool isLocked)
    {
        ringCells[cellIndex].DroopedOn(tile);
        ringCells[cellIndex].SetAsLocked(isLocked);
    }

    public void InsertTileToCell()
    {
        onAddTile?.Invoke();
    }

    public void CallOnRemoveTileFromRing()
    {
        onRemoveTile?.Invoke();
    }

    private void UpdateFilledAndConnectDataCount()
    {
        filledCellsCount--;

        ChangeCellCountAndConnectionDataOnRemove();
    }

    private void ChangeCellCountAndConnectionDataOnRemove()
    {
        unsuccessfulConnectionsCount = 0;

        foreach (Cell cell in ringCells)
        {
            unsuccessfulConnectionsCount += cell.GetUnsuccessfullConnections();
        }
    }

    public void ClearActions()
    {
        onAddTile = null;
        onRemoveTile = null;
    }

}
