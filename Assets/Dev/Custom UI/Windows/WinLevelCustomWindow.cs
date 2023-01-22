using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLevelCustomWindow : BasicCustomUIWindow
{
    [SerializeField] private RectTransform FlowerRect;

    [SerializeField] private float timeToReveaFlowers;
    
   void Start()
    {
        FlowerRect.sizeDelta = new Vector2(0, 0);

        LeanTween.value(FlowerRect.sizeDelta.y, 1000, timeToReveaFlowers).setOnUpdate((float val) =>
        {
            FlowerRect.sizeDelta = new Vector2(FlowerRect.sizeDelta.x, val);
        });

        LeanTween.value(FlowerRect.sizeDelta.x, 1000, timeToReveaFlowers).setOnUpdate((float val) =>
        {
            FlowerRect.sizeDelta = new Vector2(val, FlowerRect.sizeDelta.y);
        });

    }
}
