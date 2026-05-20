using System;
using UnityEngine;

public class RotatingRoll : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 100f;
    
    public Material myMaterial;
    private bool shouldRotate = false;

    void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Bomb_rolle", System.StringComparison.OrdinalIgnoreCase))
            {
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    myMaterial = renderer.sharedMaterial;
                    break; 
                }
            }
        }
    }

    private void Start()
    {
        if (BombMachineManager.Instance != null)
        {
            Debug.Log("is gonna register");
            BombMachineManager.Instance.RegisterRoll(this);
        }
    }

    void Update()
    {
        if (shouldRotate)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    public Material GetMaterial()
    {
        return myMaterial;
    }

    public void StartSpinning()
    {
        shouldRotate = true;
    }
    
    public void StopSpinning()
    {
        shouldRotate = false;
    }

    void OnDestroy()
    {
        if (BombMachineManager.Instance != null)
        {
            BombMachineManager.Instance.UnregisterRoll(this);
        }
    }
}