using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //TEMP - LEARN DEPENDENCY INJECTION

    [Header("In game Data")]
    public static Ring gameRing;
    public static ClipManager gameClip;
    public static LevelSO currentLevel;
    public static InLevelUserControls gameControls;
    public LevelSO tempcurrentlevel; //temp

    private System.Action BeforeRingActions;
    private System.Action RingActions;
    private System.Action AfterRingActions;
    /// <summary>
    /// Never add to this directly - always use the function "AddToEndlevelCleanup(action to add) with the action we want to add".
    /// We do this since we REQUIRE that the last action will be a specific one.
    /// </summary>
    private System.Action endLevelActions;

    [Header("General Data")]
    [SerializeField] private Transform inLevelParent;
    [SerializeField] private ZoneManager zoneManager;

    [SerializeField] private GameObject[] gameRingsPrefabs;
    [SerializeField] private GameObject[] gameRingsSlicePrefabs;
    [SerializeField] private GameObject[] gameRingsClipPrefabs;
    [SerializeField] private GameObject[] gameRingsUserControlsPrefabs;

    void Start()
    {
        // TO DO
        // if we use a scene transfer system then  make sure the Instance is deleted if we transfer a scene
        // consider changing Sigleton access to something else.

        instance = this;
        //currentLevel = tempcurrentlevel;

        //SetLevel(currentLevel);
    }

    private void Update()
    {
        //if(gameClip)
        //{
        //    Debug.Log("game clip is summoned");
        //}
    }

    //called from button click
    public void SetLevel()
    {
        //first clean all subscribes if there are any.
        endLevelActions?.Invoke();

        //currentLevel = level; //choose level here

        //this is the only place in code where we add delegates to the actions of before, during and after ring.
        // this will not actually invoke the unity event functions - it will add it's invoked functions to the action in the order they are created.
        BeforeRingActions += () => currentLevel.beforeRingSpawnActions.Invoke();
        BeforeRingActions += SpawnLevelBG;

        RingActions += BuildLevel;
        RingActions += () => currentLevel.ringSpawnActions.Invoke();

        AfterRingActions += () => currentLevel.afterRingSpawnActions.Invoke();


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

       
        AddToEndlevelActions(DestroyAllCurrentLevel);
        AddToEndlevelActions(gameRing.ClearActions);

        //every level launch, no matter what, we launch the in level UI
        UIManager.instance.DisplayInLevelUI();
    }

    private void BuildLevel()
    {
        //// All of these should be part of the list "Before ring spawn actions" or "after...."???? (either? or? none?)

        // Spawn ring by type from level
        gameRing = Instantiate(gameRingsPrefabs[(int)currentLevel.ringType], inLevelParent).GetComponent<Ring>();
        if (!gameRing)
        {
            Debug.LogError("No ring!");
        }
        gameRing.InitRing();

        // Spawn clip by type from level (or a general clip)
        gameClip = Instantiate(gameRingsClipPrefabs[(int)currentLevel.ringType], inLevelParent).GetComponent<ClipManager>();
        if (!gameClip)
        {
            Debug.LogError("No Clip!");
        }

        // Init clip - spawn according to rules
        gameClip.InitClipManager();


        //Spawn User Controls For Level
        gameControls = Instantiate(gameRingsUserControlsPrefabs[(int)currentLevel.ringType], inLevelParent).GetComponent<InLevelUserControls>();
        if (!gameControls)
        {
            Debug.LogError("No User Controls!");
        }

        //local Init User Controls For Level - we don't do enoguh to merit own Init function
        gameControls.InitUserControls(gameRing, gameClip);

        //Init slices that pass information to cells (run 2)
    }

    private void SpawnLevelBG()
    {
        GameObject go = Resources.Load<GameObject>(zoneManager.ReturnBGPathByType(currentLevel.worldName));
        GameObject levelBG = Instantiate(go, inLevelParent);

        ZoneMaterialData zoneData;
        levelBG.TryGetComponent(out zoneData);

        if(zoneData)
        {
            zoneData.ChangeZoneToBlurryZoneDisplay();
        }
    }
    private void ClearLevelActions()// this must be added last to "endLevelActions"
    {
        BeforeRingActions = null;
        RingActions = null;
        AfterRingActions = null;
        endLevelActions = null;
    }

    public void InitiateDestrucionOfLevel()
    {
        endLevelActions?.Invoke();
    }

    private void DestroyAllCurrentLevel()
    {
        if(inLevelParent.childCount > 0)
        {
            for (int i = 0; i < inLevelParent.childCount; i++)
            {
                Destroy(inLevelParent.GetChild(i).gameObject);
            }
        }

        //gameRing = null;
        //gameClip = null;
        //currentLevel = null;
    }

    // This function makes sure that we have "ClearLevelActions" set as the last action to be made
    private void AddToEndlevelActions(System.Action actionToAdd)
    {
        endLevelActions -= ClearLevelActions;// this has to be the last added func

        endLevelActions += actionToAdd;

        endLevelActions += ClearLevelActions;// this has to be the last added func

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
            for (int i = 0; i < inLevelParent.childCount; i++)
            {
                Destroy(inLevelParent.GetChild(i).gameObject);
            }

            yield return new WaitForEndOfFrame();

            SetLevel();
        }
    }

    public void ClickOnLevelIconMapSetData(LevelSO levelSO)
    {
        currentLevel = levelSO;
        tempcurrentlevel = levelSO; // this is temp
    }

    public static void TestButtonDelegationWorks()
    {
        //THIS IS TEMP
        Debug.Log("Works!");
    }


    /**/
    // general methods area - methods that can be dropped and used in any class - mostly inspector things for now
    /**/

    //public GameObject preafabToInstantiateInspector;

    //[ContextMenu("Instantiate prefab under object")]
    //public void InstantiatePrefabUnderObject ()
    //{
    //    GameObject go = PrefabUtility.InstantiatePrefab(preafabToInstantiateInspector, transform) as GameObject;
    //    go.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
    //}

    //[ContextMenu("Destroy self and move child 1 up in herarchy")]
    //public void DestroySelfAndMoveChildUpInHerarchy()
    //{
    //    transform.GetChild(0).SetParent(transform.parent);
    //    DestroyImmediate(gameObject);
    //}
}


