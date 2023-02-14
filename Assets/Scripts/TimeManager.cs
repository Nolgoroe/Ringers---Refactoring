using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    const string DAILY_TIMER_PATTERN = "{0:D2}:{1:D2}:{2:D2}";
    const string DROPS_TIMER_PATTERN = "{0:D2}:{1:D2}";

    [SerializeField] private DateTime targetDailyRewardTime;
    [SerializeField] private DateTime currentDateTime;
    [SerializeField] private DateTime targetDewDropTime;

    [SerializeField] private int constTimeLeftGiveDewDrop;
    [SerializeField] private int currentTimeLeftGiveDewDrop; //we might not need this variable which will save us alot of trouble - ask lior

    [Header("Required refs")]
    [SerializeField] private Player player; // go over with lior on why is this here?
    [SerializeField] private TMP_Text dewDropsTimeText; // go over with lior on why is this here?
    [SerializeField] private TMP_Text dailyRewardText; // go over with lior on why is this here?

    [Header("TEMP")]
    public string timeString;
    public string lastTimeString;

    long lastTime;
    void Start()
    {
        //initial values
        TimeSpan difference = TimeSpan.Zero;
        currentDateTime = System.DateTime.UtcNow;

        if (PlayerPrefs.HasKey("NextTimeDaily")) // will become server...
        {
            lastTime = Convert.ToInt64(PlayerPrefs.GetString("NextTimeDaily"));

            targetDailyRewardTime = DateTime.FromBinary(lastTime);

        }
        else
        {
            /**/
            // this is what needs to happen after I take daily loot.
            // will also need to save the target daily reward time and check it every time we open the daily rewards screen / every time we open the game.
            /**/
            targetDailyRewardTime = DateTime.UtcNow.AddDays(1);
        }

        CheckDailyRewardsAvailableOnStart();

        if (PlayerPrefs.HasKey("LatestDewTime")) // will become server...
        {
            currentTimeLeftGiveDewDrop = PlayerPrefs.GetInt("LatestDewTime");

        }
        else
        {
            currentTimeLeftGiveDewDrop = constTimeLeftGiveDewDrop;
        }

        if (PlayerPrefs.HasKey("QuitTime")) // will become server...
        {
            //Grab the old time from the player prefs as a long
            //we use long since we can then use "FromBinary" which is easy to use...
            lastTime = Convert.ToInt64(PlayerPrefs.GetString("QuitTime"));

            //Convert the old time from binary to a DataTime variable
            DateTime oldDateTime = DateTime.FromBinary(lastTime);

            //string formated = string.Format(REWARD_TIMER_PATTERN, oldDateTime.Hour, oldDateTime.Minute, oldDateTime.Second);
            lastTimeString = oldDateTime.ToString();

            //Use the Subtract method and store the result as a timespan variable
            difference = currentDateTime.Subtract(oldDateTime);

            CheckGiveAmountOfDewDropsOnStart(MathF.Floor((float)(difference.TotalSeconds / constTimeLeftGiveDewDrop)));
        }
        else
        {
            Debug.Log("No previous time was saved");
        }

        currentTimeLeftGiveDewDrop -= difference.Seconds;

        if (currentTimeLeftGiveDewDrop < 0)
        {
            // if we have 4 seconds left but the difference seconds is 44
            // then 4 - 44 means that we have 40 seconds we need to take off the max time.
            // so then we do max time - 40. we basically do a loop

            currentTimeLeftGiveDewDrop = constTimeLeftGiveDewDrop - (int)MathF.Abs(currentTimeLeftGiveDewDrop);
        }

        //these are only the seconds that are left since we already added the 
        targetDewDropTime = DateTime.UtcNow.AddSeconds(currentTimeLeftGiveDewDrop);
    }

    void Update()
    {
        currentDateTime = System.DateTime.UtcNow;

        TimeSpan dropsTimeSpan = targetDewDropTime.Subtract(currentDateTime);
        //print(deltaTime);
        currentTimeLeftGiveDewDrop = (int)dropsTimeSpan.TotalSeconds;

        if (currentTimeLeftGiveDewDrop <= 0)
        {
            GiveAmountOfDewDros(1);
            Debug.Log("Giving drop now");
        }

        /**/
        //Set texsts
        /**/
        UpdateTexsts();
    }

    private void UpdateTexsts()
    {
        TimeSpan dropsTimeSpan = targetDewDropTime.Subtract(currentDateTime);
        string dropsText = string.Format(DROPS_TIMER_PATTERN, dropsTimeSpan.Minutes, dropsTimeSpan.Seconds);
        dewDropsTimeText.text = dropsText;

        TimeSpan dailySpan = targetDailyRewardTime.Subtract(currentDateTime);
        string dailyText = string.Format(DAILY_TIMER_PATTERN, dailySpan.Hours, dailySpan.Minutes, dailySpan.Seconds);
        dailyRewardText.text = dailyText;
    }
    private void CheckGiveAmountOfDewDropsOnStart(float amount)
    {
        //This function is used only during start.

        if (amount > 0)
        {
            player.AddTears((int)amount);
        }
    }

    private void CheckDailyRewardsAvailableOnStart()
    {
        TimeSpan dailySpan = targetDailyRewardTime - currentDateTime;

        if (dailySpan < TimeSpan.Zero)
        {
            // can give daily loot now since time is negative
        }
    }

    private void GiveAmountOfDewDros(float amount)
    {
        //This function is used during runtime.

        if (amount > 0)
        {
            player.AddTears((int)amount);

            AfterGiveDewDrop();
        }
    }
    private void AfterGiveDewDrop()
    {
        currentTimeLeftGiveDewDrop = constTimeLeftGiveDewDrop;

        if (constTimeLeftGiveDewDrop >= 60)
        {
            int minutes = constTimeLeftGiveDewDrop / 60;
            int seconds = constTimeLeftGiveDewDrop % 60;
            targetDewDropTime = DateTime.UtcNow.AddMinutes(minutes);
            targetDewDropTime = targetDewDropTime.AddSeconds(seconds);
        }
        else
        {
            targetDewDropTime = DateTime.UtcNow.AddSeconds(constTimeLeftGiveDewDrop);
        }

    }

    private void OnApplicationQuit()
    {
        // Save the current system time as a string in the player prefs class
        // This will be on server in future
        PlayerPrefs.SetString("QuitTime", DateTime.UtcNow.ToBinary().ToString());
        PlayerPrefs.SetInt("LatestDewTime", currentTimeLeftGiveDewDrop);
        PlayerPrefs.SetString("NextTimeDaily", targetDailyRewardTime.ToBinary().ToString());

    }
}
