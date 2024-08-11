using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTrigger : MonoBehaviour
{
    public static Action InShadowEvent;
    public static Action OutShadowEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerInput playerInput))
        {
            InShadowEvent();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerInput playerInput))
        {
            OutShadowEvent();
        }
    }
}
