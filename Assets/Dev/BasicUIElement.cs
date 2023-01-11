using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BasicUIElement : MonoBehaviour
{
    public bool isSolo;
    public bool isOverrideSolo;
    public bool isInteractable = true;

    public TMP_Text[] textRefrences;
    public Image[] imageRefrences;

    [Tooltip("move by curve explanation for leantween: in a curve we cannot go past 1 to the right as the curve moves from 0(start of animations) to 1(the end of the animations) - so if we move past 1 to the right, it's like asking to continue reading from an animation that does not exist. We can go up as high or as low as we want, but 1 symbolizes the target position we want to achieve. If we want to move to new Vector3(0, 0, 50), then 1 will be 0, 0, 50 ")]
    public AnimationCurve HoverOverMeToGetInfo;

    private void Start()
    {
    }

    /**/
    // Move zone
    /**/
    public void MoveToTarget(Vector3 targetPos, float time)
    {

    }
    public void MoveToTarget(Vector2 targetPos, float time)
    {

    }
    public void MoveToTarget(AnimationCurve moveCurve, Vector3 targetPos, float time)
    {
        LeanTween.move(gameObject, targetPos, time).setEase(moveCurve);
    }

    /**/
    // Scale zone
    /**/
    public void ScaleToTarget(Vector3 targetScale, float time)
    {

    }
    public void ScaleToTarget(Vector2 targetScale, float time)
    {

    }
    public void ScaleToTarget(AnimationCurve scaleCurve, Vector3 targetScale, float time)
    {
        LeanTween.scale(gameObject, targetScale, time).setEase(scaleCurve);
    }

    /**/
    // Rotate zone
    /**/
    public void RotateToTarget(Quaternion targetRotation, float time)
    {

    }
    public void RotateToTarget(AnimationCurve rotationCurve, Quaternion targetRotation, float time)
    {
        LeanTween.rotate(gameObject, targetRotation.eulerAngles, time).setEase(rotationCurve);
    }

    /**/
    // Color zone
    /**/
    public void ColorToTarget(Color targetColor, float time)
    {
        
    }

    /**/
    // General zone
    /**/
    /// <summary>
    /// change color of image over time.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="time"></param>
    /// <param name="easeType"></param>
    /// <param name="image"></param>
    public void GeneralFloatValueTo(GameObject gameObject, float from, float to, float time, LeanTweenType easeType, Image image)
    {
        //LeanTween.value(fadeIntoLevel, 0, 1, fadeIntoLevelSpeed).setEase(LeanTweenType.linear).setOnComplete(() => StartCoroutine(GameManager.Instance.ResetDataStartBossLevel())).setOnUpdate((float val) =>
        //{
        //    Image sr = fadeIntoLevel.GetComponent<Image>();
        //    Color newColor = sr.color;
        //    newColor.a = val;
        //    sr.color = newColor;
        //});
    }

    /// <summary>
    /// change color of text over time.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="time"></param>
    /// <param name="easeType"></param>
    /// <param name="textObject"></param>
    public void GeneralFloatValueTo(GameObject gameObject, float from, float to, float time, LeanTweenType easeType, TMP_Text textObject)
    {
        //LeanTween.value(introImages[introImageIndex].textObjects[introImages[introImageIndex].textObjects.Count() - 1], 1, 0, speedFadeOutIntro).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
        //{
        //    TMP_Text sr = introImages[introImageIndex].textObjects[introImages[introImageIndex].textObjects.Count() - 1].GetComponent<TMP_Text>();
        //    Color newColor = sr.color;
        //    newColor.a = val;
        //    sr.color = newColor;
        //});
    }

    /// <summary>
    /// change color of Sprite over time.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="time"></param>
    /// <param name="easeType"></param>
    /// <param name="sprite"></param>
    public void GeneralFloatValueTo(GameObject gameObject, float from, float to, float time, LeanTweenType easeType, Sprite sprite)
    {
        //LeanTween.value(introImages[introImageIndex].textObjects[introImages[introImageIndex].textObjects.Count() - 1], 1, 0, speedFadeOutIntro).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
        //{
        //    TMP_Text sr = introImages[introImageIndex].textObjects[introImages[introImageIndex].textObjects.Count() - 1].GetComponent<TMP_Text>();
        //    Color newColor = sr.color;
        //    newColor.a = val;
        //    sr.color = newColor;
        //});
    }

    /// <summary>
    /// change alpha of CanvasGroup over time.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="time"></param>
    /// <param name="easeType"></param>
    /// <param name="canvasGroup"></param>
    public void GeneralFloatValueTo(GameObject gameObject, float from, float to, float time, LeanTweenType easeType, CanvasGroup canvasGroup)
    {
        //LeanTween.value(introImages[introImageIndex].textObjects[introImages[introImageIndex].textObjects.Count() - 1], 1, 0, speedFadeOutIntro).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
        //{
        //    TMP_Text sr = introImages[introImageIndex].textObjects[introImages[introImageIndex].textObjects.Count() - 1].GetComponent<TMP_Text>();
        //    Color newColor = sr.color;
        //    newColor.a = val;
        //    sr.color = newColor;
        //});
    }

    /// <summary>
    /// Change float value over time.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="time"></param>
    public void GeneralFloatValueTo(float from, float to, float time)
    {
        //LeanTween.value(introImages[introImageIndex].textObjects[introImages[introImageIndex].textObjects.Count() - 1], 1, 0, speedFadeOutIntro).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
        //{
        //    TMP_Text sr = introImages[introImageIndex].textObjects[introImages[introImageIndex].textObjects.Count() - 1].GetComponent<TMP_Text>();
        //    Color newColor = sr.color;
        //    newColor.a = val;
        //    sr.color = newColor;
        //});
    }

    /**/
    //Abstract zone (inheritence zone)
    /**/
    public abstract void SetMe(string[] texts, Sprite[] sprites);

}
