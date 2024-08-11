using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FishnetSide : MonoBehaviour
{

    [SerializeField]
    private float netUpSpeed;
	[SerializeField]
	private float netTurnSpeed;
	[SerializeField]
	private float netTravelSpeed;
    [SerializeField]
    private bool leftSide;

	private float turnAngle = 0;

    private float leftSideFactor = -1;

	private float distanceTravelled = 0;
	public float maxTravelDistance;

	private void Start()
	{
        if (!leftSide)
        {
            leftSideFactor = 1;
        }
	}

	void Update()
    {
        ReturnNet();
    }

    void ReturnNet()
    {
        if (distanceTravelled < maxTravelDistance)
        {
            transform.Translate(leftSideFactor * netTravelSpeed * Time.deltaTime, 0, 0, Space.World);
            distanceTravelled += netTravelSpeed * Time.deltaTime;
        }
        if(distanceTravelled > maxTravelDistance - 15)
        {
            if (turnAngle < 90)
            {
                transform.Rotate(0, 0,leftSideFactor * netTurnSpeed * Time.deltaTime);
                turnAngle += netTurnSpeed * Time.deltaTime;

			}
            if (turnAngle > 70 && transform.position.y < 8.5)
            {
                transform.Translate(0, netUpSpeed * Time.deltaTime, 0, Space.World);
            }
		}
	}
}
