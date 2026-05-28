using UnityEngine;
using System.Collections;

public class DelayedIntro : MonoBehaviour
{
    private int delay = 8;
    
    void Start()
    {
        StartCoroutine(delayedAudio());
    }

    IEnumerator delayedAudio()
    {
        yield return new WaitForSeconds(delay);
        
        GetComponent<AudioSource>().Play();
    }
}
