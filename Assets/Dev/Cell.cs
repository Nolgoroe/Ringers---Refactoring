using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : TileHolder
{
    public Cell leftCell, rightCell;

    public List<SliceConditionsEnums> sliceConditionsLeft, sliceConditionsRight;

    public bool goodConnectLeft, goodConnectRight;
    public bool isLocked;

    [Header("Testing")]
    public GameObject tilePrefab;
    public void OnPlaceTileInCell(Tile tileToPlace)
    {
        tileToPlace.transform.SetParent(transform);

        tileToPlace.transform.localPosition = Vector3.zero;
        tileToPlace.transform.localRotation = Quaternion.identity;
        tileToPlace.transform.localScale = Vector3.one;

        heldTile = tileToPlace;

    }

    public override void OnRemoveTile()
    {
        heldTile = null;
    }

    private void CheckConnection()
    {

    }

    [ContextMenu("Summon tile")]
    public void SummonTest()
    {
        Tile tile = Instantiate(tilePrefab, transform).GetComponent<Tile>();
        heldTile = tile;
        tile.transform.localPosition = Vector3.zero;
        tile.transform.transform.localRotation = Quaternion.identity;
    }
}
