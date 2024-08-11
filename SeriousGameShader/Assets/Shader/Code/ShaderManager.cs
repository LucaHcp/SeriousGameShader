using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

enum Intensity
{
    High,
    Medium,
    Low
}

public class ShaderManager : MonoBehaviour
{
    public static ShaderManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }

    }

    [Header("Vingette")]
    [SerializeField] float vingetteIntensityMultiplier = 0.5f;
    [SerializeField] float vingetteShadowIntensity = 0.0f;
    [SerializeField] float vingetteNetIntensity = 0.0f;


    [Header("Color Adjustments")]
    [SerializeField] Intensity brightness;
    [SerializeField] float brightnessMultiplier;
    [SerializeField] Intensity contrast;
    [SerializeField] float contrastMultiplier;
    [SerializeField] Intensity saturation;
    [SerializeField] float saturationMultiplier;

    [Header("Wave Effect")]
    [SerializeField] Intensity waveIntensity;
    [SerializeField] float waveIntensityInterpolation;

    [SerializeField] Material matClose;
    [SerializeField] Material matFar;

    [Header("BoatArrieved")]
    bool boatArrived;
    bool netClose;


    [Header("Volume")]
    [SerializeField] Volume volume;
    private Vignette vignette;
    private ColorAdjustments colorAdjustments;


    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out colorAdjustments);

        FishEating.HungerLowEvent += HandleHungerLowEvent;
        FishEating.HungerMediumEvent += HandleHungerMediumEvent;
        FishEating.HungerHighEvent += HandleHungerHighEvent;

        FishnetManager.BoatArrivalEvent += HandleBoatArrivalEvent;
        FishnetManager.BoatArrivedEvent += HandleBoatArrivedEvent;
        FishnetManager.BoatGoneEvent += HandleBoatGoneEvent;

        ShadowTrigger.InShadowEvent += HandleInShadowEvent;
        ShadowTrigger.OutShadowEvent += HandleOutShadowEvent;

        ManageColorAdjustments();
        ManageWaveEffect();
    }

    void Update()
    {
        if (volume == null) return;

        ManageVignett();
    }

    void ManageVignett()
    {
        if (vignette == null)
        {
            Debug.Log("Vignett null");
        }
        vignette.intensity.value = Mathf.Clamp((1 - ((Camera.main.transform.position.y + 30) / 43)) * vingetteIntensityMultiplier + vingetteNetIntensity + vingetteShadowIntensity, 0f, 1f);
    }

    void ManageColorAdjustments()
    {
        StopCoroutine("InterpolateBrightness");
        StopCoroutine("InterpolateContrast");
        StopCoroutine("InterpolateSaturation");

        switch (brightness)
        {
            case Intensity.High:
                StartCoroutine("InterpolateBrightness", 0.25f * brightnessMultiplier);

                break;
            case Intensity.Medium:
                StartCoroutine("InterpolateBrightness", 0.0f);
                break;
            case Intensity.Low:
                StartCoroutine("InterpolateBrightness", -0.5f * brightnessMultiplier);
                break;
            default:
                StartCoroutine("InterpolateBrightness", 0.0f);
                break;
        }

        switch (contrast)
        {
            case Intensity.High:
                StartCoroutine("InterpolateContrast", 30.0f * contrastMultiplier);
                break;
            case Intensity.Medium:
                StartCoroutine("InterpolateContrast", 0.0f);
                break;
            case Intensity.Low:
                StartCoroutine("InterpolateContrast", -30.0f * contrastMultiplier);
                break;
            default:
                StartCoroutine("InterpolateContrast", 0.0f);
                break;
        }

        switch (saturation)
        {
            case Intensity.High:
                StartCoroutine("InterpolateSaturation", 40.0f * brightnessMultiplier);
                break;
            case Intensity.Medium:
                StartCoroutine("InterpolateSaturation", 10.0f * brightnessMultiplier);
                break;
            case Intensity.Low:
                StartCoroutine("InterpolateSaturation", -20.0f * brightnessMultiplier);
                break;
            default:
                StartCoroutine("InterpolateSaturation", 0.0f);
                break;
        }
    }

    IEnumerator InterpolateBrightness(float value)
    {
        float diff = colorAdjustments.postExposure.value - value;

        for (int i = 0; i < 25; i++)
        {
            colorAdjustments.postExposure.value -= diff * 0.1f;
            diff = colorAdjustments.postExposure.value - value;

            yield return new WaitForSeconds(0.1f);
        }

        colorAdjustments.postExposure.value = value;
        yield return null;
    }

    IEnumerator InterpolateContrast(float value)
    {
        float diff = colorAdjustments.contrast.value - value;

        for (int i = 0; i < 25; i++)
        {
            colorAdjustments.contrast.value -= diff * 0.1f;
            diff = colorAdjustments.contrast.value - value;
            yield return new WaitForSeconds(0.1f);
        }

        colorAdjustments.contrast.value = value;
        yield return null;
    }

    IEnumerator InterpolateSaturation(float value)
    {
        float diff = colorAdjustments.saturation.value - value;

        for (int i = 0; i < 25; i++)
        {
            colorAdjustments.saturation.value -= diff * 0.1f;
            diff = colorAdjustments.saturation.value - value;
            yield return new WaitForSeconds(0.1f);
        }

        colorAdjustments.saturation.value = value;
        yield return null;
    }
    public void ManageWaveEffect()
    {

        switch (waveIntensity)
        {
            case Intensity.High:
                matClose.SetFloat("_Intensity", 1f);
                matClose.SetFloat("_Speed", 1f);

                matFar.SetFloat("_Intensity", 5f);
                matFar.SetFloat("_Speed", 1f);
                break;

            case Intensity.Medium:
                matClose.SetFloat("_Intensity", 0.5f);
                matClose.SetFloat("_Speed", 0.5f);

                matFar.SetFloat("_Intensity", 1f);
                matFar.SetFloat("_Speed", 0.5f);
                break;

            case Intensity.Low:
                matClose.SetFloat("_Intensity", 0.25f);
                matClose.SetFloat("_Speed", 0.25f);

                matFar.SetFloat("_Intensity", 0.5f);
                matFar.SetFloat("_Speed", 0.25f);
                break;

            default:

                matClose.SetFloat("_Intensity", 0.5f);
                matClose.SetFloat("_Speed", 0.5f);

                matFar.SetFloat("_Intensity", 1f);
                matFar.SetFloat("_Speed", 0.5f);

                break;
        }
    }

    IEnumerator SetHighWaveIntensity()
    {
        float cIntensity = 0.25f;
        float cSpeed = 0.25f;

        float fIntensity = 0.5f;
        float fSpeed = 0.25f;

        for (int i = 1; i < 11; i++)
        {
            matClose.SetFloat("_Intensity", cIntensity + 0.075f * i);
            matClose.SetFloat("_Speed", cSpeed + 0.075f * i);

            matFar.SetFloat("_Intensity", fIntensity + 0.45f);
            matFar.SetFloat("_Speed", fSpeed + 0.075f * i);
            yield return new WaitForSeconds(0.2f);
        }

        matClose.SetFloat("_Intensity", 1f);
        matClose.SetFloat("_Speed", 1f);

        matFar.SetFloat("_Intensity", 5f);
        matFar.SetFloat("_Speed", 1f);
    }

    IEnumerator SetMediumWaveIntensity()
    {
        float cIntensity = 1f;
        float cSpeed = 1f;

        float fIntensity = 5f;
        float fSpeed = 1f;

        for (int i = 1; i < 11; i++)
        {
            matClose.SetFloat("_Intensity", cIntensity - 0.05f * i);
            matClose.SetFloat("_Speed", cSpeed - 0.05f * i);

            matFar.SetFloat("_Intensity", fIntensity - 0.4f);
            matFar.SetFloat("_Speed", fSpeed - 0.05f * i);
            yield return new WaitForSeconds(0.2f);
        }

        matClose.SetFloat("_Intensity", 0.5f);
        matClose.SetFloat("_Speed", 0.5f);

        matFar.SetFloat("_Intensity", 1f);
        matFar.SetFloat("_Speed", 0.5f);
    }

    IEnumerator SetLowWaveIntensity()
    {
        float cIntensity = 0.5f;
        float cSpeed = 0.5f;

        float fIntensity = 1f;
        float fSpeed = 0.5f;

        for (int i = 1; i < 11; i++)
        {
            matClose.SetFloat("_Intensity", cIntensity - 0.025f * i);
            matClose.SetFloat("_Speed", cSpeed - 0.025f * i);

            matFar.SetFloat("_Intensity", fIntensity - 0.05f);
            matFar.SetFloat("_Speed", fSpeed - 0.025f * i);
            yield return new WaitForSeconds(0.2f);
        }

        matClose.SetFloat("_Intensity", 0.25f);
        matClose.SetFloat("_Speed", 0.25f);

        matFar.SetFloat("_Intensity", 0.5f);
        matFar.SetFloat("_Speed", 0.25f);
    }

    //Events

    void HandleHungerLowEvent()
    {
        if (netClose)
        {
            brightness = Intensity.Medium;
            saturation = Intensity.Medium;
        }
        else
        {
            brightness = Intensity.High;
            saturation = Intensity.High;
        }

        ManageColorAdjustments();
    }
    void HandleHungerMediumEvent()
    {
        if (netClose)
        {
            brightness = Intensity.Low;
            saturation = Intensity.Low;
        }
        else
        {
            brightness = Intensity.Medium;
            saturation = Intensity.Medium;
        }

        ManageColorAdjustments();
    }
    void HandleHungerHighEvent()
    {
        brightness = Intensity.Low;
        saturation = Intensity.Low;

        ManageColorAdjustments();
    }

    void HandleBoatArrivalEvent()
    {
        waveIntensity = Intensity.Low;
        ManageWaveEffect();
    }
    void HandleBoatArrivedEvent()
    {
        boatArrived = true;
        waveIntensity = Intensity.High;
        ManageWaveEffect();
        checkBoatDistance();
    }

    void HandleBoatGoneEvent()
    {
        boatArrived = false;
        waveIntensity = Intensity.Medium;
        ManageWaveEffect();
    }

    void HandleInShadowEvent()
    {
        StopCoroutine("InterpolateVingetteShadow");
        StartCoroutine("InterpolateVingetteShadow", 0.2f);
    }

    void HandleOutShadowEvent()
    {
        StopCoroutine("InterpolateVingetteShadow");
        StartCoroutine("InterpolateVingetteShadow", 0.0f);
    }

    IEnumerator InterpolateVingetteShadow(float value)
    {
        float diff = vingetteShadowIntensity - value;

        for (int i = 0; i < 25; i++)
        {
            vingetteShadowIntensity -= diff * 0.1f;
            diff = vingetteShadowIntensity - value;
            yield return new WaitForSeconds(0.1f);
        }

        vingetteShadowIntensity = value;
        yield return null;
    }


    bool intensitisSettet = false;
    void checkBoatDistance()
    {
        if (!boatArrived) return;

        GameObject fishnet = GameObject.Find("Fishnet");
        if (fishnet != null)
        {
            float distance = (Camera.main.GetComponent<CameraScript>().target.transform.position - fishnet.transform.position).magnitude;
            if (distance <= 15f)
            {
                if (!intensitisSettet)
                {
                    StopCoroutine("InterpolateVingetteNetIntensity");
                    StartCoroutine("InterpolateVingetteNetIntensity", 0.1f);
                    contrast = Intensity.Low;
                    //if (brightness == Intensity.High) brightness = Intensity.Medium;
                    //else if (brightness == Intensity.Medium) brightness = Intensity.Low;
                    //
                    //if (saturation == Intensity.High) saturation = Intensity.Medium;
                    //else if (saturation == Intensity.Medium) saturation = Intensity.Low;
                    intensitisSettet = true;

                    ManageColorAdjustments();
                }
                netClose = true;
            }
            else
            {
                if (intensitisSettet)
                {
                    StopCoroutine("InterpolateVingetteNetIntensity");
                    StartCoroutine("InterpolateVingetteNetIntensity", 0.0f);
                    contrast = Intensity.Medium;
                    //if (brightness == Intensity.Low) brightness = Intensity.Medium;
                    //else if (brightness == Intensity.Medium) brightness = Intensity.High;
                    //
                    //if (saturation == Intensity.Low) saturation = Intensity.Medium;
                    //else if (saturation == Intensity.Medium) saturation = Intensity.High;
                    intensitisSettet = false;
                    ManageColorAdjustments();
                }

                netClose = false;
            }
        }
        else
        {
            contrast = Intensity.Medium;
            netClose = false;
        }

        Invoke("checkBoatDistance", 0.5f);
    }

    IEnumerator InterpolateVingetteNetIntensity(float value)
    {
        float diff = vingetteNetIntensity - value;

        for (int i = 0; i < 25; i++)
        {
            vingetteNetIntensity -= diff * 0.1f;
            diff = vingetteNetIntensity - value;
            yield return new WaitForSeconds(0.1f);
        }

        vingetteNetIntensity = value;
        yield return null;
    }
}
