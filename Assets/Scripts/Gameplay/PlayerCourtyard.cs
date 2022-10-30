using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCourtyard : Player
{
    protected override IEnumerator Footstep()
    {
        //Debug.Log("Child call");
        StepCooldown = true;
        yield return new WaitForSeconds(0.0f);
    }
}
