using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StickSnapRotation : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("Drag the Pivot_Stick_Large (the parent) here")]
    public Transform pivotToRotate; 
    
    [Tooltip("Drag the Sun_lowerHalf here")]
    public Transform centerSun; 

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor currentInteractor = null;
    private const float SNAP_ANGLE = 15f; // 360 degrees / 24 positions = 15

    void Awake()
    {
        // Automatically find the interactable on this stick
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnGrab);
        interactable.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // The player grabbed the stick. Record which hand is grabbing it.
        currentInteractor = args.interactorObject;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // The player let go.
        currentInteractor = null;
    }

    void Update()
    {
        // If a hand is currently holding the stick...
        if (currentInteractor != null)
        {
            // 1. Find where the player's hand is relative to the sun
            Vector3 handPos = currentInteractor.transform.position;
            Vector3 direction = handPos - centerSun.position;
            
            // Ignore height (Y) so we only calculate the flat circle rotation
            direction.y = 0; 

            // 2. Calculate the exact angle of the hand from the center
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // 3. Round that angle to the nearest 15 degrees to create the "snap" effect
            float snappedAngle = Mathf.Round(targetAngle / SNAP_ANGLE) * SNAP_ANGLE;

            // 4. Smoothly rotate the PARENT PIVOT to that snapped angle
            Quaternion targetRotation = Quaternion.Euler(0, snappedAngle, 0);
            pivotToRotate.rotation = Quaternion.Lerp(pivotToRotate.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
