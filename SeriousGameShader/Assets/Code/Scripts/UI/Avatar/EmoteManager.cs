using UnityEngine;
using UnityEngine.UI;


public class EmoteManager : MonoBehaviour
{
    public static EmoteManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [SerializeField]
    Image happynessEmote;
    [SerializeField]
    Image hungryEmote;
    [SerializeField]
    Image tempratureEmote;
    [SerializeField]
    Image poopEmote;

    [SerializeField]
    Sprite[] happynessStatusEmotes;
    [SerializeField]
    Sprite[] tempratureStatusEmotes;

    public void SetHappynessStatus(int _happynessStatus)
    {
        switch (_happynessStatus)
        {
            case 0:
                happynessEmote.sprite = happynessStatusEmotes[0];
                break;
            case 1:
                happynessEmote.sprite = happynessStatusEmotes[1];
                break;
            case 2:
                happynessEmote.sprite = happynessStatusEmotes[2];
                break;
            case 3:
                happynessEmote.sprite = happynessStatusEmotes[3];
                break;
            case 4:
                happynessEmote.sprite = happynessStatusEmotes[4];
                break;
            default:
                happynessEmote.sprite = happynessStatusEmotes[0];
                break;
        }
    }

    public void SetHungryStatus(int _hungryStatus)
    {
        switch (_hungryStatus)
        {
            case 0:
                hungryEmote.enabled = false;
                break;
            case 1:
                hungryEmote.enabled = true;
                hungryEmote.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 1);
                break;
            case 2:
                hungryEmote.enabled = true;
                hungryEmote.gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 2);
                break;
            default:
                hungryEmote.enabled = false;
                break;
        }
    }

    public void SetTempratureStatus(int _tempratureStatus)
    {
        switch (_tempratureStatus)
        {
            case 0:
                tempratureEmote.enabled = false;
                break;
            case 1:
                tempratureEmote.enabled = true;
                tempratureEmote.sprite = tempratureStatusEmotes[0];
                break;
            case 2:
                tempratureEmote.enabled = true;
                tempratureEmote.sprite = tempratureStatusEmotes[1];
                break;

            default:
                tempratureEmote.enabled = false;
                break;
        }
    }

    public void SetPoopStatus(int _poopStatus)
    {
        switch (_poopStatus)
        {
            case 0:
                poopEmote.enabled = false;
                break;

            default:
                hungryEmote.enabled = false;
                break;
        }
    }
}
