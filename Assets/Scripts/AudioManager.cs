using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip NormalBGM;
    public AudioClip EventBGM;
    public AudioClip HoldBomb;
    public AudioClip PassBomb;
    public AudioClip BombExplosion;
    public AudioClip TriggerEvent;


    private void Awake()
    {
        instance = this;
    }

}
