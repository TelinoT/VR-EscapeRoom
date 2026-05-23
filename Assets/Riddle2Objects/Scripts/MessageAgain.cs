using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class MessageAgain : XRSimpleInteractable
{
    public float pushDistance = 0.05f;
    
    private Vector3 originalPosition;

    private bool canClick = true;

    public AudioSource story;

    protected override void Awake()
    {
        base.Awake();

        originalPosition = transform.localPosition;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (canClick)
        {
            canClick = false;
            ToggleLights();   
        }
    }
    
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        
        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor  && canClick)
        {
            canClick = false;
            ToggleLights();
        }
    }

    private void ToggleLights()
    {
        story.Play();
        
        StopAllCoroutines(); 
        StartCoroutine(ButtonPressAnimation());
    }
    
    private IEnumerator ButtonPressAnimation()
    {
        this.GetComponent<AudioSource>().Play();
        transform.localPosition = originalPosition + new Vector3(pushDistance, 0, 0);
        
        yield return new WaitForSeconds(1f);
        
        transform.localPosition = originalPosition;

        canClick = true;
    }
}
