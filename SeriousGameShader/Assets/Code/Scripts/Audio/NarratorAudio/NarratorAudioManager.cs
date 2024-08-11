using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NarratorAudioManager : MonoBehaviour
{
    [Header("Intro")]
    public bool skipIntroBoolean;

    [Header("SideFacts")]
    [SerializeField] float timeForSideFact;
    [SerializeField] int sideFactTimer;

    [Header("AudioClips")]
    [Header("Start")]
    public AudioClip startClip;
    [Header("Eat")]
    public AudioClip eatClip;
    public AudioClip eatenClip;
    [Header("FishNet")]
    public AudioClip fishNetClip;
    public AudioClip pullOutClip;
    public AudioClip caughtFirstClip;
    public AudioClip caughtSecoundClip;
    public AudioClip escapedNetClip;
    public AudioClip inNetClip;
    public AudioClip populationDyingClip;
    [Header("Endings")]
    public AudioClip growDeathClip;
    public AudioClip hungerDeathClip;
    public AudioClip skelletFishClip;
    public AudioClip catchedEndingClip;
    public AudioClip mateEndingClip;
    public AudioClip diedEndingClip;
    [Header("SideFacts")]
    public AudioClip sideFactPhytoplanktonClip;
	public AudioClip poopClip;

	[Header("Booleans")]
    [Header("Start")]
    public bool startBoolean;
    [Header("Eat")]
    public bool eatClipBoolean;
    public bool eatenClipBoolean;
    [Header("FishNet")]
    public bool fishNetBoolean;
    public bool pullOutBoolean;
    public bool caughtFirstBoolean;
    public bool caughtSecoundBoolean;
    public bool escapedNetBoolean;
    public bool inNetBoolean;
    public bool populationDyingBoolean;
    [Header("Ending")]
    public bool growDeathBoolean;
    public bool hungerDeathBoolean;
    public bool skelletFishBoolean;
    public bool catchedEndingBoolean;
    public bool mateEndingBoolean;
    public bool diedEndingBoolean;
    [Header("SideFacts")]
    public bool sideFactPhytoplanktonBoolean;
	public bool poopBoolean;

	AudioSource audioSource;

    Queue<AudioClip> clipQueue = new Queue<AudioClip>();

    public bool endingQueued;

    public static NarratorAudioManager instance;
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

	private void Start()
	{
		Invoke("SideFactTimerIncrement", 1f);
        endingQueued = false;
	}

	private void Update()
    {
        CheckClipQueue();
        EnableQueueAgain();
    }

    void CheckClipQueue()
    {
        if (audioSource.isPlaying == false && clipQueue.Count > 0)
        {
            sideFactTimer = 0;
            audioSource.clip = clipQueue.Dequeue();
            audioSource.Play();
        }
	}

    public bool PlayNarratorClip(AudioClip audioClip, bool isEnding = false, bool netRelated = false)
    {
        if (!endingQueued && CheckForPlayerNotInNet(netRelated))
        {
			endingQueued = isEnding;
			clipQueue.Enqueue(audioClip);
            return true;
        }
        return false;
    }

    void EnableQueueAgain()
    {
        if (audioSource.isPlaying == false)
        {
            endingQueued = false;
        }
    }

    bool CheckForPlayerNotInNet(bool netRelated)
    {
        if (netRelated) return true;
        if (GameObject.Find("FishNetManager") != null && GameObject.Find("FishNetManager").GetComponent<FishnetManager>().playerInsideNet)
        {
            return false;
        }
        return true;
    }

    void SideFactTimerIncrement()
    {
        if (SceneManager.GetActiveScene().name == "Level_1" && !audioSource.isPlaying && fishNetBoolean && CheckForPlayerNotInNet(false))
        {
            sideFactTimer++;
        }
        if (sideFactTimer >= timeForSideFact)
        {
            sideFactTimer = 0;
            if (!sideFactPhytoplanktonBoolean)
            {
                sideFactPhytoplanktonBoolean = true;
                clipQueue.Enqueue(sideFactPhytoplanktonClip);
            }
            else if (!poopBoolean)
            {
                poopBoolean = true;
                clipQueue.Enqueue(poopClip);
            }
			sideFactTimer = 0;
        }
        Invoke("SideFactTimerIncrement",1f);
    }
}
