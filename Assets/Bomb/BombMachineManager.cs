using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BombMachineManager : MonoBehaviour
{
    public static BombMachineManager Instance { get; private set; }

    public List<RotatingRoll> allRolls = new List<RotatingRoll>();
    
    public float staggerDelay = 0.05f;
    
    public Material riddleColorMaterial1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(5f);
        TriggerRollsByMaterial(riddleColorMaterial1);
        
    }

    public void RegisterRoll(RotatingRoll roll)
    {
        if (!allRolls.Contains(roll))
        {
            allRolls.Add(roll);
        }
    }

    public void UnregisterRoll(RotatingRoll roll)
    {
        if (allRolls.Contains(roll))
        {
            allRolls.Remove(roll);
        }
    }
    
    public void TriggerRollsByMaterial(Material targetMaterial)
    {
        if (targetMaterial == null) return;

        StartCoroutine(StaggeredRollActivation(targetMaterial));
    }

    private IEnumerator StaggeredRollActivation(Material targetMaterial)
    {
        foreach (RotatingRoll roll in allRolls)
        {
            if (roll.GetMaterial() != null && roll.GetMaterial() == targetMaterial)
            {
                roll.rotationSpeed = Random.Range(95, 105);
                
                roll.StartSpinning();
                
                yield return new WaitForSeconds(staggerDelay);
            }
        }
    }
}