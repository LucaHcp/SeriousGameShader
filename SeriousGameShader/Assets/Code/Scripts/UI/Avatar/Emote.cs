using UnityEngine;

public class Emote : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float sizeSpeed;

    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Invoke("SwitchMove", 5f + Random.Range(-1f,1f));
        Invoke("SwitchSize", 10f + Random.Range(-1f, 1f));
    }

    void Update()
    {
        Move();
        Size();
    }

    void SwitchMove()
    {
        speed = -speed;
        Invoke("SwitchMove", 5f);
    }
    void SwitchSize()
    {
        sizeSpeed = -sizeSpeed;
        Invoke("SwitchSize", 10f);
    }

    void Move()
    {
        rectTransform.localPosition += new Vector3(0,speed * Time.deltaTime,0);
    }

    void Size()
    {
        rectTransform.localScale += new Vector3(0.0001f, 0.0001f, 0f) * sizeSpeed;
    }
}
