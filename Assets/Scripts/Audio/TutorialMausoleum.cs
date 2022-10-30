using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMausoleum : Mausoleum
{
    public GameObject Minigame;
    private GameObject MinigameInstance;

    public Canvas Canvas;

    private TorpedoMinigame MinigameScript;

    private bool InMinigame = false; // Track if the minigame is running

    private bool FinishedMinigame = false; // If the player has finished playing the minigame

    // Start is called before the first frame update
    void Start()
    {
        MinigameInstance = Instantiate(Minigame, Canvas.transform);
        MinigameScript = MinigameInstance.GetComponent<TorpedoMinigame>();
        MinigameScript.SetCoffinOpeningDelay(4);
        MinigameScript.SetGoalClickedButtons(25);
        MinigameScript.SetTimerStartingNumber(200);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && Interactable && !InMinigame && !FinishedMinigame && GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<TutorialProgression>().GetMausoleumInteractable())
        {
            GameManager.Instance.ToggleMinigame(MinigameInstance, true);
            InMinigame = true;
        }

        if (InMinigame && (MinigameScript.CheckWin() || MinigameScript.CheckLose()))
        {
            GameManager.Instance.ToggleMinigame(MinigameInstance, false);
            InMinigame = false;
            FinishedMinigame = true;
            StartCoroutine(GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<TutorialProgression>().MausoleumFailureCall());
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        
        if (other.tag == "Player")
        {
            GameManager.Instance.ToggleMinigame(MinigameInstance, false);
            InMinigame = false;
        }
    }
}
