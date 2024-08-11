using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenShip : MonoBehaviour
{
    private GameObject leftShipPart;
    private GameObject rightShipPart;

    private bool shipFullBroken = false;

    void Start()
    {
        leftShipPart = transform.GetChild(1).gameObject;
        rightShipPart = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (!shipFullBroken)
        {
            BreakShip();
        }
        else
        {
            Sink();
        }
    }

    void BreakShip()
    {
        if (leftShipPart.transform.localPosition.x < 3)
        {
            leftShipPart.transform.Translate(0.1f * Time.deltaTime, 0, 0);
            rightShipPart.transform.Translate(-0.1f * Time.deltaTime, 0, 0);
        }
        else
        {
            shipFullBroken = true;
        }
	}

    void Sink()
    {
		if (leftShipPart.transform.localRotation.z < 0.35)
		{
			leftShipPart.transform.Rotate(0, 0, 3 * Time.deltaTime);
			rightShipPart.transform.Rotate(0, 0, -3 * Time.deltaTime);
		}
        if (leftShipPart.transform.localPosition.y > -12)
        {
            leftShipPart.transform.Translate(0, -1 * Time.deltaTime, 0, Space.World);
            rightShipPart.transform.Translate(0, -1 * Time.deltaTime, 0, Space.World);
        }
	}
}
