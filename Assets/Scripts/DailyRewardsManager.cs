using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DailyRewardsManager : MonoBehaviour
{
    //is it ok to connect between display and logic like we do in this script? ask lior

    [Header("Required refrences")]
    [SerializeField] private DailyRewardsSO[] allWeekSOOptions;
    [SerializeField] private DailyRewardsEntryCustomSpecificDisplayer dailyRewardPrefab;
    [SerializeField] private BasicCustomButton getDailyButton;
    [SerializeField] private Transform GridLayourParent;
    [SerializeField] private TimeManager timeManager; // This is a two way dependency! ask lior!!
    [SerializeField] private Player player; // talk with lior - is there a batter way?
    [SerializeField] private PowerupManager powerupManager; // talk with lior - is there a batter way?

    [Header("Automatic elements")]
    [SerializeField] private DailyRewardsEntryCustomSpecificDisplayer currentVFXDisplay;
    [SerializeField] private int currentDay;
    [SerializeField] private DailyRewardsSO currentWeekSO;
    [SerializeField] private int chosenWeekIndex;
    [SerializeField] private bool canRecieveDaily;
    [SerializeField] private List<DailyRewardsEntryCustomSpecificDisplayer> spawnedDisplayers; // go over with Lior
    private CanvasGroup dailyButtonCanvasGroup; // go over with Lior

    private void Awake()
    {
        if (PlayerPrefs.HasKey("latestCurrentDay")) // will become server...
        {
            currentDay = Convert.ToInt32(PlayerPrefs.GetInt("latestCurrentDay"));
        }

        if (PlayerPrefs.HasKey("latestChosenWeek")) // will become server...
        {
            // if we haven't ever chosen a week, just choose the first one and random from there.
            chosenWeekIndex = Convert.ToInt32(PlayerPrefs.GetInt("latestChosenWeek"));
        }

    }

    private void Start()
    {
        currentWeekSO = allWeekSOOptions[chosenWeekIndex];
        DisplayDailyRewards();
    }

    private void DisplayDailyRewards()
    {
        StartCoroutine(ClearAllRewards());

        for (int i = 0; i < currentWeekSO.rewards.Length; i++)
        {
            DailyRewardsEntryCustomSpecificDisplayer display = Instantiate(dailyRewardPrefab, GridLayourParent);

            if (display == null)
            {
                Debug.LogError("Failed to summon display!");
                return;
            }

            int amount = currentWeekSO.rewards[i].rewardAmount;
            int dayNum = i + 1;

            string[] texsts = new string[] { amount.ToString(), dayNum.ToString() };

            Sprite sprite = currentWeekSO.rewards[i].rewardSprite;

            Sprite[] sprites = new Sprite[] { sprite };
            display.OverrideSetMyElement(texsts, sprites, null);

            if (i == currentDay)
            {
                display.SetDisplayTodaysReward(true);
                currentVFXDisplay = display;
            }
            else if(i < currentDay)
            {
                display.SetAsAlreadyGiven();
            }
            else
            {
                display.SetDisplayTodaysReward(false);
            }

            spawnedDisplayers.Add(display);
        }

        SetDailyButtonOnStart();
    }

    private IEnumerator ClearAllRewards() // go over with lior
    {
        for (int i = 0; i < GridLayourParent.childCount; i++)
        {
            Destroy(GridLayourParent.GetChild(i).gameObject);
        }

        spawnedDisplayers.Clear();

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator RecieveReward()
    {
        // called from button in scene

        #region Display
        if (currentVFXDisplay == null)
        {
            Debug.LogError("No display has been selected!");
            yield break;
        }
        yield return StartCoroutine(currentVFXDisplay.AfterRecievedReward());

        LeanTween.cancel(getDailyButton.gameObject); // if it's during tween and I click on it..
        dailyButtonCanvasGroup.alpha = 0;

        #endregion

        #region Give Rewards To Player
        RewardPlayer();
        #endregion

        #region DailyRewardData

        currentDay++;

        if (currentDay == currentWeekSO.rewards.Length)
        {
            currentDay = 0;

            // refresh the reward data to another list of rewards.. this is where we end a list of rewards

            ChooseWeekSO();
        }
        else
        {
            currentVFXDisplay = spawnedDisplayers[currentDay];
            currentVFXDisplay.SetDisplayTodaysReward(true);
        }

        canRecieveDaily = false;
        timeManager.SetTargetDailyRewardTime();

        #endregion


        //give rewards here


    }

    private void RewardPlayer() // go over this with Lior - this MUST (?) change!
    {
        int amount = currentWeekSO.rewards[currentDay].rewardAmount;

        Ingredients ingredient = currentWeekSO.rewards[currentDay].rewardData as Ingredients;

        if(ingredient != null)
        {
            LootToRecieve loot = new LootToRecieve(ingredient, amount);
            player.AddIngredient(loot);

            return;
        }

        PowerupScriptableObject powerup = currentWeekSO.rewards[currentDay].rewardData as PowerupScriptableObject;
        
        if (powerup != null)
        {
            //give powerup to player
            powerupManager.AddPotion(powerup.powerType);
            return;
        }

    }
    private void SetDailyButtonOnStart()
    {
        getDailyButton.TryGetComponent(out dailyButtonCanvasGroup);

        if (dailyButtonCanvasGroup == null)
        {
            Debug.LogError("No Canvas Group");
            return;
        }

        if (canRecieveDaily)
        {
            dailyButtonCanvasGroup.alpha = 1;
        }
        else
        {
            dailyButtonCanvasGroup.alpha = 0;
        }
    }
    public void MakeDailyAvailable()
    {
        canRecieveDaily = true;
        getDailyButton.isInteractable = true;

        if(currentVFXDisplay)
        {
            // we have this check since we can get to this function before the displays even summon
            // from the time manager, and we can get here at runtime.
            // the only difference is if we have a display or not.
            currentVFXDisplay.SetDisplayTodaysReward(true);


            if (currentDay == 0)
            {
                DisplayDailyRewards();
            }

            FadeInClaimButton();
        }
    }

    private void FadeInClaimButton()
    {
        if (dailyButtonCanvasGroup == null) return;
        getDailyButton.GeneralFloatValueTo(dailyButtonCanvasGroup.gameObject, dailyButtonCanvasGroup.alpha, 1, 0.5f, LeanTweenType.linear, dailyButtonCanvasGroup, null);
    }
    private void ChooseWeekSO()
    {
        int rand = UnityEngine.Random.Range(0, allWeekSOOptions.Length);
        currentWeekSO = allWeekSOOptions[rand];
        chosenWeekIndex = rand;
    }


    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("latestCurrentDay", currentDay);
        PlayerPrefs.SetInt("latestChosenWeek", chosenWeekIndex);
    }



    public bool GetIsDailyAvailable => canRecieveDaily;
}
