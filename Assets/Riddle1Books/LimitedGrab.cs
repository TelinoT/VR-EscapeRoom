using UnityEngine;


public class LimitedGrab : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    public float maxDistance;
    
    public override bool IsSelectableBy(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor)
    {
        bool normalGrab = base.IsSelectableBy(interactor);
        
        if (isSelected) 
        {
            return normalGrab;
        }
        
        float distance = Vector3.Distance(transform.position, interactor.transform.position);
        return normalGrab && (distance <= maxDistance);
    }
    
    public override bool IsHoverableBy(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRHoverInteractor interactor)
    {
        bool normalHover = base.IsHoverableBy(interactor);
        
        if (isSelected) 
        {
            return normalHover;
        }
        
        float distance = Vector3.Distance(transform.position, interactor.transform.position);
        return normalHover && (distance <= maxDistance);
    }
}
