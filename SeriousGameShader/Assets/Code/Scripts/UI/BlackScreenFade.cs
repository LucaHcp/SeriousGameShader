using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenFade : MonoBehaviour
{
    [SerializeField]
    bool BlendOut;
    void Start()
    {
        Image image = GetComponent<Image>();
        if (BlendOut)
        {
            StartCoroutine(BlendOutImage(image, 3));
        }
        else
        {
            StartCoroutine(BlendInImage(image, 3));
        }
    }


    IEnumerator BlendOutImage(Image image, int duration)
    {
        int timeSteps = 100;
        timeSteps *= duration;
        image.enabled = true;
        float alphaSub = ((float)1 / (float)timeSteps);
        Color currentColor = image.color;
        for (int i = 0; i < timeSteps; i++)
        {
            currentColor.a -= alphaSub;
            image.color = currentColor;
            yield return new WaitForSeconds(alphaSub);
        }
        Destroy(gameObject);
    }

    IEnumerator BlendInImage(Image image, int duration)
    {
        int timeSteps = 100;
        timeSteps *= duration;
        image.enabled = true;
        float alphaAdd = ((float)1 / (float)timeSteps);
        Color currentColor = image.color;
        for (int i = 0; i < timeSteps; i++)
        {
            currentColor.a += alphaAdd;
            image.color = currentColor;
            yield return new WaitForSeconds(alphaAdd);
        }
        //Destroy(gameObject);
    }
}
