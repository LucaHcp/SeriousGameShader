using System;
using UnityEngine;

public class FishEating : MonoBehaviour
{
    [SerializeField]
    Collider2D ignoreCollider;

    [SerializeField]
    private GameObject eatArrow;

    [Header("Eating")]
    public int foodCount;
    [SerializeField]
    float EatCooldown;
    [SerializeField]
    public bool canEat;

    [Header("Hunger")]
    public int hunger;
    [SerializeField]
    float hungerTimerPlayer;
    [SerializeField]
    float hungerTimerAI;

    //Events
    public static event Action HungerLowEvent;
    public static event Action HungerMediumEvent;
    public static event Action HungerHighEvent;


    private void Awake()
    {
        canEat = true;
        if (TryGetComponent(out PlayerInput playerInput))
        {
            Invoke("ReduceHunger", hungerTimerPlayer);
        }
        else if (TryGetComponent(out FishAIInput fishAIInput))
        {
            Invoke("ReduceHunger", hungerTimerAI);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CircleCollider2D circleCollider2D = collision as CircleCollider2D;
        if (circleCollider2D != null)
        {
            return;
        }

        if (collision.gameObject.TryGetComponent(out Eatable eatable))
        {
            if (eatable != GetComponent<Eatable>())
            {
                Eat(eatable);
            }
        }
    }

    void Eat(Eatable eatable)
    {
        if (!canEat)
        {
            return;
        }
        if (foodCount < eatable.requiredEatFactor)
        {
            return;
        }

        canEat = false;

        foodCount += eatable.foodValue;
        IncreaseHunger(eatable.foodValue);
        Destroy(eatable.gameObject);
        Invoke("ResetEat", EatCooldown);

        FishGrow.instance.Grow(this.gameObject);

        if (eatable.TryGetComponent(out PlayerInput playerInput))
        {
            gameObject.AddComponent<PlayerInput>();
            gameObject.GetComponent<PlayerInput>().enabled = false;
            if (!NarratorAudioManager.instance.eatenClipBoolean)
            {
                NarratorAudioManager.instance.eatenClipBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.eatenClip);
				Invoke("GetNewPlayerFish", 9f);
            }
            else
            {
                Invoke("GetNewPlayerFish", 3f);
            }
        }

        if (TryGetComponent(out FishAudio fishAudio))
        {
            fishAudio.PlayEatAudio();
        }

        if (TryGetComponent(out PlayerInput player))
        {
            if (!NarratorAudioManager.instance.eatClipBoolean)
            {
                NarratorAudioManager.instance.eatClipBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.eatClip);
			}

            //ProgressBarManager.instance.MakeProgressRose(eatable.foodValue);
            if (eatable.foodValue >= 2)
            {
                ProgressBarManager.instance.MakeProgressRose(2);
            }
            else
            {
                ProgressBarManager.instance.MakeProgressRose(1);
            }


        }
    }

    void ResetEat()
    {
        canEat = true;
    }

    void IncreaseHunger(int factor)
    {
        hunger += factor * 2;
        if (hunger > 10)
        {
            hunger = 10;
        }
        else if (hunger < 0)
        {
            hunger = 0;
        }
        if (TryGetComponent(out PlayerInput playerInput))
        {
            SetEmotes();
            CheckHungerEvent();
            AvatarFrameManager.instance.UpdateHungerBar(hunger);
        }


    }

    void ReduceHunger()
    {
        hunger--;
        if (hunger < 1)
        {
            hunger = 0;
            if (TryGetComponent(out PlayerInput Input))
            {
                SetEmotes();
                if (!NarratorAudioManager.instance.hungerDeathBoolean)
                {
                    NarratorAudioManager.instance.hungerDeathBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.hungerDeathClip);
				}
            }
            FishDying();
        }

        if (TryGetComponent(out PlayerInput playerInput))
        {
            SetEmotes();
            AvatarFrameManager.instance.UpdateHungerBar(hunger);

            CheckHungerEvent();

            Invoke("ReduceHunger", hungerTimerPlayer);

        }
        else if (TryGetComponent(out FishAIInput fishAIInput))
        {
            Invoke("ReduceHunger", hungerTimerAI);
        }



    }

    void CheckHungerEvent()
    {
        if (hunger >= 7)
        {
            HungerLowEvent?.Invoke();
        }
        else if (hunger >= 4)
        {
            HungerMediumEvent?.Invoke();
        }
        else
        {
            HungerHighEvent?.Invoke();
        }
    }

    public void FishDying()
    {
        bool player = false;
        if (TryGetComponent<PlayerInput>(out PlayerInput playerInput))
        {
            Destroy(GetComponent<PlayerInput>());
            player = true;

            if (!NarratorAudioManager.instance.skelletFishBoolean)
            {
                NarratorAudioManager.instance.skelletFishBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.skelletFishClip);
			}
        }
        else if (TryGetComponent<FishAIInput>(out FishAIInput aiInput))
        {
            Destroy(GetComponent<FishAIInput>());
        }

        Destroy(GetComponent<FishMovement>());
        Destroy(GetComponent<FishAudio>());
        Destroy(GetComponent<FishGravity>());
        Destroy(GetComponent<CircleCollider2D>());

        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        FishDead fishDead = gameObject.AddComponent<FishDead>();
        fishDead.player = player;



        Destroy(this);
    }

    void SetEmotes()
    {
        if (hunger > 6)
        {
            EmoteManager.instance.SetHappynessStatus(0);
            EmoteManager.instance.SetHungryStatus(0);
        }
        else if (hunger > 4)
        {
            EmoteManager.instance.SetHappynessStatus(0);
            EmoteManager.instance.SetHungryStatus(1);
        }
        else if (hunger > 0)
        {
            EmoteManager.instance.SetHappynessStatus(1);
            EmoteManager.instance.SetHungryStatus(2);
        }
        else
        {
            EmoteManager.instance.SetHappynessStatus(4);
        }
    }

    void GetNewPlayerFish()
    {
        Instantiate(eatArrow, transform);
        Destroy(gameObject.GetComponent<FishAIInput>());
        GetComponent<PlayerInput>().enabled = true;
        gameObject.layer = 3;
        CameraScript.instance.target = this.transform;
        AvatarCameraScript.instance.target = this.transform;

        CameraScript.instance.ChangeZoom(gameObject.transform.localScale.x);
        AvatarCameraScript.instance.ChangeZoom(gameObject.transform.localScale.x);
    }
}
