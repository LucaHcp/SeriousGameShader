using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishnetBack : MonoBehaviour
{
	[SerializeField]
	private float scalePerSecond;

	private PolygonCollider2D polygonCollider;

	private void Start()
	{
		polygonCollider = transform.GetChild(0).GetComponent<PolygonCollider2D>();
		polygonCollider.enabled = false;
	}

	void Update()
    {
		GrowNet();
    }

	void GrowNet()
	{
		if (transform.localScale.x < 2)
		{
			Vector3 scaleIncrease = new Vector3(scalePerSecond * Time.deltaTime, scalePerSecond * Time.deltaTime, scalePerSecond * Time.deltaTime);
			transform.localScale += scaleIncrease * (1 + transform.localScale.x);

			SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
			Color currentColor = spriteRenderer.color;
			currentColor.a = transform.localScale.x / 2;
			spriteRenderer.color = currentColor;

		}
		else
		{
			if (polygonCollider != null && polygonCollider.enabled == false)
			{
				polygonCollider.enabled = true;
			}
			ReturnNet();
		}
	}

	void ReturnNet()
	{
		if (transform.position.y < 8.5)
		{
			transform.Translate(0,3 * Time.deltaTime,0,Space.World);
		}
	}
}
