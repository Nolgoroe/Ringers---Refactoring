using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public int filledCells;
    public int unsuccessfulConnections;


    public void OnAddTileToRing()
    {
        filledCells++;

        if (filledCells == GameManager.instance.currentLevel.numOfCells && unsuccessfulConnections == 0)
        {
            Debug.Log("Win Level");
        }
    }

    public void OnRemoveTileFromRing()
    {
        filledCells--;
    }
}
