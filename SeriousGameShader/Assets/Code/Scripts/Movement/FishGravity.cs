using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGravity : MonoBehaviour
{
    [SerializeField]
    float gravityScale;
    static float seaLevel;

    private new Rigidbody2D rigidbody2D;

    private void Awake()
    {
        seaLevel = 15f;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckWater();
    }

    void CheckWater()
    {
        if (transform.position.y < seaLevel)
        {
            Float();
        }
        else
        {
            Fall();
        }
    }
    void Fall()
    {
        rigidbody2D.drag = 0.05f;
        rigidbody2D.angularDrag = 0.05f;
        rigidbody2D.gravityScale = gravityScale;
    }
    void Float ()
    {
        rigidbody2D.drag = 2f;
        rigidbody2D.angularDrag = 1f;
        rigidbody2D.gravityScale = 0f;
    }
}
