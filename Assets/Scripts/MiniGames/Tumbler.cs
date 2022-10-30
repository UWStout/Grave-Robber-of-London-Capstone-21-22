using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tumbler : MonoBehaviour
{
    public LockpickMinigame LockPick;
    bool Stop = true;
    void OnEnable()
    {
        LockPick = transform.parent.GetComponentInParent(typeof(LockpickMinigame)) as LockpickMinigame;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Stop && other.name == "LockPick")
        {
            Debug.Log("Lock pick picked");
            LockPick.Solve();
            Destroy(gameObject);
            Stop = false;
        }
        
    }
}
