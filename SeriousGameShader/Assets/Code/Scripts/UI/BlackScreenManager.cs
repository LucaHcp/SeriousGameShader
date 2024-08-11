using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreenManager : MonoBehaviour
{
    public static BlackScreenManager instance;
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

    [SerializeField] GameObject BlackOutScreen;
    [SerializeField] GameObject BlackInScreen;

    public void MakeBlackOutScreen()
    {
        Instantiate(BlackOutScreen,transform);
    }
    public void MakeBlackInScreen()
    {
        Instantiate(BlackInScreen,transform);
    }
}
