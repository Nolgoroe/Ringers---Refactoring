using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("In game Data")]
    public Ring gameRing;
    public ClipManager gameClip;
    public LevelSO currentLevel;

    public static System.Action BeforeRingActions;
    public static System.Action RingActions;
    public static System.Action AfterRingActions;

    void Start()
    {
        // TO DO
        // if we use a scene transfer system then  make sure the Instance is deleted if we transfer a scene
        // consider changing Sigleton access to something else.

        instance = this;

        SetLevel(currentLevel);
    }

    public void SetLevel(LevelSO level)
    {
        //this is the only place in code where we add delegates to the actions of before, during and after ring.


        BeforeRingActions += () => currentLevel.beforeRingSpawnActions.Invoke(); // this will not actually invoke the unity event functions - it will add it's invoked functions to the action in the order they are created.

        currentLevel = level; //choose level here

        RingActions += LevelActionSetup;


        StartLevel();
    }

    public void StartLevel()
    {
        //Before Ring
        BeforeRingActions?.Invoke();

        //Ring
        RingActions?.Invoke();

        //After Ring
        AfterRingActions?.Invoke();

    }

    private void LevelActionSetup()
    {
        //// All of these should be part of the list "Before ring spawn actions" or "after...."???? (either? or? none?)

        // Spawn ring by type from level
        gameRing = Instantiate(currentLevel.boardPrefab).GetComponent<Ring>();
        if (!gameRing)
        {
            Debug.LogError("No ring!");
        }

        // Spawn clip by type from level (or a general clip)
        gameClip = Instantiate(currentLevel.clipPrefab).GetComponent<ClipManager>();
        if (!gameClip)
        {
            Debug.LogError("No Clip!");
        }

        // Init clip - spawn according to rules
        gameClip.InitClipManager();

        //Spawn User Controls For Level
        InLevelUserControls userControls = Instantiate(currentLevel.levelSpecificUserControls).GetComponent<InLevelUserControls>();
        if (!userControls)
        {
            Debug.LogError("No User Controls!");
        }

        //local Init User Controls For Level - we don't do enoguh to merit own Init function
        userControls.UserControlsSetter(gameRing, gameClip);

        //Init slices that pass information to cells (run 2)
    }

    private void LevelActionCleanup()
    {
        BeforeRingActions = null;
        RingActions = null;
        AfterRingActions = null;

    }
}
