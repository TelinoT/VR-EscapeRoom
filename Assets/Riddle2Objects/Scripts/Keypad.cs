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

    public void ButtonPressed(string value)
    {
		if(isGreek){
			value = ConvertToGreekLetter(value);
		}	

        currentInput += value;	

        displayText.text = currentInput;

        if (currentInput.Length == correctCode.Length)
        {
            if (currentInput.Equals(correctCode))
            {
                onCorrect.Invoke();
            }
            else
            {
                onWrong.Invoke();
            }
            currentInput = "";
        }
    }

    public void Incorrect()
    {
        Debug.Log("Incorrect code");
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
