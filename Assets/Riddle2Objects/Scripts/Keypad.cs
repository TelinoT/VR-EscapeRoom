using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Keypad : MonoBehaviour
{
    public string correctCode;
    private string currentInput;

    public TextMeshProUGUI displayText;

    public UnityEvent onCorrect;
    public UnityEvent onWrong;
    
    public bool isGreek = false;

    private string greekInput;

    public void ButtonPressed(string value)
    {
	    currentInput += value;	

        displayText.text = currentInput;
        
        if(isGreek){
	        value = ConvertToGreekLetter(value);
	        greekInput += value;
	        displayText.text = greekInput;
        }

        if (currentInput.Length == correctCode.Length)
        {
            if (currentInput.Equals(correctCode))
            {
                onCorrect.Invoke();
                Debug.Log("invoked correct");
            }
            else
            {
	            StartCoroutine(waitABit());
                onWrong.Invoke();
            }
            currentInput = "";
            greekInput = "";
        }
    }

    public void Incorrect()
    {
        Debug.Log("Incorrect code");
    }

    IEnumerator waitABit()
    {
	    yield return new WaitForSeconds(1f);
	    displayText.text = "";
    }

	public string ConvertToGreekLetter(string inputNumber)
	{
    	return inputNumber switch
    	{
        	"1" => "α",
       	 	"2" => "δ",
        	"3" => "θ",
        	"4" => "λ",
        	"5" => "ξ",
        	"6" => "π",
        	"7" => "σ",
        	"8" => "φ",
        	"9" => "ψ",
        	"0" => "ω",
        	_ => inputNumber
    	};
	}
}
