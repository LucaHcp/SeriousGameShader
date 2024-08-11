using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGrow : MonoBehaviour
{
    public static FishGrow instance;
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

        Invoke("GrowAllFish", 0.1f);
    }

    public static float initialGrowSize = 0.1f;
    public static float growFactor = 0.035f;
    public Sprite fishBones;

    public void Grow(GameObject fish)
    {
        FishEating fishEating = fish.GetComponent<FishEating>();
        FishMovement fishMovement = fish.GetComponent<FishMovement>();
        Eatable eatable = fish.GetComponent<Eatable>();

        if (fishEating == null || fishMovement == null || eatable == null)
        {
            return;
        }

        // Set new Size
        float newSize = initialGrowSize * (1 + (growFactor * fishEating.foodCount));
        fish.transform.localScale = new Vector3 (newSize,newSize,1);
        
        // Adjust Cameras
        PlayerInput playerInput = fish.GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            CameraScript.instance.ChangeZoom(newSize);
            AvatarCameraScript.instance.ChangeZoom(newSize);
        }


        //Adjust MovmentProperties
        fishMovement.currentSwimspeed = fishMovement.initialSwimspeed * (1 + (fishMovement.growSwimspeedFactor * fishEating.foodCount));
        fishMovement.currentRotationSpeed = fishMovement.initialRotationSpeed * (1 + (fishMovement.growRotationSpeedFactor * fishEating.foodCount));

        //Adjust EatingProperties
        eatable.requiredEatFactor = fishEating.foodCount + 8;
        eatable.foodValue = Mathf.FloorToInt(fishEating.foodCount / 2);
    }

    void GrowAllFish()
    {
        foreach (Transform fish in transform)
        {
            Grow(fish.gameObject);
        }
    }
}
