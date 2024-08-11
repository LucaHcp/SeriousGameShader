using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAudioManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] musicClips;

    AudioSource audioSource;

    public static MusicAudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();

        CheckIfAudioClipIsPlaying();
    }


    void GetRandomClip()
    {       
        audioSource.clip = musicClips[ Random.Range(0, musicClips.Length)];
        if (audioSource.clip == musicClips[1])
        {
            audioSource.volume = 0.07f;
        }
        else if (audioSource.clip == musicClips[2])
        {
            audioSource.volume = 0.75f;
        }
        else
        {
            audioSource.volume = 0.25f;
        }
    }

    void CheckIfAudioClipIsPlaying()
    {
        if (!audioSource.isPlaying) 
        { 
            GetRandomClip();
            audioSource.Play();
        }
        Invoke("CheckIfAudioClipIsPlaying", 1f);
    }
}
