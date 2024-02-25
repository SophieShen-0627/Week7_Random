using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip NormalBGM;
    public AudioClip EventBGM;
    public AudioClip PassBomb;
    public AudioClip BombExplosion;
    public AudioClip TriggerEvent;

    SpecialEvent specialevent;
    AudioSource audioSource;
    bool HasChangedBGM = false;
    private void Awake()
    {
        instance = this;
        specialevent = FindObjectOfType<SpecialEvent>();
        audioSource = GetComponent<AudioSource>();

    }

    private void Update()           //ªªbgm∫√œÒ”–µ„∆Êπ÷
    {
        /*if (specialevent.InSpecialState && !HasChangedBGM)
        {
            HasChangedBGM = true;
            PlayNewBackgroundMusic(EventBGM);
        }
        if (!specialevent.InSpecialState && HasChangedBGM)
        {
            HasChangedBGM = false;
            PlayNewBackgroundMusic(NormalBGM);
        }*/
    }

    public void PlayNewBackgroundMusic(AudioClip newMusic)
    {
        Debug.Log("change background music");
        if (audioSource.isPlaying)
            audioSource.Stop(); 

        audioSource.clip = newMusic; 
        audioSource.Play(); 
    }

}
