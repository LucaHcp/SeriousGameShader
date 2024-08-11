using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class FishnetManager : MonoBehaviour
{
    [SerializeField]
    private float startNets;
    [SerializeField]
    private float netDuration;
    [SerializeField]
    private float netCoolDown;

    [SerializeField]
    private GameObject fishnetLeftSidePrefab;
    [SerializeField]
    private GameObject fishnetRightSidePrefab;
    [SerializeField]
    private GameObject fishnetBackPrefab;

    [SerializeField]
    private GameObject fishBoatPrefab;

    [SerializeField]
    private GameObject Canvas;
    [SerializeField]
    private GameObject BlackScreen;

    private GameObject fishnet;
    private GameObject fishBoat;

    public float newTimer;
    private bool timerSynchronized;
    private bool spawnNewNets;

    private bool createdNet = false;
    private bool createdBoat = false;
    private bool netEnd = false;

    public bool playerInsideNet = false;
    private bool playerWasInNet = false;
    private bool oneCaughtClipPerNet = false;

    private int netNumber;

    //Events
    public static event Action BoatArrivalEvent;
    public static event Action BoatArrivedEvent;
    public static event Action BoatGoneEvent;

    private void Start()
    {
        ChooseRandomNetNumber();
        CheckForPlayerFishInNet();
        newTimer = 0;
        timerSynchronized = false;
        spawnNewNets = false;
    }

    void Update()
    {
        StartNetSpawning();
        if (NarratorAudioManager.instance.eatClipBoolean)
        {
            newTimer += Time.deltaTime;
        }
        CheckBoatEvents();
    }

    void StartNetSpawning()
    {
        if (!spawnNewNets && startNets < newTimer)
        {
            spawnNewNets = true;
            newTimer = 0 + netCoolDown;
            if (!NarratorAudioManager.instance.fishNetBoolean)
            {
                NarratorAudioManager.instance.fishNetBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.fishNetClip, netRelated: true);
            }
        }
        if (spawnNewNets)
        {
            NetTimeManaging();
            CheckForSuccesfulEscape();
        }
    }

    void ChooseRandomNetNumber()
    {
        //0 left
        //1 right
        //2 back
        netNumber = UnityEngine.Random.Range(0, 3);
    }

    void NetTimeManaging()
    {
        if (!createdBoat && newTimer > netCoolDown)
        {
            createdBoat = true;
            StartCoroutine(CreateBoat());
        }

        if (createdNet)
        {
            CheckForCaughtPlayer();
            if (fishnet.transform.position.y > 8.5)
            {
                if (!playerWasInNet && playerInsideNet)
                {
                    playerWasInNet = true;
                }

                if (!timerSynchronized)
                {
                    newTimer = 0;
                    timerSynchronized = true;
                }
            }
            if (timerSynchronized && newTimer > netDuration)
            {
                PullNetOut();
            }
        }
    }

    IEnumerator CreateBoat()
    {
        float maxTravelDistance = UnityEngine.Random.Range(40f, 69f);

        if (netNumber != 2)
        {
            fishBoat = Instantiate(fishBoatPrefab, new Vector3(-100, 21.5f, 0), Quaternion.identity, transform);
            FishingBoat fishingBoat = fishBoat.GetComponent<FishingBoat>();
            fishingBoat.maxTravelDistance = maxTravelDistance;

            if (netNumber == 1)
            {
                fishBoat.transform.position = new Vector3(-60, 21.5f, 0);
                fishingBoat.leftSideFactor = 1;
                fishBoat.transform.localScale = new Vector3(-fishBoat.transform.localScale.x, fishBoat.transform.localScale.y, fishBoat.transform.localScale.z);
            }
            else
            {
                //netnumber 0
                fishBoat.transform.position = new Vector3(60, 21.5f, 0);
                fishingBoat.leftSideFactor = -1;
            }

            yield return new WaitForSeconds(12f);
        }

        createdNet = true;
        CreateNet(maxTravelDistance);
    }

    void CreateNet(float maxTravelDistance)
    {
        switch (netNumber)
        {
            case 0:
                fishnet = Instantiate(fishnetLeftSidePrefab, new Vector3(60, UnityEngine.Random.Range(-21f, -6f), 0), Quaternion.identity, transform);
                break;
            case 1:
                fishnet = Instantiate(fishnetRightSidePrefab, new Vector3(-60, UnityEngine.Random.Range(-21f, -6f), 0), Quaternion.Euler(0, 0, 180), transform);
                break;
            case 2:
                fishnet = Instantiate(fishnetBackPrefab, new Vector3(UnityEngine.Random.Range(-21f, 21f), UnityEngine.Random.Range(-14f, -6f), 0), Quaternion.Euler(0, 0, -90), transform);
                break;
        }

        if (fishnet.TryGetComponent(out FishnetSide fishNetSide))
        {
            fishNetSide.maxTravelDistance = maxTravelDistance;
        }

    }

    void PullNetOut()
    {

        if (fishnet.transform.position.y < 30)
        {
            fishnet.transform.Translate(0, 2 * Time.deltaTime, 0, Space.World);
            FishboatLeaving();

            if (!NarratorAudioManager.instance.pullOutBoolean && playerInsideNet)
            {
                NarratorAudioManager.instance.pullOutBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.pullOutClip, netRelated: true);
            }
        }
        else if (!netEnd)
        {
            netEnd = true;

            if (playerInsideNet)
            {
                StartCoroutine(LoadGameOverCatechedScene());
            }
            Invoke("ResetAll", 3f);
        }
    }

    IEnumerator LoadGameOverCatechedScene()
    {
        if (!NarratorAudioManager.instance.catchedEndingBoolean)
        {
            NarratorAudioManager.instance.catchedEndingBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.catchedEndingClip, true, netRelated: true);
        }

        Instantiate(BlackScreen, Canvas.transform.position, Quaternion.identity, Canvas.transform);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOver_Catched");
    }

    void FishboatLeaving()
    {
        if (netNumber == 0)
        {
            fishBoat.transform.Translate(-5f * Time.deltaTime, 0, 0);
        }
        else if (netNumber == 1)
        {
            fishBoat.transform.Translate(5f * Time.deltaTime, 0, 0);
        }
    }

    void ResetAll()
    {
        fishnet.transform.GetChild(0).GetComponent<Fishnet>().DestroyAllFish();
        if (spawnNewNets)
        {
            Destroy(fishBoat);
        }
        Destroy(fishnet);
        ChooseRandomNetNumber();

        createdNet = false;
        createdBoat = false;

        newTimer = 0;
        timerSynchronized = false;

        netEnd = false;

        playerWasInNet = false;

        oneCaughtClipPerNet = false;
    }

    public void DisableNewNetsForEasterEgg()
    {
        spawnNewNets = false;
        newTimer = -1000;
    }

    public GameObject GetFishBoat()
    {
        return fishBoat;
    }

    public GameObject GetFishnet()
    {
        return fishnet;
    }

    void CheckForPlayerFishInNet()
    {
        if (fishnet != null)
        {
            playerInsideNet = fishnet.transform.GetChild(0).GetComponent<Fishnet>().TryGetPlayerFish();
            if (FindFirstObjectByType<PlayerInput>() != null)
            {
                Transform playerTransform = FindFirstObjectByType<PlayerInput>().transform;
                if (playerTransform.position.y > 15.5 && playerTransform.position.x < fishnet.transform.position.x + 16 && playerTransform.position.x > fishnet.transform.position.x - 16)
                {
                    playerInsideNet = true;
                }
            }
        }
        Invoke("CheckForPlayerFishInNet", 2f);
    }

    void CheckForSuccesfulEscape()
    {
        if (!NarratorAudioManager.instance.escapedNetBoolean && !playerInsideNet && playerWasInNet)
        {
            NarratorAudioManager.instance.escapedNetBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.escapedNetClip, netRelated: true);
        }
    }

    void CheckForCaughtPlayer()
    {
        if (!oneCaughtClipPerNet && playerInsideNet)
        {
            if (!NarratorAudioManager.instance.caughtFirstBoolean)
            {
                NarratorAudioManager.instance.caughtFirstBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.caughtFirstClip, netRelated: true);
            }
            else if (!NarratorAudioManager.instance.caughtSecoundBoolean)
            {
                NarratorAudioManager.instance.caughtSecoundBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.caughtSecoundClip, netRelated: true);
            }
            else if (!NarratorAudioManager.instance.inNetBoolean)
            {
                NarratorAudioManager.instance.inNetBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.inNetClip, netRelated: true);
            }
            oneCaughtClipPerNet = true;
        }
    }

    bool boatArrival = false;
    bool boatArrived = false;
    bool boatGoing = false;
    bool boatGone = true;

    void CheckBoatEvents()
    {
        if (boatGone && newTimer >= 20f)
        {
            BoatArrivalEvent?.Invoke();
        }

        if (boatGone && newTimer >= 30f)
        {
            boatGone = false;
            boatArrival = true;
            BoatArrivedEvent?.Invoke();
        }

        if (boatArrival && newTimer <= 30f)
        {
            boatArrival = false;
            boatArrived = true;
        }

        if (boatArrived && newTimer >= 30f)
        {
            boatArrived = false;
            boatGoing = true;
        }

        if (boatGoing && newTimer <= 30f)
        {
            boatGoing = false;
            boatGone = true;
            BoatGoneEvent?.Invoke();
        }

    }
}
