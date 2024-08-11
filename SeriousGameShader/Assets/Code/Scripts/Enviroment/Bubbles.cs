using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color color;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        for (int i = 0; i < 90; i++)
        {
            color = spriteRenderer.color;
            color.a += 0.01f;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(Random.Range(1f,3f));
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        for (int i = 0; i < 90; i++)
        {
            color = spriteRenderer.color;
            color.a -= 0.01f;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
}
