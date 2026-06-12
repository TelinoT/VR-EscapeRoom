using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

public class PhysicalMazeButton : XRSimpleInteractable
{
    public MazeManager mazeManager;
    public Vector2 moveDirection;

    public float pushDistance = 0.02f; 
    public float movementSpeed = 0.1f; 

    private Vector3 originalPosition;
    private bool isPressed = false;
    private Coroutine moveCoroutine;

    protected override void Awake()
    {
        base.Awake();
        originalPosition = transform.localPosition;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        
        if (args.interactorObject is XRDirectInteractor && !isPressed)
        {
            PressDown();
        }
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        
        if (args.interactorObject is XRDirectInteractor && isPressed)
        {
            PopUp();
        }
    }

    private void PressDown()
    {
        isPressed = true;

        this.GetComponent<AudioSource>().Play();
        
        transform.localPosition = originalPosition - new Vector3(0, pushDistance, 0);
        
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveSnakeContinuously());
    }

    private void PopUp()
    {
        isPressed = false;
        
        transform.localPosition = originalPosition;
        
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
    }

    private IEnumerator MoveSnakeContinuously()
    {
        while (isPressed)
        {
            mazeManager.MoveSnake(moveDirection);
            
            yield return new WaitForSeconds(movementSpeed);
        }
    }
}
