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
    public CellBase[] ringCells;
    public Slice[] ringSlices;

    [SerializeField] private int filledCellsCount;
    [SerializeField] private int unsuccessfulConnectionsCount;

    [SerializeField] private System.Action onAddTile;
    [SerializeField] private System.Action onRemoveTile;

    [SerializeField] private GameObject sliceDisplayPrefab; // move somewhere else?

    [SerializeField] private SliceSpriteSetter[] sliceDisplayArray; // move somewhere else?
    public void InitRing()
    {
        onAddTile += OnAddTileToRing;
        onAddTile += ChangeCellCountAndConnectionDataOnRemove;

        onRemoveTile += UpdateFilledAndConnectDataCount;
    }

    public void SpawnTileInCell(int cellIndex, TileParentLogic tile, bool isLocked)
    {
        ringCells[cellIndex].DroppedOn(tile);
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

    public void ClearActions()
    {
        onAddTile = null;
        onRemoveTile = null;
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

    private void OnAddTileToRing()
    {
        filledCellsCount++;

        if (filledCellsCount == GameManager.gameRing.ringCells.Length && unsuccessfulConnectionsCount == 0)
        {
            Debug.Log("Win Level");
        }

        if (filledCellsCount == GameManager.gameRing.ringCells.Length && unsuccessfulConnectionsCount > 0)
        {
            UIManager.instance.DisplayRingHasNonMatchingMessage();
            Debug.Log("lose Level");
        }
    }

    private void UpdateFilledAndConnectDataCount()
    {
        filledCellsCount--;

        ChangeCellCountAndConnectionDataOnRemove();
    }

    private void ChangeCellCountAndConnectionDataOnRemove()
    {
        unsuccessfulConnectionsCount = 0;

        foreach (CellBase cell in ringCells)
        {
            unsuccessfulConnectionsCount += cell.GetUnsuccessfullConnections();
        }
    }

    private int CheckIndexIntInRange<T>(int index, T[] array) // this is a generic action - might want to move to different script
    {
        int returnNum = index;

        if (returnNum < 0)
        {
            returnNum = array.Length - 1;
        }

        if (returnNum > array.Length - 1)
        {
            returnNum = 0;
        }

        return returnNum;
    }

    [ContextMenu("Auto set cell neighbors")]
    private void AutoSetCellNeighbors()
    {
        int counter = 0;

        foreach (CellBase cell in ringCells)
        {
            cell.rightCell = ringCells[CheckIndexIntInRange(counter + 1, ringCells)];
            cell.leftCell = ringCells[CheckIndexIntInRange(counter - 1, ringCells)];

            cell.leftSlice = ringSlices[CheckIndexIntInRange(counter, ringSlices)];
            cell.rightSlice = ringSlices[CheckIndexIntInRange(counter + 1, ringSlices)];
            counter++;
        }

    }

}
