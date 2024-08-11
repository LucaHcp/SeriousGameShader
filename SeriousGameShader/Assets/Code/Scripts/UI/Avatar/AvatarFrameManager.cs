using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarFrameManager : MonoBehaviour
{
    public static AvatarFrameManager instance;
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
    GameObject hungerBar;
    [SerializeField]
    Sprite[] hungerBarSprites;

    [SerializeField]
    GameObject enduranceBar;
    [SerializeField]
    Sprite[] enduranceBarSprites;

    public void UpdateEnduranceBar(float factor)
    {
        enduranceBar.GetComponent<Image>().sprite = enduranceBarSprites[Mathf.RoundToInt(10 * factor)%11];
    }

    public void UpdateHungerBar(int factor)
    {
        hungerBar.GetComponent<Image>().sprite = hungerBarSprites[factor];
    }
}
