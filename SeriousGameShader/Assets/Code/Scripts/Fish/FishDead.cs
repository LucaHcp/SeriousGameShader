using UnityEngine;
using UnityEngine.SceneManagement;

public class FishDead : MonoBehaviour
{
    public bool player;
    [SerializeField]
    Sprite fishBones;

    private void Start()
    {
        Dying();
    }

    void Dying()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0.25f;
        GetComponent<Rigidbody2D>().AddTorque(30f);

        Invoke("TransformToSkelett", 20f);
    }

    void TransformToSkelett()
    {
        Destroy(GetComponent<Eatable>());
        GetComponent<SpriteRenderer>().sprite = FishGrow.instance.fishBones;
        if (player)
        {
            Invoke("BlackInScreen", 8f);
            Invoke("GameOverScreen", 10f);
        }
        Invoke("Destroy", 40f);
    }

    void BlackInScreen()
    {
        BlackScreenManager.instance.MakeBlackInScreen();
    }

    void GameOverScreen()
    {
        if (!NarratorAudioManager.instance.diedEndingBoolean)
        {
            NarratorAudioManager.instance.diedEndingBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.diedEndingClip,true);
		}
        SceneManager.LoadScene("GameOver_BoneFish");
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

}
