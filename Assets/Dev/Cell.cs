using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : TileHolder, IDroppedTileOn, IGrabTileFrom
{
    [SerializeField] Cell leftCell, rightCell;

    //public List<SliceConditionsEnums> sliceConditionsLeft, sliceConditionsRight;
    [SerializeField] ConditonsData sliceConditionLeft, sliceConditionRight;

    // by default when we put a tile and there is no contested tile - it will be considered a "bad" connection
    // to prevent hightlights of "good connections".
    [SerializeField] bool goodConnectLeft, goodConnectRight;
    [SerializeField] int amountUnsuccessfullConnections;

    Collider2D cellCollider;
    //TEMP
    int maxDistanceToAnimate;
    [SerializeField]
    int maxAnimateSpeed;

    // think about creating an action system here aswell for "on good connection" + "on bad connection" - look at gamemanger as example.

    [Header("Testing")]
    public GameObject tilePrefab;

    private void Awake()
    {
        cellCollider = GetComponent<Collider2D>();
        // this will arrive from a slice when we're instantiating them!
        //THIS IS A TEST!!!!
        ConditonsData dataLeft = new ColorAndShapeCondition();
        sliceConditionLeft = dataLeft;

        ConditonsData dataRight = new ColorAndShapeCondition();
        sliceConditionRight = dataRight;

        ////example of specific color slice
        //SpecificColorCondition dataRightT = new SpecificColorCondition();
        //dataRightT.requiredColor = SubTileColor.Blue;
        //sliceConditionRight = dataRightT;
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
            heldTile.SetSubtilesAsNOTConnectedGFX(heldTile.subTileLeft, leftCell.heldTile.subTileRight);
        }

        if (rightCell.heldTile)
        {
            heldTile.SetSubtilesAsNOTConnectedGFX(heldTile.subTileRight, rightCell.heldTile.subTileLeft);
        }
    }

    public override void RemoveTile()
    {
        amountUnsuccessfullConnections = 0;

        if (leftCell.heldTile)
        {
            SetConnectData(false, true, heldTile.subTileLeft, leftCell.heldTile.subTileRight, sliceConditionLeft, leftCell.sliceConditionRight);
        }

        if (rightCell.heldTile)
        {
            SetConnectData(false, false, heldTile.subTileRight, rightCell.heldTile.subTileLeft, sliceConditionRight, rightCell.sliceConditionLeft);
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
            good = sliceConditionLeft.CheckCondition(heldTile.subTileLeft, leftCell.heldTile.subTileRight);
            if (!good)
            {
                //bad connection if we're inside here.
                amountUnsuccessfullConnections++;
            }

            SetConnectData(good, true, heldTile.subTileLeft, leftCell.heldTile.subTileRight, sliceConditionLeft, leftCell.sliceConditionRight);
        }

        if (rightCell.heldTile)
        {
            good = sliceConditionRight.CheckCondition(heldTile.subTileRight, rightCell.heldTile.subTileLeft);
            if (!good)
            {
                //bad connection if we're inside here.
                amountUnsuccessfullConnections++;
            }

            SetConnectData(good, false, heldTile.subTileRight, rightCell.heldTile.subTileLeft, sliceConditionRight, rightCell.sliceConditionLeft);
        }

    }

    private void SetConnectData(bool isGood, bool isLeft, SubTileData mySubtile, SubTileData contestedSubTile, ConditonsData sliceCondition, ConditonsData contestedCellsliceCondition)
    {
        if(isGood)
        {
            heldTile.SetSubtilesAsConnectedGFX(mySubtile, contestedSubTile);
        }
        else
        {
            heldTile.SetSubtilesAsNOTConnectedGFX(mySubtile, contestedSubTile);
        }

        sliceCondition.conditionIsValidated = isGood;
        contestedCellsliceCondition.conditionIsValidated = isGood;

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

            GameManager.gameRing.InsertTileToCell();
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
