using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InLevelUserControls : MonoBehaviour
{
    // THINK ABOUT MAYBE CRATING INHERITENCE FOR "CONTROLS"

    [Header("Raycast data")]
    [SerializeField] private LayerMask tileGrabbingLayer;
    [SerializeField] private LayerMask tileInsertingLayer;
    [SerializeField] private float overlapRadius;

    [Header("Follow settings")]
    [SerializeField] private float pickupSpeed;
    [SerializeField] private float tileFollowSpeed;
    [SerializeField] private Vector3 tileFollowOffset;

    [Header("General")]
    [SerializeField] private TileParentLogic currentTileToMove;
    [SerializeField] private TileHolder tileOriginalHolder;

    [Header("Needed Classes")]
    [SerializeField] private Ring gameRing;
    [SerializeField] private ClipManager gameClip;

    private Vector3 touchPos;
    private Vector3 tileOriginalPos;

    private void Update()
    {
        // we use both isusingUI and isduringfade since we have some ui elements
        // as part of a ui object that we wat to not be able to click when were fadind
        // but do what to be able to click when using ui
        if (!UIManager.ISUSINGUI && !UIManager.ISDURINGFADE)
         {
            NormalControls();
            return;
        }
    }


    public void InitUserControls(Ring _gameRing, ClipManager _gameClip)
    {
        gameRing = _gameRing;
        gameClip = _gameClip;
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

    private void OnTouchBegin()
    {
        RaycastHit2D[] intersectionsArea = GetIntersectionsArea(touchPos, tileGrabbingLayer);
        // we also already have a point on raycast function called "GetIntersectionsAtPoint"

        if (intersectionsArea.Length > 0)
        {
            TileHolder holder = intersectionsArea[0].transform.GetComponent<TileHolder>();

            if (holder.heldTile)
            {
                GrabTile(holder);
            }
        }
        else
        {
            ReleaseData();
        }
    }

    private void GrabTile(TileHolder holder)
    {
        IGrabTileFrom grabbedObject = holder.GetComponent<IGrabTileFrom>();

        if(grabbedObject != null)
        {
            currentTileToMove = holder.heldTile;
            holder.OnRemoveTileDisplay();
            tileOriginalHolder = holder;
            grabbedObject.GrabTileFrom();
            tileOriginalPos = currentTileToMove.transform.position;

            LeanTween.move(currentTileToMove.gameObject, TargetPosOffset(), pickupSpeed);

            RotateTileTowardsBoard();
        }
        else
        {
            Debug.LogError("Tried to grab object that doesn't have grabbable interface");
        }
    }

    private void OnTouchMoveOrStationairy()
    {
        RotateTileTowardsBoard();

        SmoothPieceMover();

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
            CellBase droopedOnObject = intersectionsArea[0].transform.GetComponent<CellBase>();

            if(droopedOnObject == null)
            {
                Debug.LogError("no interface of type dropped on.");
                return;
            }

            //don't place the tile if it's the last one and we have problems in ring
            if (!droopedOnObject.heldTile && gameRing.LastPieceRingProblems())
            {
                UIManager.instance.DisplayRingHasNonMatchingMessage();

                return;
            }

            if (!droopedOnObject.DroppedOn(currentTileToMove))
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

        //ReleaseData();
    }

    private RaycastHit2D[] GetIntersectionsArea(Vector3 touchPos, LayerMask layerToHit)
    {
        Vector3 pointToCheck = touchPos;
        pointToCheck.z = gameRing.transform.position.z;

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
        float difY = gameRing.transform.position.y - currentTileToMove.transform.position.y;
        float difX = gameRing.transform.position.x - currentTileToMove.transform.position.x;

        float angle = Mathf.Atan2(difY, difX) * Mathf.Rad2Deg;

        currentTileToMove.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
    }

    private void ReturnHome()
    {
        LeanTween.cancel(currentTileToMove.gameObject);

        tileOriginalHolder.RecieveTileDisplayer(currentTileToMove);
    }
    public void ReturnHomeBadRingConnections()
    {
        LeanTween.cancel(currentTileToMove.gameObject);

        tileOriginalHolder.RecieveTileDisplayer(currentTileToMove);
    }

    private void ReleaseData()
    {
        tileOriginalPos = Vector3.zero;
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

        Gizmos.DrawWireSphere(posCheck + transform.right * 0, overlapRadius);
    }
}
