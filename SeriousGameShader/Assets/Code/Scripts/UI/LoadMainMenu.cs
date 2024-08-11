using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject Canvas;
    [SerializeField]
    private GameObject BlackScreen;


    void Start()
    {
        Invoke("LoadMainMenuScene", 20f);
    }

	private void Update()
	{
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenuScene();
        }
	}

    void LoadMainMenuScene()
    {
		StartCoroutine(LoadMainMenuSceneCoroutine());
	}

	IEnumerator LoadMainMenuSceneCoroutine()
    {
        Instantiate(BlackScreen, transform.position, Quaternion.identity, Canvas.transform);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }
}
