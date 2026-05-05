using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class Lightswitch : XRSimpleInteractable
{
    public GameObject[] lights;

    public GameObject[] appearingTexts;

    public float dimmedLight = 0.5f;

    private bool isOn = true;
    
    public float pushDistance = 0.05f;

    private float[] originalIntensities;

    private Vector3 originalPosition;

    protected override void Awake()
    {
        base.Awake();

        originalPosition = transform.localPosition;
        
        originalIntensities = new float[lights.Length];
        
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i] != null)
            {
                Light lightComponent = lights[i].GetComponent<Light>();
                if (lightComponent != null)
                {
                    originalIntensities[i] = lightComponent.intensity;
                }
            }
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        ToggleLights();
        
    }
    
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        
        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor)
        {
            ToggleLights();
        }
    }

    private void ToggleLights()
    {
        isOn = !isOn;

        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i] != null)
            {
                Light lightComponent = lights[i].GetComponent<Light>();
                if (lightComponent != null)
                {
                    if (isOn)
                    {
                        lightComponent.intensity = originalIntensities[i];
                    }
                    else
                    {
                        lightComponent.intensity = dimmedLight;
                    }
                }
            }

        }
        
        foreach (GameObject text in appearingTexts)
        {
            if(text != null) text.SetActive(!isOn);
        }
        
        StopAllCoroutines(); 
        StartCoroutine(ButtonPressAnimation());
    }
    
    private IEnumerator ButtonPressAnimation()
    {
        transform.localPosition = originalPosition + new Vector3(0, 0, pushDistance);
        
        yield return new WaitForSeconds(1f);
        
        transform.localPosition = originalPosition;
    }
}
