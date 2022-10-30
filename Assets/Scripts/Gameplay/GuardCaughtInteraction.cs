using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCaughtInteraction : MonoBehaviour
{
    // Options for different scripts the guard could choose from for bribing
    public Script GuardDialogueOne;

    // Checks if guard is currently talking to player
    private bool BribeStarted = false;

    // Amount the guard will ask for
    private int BribeAmount = 0;

    private void Update()
    {
        if (BribeStarted && Input.GetButtonDown("Interact"))
        {
            if (!GameManager.Instance._DialogueManager.NextLineVars())
            {
                UIManager.Instance.GetBribeOptions().GetComponent<BribingOptions>().StartBribe(gameObject, BribeAmount);
                BribeStarted = false;
            }
        }
    }

    /// <summary>
    /// Enable's the guard's bribe dialogue and begins bribing sequence.
    /// </summary>
    public void StartBribe()
    {
        Debug.Log("start teh bribe");
        BribeStarted = true;
        BribeAmount = GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapCommands>().GetRandomBribe();
        GameManager.Instance._DialogueManager.AddVar(new string[] { "{cost}", BribeAmount.ToString() });

        int RandVar = 1; // Randomize if multiple dialogues are needed

        if (RandVar == 1)
        {
            GameManager.Instance._DialogueManager.SetScriptVars(GuardDialogueOne);
            GameManager.Instance._DialogueManager.ResetDialogue();
            UIManager.Instance.DialogueEnable();
        }
    }
}
