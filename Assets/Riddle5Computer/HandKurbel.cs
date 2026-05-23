using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class HandKurbel : MonoBehaviour
{
    public float degreesRequired = 720f; 
    public float currentRotation = 0f;

    public UnityEvent onPowerThresholdReached;

    private LimitedGrab grab;
    private Transform handTransform;
    private Vector3 previousHandDir;
    private bool isGrabbed = false;
    private bool isFinished = false;

    public GameObject noPower;
    public GameObject powering;
    
    // Cache the baseline layout rotation of the model
    private Quaternion startRotation;

    void Awake()
    {
        grab = GetComponent<LimitedGrab>();
        grab.selectEntered.AddListener(StartCrank);
        grab.selectExited.AddListener(StopCrank);
        
        // Lock in whatever rotation offset the model has in the inspector
        startRotation = transform.localRotation;
    }

    private void StartCrank(SelectEnterEventArgs args)
    {
        if (isFinished) return;
        isGrabbed = true;
        handTransform = args.interactorObject.transform;

        Vector3 dirToHand = handTransform.position - transform.position;
        
        // FIXED: Project onto the Z-axis (forward) because that is the axle we are rotating around
        previousHandDir = Vector3.ProjectOnPlane(dirToHand, transform.forward).normalized;
        
        this.gameObject.GetComponent<AudioSource>().Play();
        noPower.SetActive(false);
        powering.SetActive(true);
    }

    private void StopCrank(SelectExitEventArgs args)
    {
        isGrabbed = false;
        handTransform = null;
        this.gameObject.GetComponent<AudioSource>().Stop();
    }

    void Update()
    {
        if (isGrabbed && !isFinished)
        {
            Vector3 dirToHand = handTransform.position - transform.position;
            
            // FIXED: Track relative to the stable Z-axis (forward) axle
            Vector3 currentHandDir = Vector3.ProjectOnPlane(dirToHand, transform.forward).normalized;

            // Calculate how far the wrist twisted this frame
            float deltaAngle = Vector3.SignedAngle(previousHandDir, currentHandDir, transform.forward);
            currentRotation += deltaAngle;
            
            // Apply the new accumulated rotation directly to the Z-axis of the model's original baseline
            transform.localRotation = startRotation * Quaternion.Euler(0, 0, currentRotation);

            previousHandDir = currentHandDir;

            if (Mathf.Abs(currentRotation) >= degreesRequired)
            {
                Finish();
            }
        }
    }

    private void Finish()
    {
        isFinished = true;
        isGrabbed = false;
        onPowerThresholdReached.Invoke();
        Debug.Log("Power Restored!");
        powering.SetActive(false);
    }
}

