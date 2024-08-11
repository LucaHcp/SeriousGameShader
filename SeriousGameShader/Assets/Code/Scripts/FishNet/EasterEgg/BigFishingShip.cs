using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigFishingShip : MonoBehaviour
{
    [SerializeField]
    private GameObject fishnetPrefab;

	[SerializeField]
	private GameObject BlackScreen;

    [SerializeField]
    private float maxTravelDistance;
    private float distanceTravelled = 0;
	private bool fishChecked = false;

	private GameObject fishnet;

	public bool playerInsideNet = false;

	private void Start()
	{
        CreateNet();
		CheckForPlayerFishInNet();
	}

	void Update()
    {
        MoveBoat();
		CheckForCaughtPlayer();
    }

    void CreateNet()
    {
        fishnet = Instantiate(fishnetPrefab, new Vector3(-165.94f, -7.7f, 0), Quaternion.Euler(0,0,180), GameObject.Find("FishNetManager").transform);
        fishnet.transform.localScale = new Vector3(3.7f,3.7f,1);
        fishnet.GetComponent<FishnetSide>().maxTravelDistance = maxTravelDistance;
    }

    void MoveBoat()
    {
		if (distanceTravelled < maxTravelDistance)
		{
			transform.Translate(3 * Time.deltaTime, 0, 0);
			distanceTravelled += 3 * Time.deltaTime;
		}
		if(distanceTravelled > maxTravelDistance - 10)
		{
			PullNetOut();
		}
	}

	void PullNetOut()
	{
		if (fishnet.transform.position.y < 35)
		{
			fishnet.transform.Translate(0, 3 * Time.deltaTime, 0, Space.World);
			transform.Translate(5 * Time.deltaTime, 0, 0);

			if (!NarratorAudioManager.instance.pullOutBoolean && playerInsideNet && fishnet.transform.position.y > 15)
			{
				NarratorAudioManager.instance.pullOutBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.pullOutClip,netRelated:true);
			}
		}
		else if (!fishChecked)
		{
			CheckForCatchedFish();
			fishChecked = true;
		}
	}

	void CheckForCatchedFish()
	{
		if (playerInsideNet)
		{
			StartCoroutine(LoadGameOverCatechedScene());
		}
		else if(!NarratorAudioManager.instance.escapedNetBoolean)
		{
			NarratorAudioManager.instance.escapedNetBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.escapedNetClip,netRelated:true);
		}
		Invoke("DestroyShip", 3f);
	}

	void DestroyShip()
	{
		fishnet.transform.GetChild(0).GetComponent<Fishnet>().DestroyAllFish();
		Destroy(fishnet);
		Destroy(gameObject);
	}

	IEnumerator LoadGameOverCatechedScene()
	{
		if (!NarratorAudioManager.instance.catchedEndingBoolean)
		{
			NarratorAudioManager.instance.catchedEndingBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.catchedEndingClip,true);
		}

		Instantiate(BlackScreen, GameObject.Find("Canvas").transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene("GameOver_Catched");
	}

	void CheckForPlayerFishInNet()
	{
		if (fishnet != null)
		{
			playerInsideNet = fishnet.transform.GetChild(0).GetComponent<Fishnet>().TryGetPlayerFish();
		}
		Invoke("CheckForPlayerFishInNet", 2);
	}

	void CheckForCaughtPlayer()
	{
		if (!NarratorAudioManager.instance.caughtSecoundBoolean && playerInsideNet)
		{
			NarratorAudioManager.instance.caughtSecoundBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.caughtSecoundClip, netRelated: true);
		}
	}
}
