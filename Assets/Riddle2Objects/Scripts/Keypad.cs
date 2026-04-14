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

    public void ButtonPressed(string value)
    {
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
}
