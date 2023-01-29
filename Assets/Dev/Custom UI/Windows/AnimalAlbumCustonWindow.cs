using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

[Serializable]
public class AnimalPagesByType
{
    public AnimalTypesInGame animalType;
    public List<SpecificDisplayerAnimalAlbum> animalsInPage;
}

public class AnimalAlbumCustonWindow : BasicCustomUIWindow
{
    private AnimalsManager localAnimalManager;
    [SerializeField] private ImageSwapHelper[] animalTypeSwapHelpers;
    [SerializeField] private Shader animalFadeShader;

    [SerializeField] private Transform pagesParent;
    [SerializeField] private List<Transform> allPages;
    [SerializeField] private List<AnimalPagesByType> animalDisplayers;


    [SerializeField] private List<Image> backgroundImages;
    [SerializeField] private List<Image> hiddenAnimalImages;
    [SerializeField] private List<Image> revealedAnimalImages;


    [SerializeField] private int currentlyOpenPageIndex;
    [SerializeField] private int filledAnimalsCount;
    [SerializeField] private GameObject presentImage;
    [SerializeField] private float revealTime;
    [SerializeField] private CanvasGroup getRewardsButton;

    private AnimalTypesInGame currentOpenType;

    private void Awake()
    {
        SetInitialData();
    }

    public void InitAnimalAlbum(AnimalsManager animalManager)
    {
        localAnimalManager = animalManager;

        SwitchAnimalCategory(0);

    }

    public void SwitchAnimalCategory(int index)
    {
        SetCategoriesDisplay(index);
    }
    private void SetCategoriesDisplay(int index)
    {
        //swap active screen and set new index
        currentlyOpenPageIndex = index;
        filledAnimalsCount = 0;
        getRewardsButton.alpha = 0;
        getRewardsButton.gameObject.SetActive(false);
        presentImage.gameObject.SetActive(true);

        currentOpenType = animalDisplayers[currentlyOpenPageIndex].animalType;
        for (int i = 0; i < animalTypeSwapHelpers.Length; i++)
        {
            if (i == index)
            {
                animalTypeSwapHelpers[i].SetActivatedChild();

                if (i > allPages.Count - 1) continue;
                allPages[i].gameObject.SetActive(true);
            }
            else
            {
                animalTypeSwapHelpers[i].SetDeActivatedChild();

                if (i > allPages.Count - 1) continue;
                allPages[i].gameObject.SetActive(false);
            }
        }

        List<Image> imagesToReveal;
        List<Image> deactivateOnEnd;
        RevealAnimalLists(out imagesToReveal, out deactivateOnEnd);
    }

    private void RevealAnimalLists(out List<Image> imagesToReveal, out List<Image> deactivateOnEnd)
    {
        imagesToReveal = new List<Image>();
        deactivateOnEnd = new List<Image>();

        bool isRevealing = false;

        foreach (OwnedAnimalDataSet ownedAnimal in localAnimalManager.GetUnlockedAnimals())
        {
            bool animalAlreadyRevealed = localAnimalManager.CheckAnimalAlreadyInAlbum(ownedAnimal.animalEnum);
            bool sameType = ownedAnimal.animalTypeEnum == currentOpenType;

            if(sameType)
            {
                // counts filled animals in page
                filledAnimalsCount++;
            }

            if (!animalAlreadyRevealed && sameType)
            {
                AnimalPagesByType page = animalDisplayers.Where(p => p.animalType == ownedAnimal.animalTypeEnum).SingleOrDefault();
                if (page == null)
                {
                    Debug.LogError("No page!");
                    return;
                }

                isRevealing = true;

                localAnimalManager.AddAnimalToAlbum(ownedAnimal.animalEnum);

                foreach (SpecificDisplayerAnimalAlbum specificAnimalDisplayer in page.animalsInPage)
                {
                    if (specificAnimalDisplayer.animal == ownedAnimal.animalEnum)
                    {
                        imagesToReveal.Add(specificAnimalDisplayer.revealedBGImage);
                        imagesToReveal.Add(specificAnimalDisplayer.revealedAnimalImage);

                        deactivateOnEnd.Add(specificAnimalDisplayer.hiddenAnimalImage);

                        break;
                    }
                }
                Debug.Log("Got here with animal: " + ownedAnimal.animalEnum + " of type: " + ownedAnimal.animalTypeEnum);
            }
        }

        if(isRevealing)
        {
            StartCoroutine(RevealAnimalsAction(imagesToReveal, deactivateOnEnd));
        }

        //make this somehow not hardcoded!
        if (!isRevealing && filledAnimalsCount == 4)
        {
            if(localAnimalManager.CheckPageAlreadyClaimedInAlbum(currentOpenType))
            {
                presentImage.SetActive(false);
            }
            else
            {
                getRewardsButton.gameObject.SetActive(true);
                getRewardsButton.alpha = 1;
            }
        }
    }

