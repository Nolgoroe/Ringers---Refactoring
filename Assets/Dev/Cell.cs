using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : TileHolder, IDroppedTileOn, IGrabTileFrom
{
    public Cell leftCell, rightCell;

    public Slice leftSlice, rightSlice;

    // by default when we put a tile and there is no contested tile - it will be considered a "bad" connection
    // to prevent hightlights of "good connections".
    [SerializeField] bool goodConnectLeft, goodConnectRight;
    [SerializeField] int amountUnsuccessfullConnections;

    Collider2D cellCollider;

    //TEMP

    [SerializeField]
    int maxDistanceToAnimate;
    [SerializeField]
    int maxAnimateSpeed;

    // think about creating an action system here aswell for "on good connection" + "on bad connection" - look at gamemanger as example.

    [Header("Testing")]
    public GameObject tilePrefab;

    private void Awake()
    {
        cellCollider = GetComponent<Collider2D>();
    }

    public override void RecieveTile(TileParentLogic tileToPlace)
    {
        AcceptTileToHolder(tileToPlace); 

        CheckConnections();

        tileToPlace.SetPlaceTileData(true);
    }

    public override void OnRemoveTileDisplay()
    {
        if (leftCell.heldTile)
        {
            heldTile.SetSubtilesConnectedGFX(false, heldTile.subTileLeft, leftCell.heldTile.subTileRight);
        }

        if (rightCell.heldTile)
        {
            heldTile.SetSubtilesConnectedGFX(false, heldTile.subTileRight, rightCell.heldTile.subTileLeft);
        }
    }

    public override void RemoveTile()
    {
        amountUnsuccessfullConnections = 0;

        if (leftCell.heldTile)
        {
            SetConnectData(false, true, heldTile.subTileLeft, leftCell.heldTile.subTileRight, leftSlice);
        }

        if (rightCell.heldTile)
        {
            SetConnectData(false, false, heldTile.subTileRight, rightCell.heldTile.subTileLeft, rightSlice);
        }

        heldTile.SetPlaceTileData(false);

        heldTile = null;


        GameManager.gameRing.CallOnRemoveTileFromRing();
    }

    private void CheckConnections()
    {
        amountUnsuccessfullConnections = 0;

        bool good = false;

        if (leftCell.heldTile)
        {
            good = leftSlice.sliceData.CheckCondition(heldTile.subTileLeft, leftCell.heldTile.subTileRight);
            if (!good)
            {
                //bad connection if we're inside here.
                amountUnsuccessfullConnections++;
            }

            SetConnectData(good, true, heldTile.subTileLeft, leftCell.heldTile.subTileRight, leftSlice);
        }

        if (rightCell.heldTile)
        {
            good = rightSlice.sliceData.CheckCondition(heldTile.subTileRight, rightCell.heldTile.subTileLeft);
            if (!good)
            {
                //bad connection if we're inside here.
                amountUnsuccessfullConnections++;
            }

            SetConnectData(good, false, heldTile.subTileRight, rightCell.heldTile.subTileLeft, rightSlice);
        
        
        }

    }

    private void SetConnectData(bool isGood, bool isLeft, SubTileData mySubtile, SubTileData contestedSubTile, Slice mySlice)
    {
        heldTile.SetSubtilesConnectedGFX(isGood, mySubtile, contestedSubTile);

        mySlice.sliceData.conditionIsValidated = isGood;

        if (isLeft)
        {
            goodConnectLeft = isGood;
            leftCell.goodConnectRight = isGood;
        }
        else
        {
            goodConnectRight = isGood;
            rightCell.goodConnectLeft = isGood;
        }

        if (isGood)
        {
            mySlice.sliceData.onGoodConnectionActions?.Invoke();
        }
    }

    public override void AcceptTileToHolder(TileParentLogic recievedTile)
    {
        recievedTile.transform.SetParent(tileGFXParent);

        // all of this should happen in an animation manager?? or something that will manage animations


        float distanceFromTarget = Vector3.Distance(recievedTile.transform.localPosition, Vector3.zero);
        
        if(distanceFromTarget > maxDistanceToAnimate)
        {
            recievedTile.transform.localPosition = Vector3.zero;
            recievedTile.transform.localRotation = Quaternion.Euler(Vector3.zero);
            recievedTile.transform.localScale = Vector3.one;
        }
        else
        {
            float timeToAnimate = distanceFromTarget / maxAnimateSpeed;
            LeanTween.moveLocal(recievedTile.gameObject, Vector3.zero, timeToAnimate);
            LeanTween.rotateLocal(recievedTile.gameObject, Vector3.zero, timeToAnimate);
            LeanTween.scale(recievedTile.gameObject, Vector3.one, timeToAnimate);
        }

        heldTile = recievedTile;
    }

    public int GetUnsuccessfullConnections()
    {
        return amountUnsuccessfullConnections;
    }

    public bool DroopedOn(TileParentLogic tile)
    {
        if(!heldTile)
        {
            RecieveTile(tile);

            GameManager.gameRing.CallOnAddTileActions();
            return true;
        }

        return false;
    }

    public void GrabTileFrom()
    {
    }

    public void SetAsLocked(bool locked)
    {
        isLocked = locked;

        cellCollider.enabled = !locked;
    }
}
