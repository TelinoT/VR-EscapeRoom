using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class RayHider : MonoBehaviour
{
    public InputActionReference teleportAimButton;
    public GameObject teleportRayObject;
    
    private void Start()
    {
        teleportRayObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        teleportAimButton.action.started += OnAimStart;
        teleportAimButton.action.canceled += OnTeleportExecute;
    }
    
    private void OnAimStart(InputAction.CallbackContext context)
    {
        teleportRayObject.SetActive(true);
    }
    
    private void OnTeleportExecute(InputAction.CallbackContext context)
    {
        StartCoroutine(TurnOffRayDelay());
    }

    private void OnDisable()
    {
        teleportAimButton.action.started -= OnAimStart;
        
        teleportAimButton.action.canceled -= OnTeleportExecute;
    }
    

    private IEnumerator TurnOffRayDelay()
    {
        yield return new WaitForEndOfFrame();
        teleportRayObject.SetActive(false);
    }
}
