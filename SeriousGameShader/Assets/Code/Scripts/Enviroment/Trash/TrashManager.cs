using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashManager : MonoBehaviour
{
    [SerializeField]
    private float trashPerMinute;
    [SerializeField]
    private float maxTrash;
    private float trashSpawned = 0;

    [SerializeField]
    private GameObject[] trashList;

    private bool spawnTrash = true;

	private void Start()
	{
        StartCoroutine(SpawnTrash());
	}

	void Update()
    {
        HandleTrashSpawnRate();
    }

    IEnumerator SpawnTrash()
    {
        while (spawnTrash)
        {
            yield return new WaitForSeconds(60 / trashPerMinute);
            Instantiate(trashList[Random.Range(0, trashList.Length)], new Vector3(Random.Range(-37, 37), 25, 0), Quaternion.identity, transform);
            trashSpawned++;
        }
    }

    void HandleTrashSpawnRate()
    {
        if (trashSpawned >= maxTrash - 1)
        {
            spawnTrash = false;
        }
        else
        {
            if (trashPerMinute < 30)
            {
                trashPerMinute += Time.deltaTime * (Time.time / 300);
            }
		}
    }
}
