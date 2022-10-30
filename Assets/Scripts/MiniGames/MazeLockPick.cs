using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeLockPick : MonoBehaviour
{
    public LockPick LockPick;
    void OnEnable()
    {
        LockPick = transform.parent.GetComponentInChildren(typeof(LockPick)) as LockPick;
    }
    private void OnTriggerEnter(Collider other)
    {
        LockPick.ReturnHome();
    }
}
