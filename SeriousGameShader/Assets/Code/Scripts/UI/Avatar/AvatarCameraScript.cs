using UnityEngine;

public class AvatarCameraScript : MonoBehaviour
{
    public Transform target;
    public Vector3 initialOffset;
    public Vector3 offset;

    public bool AvatarCamera;

    public float smoothSpeed = 0.125f;


    public static AvatarCameraScript instance;
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
            transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothSpeed * Time.deltaTime);
        }
    }

    public void ChangeZoom(float size)
    {
        GetComponent<Camera>().orthographicSize = size * 30;
        offset = (initialOffset * (size * 10f)) + new Vector3(0,0,10f);
    }
}
