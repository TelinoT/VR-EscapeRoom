using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

public class PhysicalMazeButton : XRSimpleInteractable
{
    [Header("Maze Configuration")]
    public MazeManager mazeManager;
    [Tooltip("Set to (0,1) for Up, (0,-1) for Down, (-1,0) for Left, (1,0) for Right")]
    public Vector2 moveDirection;

    [Header("Physical Mechanics")]
    public float pushDistance = 0.02f; // How far the button sinks into the keyboard
    public float movementSpeed = 0.1f; // How fast the snake moves while holding the button

    private Vector3 originalPosition;
    private bool isPressed = false;
    private Coroutine moveCoroutine;

    protected override void Awake()
    {
        base.Awake();
        originalPosition = transform.localPosition;
    }

    // Triggers the exact moment your virtual hand touches the wood/plastic
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        
        // Ensure it's a physical hand touching it, not a laser pointer!
        if (args.interactorObject is XRDirectInteractor && !isPressed)
        {
            PressDown();
        }
    }

    // Triggers the exact moment your virtual hand pulls away
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
        
        // Optional: Play a mechanical click sound
        // GetComponent<AudioSource>()?.Play();

        // Push the 3D model down into the desk. 
        // NOTE: Depending on your model, you might need to change the Y to a Z: new Vector3(0, 0, pushDistance)
        transform.localPosition = originalPosition - new Vector3(0, pushDistance, 0);
        
        // Start moving the snake repeatedly
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveSnakeContinuously());
    }

    private void PopUp()
    {
        isPressed = false;
        
        // Snap the button back to its original resting height
        transform.localPosition = originalPosition;
        
        // Stop the snake from moving
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
    }

    // This loop acts like holding down a key on a keyboard
    private IEnumerator MoveSnakeContinuously()
    {
        while (isPressed)
        {
            // Tell the MazeManager to move the snake one step
            mazeManager.MoveSnake(moveDirection);
            
            // Wait a tiny fraction of a second before stepping again
            yield return new WaitForSeconds(movementSpeed);
        }
    }
}
