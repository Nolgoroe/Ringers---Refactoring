using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    //public static GameManager instance;

    [Header("In game Data")]
    public static Ring gameRing;
    public static ClipManager gameClip;
    public static LevelSO currentLevel;
    public LevelSO tempcurrentlevel; //temp

    private System.Action BeforeRingActions;
    private System.Action RingActions;
    private System.Action AfterRingActions;
    private System.Action endLevelCleanup;

    [SerializeField] Transform gameParent;
    [SerializeField] CustomButton dealButton;

    [SerializeField] GameObject[] gameRings;

    void Start()
    {
        // TO DO
        // if we use a scene transfer system then  make sure the Instance is deleted if we transfer a scene
        // consider changing Sigleton access to something else.

        //instance = this;
        currentLevel = tempcurrentlevel;

        SetLevel(currentLevel);
    }

    public void SetLevel(LevelSO level)
    {
        //this is the only place in code where we add delegates to the actions of before, during and after ring.
        RingActions += LevelSetup;
        currentLevel = level; //choose level here

        // this will not actually invoke the unity event functions - it will add it's invoked functions to the action in the order they are created.
        BeforeRingActions += () => currentLevel.beforeRingSpawnActions.Invoke();
        RingActions += () => currentLevel.ringSpawnActions.Invoke();
        AfterRingActions += () => currentLevel.afterRingSpawnActions.Invoke();


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

        endLevelCleanup += gameRing.ClearActions;

        endLevelCleanup += LevelActionCleanup;// this has to be the last added func
    }

    private void LevelSetup()
    {
        //// All of these should be part of the list "Before ring spawn actions" or "after...."???? (either? or? none?)

        // Spawn ring by type from level
        gameRing = Instantiate(gameRings[(int)currentLevel.ringType], gameParent).GetComponent<Ring>();
        if (!gameRing)
        {
            Debug.LogError("No ring!");
        }
        gameRing.InitRing();

        // Spawn clip by type from level (or a general clip)
        gameClip = Instantiate(currentLevel.clipPrefab, gameParent).GetComponent<ClipManager>();
        if (!gameClip)
        {
            Debug.LogError("No Clip!");
        }

        // Init clip - spawn according to rules
        gameClip.InitClipManager();

        AfterRingActions += AddDealEvent;

        //Spawn User Controls For Level
        InLevelUserControls userControls = Instantiate(currentLevel.levelSpecificUserControls, gameParent).GetComponent<InLevelUserControls>();
        if (!userControls)
        {
            Debug.LogError("No User Controls!");
        }

        //local Init User Controls For Level - we don't do enoguh to merit own Init function
        userControls.UserControlsSetter(gameRing, gameClip);

        //Init slices that pass information to cells (run 2)
    }

    private void LevelActionCleanup()// this must be added last
    {
        BeforeRingActions = null;
        RingActions = null;
        AfterRingActions = null;
        endLevelCleanup = null;
    }

    public void AddToEndlevelCleanup(System.Action actionToAdd)
    {
        endLevelCleanup -= LevelActionCleanup;// this has to be the last added func

        endLevelCleanup += actionToAdd;

        endLevelCleanup += LevelActionCleanup;// this has to be the last added func

    }

    public void AddDealEvent()
    {
        dealButton.buttonEvents.AddListener(gameClip.CallDealAction);
    }


    public static Tiletype returnTileTypeStone()
    {
        Tiletype type = Tiletype.Normal;

        switch (currentLevel.ringType)
        {
            case Ringtype.ring8:
                type = Tiletype.Stone8;
                break;
            case Ringtype.ring12:
                type = Tiletype.Stone12;
                break;
            case Ringtype.NoType:
                break;
            default:
                break;
        }

        return type;
    }
    public static Ringtype returnRingType()
    {
        Ringtype type = Ringtype.NoType;

        switch (currentLevel.ringType)
        {
            case Ringtype.ring8:
                type = Ringtype.ring8;
                break;
            case Ringtype.ring12:
                type = Ringtype.ring12;
                break;
            case Ringtype.NoType:
                break;
            default:
                break;
        }
        return type;
    }
}
