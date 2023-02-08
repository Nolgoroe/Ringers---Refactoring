using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public abstract class BasicUIElement : MonoBehaviour
{
    public bool isSolo;
    public bool isOverrideSolo;
    public bool isPermanent;
    public bool isInteractable = true;

    [SerializeField] protected TMP_Text[] textRefrences;
    [SerializeField] protected Image[] imageRefrences;
    [SerializeField] protected SpriteRenderer[] spriterRendererRefrences;
    [SerializeField] protected CustomButtonParent[] buttonRefs;
    //public UnityEvent connectedEvents;

    [Tooltip("move by curve explanation for leantween: in a curve we cannot go past 1 to the right as the curve moves from 0(start of animations) to 1(the end of the animations) - so if we move past 1 to the right, it's like asking to continue reading from an animation that does not exist. We can go up as high or as low as we want, but 1 symbolizes the target position we want to achieve. If we want to move to new Vector3(0, 0, 50), then 1 will be 0, 0, 50 ")]
    protected AnimationCurve HoverOverMeToGetInfo;

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
        LeanTween.value(gameObject, from, to, time).setEase(easeType).setOnUpdate((float val) =>
        {
            Color newColor = image.color;
            newColor.a = val;
            image.color = newColor;
        });
    }

    /// <summary>
    /// change material variable over time.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="time"></param>
    /// <param name="easeType"></param>
    /// <param name="image"></param>
    public void GeneralFloatValueTo(GameObject gameObject, float from, float to, float time, LeanTweenType easeType, Material mat, string keyName, System.Action action)
    {
        LeanTween.value(gameObject, from, to, time).setEase(easeType).setOnComplete(action).setOnUpdate((float val) =>
        {
            mat.SetFloat(keyName, val);
        });
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
    public void GeneralFloatValueTo(GameObject gameObject, float from, float to, float time, LeanTweenType easeType, CanvasGroup canvasGroup, System.Action action)
    {
        LeanTween.value(gameObject, from, to, time).setEase(easeType).setOnComplete(action).setOnUpdate((float val) =>
        {
            canvasGroup.alpha = val;
        });
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
    //Abstract + virtual zone (inheritence zone)
    /**/
    public virtual void SetMe(string[] texts, Sprite[] sprites)
    {
        if (texts != null && texts.Length > 0)
        {
            for (int i = 0; i < textRefrences.Length; i++)
            {
                textRefrences[i].text = texts[i];
            }
        }

        if (imageRefrences.Length > 0 && sprites != null && sprites.Length > 0)
        {
            for (int i = 0; i < imageRefrences.Length; i++)
            {
                imageRefrences[i].sprite = sprites[i];
            }
        }

        if (spriterRendererRefrences.Length > 0 && sprites != null && sprites.Length > 0)
        {
            for (int i = 0; i < spriterRendererRefrences.Length; i++)
            {
                spriterRendererRefrences[i].sprite = sprites[i];
            }
        }
    }

    public abstract void OverrideSetMe(string[] texts, Sprite[] sprites, System.Action[] actions);


    public CustomButtonParent[] getButtonRefrences => buttonRefs;
    public TMP_Text[] getTextRefrences => textRefrences;
    //public CustomButtonParent[] getButtonRefrences => buttonRefs;
}
