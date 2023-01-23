using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestBarLogic : MonoBehaviour
{
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private RectTransform starsParent;
    [SerializeField] private Slider chestBarSlider;
    [SerializeField] private float barAnimationSpeed;


    List<ImageSwapHelper> summonedStars;
    private void OnEnable()
    {
        summonedStars = new List<ImageSwapHelper>();
        float barWidth = starsParent.sizeDelta.x;
        float sections = GameManager.instance.ReturnNumOfLevelsInCluster();
        int currentIndex = GameManager.instance.ReturnCurrentIndexInCluster();
        float halfRectSizeWidth = 0;

        // we use this list to know which stars were spawned in this frame.
        // we destroyed stars in the "same" frame so the system doesn't update on time
        // so we keep a refrence to the stars actually spawned, before the "next" frame
        
        if (sections > 0)
        {
            float amout = barWidth / sections;

            //we start from 1 since the "chest" is already considerd a "section"
            for (int i = 1; i < sections; i++)
            {
                GameObject star = Instantiate(starPrefab, starsParent);
                RectTransform starRect = star.GetComponent<RectTransform>();

                halfRectSizeWidth = starRect.sizeDelta.x / 2;

                starRect.anchoredPosition = new Vector2((amout * i) - halfRectSizeWidth, 0);

                ImageSwapHelper swapHelper = star.GetComponent<ImageSwapHelper>();
                summonedStars.Add(swapHelper);

                swapHelper.SetDeActivatedChild();
            }

            for (int i = 0; i < currentIndex; i++)
            {
                summonedStars[i].SetActivatedChild();
            }

            chestBarSlider.value = currentIndex;
        }

        GameManager.instance.chestBarLogic = this;
    }
    private void OnDisable()
    {
        if (starsParent.childCount > 0)
        {
            for (int i = 0; i < starsParent.childCount; i++)
            {
                Destroy(starsParent.GetChild(i).gameObject);
            }
        }
    }

    public void AddToChestBar() 
    {
        LeanTween.value(chestBarSlider.gameObject, chestBarSlider.value, chestBarSlider.value + 1, barAnimationSpeed)
            .setOnComplete(() => CheckGiveChest(chestBarSlider.value))
            .setOnUpdate((float val) =>
        {
            chestBarSlider.value = val;
        });
    }

    private void CheckGiveChest(float barValue)
    {
        float sections = GameManager.instance.ReturnNumOfLevelsInCluster();

        if (barValue == sections)
        {
            //Give Chest here
            //chestAnimator = Instantiate(chestPrefab).GetComponent<Animator>();
        }
        else
        {
            int currentIndex = GameManager.instance.ReturnCurrentIndexInCluster();

            Animator anim;
            summonedStars[currentIndex].TryGetComponent<Animator>(out anim);
            if (anim == null)
            {
                Debug.LogError("Problem");
                return;
            }

            anim.enabled = true;
            // we have the anim disabled at start to make sure the default state
            // of the animation does not impact our logic
            anim.SetTrigger("Activate Star");
        }
    }
}
