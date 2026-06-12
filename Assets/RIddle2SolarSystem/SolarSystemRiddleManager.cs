using UnityEngine;
using System.Collections;

public class SolarSystemRiddleManager : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor earthSocket;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor marsSocket;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor neptuneSocket;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor saturnSocket;

    public GameObject correctEarth;
    public GameObject correctMars;
    public GameObject correctNeptune;
    public GameObject correctSaturn;
    
    public Transform earthPivot;
    public Transform marsPivot;
    public Transform neptunePivot;
    public Transform saturnPivot;

    public float targetEarthAngle; 
    public float targetMarsAngle;
    public float targetNeptuneAngle;
    public float targetSaturnAngle;

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
        
        if (!earthSocket.hasSelection || earthSocket.interactablesSelected[0].transform.gameObject != correctEarth) 
            return false;
        if (!IsAngleCorrect(earthPivot.eulerAngles.y-180, targetEarthAngle)) 
            return false;
        
        if (!marsSocket.hasSelection || marsSocket.interactablesSelected[0].transform.gameObject != correctMars) 
            return false;
        if (!IsAngleCorrect(marsPivot.eulerAngles.y-180, targetMarsAngle)) 
            return false;

        if (!neptuneSocket.hasSelection || neptuneSocket.interactablesSelected[0].transform.gameObject != correctNeptune) 
            return false;
        if (!IsAngleCorrect(neptunePivot.eulerAngles.y-180, targetNeptuneAngle)) 
            return false;

        if (!saturnSocket.hasSelection || saturnSocket.interactablesSelected[0].transform.gameObject != correctSaturn) 
            return false;
        if (!IsAngleCorrect(saturnPivot.eulerAngles.y-180, targetSaturnAngle)) 
            return false;

        return true; 
    }

    bool IsAngleCorrect(float current, float target)
    {
        float diff = Mathf.DeltaAngle(current, target);
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