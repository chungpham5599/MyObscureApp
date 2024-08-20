using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instace { get; private set; }

    [SerializeField] AudioSource sfxSource;

    public AudioClip flip;
    public AudioClip right;
    public AudioClip wrong;
    public AudioClip gameover;
    public void Awake()
    {
        if (Instace != null && Instace != this)
            Destroy(this);
        else
            Instace = this;
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxSource.PlayOneShot(sfx);
    }
}
