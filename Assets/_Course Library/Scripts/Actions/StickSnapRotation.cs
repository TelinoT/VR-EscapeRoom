using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StickSnapRotation : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("The parent pivot group (e.g., Pivot_Stick_Middle_Long) that needs to turn.")]
    public Transform pivotToRotate; 
    
    [Tooltip("Drag the central Sun_lowerHalf transform here.")]
    public Transform centerSun; 

    [Header("Movement Settings")]
    [Tooltip("The speed the arm glides between mechanical snap slots (degrees per second).")]
    public float travelSpeed = 180f;

    private XRSimpleInteractable simpleInteractable;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor currentInteractor = null;
    
    private const float SNAP_ANGLE = 15f; 

    private float initialGrabHandAngle = 0f;
    private float initialPivotWorldAngle = 0f;
    
    // Tracks the targeted 15-degree slot assignment mathematically
    private float targetSnappedAngle = 0f;
    private float currentVisualAngle = 0f;

    private bool hasPlayed = false;

    void Awake()
    {
        simpleInteractable = GetComponent<XRSimpleInteractable>();
        
        currentVisualAngle = targetSnappedAngle;
        
        if (simpleInteractable != null)
        {
            simpleInteractable.selectEntered.AddListener(OnStickActivated);
            simpleInteractable.selectExited.AddListener(OnStickDeactivated);
        }
    }

    void Start()
    {
        if (pivotToRotate != null)
        {
            // Initialize our tracking angles to match the scene placement
            targetSnappedAngle = pivotToRotate.rotation.eulerAngles.y;
            currentVisualAngle = targetSnappedAngle;
        }
    }

    private void OnStickActivated(SelectEnterEventArgs args)
    {
        currentInteractor = args.interactorObject;

        if (currentInteractor != null && pivotToRotate != null && centerSun != null)
        {
            Vector3 handPos = currentInteractor.transform.position;
            Vector3 directionToHand = handPos - centerSun.position;
            directionToHand.y = 0;

            if (directionToHand.sqrMagnitude > 0.001f)
            {
                initialGrabHandAngle = Mathf.Atan2(directionToHand.x, directionToHand.z) * Mathf.Rad2Deg;
            }

            initialPivotWorldAngle = targetSnappedAngle;
        }
    }

    private void OnStickDeactivated(SelectExitEventArgs args)
    {
        currentInteractor = null;
    }

    void Update()
    {
        if (pivotToRotate == null) return;

        // 1. Calculate target snapping indices only while actively being dragged
        if (currentInteractor != null && centerSun != null)
        {
            Vector3 handPos = currentInteractor.transform.position;
            Vector3 directionToHand = handPos - centerSun.position;
            directionToHand.y = 0;

            if (directionToHand.sqrMagnitude > 0.001f)
            {
                float currentHandAngle = Mathf.Atan2(directionToHand.x, directionToHand.z) * Mathf.Rad2Deg;
                float angleDelta = currentHandAngle - initialGrabHandAngle;
                float targetWorldAngle = initialPivotWorldAngle + angleDelta;

                // Under the hood, this target variable STILL instantly jumps by 15 degrees
                targetSnappedAngle = Mathf.Round(targetWorldAngle / SNAP_ANGLE) * SNAP_ANGLE;
            }
            
            if (currentVisualAngle != targetSnappedAngle && hasPlayed)
            {
                this.GetComponent<AudioSource>().Play();
            }

            hasPlayed = true;
        }

        // 2. SMOOTH TRANSIT: Move smoothly from our current visual position to the hard targets
        // Mathf.MoveTowardsAngle automatically handles 360 to 0 wrap-arounds smoothly!
        currentVisualAngle = Mathf.MoveTowardsAngle(currentVisualAngle, targetSnappedAngle, travelSpeed * Time.deltaTime);

        // 3. Force the clean world coordinate translation onto the parent transform row
        pivotToRotate.rotation = Quaternion.Euler(0, currentVisualAngle, 0);
    }

    void OnDestroy()
    {
        if (simpleInteractable != null)
        {
            simpleInteractable.selectEntered.RemoveListener(OnStickActivated);
            simpleInteractable.selectExited.RemoveListener(OnStickDeactivated);
        }
    }
}