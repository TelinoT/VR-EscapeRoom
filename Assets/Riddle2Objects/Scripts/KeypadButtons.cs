using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KeypadButtons : XRSimpleInteractable
{
    public Keypad keypad;
    public string value;

    public Material touchMaterial;
    private Material originalMaterial;
    private MeshRenderer meshRenderer;

    protected override void Awake()
    {
        base.Awake();
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        
        meshRenderer.material = touchMaterial;
        keypad.ButtonPressed(value);

        if (this.GetComponent<AudioSource>() != null)
        {
            this.GetComponent<AudioSource>().Play();
        }
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        
        meshRenderer.material = originalMaterial;
    }
}
