using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggManager : MonoBehaviour
{
	[SerializeField]
	private GameObject brokenBoatPrefab;
	[SerializeField]
	private GameObject bigBoatPrefab;
	[SerializeField]
	private AudioClip boatCrash;
	[SerializeField]
	private AudioClip titanic;
	private AudioSource boatSound;

	private void Awake()
	{
		boatSound = gameObject.AddComponent<AudioSource>();
		boatSound.clip = boatCrash;
	}

	private bool active = false;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.gameObject.TryGetComponent<PlayerInput>(out PlayerInput playerInput))
		{
			if (!active)
			{
				active = true;
				StartCoroutine(PrepareBigBoat());
				boatSound.Play();
			}
		}
	}

	IEnumerator PrepareBigBoat()
	{
		ResetCurrentBoat();
		SpawnBrokenBoat();
		yield return new WaitForSeconds(2);
		boatSound.clip = titanic;
		boatSound.volume = 0.3f;
		boatSound.Play();
		yield return new WaitForSeconds(10);
		SpawnBigBoat();
	}

	void ResetCurrentBoat()
	{
		transform.parent.GetComponent<SpriteRenderer>().enabled = false;
		transform.parent.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
		transform.parent.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
		transform.parent.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
		transform.parent.GetComponent<PolygonCollider2D>().enabled = false;
		Destroy(FindFirstObjectByType<FishnetManager>().GetFishnet());
		FindFirstObjectByType<FishnetManager>().GetFishBoat().GetComponent<FishingBoat>().StopAllCoroutines();
		FindFirstObjectByType<FishnetManager>().DisableNewNetsForEasterEgg();
	}

	void SpawnBrokenBoat()
	{
		int leftFactor = FindFirstObjectByType<FishnetManager>().GetFishBoat().GetComponent<FishingBoat>().leftSideFactor;
		Vector3 oldPosition = FindFirstObjectByType<FishnetManager>().GetFishBoat().transform.position;
		GameObject brokenFishBoat = Instantiate(brokenBoatPrefab, new Vector3(oldPosition.x - 5.71f, oldPosition.y - 1.75f, 0), Quaternion.identity, GameObject.Find("FishNetManager").transform);
		if (leftFactor == 1)
		{
			brokenFishBoat.transform.localScale = new Vector3(-brokenFishBoat.transform.localScale.x, brokenFishBoat.transform.localScale.y, brokenFishBoat.transform.localScale.z);
			brokenFishBoat.transform.Translate(12, 0, 0);
		}
	}

	void SpawnBigBoat()
	{
		Instantiate(bigBoatPrefab, new Vector3(-100, 32.8f, 0), Quaternion.identity, GameObject.Find("FishNetManager").transform);
	}
}
