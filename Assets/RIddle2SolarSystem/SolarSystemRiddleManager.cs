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

    [Header("Pivots to Check Rotation")]
    [Tooltip("CRITICAL: Drag the Pivot_Stick_... objects here instead of the raw meshes!")]
    public Transform earthPivot;
    public Transform marsPivot;
    public Transform neptunePivot;
    public Transform saturnPivot;

    [Header("Target World Angles (0 to 360)")]
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
        if (!isSolved && CheckSolution())
        {
            SolveRiddle();
        }
    }

    bool CheckSolution()
    {
        
        // --- 1. EARTH CHECK ---
        if (!earthSocket.hasSelection || earthSocket.interactablesSelected[0].transform.gameObject != correctEarth) 
            return false;
        // FIXED: Swapped to eulerAngles.y to read our clean world space assignments
        if (!IsAngleCorrect(earthPivot.eulerAngles.y-180, targetEarthAngle)) 
            return false;
        
        // --- 2. MARS CHECK ---
        if (!marsSocket.hasSelection || marsSocket.interactablesSelected[0].transform.gameObject != correctMars) 
            return false;
        if (!IsAngleCorrect(marsPivot.eulerAngles.y-180, targetMarsAngle)) 
            return false;

        // --- 3. NEPTUNE CHECK ---
        if (!neptuneSocket.hasSelection || neptuneSocket.interactablesSelected[0].transform.gameObject != correctNeptune) 
            return false;
        if (!IsAngleCorrect(neptunePivot.eulerAngles.y-180, targetNeptuneAngle)) 
            return false;

        // --- 4. SATURN CHECK ---
        if (!saturnSocket.hasSelection || saturnSocket.interactablesSelected[0].transform.gameObject != correctSaturn) 
            return false;
        if (!IsAngleCorrect(saturnPivot.eulerAngles.y-180, targetSaturnAngle)) 
            return false;

        return true; 
    }

    bool IsAngleCorrect(float current, float target)
    {
        float diff = Mathf.DeltaAngle(current, target);
        // Generous 1.5 degree buffer to account for smooth travel lerping adjustments
        return Mathf.Abs(diff) < 1.5f; 
    }

    void SolveRiddle()
    {
        isSolved = true;
        StartCoroutine(OpenSunSequence());
    }

    IEnumerator OpenSunSequence()
    {
        Vector3 startPos = sunUpperHalf.localPosition;
        Vector3 endPos = startPos + new Vector3(0, 0.5f, 0);
        float timeElasped = 0;
        float duration = 2.0f;

        while (timeElasped < duration) 
        {
            sunUpperHalf.localPosition = Vector3.Lerp(startPos, endPos, timeElasped / duration);
            timeElasped += Time.deltaTime;
            yield return null;
        }

        sunUpperHalf.localPosition = endPos;
        hammer.SetActive(true);

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