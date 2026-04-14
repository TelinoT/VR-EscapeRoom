using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

public class RayHider : MonoBehaviour
{
    public InputActionReference teleportAimButton;
    public GameObject teleportRayObject;
    
    public XRBaseInteractor blockingInteractor;
    
    private void Start()
    {
        teleportRayObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        teleportAimButton.action.started += OnAimStart;
        teleportAimButton.action.canceled += OnTeleportExecute;
        
        if (blockingInteractor != null)
        {
            blockingInteractor.selectEntered.AddListener(ForceTurnOff);
        }
    }
    
    private void OnDisable()
    {
        if (blockingInteractor != null)
        {
            blockingInteractor.selectEntered.RemoveListener(ForceTurnOff);
        }
        
        teleportAimButton.action.started -= OnAimStart;
        teleportAimButton.action.canceled -= OnTeleportExecute;
    }
    
    private void OnAimStart(InputAction.CallbackContext context)
    {
        if (blockingInteractor != null && blockingInteractor.hasSelection)
        {
            return; 
        }

        teleportRayObject.SetActive(true);
    }
    
    private void OnTeleportExecute(InputAction.CallbackContext context)
    {
        if (!teleportRayObject.activeSelf) return;
        
        XRBaseInteractor myInteractor = teleportRayObject.GetComponent<XRBaseInteractor>();
        
        if (myInteractor != null && myInteractor.hasSelection)
        {
            myInteractor.selectExited.AddListener(TurnOffAfterDrop);
        }
        else
        {
            StartCoroutine(TurnOffRayDelay());
        }
    }

    private void TurnOffAfterDrop(SelectExitEventArgs args)
    {
        teleportRayObject.SetActive(false); 
        
        args.interactorObject.selectExited.RemoveListener(TurnOffAfterDrop);
    }
    
    private IEnumerator TurnOffRayDelay()
    {
        yield return new WaitForSeconds(0.1f);
        teleportRayObject.SetActive(false);
    }
    
    private void ForceTurnOff(SelectEnterEventArgs args)
    {
        teleportRayObject.SetActive(false);
    }
}