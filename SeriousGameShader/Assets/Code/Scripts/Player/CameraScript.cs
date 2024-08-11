using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;

    public bool AvatarCamera;

    public float smoothSpeed = 0.125f;
    public float zOffset = 10f;


    public static CameraScript instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0,0, zOffset), smoothSpeed * Time.deltaTime);
        }
    }

    public void ChangeZoom(float size)
    {
        zOffset = size * 100;
    }
}
