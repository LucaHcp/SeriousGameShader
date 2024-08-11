using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundFishMovement : MonoBehaviour
{
    Vector3 targetPosition;
    [SerializeField]
    float speed;

    void Awake()
    {
        GetRandomTargetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTargetPosition();
    }

    void MoveToTargetPosition()
    {
        Vector3 dir = targetPosition - transform.position;
        transform.position += dir.normalized * speed * Time.deltaTime;
        if (dir.magnitude < 1f)
        {
            GetRandomTargetPosition();
        }
    }

    void GetRandomTargetPosition()
    {
        targetPosition = new Vector3(Random.Range(-40f, 40f), Mathf.Clamp( Random.Range(transform.position.y -5f, transform.position.y + 5f),-40f,10f), transform.position.z);
        Vector3 dir = targetPosition - transform.position;
        if (dir.x >= 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
