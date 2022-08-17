using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InLevelUserControls : MonoBehaviour
{
    [Header("Raycast data")]
    public LayerMask tileLayer;
    //public LayerMask boardCellLayer;
    public LayerMask tileHolderLayer;
    public string cellTag;
    public float overlapRadius;

    [Header("Follow settings")]
    public float pickupSpeed;
    public float tileFollowSpeed;
    public Vector3 tileFollowOffset;

    [Header("General")]
    public Tile currentTileToMove;
    public TileHolder tileParent;

    [Header("Needed Classes")]
    public ClipManager clipManager;

    Vector3 touchPos;
    float tileDragDist;
    Vector3 OriginalPos;
    Quaternion OriginalRot;

    void Update()
    {
        if (!UIDisplayer.USINGUI)
        {
            NormalControls();
            return;
        }
    }

    private void NormalControls()
    {
        Touch touch;


        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            touchPos = touch.position;

            if (touch.phase == TouchPhase.Began)
            {

                RaycastHit2D[] intersectionsArea = GetIntersectionsArea(touchPos, tileHolderLayer);
                // we also already have a point on raycast function called "GetIntersectionsAtPoint"

                if (intersectionsArea.Length > 0 && intersectionsArea[0].transform)
                {
                    TileHolder holder = intersectionsArea[0].transform.GetComponent<TileHolder>();

                    if (holder.heldTile)
                    {
                        GrabTile(holder);
                    }
                }
            }
            
            if((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && currentTileToMove)
            {
                RotateTileTowardsBoard();

                Vector3 targetPosClac = OriginalPos + tileFollowOffset;

                Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, targetPosClac.z));

                currentTileToMove.transform.position = Vector3.Lerp(currentTileToMove.transform.position, targetPos, Time.deltaTime * tileFollowSpeed);


                RaycastHit2D[] intersectionsArea = GetIntersectionsArea(touchPos, tileHolderLayer);
                // we also already have a point on raycast function called "GetIntersectionsAtPoint"

                /// do VFX according to hits here.

            }

            if (touch.phase == TouchPhase.Ended && currentTileToMove)
            {
                RaycastHit2D[] intersectionsArea = GetIntersectionsArea(touchPos, tileHolderLayer);
                // we also already have a point on raycast function called "GetIntersectionsAtPoint"

                if (intersectionsArea.Length > 0)
                {
                    Transform hit = intersectionsArea[0].transform;

                    if(hit && hit != tileParent.transform && hit.transform.CompareTag(cellTag))
                    {
                        Cell cell = hit.transform.GetComponent<Cell>();
                        cell.OnPlaceTileInCell(currentTileToMove);

                        if(tileParent is ClipSlot) //if parent can be casted to a "ClipSlot" type, meanin it came from the clip
                        {
                            clipManager.RePopulateSpecificSlot((ClipSlot)tileParent);
                        }

                        ReleaseData();
                    }
                    else
                    {
                        ReturnHome();
                    }
                }
                else
                {
                    ReturnHome();
                }
            }
        }

    }

    private RaycastHit2D GetIntersectionsAtPoint(Vector3 touchPos, LayerMask layerToHit)
    {
        Ray touchRay;

        touchRay = Camera.main.ScreenPointToRay(touchPos);

        RaycastHit2D hit2D = Physics2D.GetRayIntersection(touchRay, Mathf.Infinity, layerToHit);

        return hit2D;

    }

    private RaycastHit2D[] GetIntersectionsArea(Vector3 touchPos, LayerMask layerToHit)
    {
        Vector3 pointToCheck = Input.mousePosition;
        pointToCheck.z = 35;

        Vector3 posCheck = Camera.main.ScreenToWorldPoint(pointToCheck);

        RaycastHit2D[] hit2D = Physics2D.CircleCastAll(posCheck, overlapRadius, transform.right, 0, layerToHit);


        foreach (RaycastHit2D t in hit2D)
        {
            Debug.Log(t.transform.name);
        }

        return hit2D;

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
    private void GrabTile(TileHolder holder)
    {
        currentTileToMove = holder.heldTile;
        tileParent = holder;

        OriginalPos = holder.heldTile.transform.position;
        OriginalRot = holder.heldTile.transform.rotation;

        holder.OnRemoveTile();

        Vector3 targetPosClac = currentTileToMove.transform.position + tileFollowOffset;

        Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, targetPosClac.z));

        LeanTween.move(currentTileToMove.gameObject, targetPos, pickupSpeed);

        RotateTileTowardsBoard();
    }
    private void RotateTileTowardsBoard()
    {
        float difY = GameManager.instance.gameBoard.position.y - currentTileToMove.transform.position.y;
        float difX = GameManager.instance.gameBoard.position.x - currentTileToMove.transform.position.x;

        float angle = Mathf.Atan2(difY, difX) * Mathf.Rad2Deg;

        currentTileToMove.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
    }

    private void ReturnHome()
    {
        LeanTween.cancel(currentTileToMove.gameObject);

        tileParent.heldTile = currentTileToMove;
        currentTileToMove.transform.position = OriginalPos;
        currentTileToMove.transform.rotation = OriginalRot;

        ReleaseData();
    }

    private void ReleaseData()
    {
        OriginalPos = Vector3.zero;
        OriginalRot = Quaternion.identity;
        currentTileToMove = null;
        tileParent = null;
    }
}
