using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BribingOptions : MonoBehaviour
{
    private GameObject CurrentGuard; // The guard that caught the player
    private int BribeCost = 0; // The amount they are asking for the bribe

    /// <summary>
    /// Activates the bribe options for the guard catching the player.
    /// </summary>
    /// <param name="ActivatingGuard">
    /// A reference to the guard that catches the player.
    /// </param>
    /// <param name="BribeAmount">
    /// The amount of money required to bribe the guard.
    /// </param>
    public void StartBribe(GameObject ActivatingGuard, int BribeAmount)
    {
        CurrentGuard = ActivatingGuard;
        BribeCost = BribeAmount;
        UIManager.Instance.SetBribeOptions(true, GameManager.Instance.GetMoney() >= BribeAmount);
        Debug.Log("bribe started");
    }

    /// <summary>
    /// Deducts the required money, and the guard returns to their route.
    /// </summary>
    public void AcceptBribe()
    {
        GameManager.Instance.UpdateMoney(-BribeCost);
        CurrentGuard.GetComponent<Guard>().BribedReturnToRoute();
        UIManager.Instance.DialogueDisable();
        UIManager.Instance.SetBribeOptions(false);
    }

    /// <summary>
    /// Ends the current run, and causes other negative effects (to be added).
    /// </summary>
    public void DeclineBribe()
    {
        UIManager.Instance.DialogueDisable();
        UIManager.Instance.SetBribeOptions(false);
        GameManager.Instance._RunManager.PurgeBodies();
        GameManager.Instance.EndRun();
    }
}
