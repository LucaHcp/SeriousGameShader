using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poop : MonoBehaviour
{
    private void Start()
    {
        Invoke("EnableCollider", 3f);  
    }

    void EnableCollider()
    {
        GetComponent<PolygonCollider2D>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") 
        {
            Invoke("Remove", 10f);
        }
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}
