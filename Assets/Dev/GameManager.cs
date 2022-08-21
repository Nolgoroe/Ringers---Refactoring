using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("In game Data")]
    public Transform gameBoard;
    public LevelSO currentLevel; // maybe temporary
    void Start()
    {
        instance = this;

        StartLevel();
    }

    public void SetLevel(LevelSO level)
    {
        currentLevel = level;
        //choose level here
    }

    public void StartLevel()
    {
        //// All of these should be part of the list "Before ring spawn actions" or "after...."???? (either? or? none?)
        




        // Spawn ring by type from level
        Ring ring = Instantiate(currentLevel.boardPrefab).GetComponent<Ring>();
        if (!ring)
        {
            Debug.LogError("No ring!");
        }

        gameBoard = ring.transform.transform;

        // Spawn clip by type from level (or a general clip)
        ClipManager clip = Instantiate(currentLevel.clipPrefab).GetComponent<ClipManager>();
        if(!clip)
        {
            Debug.LogError("No Clip!");
        }

        // Init clip - spawn according to rules
        clip.InitClipManager();

        //Spawn User Controls For Level
        InLevelUserControls userControls = Instantiate(currentLevel.levelSpecificUserControls).GetComponent<InLevelUserControls>();
        if(!userControls)
        {
            Debug.LogError("No User Controls!");
        }


        //local Init User Controls For Level - we don't do enoguh to merit own Init function
        userControls.clipManager = clip;

        //Init slices that pass information to cells (run 2)
    }

}
