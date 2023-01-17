using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private bool isMusicMutedPrivate; // temp here - just to see in inspector
    [SerializeField] private bool isSFXMutedPrivate; // temp here - just to see in inspector
    public bool isMusicMuted
    {
        get { return isMusicMutedPrivate; }
        private set { isMusicMutedPrivate = value; } 
    }

    public bool isSFXMuted
    {
        get { return isSFXMutedPrivate; }
        private set { isSFXMutedPrivate = value; }
    }


    private void Start()
    {
        instance = this;
    }
    public void MuteSFX()
    {
        //called from button

        if (isSFXMuted)
        {
            // un-mute music
        }
        else
        {
            // mute music
        }

        isSFXMuted = !isSFXMuted;
    }
    public void MuteMusic()
    {
        //called from button
        if (isMusicMuted)
        {

            // un-mute music
            Debug.Log("Un-Muted Music");
        }
        else
        {
            // mute music
            Debug.Log("Muted Music");
        }

        isMusicMuted = !isMusicMuted;
    }
}
