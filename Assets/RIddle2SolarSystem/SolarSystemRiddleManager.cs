using UnityEngine;

using System.Collections;

public class SolarSystemRiddleManager : MonoBehaviour
{
    [Header("Sockets (The Empty Objects on the Sticks)")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor earthSocket;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor marsSocket;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor neptuneSocket;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor saturnSocket;

    [Header("Correct Planets (The Grabbable Planet Objects)")]
    public GameObject correctEarth;
    public GameObject correctMars;
    public GameObject correctNeptune;
    public GameObject correctSaturn;

    [Header("Sticks to Check Rotation")]
    public Transform earthStick;
    public Transform marsStick;
    public Transform neptuneStick;
    public Transform saturnStick;

    [Header("Target Angles (0 to 360)")]
    public float targetEarthAngle; 
    public float targetMarsAngle;
    public float targetNeptuneAngle;
    public float targetSaturnAngle;

    [Header("Reward")]
    public Transform sunUpperHalf;
    public GameObject hammer; 

    private bool isSolved = false;

    void Update()
    {
        // Constantly check if the puzzle is solved, but only if it hasn't been solved yet
        if (!isSolved && CheckSolution())
        {
            SolveRiddle();
        }
    }

    bool CheckSolution()
    {
        // --- 1. EARTH CHECK ---
        // Does the socket have an object? And is that object the Earth?
        if (!earthSocket.hasSelection || earthSocket.interactablesSelected[0].transform.gameObject != correctEarth) 
            return false;
        // Is the stick rotated to the correct angle?
        if (!IsAngleCorrect(earthStick.localEulerAngles.y, targetEarthAngle)) 
            return false;

        // --- 2. MARS CHECK ---
        if (!marsSocket.hasSelection || marsSocket.interactablesSelected[0].transform.gameObject != correctMars) 
            return false;
        if (!IsAngleCorrect(marsStick.localEulerAngles.y, targetMarsAngle)) 
            return false;

        // --- 3. NEPTUNE CHECK ---
        if (!neptuneSocket.hasSelection || neptuneSocket.interactablesSelected[0].transform.gameObject != correctNeptune) 
            return false;
        if (!IsAngleCorrect(neptuneStick.localEulerAngles.y, targetNeptuneAngle)) 
            return false;

        // --- 4. SATURN CHECK ---
        if (!saturnSocket.hasSelection || saturnSocket.interactablesSelected[0].transform.gameObject != correctSaturn) 
            return false;
        if (!IsAngleCorrect(saturnStick.localEulerAngles.y, targetSaturnAngle)) 
            return false;

        // If it passes EVERY check above without returning false, the puzzle is correct!
        return true; 
    }

    bool IsAngleCorrect(float current, float target)
    {
        // Calculate the shortest difference between the current angle and target angle
        float diff = Mathf.DeltaAngle(current, target);
        // Returns true if the stick is within 1 degree of the target angle (allows for tiny floating point errors)
        return Mathf.Abs(diff) < 1.0f; 
    }

    void SolveRiddle()
    {
        isSolved = true;
        StartCoroutine(OpenSunSequence());
    }

    IEnumerator OpenSunSequence()
    {
        Vector3 startPos = sunUpperHalf.localPosition;
        Vector3 endPos = startPos + new Vector3(0, 0.5f, 0); // Adjust the 0.5f if you want it to open higher
        float timeElasped = 0;
        float duration = 2.0f; // Takes 2 seconds to open

        while (timeElasped < duration) 
        {
            sunUpperHalf.localPosition = Vector3.Lerp(startPos, endPos, timeElasped / duration);
            timeElasped += Time.deltaTime;
            yield return null;
        }

        // Snap to final position to be perfectly accurate
        sunUpperHalf.localPosition = endPos;

        hammer.SetActive(true);

        // Enable picking up the hammer (Assuming it has an XRGrabInteractable)
        if (hammer.GetComponent<LimitedGrab>() != null)
        {
            hammer.GetComponent<LimitedGrab>().enabled = true;
        }
        
        if (BombMachineManager.Instance != null)
        {
            BombMachineManager.Instance.SolarSystemRiddleDone();
            BombMachineManager.Instance.riddlesUp();
        }
    }
}