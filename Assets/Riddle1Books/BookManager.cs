using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BookManager : MonoBehaviour
{
    [System.Serializable]
    public struct BookSlot
    {
        public XRSocketInteractor socket;
        public BookID expectedBook;
    }
    
    public BookSlot[] slots;

    private bool isSolved = false;

    public GameObject doneText;
    
    public UnityEvent onPuzzleSolved;

    public void CheckPuzzle()
    {
        if (isSolved)
        {
            return;
        }

        foreach (var slot in slots)
        {
            if (!slot.socket.hasSelection) return;

            BookID currentBook = slot.socket.interactablesSelected[0].transform.GetComponent<BookID>();

            if (currentBook.spot != slot.expectedBook.spot) return;

        }

        isSolved = true;
        onPuzzleSolved.Invoke();
    }
}
