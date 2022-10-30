using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TutorialCourtyardProgression : MonoBehaviour
{
    public Script MentorInitialScript; // Mentor disowns player
    public Script DelivererScriptOne; // Up to giving the player the medallion
    public Script DelivererScriptTwo; // Remainder of dialogue

    public Transform MentorLeavePosition; // Mentor leaves through bottom of screen
    public Transform DelivererMovePosition;

    public NavMeshSurface NMSurface;

    public GameObject Mentor;
    public GameObject Deliverer;

    private GameObject Player;

    private bool InDialogue = false;

    private int ProgressionCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.DisableBlackScreen();

        NMSurface.BuildNavMesh();

        Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<PlayerCourtyard>().SetPaused(true);

        Mentor.GetComponent<TutorialMentor>().SetTalkSign(false);

        StartCoroutine(SceneSetup());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (InDialogue)
            {
                if (ProgressionCount == 0)
                {
                    if (!GameManager.Instance._DialogueManager.NextLine())
                    {
                        InDialogue = false;
                        UIManager.Instance.DialogueDisable();

                        Mentor.GetComponent<TutorialMentor>().MoveToPosition(MentorLeavePosition);

                        ProgressionCount = 1;
                        StartCoroutine(DelayAction(3.0f, DelivererApprouch()));
                    }
                }
                else if (ProgressionCount == 1)
                {
                    if (!GameManager.Instance._DialogueManager.NextLine())
                    {
                        InDialogue = false;
                        UIManager.Instance.DialogueDisable();

                        UIManager.Instance.MedallionOn();
                        GameManager.Instance.UpdateMoney(20);

                        ProgressionCount = 2;
                        StartCoroutine(DelayAction(2.0f, MedallionWait()));
                    }
                }
                else if (ProgressionCount == 2)
                {
                    if (!GameManager.Instance._DialogueManager.NextLine())
                    {
                        InDialogue = false;
                        UIManager.Instance.DialogueDisable();
                        UIManager.Instance.EnableBlackScreen();

                        StartCoroutine(DelayAction(1.0f, SceneEnding()));
                    }
                }
            }
        }
    }

    private IEnumerator DelayAction(float WaitTime, IEnumerator RunFunction)
    {
        yield return new WaitForSeconds(WaitTime);

        StartCoroutine(RunFunction);
    }

    private IEnumerator SceneSetup()
    {
        yield return new WaitForSeconds(3.0f);

        GameManager.Instance._DialogueManager.SetScript(MentorInitialScript);
        GameManager.Instance._DialogueManager.ResetDialogue();

        UIManager.Instance.DialogueEnable();
        InDialogue = true;
    }

    private IEnumerator DelivererApprouch()
    {
        Deliverer.GetComponent<TutorialMentor>().MoveToPosition(DelivererMovePosition);

        yield return new WaitForSeconds(2.5f);

        GameManager.Instance._DialogueManager.SetScript(DelivererScriptOne);
        GameManager.Instance._DialogueManager.ResetDialogue();

        UIManager.Instance.DialogueEnable();
        InDialogue = true;
    }

    private IEnumerator MedallionWait()
    {
        yield return new WaitForSeconds(0.0f);

        GameManager.Instance._DialogueManager.SetScript(DelivererScriptTwo);
        GameManager.Instance._DialogueManager.ResetDialogue();

        UIManager.Instance.DialogueEnable();
        InDialogue = true;
    }

    private IEnumerator SceneEnding()
    {
        yield return new WaitForSeconds(0.0f);
        //GameManager.Instance.ChangeScene(SceneName.CourYard);
        GameManager.Instance._SceneManager.AvoidLoadSceneChangeScene((int)SceneName.CourYard);
    }
}
