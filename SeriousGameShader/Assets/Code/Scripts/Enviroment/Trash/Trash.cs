using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField]
    private float sinkSpeed;

    [SerializeField]
    private float fallSpeed;

    private float depthPosition;

	private void Start()
	{
        depthPosition = Random.Range(-30, 13);
	}

	void Update()
    {
        Move();
    }

    void Move()
    {
        float currentSpeed = 0;

        if (transform.position.y > 14.5)
        {
            currentSpeed = fallSpeed;
        }
        else if (transform.position.y > depthPosition)
        {
            currentSpeed = sinkSpeed;
        }

        transform.Translate(0,-currentSpeed * Time.deltaTime,0, Space.World);
    }
}
