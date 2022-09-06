using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : TileHolder
{
    [SerializeField] Cell leftCell, rightCell;
    [SerializeField] Ring ringParent;

    //public List<SliceConditionsEnums> sliceConditionsLeft, sliceConditionsRight;
    [SerializeField] ConditonsData sliceConditionLeft, sliceConditionRight;

    // by default when we put a tile and there is no contested tile - it will be considered a "bad" connection
    // to prevent hightlights of "good connections".
    [SerializeField] bool goodConnectLeft, goodConnectRight;
    [SerializeField] bool isLocked;

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

        // this will arrive from a slice when we're instantiating them!
        //THIS IS A TEST!!!!
        ConditonsData dataLeft = new ColorAndShapeCondition();
        sliceConditionLeft = dataLeft;

        ConditonsData dataRight = new ColorAndShapeCondition();
        sliceConditionRight = dataRight;

        //example of specific color slice
        SpecificColorCondition dataRightT = new SpecificColorCondition();
        dataRightT.requiredColor = SubTileColor.Blue;
        sliceConditionRight = dataRightT;
    }

    public override void RecieveTile(TileParentLogic tileToPlace)
    {
        ringParent.OnAddTileToRing();

        AcceptTileToHolder(tileToPlace); 

        CheckConnectionLeft();
        CheckConnectionRight();
    }

    public override TileParentLogic OnRemoveTile()
    {
        ringParent.OnRemoveTileFromRing(); // changes the filled cell count and checks win condition

        UpdateConnectionDataRingOnRemove(); // changes the unsuccessful connections num

        if (leftCell.heldTile)
        {
            SetConnectDataLeft(false); // resets bool data on cell to "empty" (all false) since not on board
        }

        if(rightCell.heldTile)
        {
            SetConnectDataRight(false);
        }

        TileParentLogic temp =  heldTile;
        heldTile = null;
        return temp;
    }

    [ContextMenu("Summon tile")]
    public void SummonTest()
    {
        Tile tile = Instantiate(tilePrefab, transform).GetComponent<Tile>();
        heldTile = tile;
        tile.transform.localPosition = Vector3.zero;
        tile.transform.transform.localRotation = Quaternion.identity;
    }

    private void CheckConnectionLeft()
    {
        if(leftCell.heldTile)
        {
            if (!sliceConditionLeft.CheckCondition(heldTile.subTileLeft, leftCell.heldTile.subTileRight))
            {
                ringParent.unsuccessfulConnectionsCount++;
                
                return;
            }

            SetConnectDataLeft(true);
        }
    }

    private void CheckConnectionRight()
    {
        if(rightCell.heldTile)
        {
            if (!sliceConditionRight.CheckCondition(heldTile.subTileRight, rightCell.heldTile.subTileLeft))
            {
                ringParent.unsuccessfulConnectionsCount++;
                return;
            }


            SetConnectDataRight(true);
        }
    }

    void UpdateConnectionDataRingOnRemove()
    {
        if(leftCell.heldTile && !goodConnectLeft)
        {
            ringParent.unsuccessfulConnectionsCount--;
        }

        if(rightCell.heldTile && !goodConnectRight)
        {
            ringParent.unsuccessfulConnectionsCount--;
        }
    }
    private void SetConnectDataLeft(bool isGood)
    {
        if(isGood)
        {
            heldTile.SetSubtilesAsConnectedGFX(heldTile.subTileLeft, leftCell.heldTile.subTileRight);

            sliceConditionLeft.conditionIsValidated = true;
            leftCell.sliceConditionRight.conditionIsValidated = true;

            goodConnectLeft = true;
            leftCell.goodConnectRight = true;
        }
        else
        {
            heldTile.SetSubtilesAsNOTConnectedGFX(heldTile.subTileLeft, leftCell.heldTile.subTileRight);

            sliceConditionLeft.conditionIsValidated = false;
            leftCell.sliceConditionRight.conditionIsValidated = false;

            goodConnectLeft = false;
            leftCell.goodConnectRight = false;

        }
    }

    private void SetConnectDataRight(bool isGood)
    {
        if(isGood)
        {
            heldTile.SetSubtilesAsConnectedGFX(heldTile.subTileRight, rightCell.heldTile.subTileLeft);

            sliceConditionRight.conditionIsValidated = true;
            rightCell.sliceConditionLeft.conditionIsValidated = true;

            goodConnectRight = true;
            rightCell.goodConnectLeft = true;
        }
        else
        {
            heldTile.SetSubtilesAsNOTConnectedGFX(heldTile.subTileRight, rightCell.heldTile.subTileLeft);

            sliceConditionRight.conditionIsValidated = false;
            rightCell.sliceConditionLeft.conditionIsValidated = false;

            goodConnectRight = false;
            rightCell.goodConnectLeft = false;

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
}
