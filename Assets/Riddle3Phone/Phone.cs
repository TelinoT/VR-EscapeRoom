using UnityEngine;
using System.Collections; // Required for Coroutines

public class Phone : MonoBehaviour
{
   public string correctNumber = "";
   private string current = "";

   public AudioSource receiverSource;
   
   [Header("Audio Clips")]
   [Tooltip("Drag the phone ringing sound effect here.")]
   public AudioClip phoneRingAud; 
   public AudioClip story;
   public AudioClip wrongNumberAud;

   private Coroutine activeCallRoutine;

   public void addNum(string num)
   {
      current += num;
      
      Debug.Log(num);

      if (current.Length == correctNumber.Length)
      {
         if (current.Equals(correctNumber))
         {
            Debug.Log("correct number");
            current = "";
            
            // If another sequence is playing (like the wrong number clip), cut it off safely
            if (activeCallRoutine != null)
            {
               StopCoroutine(activeCallRoutine);
            }

            // Start our sequential phone call audio flow
            activeCallRoutine = StartCoroutine(PlayRingThenStorySequence());
         }
         else
         {
            current = "";
            Debug.Log("wrong number");

            if (activeCallRoutine != null)
            {
               StopCoroutine(activeCallRoutine);
               activeCallRoutine = null;
            }

            if (wrongNumberAud != null)
            {
               receiverSource.clip = wrongNumberAud;
               receiverSource.Play();
            }
         }
      }
   }

   private IEnumerator PlayRingThenStorySequence()
   {
      // --- Phase 1: Play the Phone Ring ---
      if (phoneRingAud != null)
      {
         receiverSource.clip = phoneRingAud;
         receiverSource.Play();

         // Pause the code execution thread for the exact length of the audio clip
         yield return new WaitForSeconds(phoneRingAud.length);
      }

      // --- Phase 2: Automatically Swap and Play the Story ---
      if (story != null)
      {
         receiverSource.clip = story;
         receiverSource.Play();
      }

      activeCallRoutine = null;
   }
}