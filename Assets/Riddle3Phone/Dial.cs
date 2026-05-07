using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class Dial : MonoBehaviour
{
    public Phone phone;
    private LimitedGrab grab;
    private Rigidbody rb;

    public float degree = 30f;
    public float returnSpeed = 150f;

    public string[] chars = { "0", "9", "8", "7", "6", "5", "4", "3", "2", "1", "#", "*" };

    private Coroutine returnRoutine;
    private Quaternion startRotation;

    private bool isGrabbed = false;
    private Transform handTransform;
    
    private Vector3 previousHandDir;
    
    private float currentVisualAngle = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<LimitedGrab>();
        
        startRotation = transform.localRotation;

        if (grab != null)
        {
            grab.selectEntered.AddListener(OnDialGrabbed);
            grab.selectExited.AddListener(OnReleased);
        }
    }

    private void OnDialGrabbed(SelectEnterEventArgs args)
    {
        if (returnRoutine != null)
        {
            StopCoroutine(returnRoutine);
            returnRoutine = null;
        }
        
        rb.isKinematic = true;
        isGrabbed = true;
        handTransform = args.interactorObject.transform;

        Vector3 dirToHand = handTransform.position - transform.position;
        
        previousHandDir = Vector3.ProjectOnPlane(dirToHand, transform.up).normalized;
    }

    void Update()
    {
        if (isGrabbed && handTransform != null)
        {
            Vector3 dirToHand = handTransform.position - transform.position;
            Vector3 currentHandDir = Vector3.ProjectOnPlane(dirToHand, transform.up).normalized;

            float deltaAngle = Vector3.SignedAngle(previousHandDir, currentHandDir, transform.up);
            
            currentVisualAngle += deltaAngle;

            transform.localRotation = startRotation * Quaternion.Euler(0, currentVisualAngle, 0);

            previousHandDir = currentHandDir;
        }

    }

    public void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
        handTransform = null;
        
        int index = Mathf.RoundToInt(Mathf.Abs(currentVisualAngle) / degree);

        if (index > 0 && index <= chars.Length)
        {
            phone.addNum(chars[index]);
        }

        returnRoutine = StartCoroutine(SpinBackToZero());
    }
    
    private IEnumerator SpinBackToZero()
    {
        rb.isKinematic = true;

        while (Mathf.Abs(currentVisualAngle) > 0.1f)
        {
            currentVisualAngle = Mathf.MoveTowards(currentVisualAngle, 0f, returnSpeed * Time.deltaTime);
            
            transform.localRotation = startRotation * Quaternion.Euler(0, currentVisualAngle, 0);
            
            yield return null; 
        }

        currentVisualAngle = 0f;
        transform.localRotation = startRotation;
        
        rb.isKinematic = false;
        returnRoutine = null;
    }
}

/*using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class Dial : MonoBehaviour
{
    public Phone phone;
    private HingeJoint hinge;

    private LimitedGrab grab;
    private Rigidbody rb;

    public float degree = 30f;

    public float returnSpeed = 100f;

    public string[] chars = { "0", "9", "8", "7", "6", "5", "4", "3", "2", "1", "#", "*" };

    private Coroutine returnRoutine;
    private Quaternion startRotation;
    void Awake()
    {
        hinge = GetComponent<HingeJoint>();
        //Debug.Log("got hinge");
        rb = GetComponent<Rigidbody>();
        //Debug.Log("got rb");
        grab = GetComponent<LimitedGrab>();
        //Debug.Log("got grab");
        
        startRotation = transform.localRotation;
        //Debug.Log("got rotation");

        if (grab != null)
        {
            grab.selectEntered.AddListener(OnDialGrabbed);
            grab.selectExited.AddListener(OnReleased);
        }
        //Debug.Log("got listeners");

        StartCoroutine(SpinBackToZero(0));
        Debug.Log("started coroutine");
    }
    

    private void OnDialGrabbed(SelectEnterEventArgs args)
    {
        if (returnRoutine != null)
        {
            StopCoroutine(returnRoutine);
            returnRoutine = null;
        }
        rb.isKinematic = false;
    }

    public void OnReleased(SelectExitEventArgs args)
    {
        float rawAngle = hinge.angle;
        
        if (rawAngle > 0)
        {
            rawAngle -= 360f; 
        }
        
        int index = Mathf.RoundToInt(Mathf.Abs(rawAngle) / degree);

        if (index > 0 && index <= chars.Length)
        {
            phone.addNum(chars[index-1]);
        }

        returnRoutine = StartCoroutine(SpinBackToZero(rawAngle));
    }
    
    private IEnumerator SpinBackToZero(float startingAngle)
    {
        Debug.Log("before 5 seconds");
        yield return new WaitForSeconds(5f);
        Debug.Log("after 5 seconds");
        
        float rawAngle = hinge.angle;
        
        if (rawAngle > 0)
        {
            rawAngle -= 360f; 
        }
        
        rb.isKinematic = true;

        float currentAngle = startingAngle;

        while (Mathf.Abs(currentAngle) > 0.1f)
        {
            currentAngle = Mathf.MoveTowards(currentAngle, 0f, returnSpeed * Time.deltaTime);

            transform.localRotation = startRotation * Quaternion.Euler(0, currentAngle, 0);
            
            yield return null; 
        }

        transform.localRotation = startRotation;
        
        rb.isKinematic = false;
        returnRoutine = null;
    }
}*/
