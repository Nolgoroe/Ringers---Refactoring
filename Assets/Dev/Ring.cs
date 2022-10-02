using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SliceSpriteSetter
{
    public SliceConditionsEnums sliceEnum;
    public Sprite[] slicePossibleSprites;
}
public class Ring : MonoBehaviour
{
    public int filledCellsCount;
    public int unsuccessfulConnectionsCount;

    public Cell[] ringCells;
    public Slice[] ringSlices;

    public System.Action onAddTile;
    public System.Action onRemoveTile; // not yet implemented

    public GameObject sliceDisplayPrefab;

    public SliceSpriteSetter[] sliceDisplayArray;
    public void InitRing()
    {
        onAddTile += OnAddTileToRing;
        onAddTile += ChangeCellCountAndConnectionDataOnRemove;

        onRemoveTile += UpdateFilledAndConnectDataCount;
    }

    private void OnAddTileToRing()
    {
        filledCellsCount++;

        if (filledCellsCount == GameManager.gameRing.ringCells.Length && unsuccessfulConnectionsCount == 0)
        {
            Debug.Log("Win Level");
        }
    }

    public void DropTileIntoCell(int cellIndex, TileParentLogic tile, bool isLocked)
    {
        ringCells[cellIndex].DroopedOn(tile);
        ringCells[cellIndex].SetAsLocked(isLocked);
    }

    public void CallOnAddTileActions()
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

    [ContextMenu("Auto set cell neighbors")]
    private void AutoSetCellNeighbors()
    {
        int counter = 0;

        foreach (Cell cell in ringCells)
        {
            cell.rightCell = ringCells[CheckCellIntInRange(counter + 1, ringCells)];
            cell.leftCell = ringCells[CheckCellIntInRange(counter - 1, ringCells)];

            cell.leftSlice = ringSlices[CheckCellIntInRange(counter, ringSlices)];
            cell.rightSlice = ringSlices[CheckCellIntInRange(counter + 1, ringSlices)];
            counter++;
        }

    }

    private int CheckCellIntInRange<T>(int index, T[] array)
    {
        int returnNum = index;

        if(returnNum < 0)
        {
            returnNum = array.Length - 1;
        }

        if(returnNum > array.Length - 1)
        {
            returnNum = 0;
        }

        return returnNum;
    }

    public void SetSliceDisplay(Slice sliceData, int sliceIndex)
    {
        SpriteRenderer sliceDisplayObject = Instantiate(sliceDisplayPrefab, ringSlices[sliceIndex].transform).GetComponent<SpriteRenderer>();

        SliceSpriteSetter relaventSliceData = sliceDisplayArray.Where(x => x.sliceEnum == sliceData.connectionType).FirstOrDefault();

        if(relaventSliceData == null)
        {
            Debug.LogError("Can't find slice data");
            return;
        }

        switch (sliceData.connectionType)
        {
            case SliceConditionsEnums.GeneralColor:
                sliceDisplayObject.sprite = relaventSliceData.slicePossibleSprites[0];
                break;
            case SliceConditionsEnums.GeneralSymbol:
                sliceDisplayObject.sprite = relaventSliceData.slicePossibleSprites[0];
                break;
            case SliceConditionsEnums.SpecificColor:
                sliceDisplayObject.sprite = relaventSliceData.slicePossibleSprites[(int)sliceData.requiredColor];
                break;
            case SliceConditionsEnums.SpecificSymbol:
                sliceDisplayObject.sprite = relaventSliceData.slicePossibleSprites[(int)sliceData.requiredSymbol];
                break;
            default:
                Debug.LogError("Problem with slice generation");
                break;
        }
    }
}
