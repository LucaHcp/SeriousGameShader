using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FishAudio : MonoBehaviour
{
	[SerializeField]
	private AudioClip eatClip;
	[SerializeField]
	private AudioClip sprintClip;

    private AudioSource eatAudioSource;
    private AudioSource swimAudioSource;
	private AudioLowPassFilter lowPassFilter;
	private AudioReverbFilter reverbFilter;

	private void Awake()
	{
        eatAudioSource = gameObject.AddComponent<AudioSource>();
        eatAudioSource.clip = eatClip;

        swimAudioSource = gameObject.AddComponent<AudioSource>();
        swimAudioSource.clip = sprintClip;

        lowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
		lowPassFilter.cutoffFrequency = 500.0f;
		lowPassFilter.lowpassResonanceQ = 1.0f;

        reverbFilter = gameObject.AddComponent<AudioReverbFilter>();
		reverbFilter.reverbPreset = AudioReverbPreset.Underwater;

        eatAudioSource.spatialBlend = 1f;
        swimAudioSource.spatialBlend = 1f;

		eatAudioSource.volume = 0.25f;
	}

	public void PlayEatAudio()
	{
        eatAudioSource.Play();
	}

	public void PlaySprintAudio()
	{
		if (!swimAudioSource.isPlaying)
		{
            swimAudioSource.Play();
		}
	}

	public void stopSwimAudio()
	{
        swimAudioSource.Stop();
	}
}
