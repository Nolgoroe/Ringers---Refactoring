using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InLevelUserControls : MonoBehaviour
{
    [Header("Raycast data")]
    public LayerMask tileLayer;
    public LayerMask boardCellLayer;
    public LayerMask tileHolderLayer;


    [Header("Follow settings")]
    public float pickupSpeed;
    public float tileFollowSpeed;

    [Header("General")]
    public Transform currentTileToMove;



    Vector3 touchPos;
    float tileDragDist;

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
        Ray mouseRay;

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            touchPos = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                mouseRay = Camera.main.ScreenPointToRay(touchPos);

                RaycastHit hit;

                if (Physics.Raycast(mouseRay, out hit, 1000, tileHolderLayer))
                {
                    TileHolder holder = hit.transform.GetComponent<TileHolder>();

                    if (holder.heldTile)
                    {
                        GrabTile(holder);
                    }
                }
            }
            
            if(touch.phase == TouchPhase.Moved && currentTileToMove)
            {
                Vector3 touchedPosWorld = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, tileDragDist));

                RotateTileTowardsBoard();

                currentTileToMove.position = Vector3.Lerp(currentTileToMove.position, touchedPosWorld, Time.deltaTime * tileFollowSpeed);
            }

            if (touch.phase == TouchPhase.Ended && currentTileToMove)
            {
                currentTileToMove = null;
                tileDragDist = -1;
            }
        }

    }

    private void GrabTile(TileHolder hodler)
    {
        hodler.OnRemoveTile();

        currentTileToMove = hodler.heldTile.transform;
        tileDragDist = currentTileToMove.position.z - Camera.main.transform.position.z;

        Vector3 touchedPosWorld = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, tileDragDist));

        LeanTween.move(currentTileToMove.gameObject, touchedPosWorld, pickupSpeed);

        RotateTileTowardsBoard();
    }
    private void RotateTileTowardsBoard()
    {
        float difY = GameManager.instance.gameBoard.position.y - currentTileToMove.transform.position.y;
        float difX = GameManager.instance.gameBoard.position.x - currentTileToMove.transform.position.x;

        float angle = Mathf.Atan2(difY, difX) * Mathf.Rad2Deg;

        currentTileToMove.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
    }
}
