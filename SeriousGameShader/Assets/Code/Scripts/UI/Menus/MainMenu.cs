using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Image BackGround;
    [SerializeField]
    Image BlackScreen;

    public void PlayGame()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        if (NarratorAudioManager.instance.skipIntroBoolean)
        {
            StartCoroutine(BlendInImage(BlackScreen, 3));
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("Level_1");
        }
        else
        {
            StartCoroutine(BlendInImage(BackGround, 1));
            yield return new WaitForSeconds(1);

            if (!NarratorAudioManager.instance.startBoolean)
            {
                NarratorAudioManager.instance.skipIntroBoolean = true;
                NarratorAudioManager.instance.startBoolean = true;
                NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.startClip);
            }

            yield return new WaitForSeconds(23);
            StartCoroutine(BlendInImage(BlackScreen, 3));
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene("Level_1");
        }

    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
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
    }
}
