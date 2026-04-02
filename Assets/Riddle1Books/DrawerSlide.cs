using System;
using UnityEngine;
using System.Collections;
public class DrawerSlide : MonoBehaviour
{
    public Vector3 slideOffset = new Vector3(0, -1.5f, 0); 
    public float slideSpeed = 2f;

    public void OpenGate()
    {
        StartCoroutine(SlideGateDown());
    }

    private IEnumerator SlideGateDown()
    {
        Vector3 targetPos = transform.localPosition + slideOffset;
        
        while (Vector3.Distance(transform.localPosition, targetPos) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * slideSpeed);
            yield return null;
        }
        
        transform.localPosition = targetPos;
    }
}
