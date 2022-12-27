using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InLevelUserControls : MonoBehaviour
{
    // THINK ABOUT MAYBE CRATING INHERITENCE FOR "CONTROLS"

    [Header("Raycast data")]
    //public LayerMask tileLayer;
    public LayerMask tileGrabbingLayer;
    public LayerMask tileInsertingLayer;
    public float overlapRadius;

    [Header("Follow settings")]
    public float pickupSpeed;
    public float tileFollowSpeed;
    public Vector3 tileFollowOffset;

    [Header("General")]
    public TileParentLogic currentTileToMove;
    public TileHolder tileOriginalHolder;

    [Header("Needed Classes")]
    public Ring ringManager;
    public ClipManager clipManager;

    Vector3 touchPos;
    //float tileDragDist;
    Vector3 tileOriginalPos;
    Quaternion tileOriginalRot;

    void Update()
    {
        if (!UIDisplayer.USINGUI)
        {
            //if (GameManager.isTapControls)
            //{
            //    //SecondaryControls();
            //}
            //else
            //{
                NormalControls();
            //}
            return;
        }
    }


    public void UserControlsSetter(Ring gameRing, ClipManager gameClip)
    {
        ringManager = gameRing;
        clipManager = gameClip;
    }

    private void NormalControls()
    {
        Touch touch;


        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            touchPos = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchBegin();
                    break;
                case TouchPhase.Moved:
                    if (currentTileToMove) OnTouchMoveOrStationairy();
                    break;
                case TouchPhase.Stationary:
                    if (currentTileToMove) OnTouchMoveOrStationairy();
                    break;
                case TouchPhase.Ended:
                    if (currentTileToMove) OnTouchEnd();
                    break;
                case TouchPhase.Canceled:
                    Debug.LogError("Cancelled??");
                    break;
                default:
                    break;
            }
        }

    }

    //private void SecondaryControls()
    //{
    //    Touch touch;


    //    if (Input.touchCount > 0)
    //    {
    //        touch = Input.GetTouch(0);

    //        touchPos = touch.position;

    //        switch (touch.phase)
    //        {
    //            case TouchPhase.Began:
    //                if (currentTileToMove)
    //                {
    //                    //OnTouchBeginSecondaryPlace();
    //                }
    //                else
    //                {
    //                    OnTouchBegin();
    //                }
    //                break;
    //            case TouchPhase.Canceled:
    //                Debug.LogError("Cancelled??");
    //                break;
    //            default:
    //                break;
    //        }
    //    }

    //}

    private void OnTouchBegin()
    {
        RaycastHit2D[] intersectionsArea = GetIntersectionsArea(touchPos, tileGrabbingLayer);
        // we also already have a point on raycast function called "GetIntersectionsAtPoint"

        if (intersectionsArea.Length > 0)
        {
            TileHolder holder = intersectionsArea[0].transform.GetComponent<TileHolder>();

            if (holder.heldTile)
            {
                //if(GameManager.isTapControls)
                //{
                //    //GrabTileSecondary(holder);
                //}
                //else
                //{
                    GrabTile(holder);
                //}
            }
        }
    }

    //private void OnTouchBeginSecondaryPlace()
    //{
    //    RaycastHit2D[] intersectionsArea = GetIntersectionsArea(touchPos, tileInsertingLayer);
    //    // we also already have a point on raycast function called "GetIntersectionsAtPoint"

    //    if (intersectionsArea.Length > 0)
    //    {
    //        IDroppedTileOn droopedObject = intersectionsArea[0].transform.GetComponent<IDroppedTileOn>();

    //        if (!droopedObject.DroopedOn(currentTileToMove))
    //        {
    //            ReturnHome();
    //        }
    //        else if (tileOriginalHolder.heldTile)
    //        {
    //            tileOriginalHolder.RemoveTile();
    //        }
    //    }
    //    else
    //    {
    //        ReturnHome();
    //    }

    //    ReleaseData();
    //}
    private void GrabTile(TileHolder holder)
    {
        IGrabTileFrom grabbedObject = holder.transform.GetComponent<IGrabTileFrom>();

        if(grabbedObject != null)
        {
            currentTileToMove = holder.heldTile;
            holder.OnRemoveTileDisplay();
            tileOriginalHolder = holder;
            grabbedObject.GrabTileFrom();
            tileOriginalPos = currentTileToMove.transform.position;
            tileOriginalRot = currentTileToMove.transform.rotation;

            LeanTween.move(currentTileToMove.gameObject, TargetPosOffset(), pickupSpeed);

            RotateTileTowardsBoard();
        }
        else
        {
            Debug.LogError("Tried to grab object that doesn't have grabbable interface");
        }
    }
    //private void GrabTileSecondary(TileHolder holder)
    //{
    //    if(holder.heldTile)
    //    {
    //        currentTileToMove = holder.heldTile;
    //        holder.OnRemoveTileDisplay();
    //        tileOriginalHolder = holder;

    //        OriginalPos = currentTileToMove.transform.position;
    //        OriginalRot = currentTileToMove.transform.rotation;
    //    }
    //}

    private void OnTouchMoveOrStationairy()
    {
        RotateTileTowardsBoard();

        SmoothPieceMover();

        RaycastHit2D[] intersectionsArea = GetIntersectionsArea(touchPos, tileInsertingLayer);
        // we also already have a point on raycast function called "GetIntersectionsAtPoint"

        /// do VFX according to hits here.
    }
    private void SmoothPieceMover()
    {
        currentTileToMove.transform.position = Vector3.Lerp(currentTileToMove.transform.position, TargetPosOffset(), Time.deltaTime * tileFollowSpeed);
    }

    private void OnTouchEnd()
    {
        RaycastHit2D[] intersectionsArea = GetIntersectionsArea(touchPos, tileInsertingLayer);
        // we also already have a point on raycast function called "GetIntersectionsAtPoint"

        if (intersectionsArea.Length > 0)
        {
            //IDroppedTileOn droopedObject = intersectionsArea[0].transform.GetComponent<IDroppedTileOn>();
            IDroppedTileOn droopedObject = intersectionsArea[0].transform.GetComponent<IDroppedTileOn>();

            if(droopedObject == null)
            {
                Debug.LogError("no interface of type dropped on.");
                return;
            }

            if (!droopedObject.DroopedOn(currentTileToMove))
            {
                ReturnHome();
            }
            else
            {
                tileOriginalHolder.RemoveTile();
            }
        }
        else
        {
            ReturnHome();
        }

        ReleaseData();
    }

    private RaycastHit2D[] GetIntersectionsArea(Vector3 touchPos, LayerMask layerToHit)
    {
        Vector3 pointToCheck = Input.mousePosition;
        pointToCheck.z = 35;

        Vector3 posCheck = Camera.main.ScreenToWorldPoint(pointToCheck);

        RaycastHit2D[] hit2D = Physics2D.CircleCastAll(posCheck, overlapRadius, transform.right, 0, layerToHit);


        return hit2D;

    }

    private RaycastHit2D GetIntersectionsAtPoint(Vector3 touchPos, LayerMask layerToHit)
    {
        Ray touchRay;

        touchRay = Camera.main.ScreenPointToRay(touchPos);

        RaycastHit2D hit2D = Physics2D.GetRayIntersection(touchRay, Mathf.Infinity, layerToHit);

        return hit2D;

    }

    private void RotateTileTowardsBoard()
    {
        float difY = ringManager.transform.position.y - currentTileToMove.transform.position.y;
        float difX = ringManager.transform.position.x - currentTileToMove.transform.position.x;

        float angle = Mathf.Atan2(difY, difX) * Mathf.Rad2Deg;

        currentTileToMove.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
    }

    private void ReturnHome()
    {
        LeanTween.cancel(currentTileToMove.gameObject);

        tileOriginalHolder.RecieveTileDisplayer(currentTileToMove);
    }

    private void ReleaseData()
    {
        tileOriginalPos = Vector3.zero;
        tileOriginalRot = Quaternion.identity;
        currentTileToMove = null;
        tileOriginalHolder = null;
    }

    private Vector3 TargetPosOffset()
    {
        float targetPosClacZ = tileOriginalPos.z + tileFollowOffset.z;

        Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x + tileFollowOffset.x, touchPos.y + tileFollowOffset.y, targetPosClacZ));

        return targetPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 pointToCheck = Input.mousePosition;
        pointToCheck.z = 35;

        Vector3 posCheck = Camera.main.ScreenToWorldPoint(pointToCheck);
        //Debug.Log(posCheck);
        Gizmos.DrawWireSphere(posCheck + transform.right * 0, overlapRadius);
    }
}
