using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField]
    private float shrimpsPerMinute;
    [SerializeField]
    private float fishPerMinute;
	[SerializeField]
	private float stopSpawningAfterSeconds;

    [SerializeField]
    private GameObject shrimpPrefab;
	[SerializeField]
	private GameObject[] fishPrefabs;

	private float newTimer;

	private bool spawnShrimps = true;
	private bool spawnFishs = true;

	[SerializeField]
	private int fishCountForNarrator;
	Transform fishHolder;

	private void Start()
	{
		newTimer = 0;
		StartCoroutine(SpawnShrimps());
        StartCoroutine(SpawnFishs());
		fishHolder = GetComponent<Transform>();
	}

	void Update()
    {
		newTimer += Time.deltaTime;
		LowerSpawningRate();
		CheckFishCount();
    }

	void LowerSpawningRate()
	{
		if (newTimer > stopSpawningAfterSeconds)
		{
			spawnShrimps = false;
			spawnFishs = false;
		}
	}

	IEnumerator SpawnShrimps()
    {
		GameObject shrimp;
        while (spawnShrimps)
        {
			yield return new WaitForSeconds(60 / shrimpsPerMinute);
			int sideFactor;
			if (Random.Range(0, 2) == 0)
			{
				sideFactor = 1;
			}
			else
			{
				sideFactor = -1;
			}
			shrimp = Instantiate(shrimpPrefab, new Vector3(55 * sideFactor,Random.Range(-30f,9f),0), Quaternion.identity,transform);
			shrimp.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		}
	}

	IEnumerator SpawnFishs()
	{
		GameObject fish;
		while (spawnFishs)
		{
			yield return new WaitForSeconds(60 / fishPerMinute);
			int sideFactor;
			if (Random.Range(0, 2) == 0)
			{
				sideFactor = 1;
			}
			else
			{
				sideFactor = -1;
			}
			fish = Instantiate(fishPrefabs[Random.Range(0,fishPrefabs.Length)], new Vector3(55 * sideFactor, Random.Range(-30f, 9f), 0), Quaternion.identity, transform);
			fish.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		}
	}

	void CheckFishCount()
	{
		if (fishHolder.childCount - 2 < fishCountForNarrator)
		{
			if (!NarratorAudioManager.instance.populationDyingBoolean)
			{
				NarratorAudioManager.instance.populationDyingBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.populationDyingClip);
			}
		}
	}
}
