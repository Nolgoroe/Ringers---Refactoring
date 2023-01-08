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
    /// <summary>
    /// Never add to this directly - always use the function "AddToEndlevelCleanup(action to add) with the action we want to add".
    /// We do this since we REQUIRE that the last action will be a specific one.
    /// </summary>
    private System.Action endLevelActions;

    [SerializeField] Transform gameParent;
    [SerializeField] CustomButton dealButton;

    [SerializeField] GameObject[] gameRingsPrefabs;
    [SerializeField] GameObject[] gameRingsSlicePrefabs;
    [SerializeField] GameObject[] gameRingsClipPrefabs;
    [SerializeField] GameObject[] gameRingsUserControlsPrefabs;

    //move to a settings script
    //public static bool isTapControls;

    void Start()
    {
        // TO DO
        // if we use a scene transfer system then  make sure the Instance is deleted if we transfer a scene
        // consider changing Sigleton access to something else.

        //instance = this;
        currentLevel = tempcurrentlevel;

        SetLevel(currentLevel);
    }

    private void SetLevel(LevelSO level)
    {
        //first clean all subscribes if there are any.
        endLevelActions?.Invoke();

        currentLevel = level; //choose level here

        //this is the only place in code where we add delegates to the actions of before, during and after ring.

        // this will not actually invoke the unity event functions - it will add it's invoked functions to the action in the order they are created.
        BeforeRingActions += () => currentLevel.beforeRingSpawnActions.Invoke();

        RingActions += BuildLevel;
        RingActions += () => currentLevel.ringSpawnActions.Invoke();

        AfterRingActions += () => currentLevel.afterRingSpawnActions.Invoke();
        AfterRingActions += AddDealEvent;


        StartLevel();
    }

    private void StartLevel()
    {
        //Before Ring
        BeforeRingActions?.Invoke();

        //Ring
        RingActions?.Invoke();

        //After Ring
        AfterRingActions?.Invoke();

       
        AddToEndlevelActions(gameRing.ClearActions);
    }

    private void BuildLevel()
    {
        //// All of these should be part of the list "Before ring spawn actions" or "after...."???? (either? or? none?)

        // Spawn ring by type from level
        gameRing = Instantiate(gameRingsPrefabs[(int)currentLevel.ringType], gameParent).GetComponent<Ring>();
        if (!gameRing)
        {
            Debug.LogError("No ring!");
        }
        gameRing.InitRing();

        // Spawn clip by type from level (or a general clip)
        gameClip = Instantiate(gameRingsClipPrefabs[(int)currentLevel.ringType], gameParent).GetComponent<ClipManager>();
        if (!gameClip)
        {
            Debug.LogError("No Clip!");
        }

        // Init clip - spawn according to rules
        gameClip.InitClipManager();


        //Spawn User Controls For Level
        InLevelUserControls userControls = Instantiate(gameRingsUserControlsPrefabs[(int)currentLevel.ringType], gameParent).GetComponent<InLevelUserControls>();
        if (!userControls)
        {
            Debug.LogError("No User Controls!");
        }

        //local Init User Controls For Level - we don't do enoguh to merit own Init function
        userControls.InitUserControls(gameRing, gameClip);

        //Init slices that pass information to cells (run 2)
    }

    private void ClearLevelActions()// this must be added last to "endLevelActions"
    {
        BeforeRingActions = null;
        RingActions = null;
        AfterRingActions = null;
        endLevelActions = null;
    }

    // This function makes sure that we have "ClearLevelActions" set as the last action to be made
    private void AddToEndlevelActions(System.Action actionToAdd)
    {
        endLevelActions -= ClearLevelActions;// this has to be the last added func

        endLevelActions += actionToAdd;

        endLevelActions += ClearLevelActions;// this has to be the last added func

    }

    private void AddDealEvent()
    {
        dealButton.buttonEvents.AddListener(gameClip.CallDealAction);
    }



    [ContextMenu("Restart")]
    public void CallRestartLevel()
    {
        StartCoroutine(RestartLevel());
    }
    private IEnumerator RestartLevel()
    {
        for (int k = 0; k < 1; k++)
        {
            yield return new WaitForSeconds(1);
            for (int i = 0; i < gameParent.childCount; i++)
            {
                Destroy(gameParent.GetChild(i).gameObject);
            }

            yield return new WaitForEndOfFrame();

            SetLevel(currentLevel);
        }
    }
}
