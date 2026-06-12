using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StickSnapRotation : MonoBehaviour
{
    public Transform pivotToRotate; 
    
    public Transform centerSun; 

    public float travelSpeed = 180f;

    private XRSimpleInteractable simpleInteractable;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor currentInteractor = null;
    
    private const float SNAP_ANGLE = 15f; 

    private float initialGrabHandAngle = 0f;
    private float initialPivotWorldAngle = 0f;
    
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

                targetSnappedAngle = Mathf.Round(targetWorldAngle / SNAP_ANGLE) * SNAP_ANGLE;
            }
            
            if (currentVisualAngle != targetSnappedAngle && hasPlayed)
            {
                this.GetComponent<AudioSource>().Play();
            }

            hasPlayed = true;
        }
        
        currentVisualAngle = Mathf.MoveTowardsAngle(currentVisualAngle, targetSnappedAngle, travelSpeed * Time.deltaTime);
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