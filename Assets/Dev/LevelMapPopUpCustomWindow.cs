using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelMapPopUpCustomWindow : BasicUIElement
{
    public override void SetMe(string[] texts, Sprite[] sprites)
    {
        if(texts !=null && texts.Length > 0)
        {
            for (int i = 0; i < textRefrences.Length; i++)
            {
                textRefrences[i].text = texts[i];
            }
        }

        if(sprites != null && sprites.Length > 0)
        {
            for (int i = 0; i < imageRefrences.Length; i++)
            {
                imageRefrences[i].sprite = sprites[i];
            }
        }
    }
}
