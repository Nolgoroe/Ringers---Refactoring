using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public int filledCells;
    public int unsuccessfulConnections;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
