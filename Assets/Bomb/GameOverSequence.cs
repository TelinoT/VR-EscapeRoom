using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Required for using Lists

public class GameOverSequence : MonoBehaviour
{
    public AudioSource firstAudioSource;
    
    public AudioSource secondAudioSource;
    
    public List<GameObject> objectsToActivate = new List<GameObject>();

    private bool isRunning = false;
    
    public void StartSequence()
    {
        if (!isRunning)
        {
            StartCoroutine(ExecuteSequenceRoutine());
        }
    }

    private void Start()
    {
        StartSequence();
    }

    private IEnumerator ExecuteSequenceRoutine()
    {
        isRunning = true;

        if (firstAudioSource != null && firstAudioSource.clip != null)
        {
            firstAudioSource.Play();
            
            yield return new WaitForSeconds(firstAudioSource.clip.length);
        }


        if (objectsToActivate != null && objectsToActivate.Count > 0)
        {
            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }
        
        yield return new WaitForSeconds(5f);

        if (secondAudioSource != null)
        {
            secondAudioSource.Play();
        }

        isRunning = false;
    }
}