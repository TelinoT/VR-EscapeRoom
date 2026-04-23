using UnityEngine;

public class Phone : MonoBehaviour
{
   public string correctNumber = "";
   private string current = "";

   public AudioSource receiverSource;
   public AudioClip story;
   public AudioClip wrongNumberAud;

   public void addNum(string num)
   {
      current += num;

      if (current.Length == correctNumber.Length)
      {
         if (current.Equals(correctNumber))
         {
            Debug.Log("correct number");
            
            current = "";
            
            if (story != null)
            {
               receiverSource.clip = story;
               receiverSource.Play();
            }

         }
         else
         {
            current = "";
            
            Debug.Log("wrong number");

            if (wrongNumberAud != null)
            {
               receiverSource.clip = wrongNumberAud;
               receiverSource.Play();
            }
         }
      }
   }
}
