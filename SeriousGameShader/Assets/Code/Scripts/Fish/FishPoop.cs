using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPoop : MonoBehaviour
{
    [SerializeField] GameObject poop;

    private void Start()
    {
        Invoke("Poop",Random.Range(50f,200f));  
    }

    void Poop()
    {
        Instantiate(poop,transform.position,Quaternion.identity,GameObject.Find("PoopHolder").transform);
        Invoke("Poop", Random.Range(50f, 200f));
    }
}
