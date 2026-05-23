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

    void Awake()
    {
        grab = GetComponent<LimitedGrab>();
        grab.selectEntered.AddListener(StartCrank);
        grab.selectExited.AddListener(StopCrank);
    }

    private void StartCrank(SelectEnterEventArgs args)
    {
        if (isFinished) return;
        isGrabbed = true;
        handTransform = args.interactorObject.transform;

        Vector3 dirToHand = handTransform.position - transform.position;
        previousHandDir = Vector3.ProjectOnPlane(dirToHand, transform.up).normalized;
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
            Vector3 currentHandDir = Vector3.ProjectOnPlane(dirToHand, transform.up).normalized;

            float deltaAngle = Vector3.SignedAngle(previousHandDir, currentHandDir, transform.up);
            currentRotation += deltaAngle;
            transform.localRotation = Quaternion.Euler(-90, currentRotation, 0);

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