    private IEnumerator RevealAnimalsAction(List<Image> imagesToReveal, List<Image> deactivateOnEnd)
    {
        foreach (Image image in imagesToReveal)
        {
            Material mat = image.material;
            string keyname = "_DissolveSprite";
            GeneralFloatValueTo(image.gameObject, 0, 1.7f, revealTime, LeanTweenType.linear, mat, keyname, null);
        }

        yield return new WaitForSeconds(revealTime + 0.1f);

        foreach (Image image in deactivateOnEnd)
        {
            image.gameObject.SetActive(false);
        }

        //make this somehow not hardcoded!
        if (filledAnimalsCount == 4)
        {
            getRewardsButton.gameObject.SetActive(true);
            getRewardsButton.blocksRaycasts = false; //can't click on
            GeneralFloatValueTo(getRewardsButton.gameObject, 0, 1, 3, LeanTweenType.linear, getRewardsButton, null);
            yield return new WaitForSeconds(3 + 0.1f);
            getRewardsButton.blocksRaycasts = true;//can click on

        }
    }

    public void GivePlayerRewards()
    {
        getRewardsButton.alpha = 0;
        getRewardsButton.gameObject.SetActive(false);
        presentImage.SetActive(false);

        localAnimalManager.AddToAlbumPagesCompleted(currentOpenType);
        Debug.Log("Gave player rewards");
    }

    [ContextMenu("Populate All Pages from inspector")]
    public void PopulateAllPages()
    {
        allPages = new List<Transform>();

        for (int i = 0; i < pagesParent.childCount; i++)
        {
            allPages.Add(pagesParent.GetChild(i));
        }
    }


    [ContextMenu("Set animal album data from inspector")]
    public void SetInitialData()
    {
        // this can be called from both inspector and in game!
        // used in game to reset everything to before revealed on awake

        animalDisplayers.Clear();
        backgroundImages.Clear();
        hiddenAnimalImages.Clear();
        revealedAnimalImages.Clear();

        for (int i = 0; i < pagesParent.childCount; i++)
        {
            AnimalPagesByType newPage = new AnimalPagesByType();
            newPage.animalType = (AnimalTypesInGame)i;
            newPage.animalsInPage = new List<SpecificDisplayerAnimalAlbum>();

            for (int k = 0; k < pagesParent.GetChild(i).childCount; k++)
            {
                SpecificDisplayerAnimalAlbum displayer;
                pagesParent.GetChild(i).GetChild(k).gameObject.TryGetComponent<SpecificDisplayerAnimalAlbum>(out displayer);

                if (displayer == null)
                {
                    Debug.LogError("No displayer");
                    return;
                }


                newPage.animalsInPage.Add(displayer);
            }

            animalDisplayers.Add(newPage);

            if (i> 0)
            {
                // make sure that only the first page is active
                pagesParent.GetChild(i).gameObject.SetActive(false);
            }
        }


        foreach (AnimalPagesByType page in animalDisplayers)
        {
            foreach (SpecificDisplayerAnimalAlbum displayer in page.animalsInPage)
            {
                backgroundImages.Add(displayer.revealedBGImage);
                hiddenAnimalImages.Add(displayer.hiddenAnimalImage);
                revealedAnimalImages.Add(displayer.revealedAnimalImage);
            }
        }

        foreach (Image image in hiddenAnimalImages)
        {
            Material mat = new Material(animalFadeShader);
            mat.renderQueue = 3001;
            image.material = mat;

            mat.SetFloat("_DissolveSprite", 1.7f);
            //string keyname = "_DissolveSprite";
            //GeneralFloatValueTo(image.gameObject, 0, 1.7f, 0, LeanTweenType.linear, mat, keyname);
        }

        foreach (Image image in revealedAnimalImages)
        {
            Material mat = new Material(animalFadeShader);
            mat.renderQueue = 3002;
            image.material = mat;
            mat.SetFloat("_DissolveSprite", 0f);
        }

        foreach (Image image in backgroundImages)
        {
            Material mat = new Material(animalFadeShader);
            mat.renderQueue = 3000;
            image.material = mat;
            mat.SetFloat("_DissolveSprite", 0f);
        }
    }
}
