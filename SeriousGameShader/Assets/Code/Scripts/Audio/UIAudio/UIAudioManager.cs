using UnityEngine;


public class UIAudioManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField]
    AudioClip playClip;
    [SerializeField]
    AudioClip optionsClip;
    [SerializeField]
    AudioClip exitClip;

    AudioSource audioSource;

    public static UIAudioManager instance;
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
    }

    public void PlaySoundForPlayButton()
    {
        audioSource.clip = playClip;
        audioSource.volume = 0.5f;
        audioSource.pitch = 0.5f;
        audioSource.Play();
    }

    public void PlaySoundForOptionsButton()
    {
        audioSource.clip = optionsClip;
        audioSource.volume = 0.5f;
        audioSource.pitch = 0.5f;
        audioSource.Play();
    }

    public void PlaySoundForExitButton()
    {
        audioSource.clip = exitClip;
        audioSource.volume = 1f;
        audioSource.pitch = 0.3f;
        audioSource.Play();
    }
}
