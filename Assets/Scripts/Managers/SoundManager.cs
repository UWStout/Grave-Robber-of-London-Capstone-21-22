/*
    @Author Zachary Boehm
    @Version 11.12.2021
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reference to a location in a list of a specifc music clip.
/// </summary>
public enum Music
{
    StartMenu = 0,
    PlayMenu = 1,
    ComboScene = 2,
}

/// <summary>
/// Reference to a location in a list of a specific sound effect.
/// </summary>
public enum SoundEffect
{
    FootStep = 0,
    Dig = 1,
}

/// <summary>
/// A manager that will handling playing sound effects and swapping music.
/// </summary>
public class SoundManager : MonoBehaviour
{
    /*============================================================================
    * Variables and Nested Classes
    **============================================================================*/
  

    [Header("Audio Components")]
    [SerializeField] private AudioSource MusicComponent; /*! Reference to the Music AudioSource*/
    [SerializeField] private AudioSource SoundEffectComponent; /*! Reference to the Sound Effect AudioSource*/

    [Header("Music Sources")]
    [SerializeField]private List<AudioClip> MusicSources = new List<AudioClip>(); /*! List of "Hashed" music clips so order doesn't matter*/

    [Header("Sound Effect Sources")]
    [SerializeField] private List<AudioClip> SoundEffectSources = new List<AudioClip>(); /*! List of sound effect audio clips*/

    [Header("Volume")]
    [Range(0, 100)]
    [SerializeField] private int MusicVolume = 0; /*! Current Volume level of the music (out of 100)*/
#if UNITY_EDITOR
    private int OldMusicVolume = 0;
#endif
    [Range(0, 100)]
    [SerializeField] private int SoundEffectVolume = 0; /*! Current volume level of sound effect (out of 100)*/
#if UNITY_EDITOR
    private int OldSoundEffectVolume = 0;
#endif

    [Header("Toggles")]
    [SerializeField] private bool PlaceHolder;
    /*============================================================================
    * Game Start and On value changed
    **============================================================================*/

    
    /// <summary>
    /// On game start set the Audio Sources volume to the current volume in the script
    /// </summary>
    public void Awake()
    {
        //OldMusicVolume = MusicVolume;
        //OldSoundEffectVolume = SoundEffectVolume;

        MusicComponent.volume = MusicVolume / 100f;
        SoundEffectComponent.volume = SoundEffectVolume / 100f;
    }
#if UNITY_EDITOR
    /// <summary>
    /// **EDITOR ONLY**
    /// Will update volume sliders based on inspector values
    /// </summary>
    public void Update()
    {
        if(OldMusicVolume != MusicVolume)
        {
            //update the volume in the Audio component
            MusicComponent.volume = MusicVolume/100f;
            OldMusicVolume = MusicVolume;
        }
        if (OldSoundEffectVolume != SoundEffectVolume)
        {
            //update the volume in the Audio component
            SoundEffectComponent.volume = SoundEffectVolume/100f;
            OldSoundEffectVolume = SoundEffectVolume;
        }
    }
#endif
    /*============================================================================
    * Functions
    **============================================================================*/

    /// <summary>
    /// Switched the currently playing music clip
    /// </summary>
    /// <param name="_Index">A enum of what music clip should be playing</param>
    /// <returns>The success of switching the music clip</returns>
    public bool ChangeTrack(Music _Index)
    {
        try
        {
            MusicComponent.clip = MusicSources[(int)_Index];
            if (MusicComponent.clip != null)
            {
                return true;
            }
        }catch(System.Exception e)
        {
            Debug.LogError($"Error: No Music Source at [{(int)_Index}] on in Sound Manager!");
            return false;
        }
        return false;
    }

    /// <summary>
    /// Plays a one off sound effect
    /// </summary>
    /// <param name="_Effect">The sound effect that is to be instantiated and played</param>
    /// <returns>The success of playing the one off</returns>
    public bool PlayEffect(SoundEffect _Effect)
    {
        try
        {
            AudioClip SoundEffect = SoundEffectSources[(int)_Effect];
            if (SoundEffect != null)
            {
                SoundEffectComponent.PlayOneShot(SoundEffect, SoundEffectVolume);
                return true;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error: No Sound Effect Source at [{(int)_Effect}] on in Sound Manager!");
            return false;
        }
        return false;
    }
}

