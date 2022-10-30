using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialProgression : MonoBehaviour
{
    private TutorialMapGen MG;
    private MapCommands MC;

    [Header("First Grave Section")]
    // Dialogues used in the scene
    public Script MentorInitialDialogue; // The first time you talk to the mentor
    public Script MentorInitialRepeatDialogue; // If you retalk to the mentor before completing the first task
    public Script MentorInitialCompleteDialogue; // When you talk to the mentor after finishing the task

    // Actor movement positions
    public Vector2Int MentorFirstInteractionMove; // Move after you talk to him once
    public Vector2Int MentorFirstGravesMove; // Moves to area before first guard

    [Header("First Guard Section")]
    public Script MentorGuardDialogue; // The mentor explaining guards to the player
    public Script MentorGuardCompleteDialogue; // After the player has made it past the guard

    public Vector2Int MentorGuardTeleport; // Teleports past first guard
    public Vector2Int MentorGuardMove; // Moves to next grave section

    [Header("Second Grave Section")]
    public Script MentorTrapInitialDialogue; // Explains that graves may have traps, need to use mouse
    public Script MentorTrapRepeatDialogue; // Retalk at second grave set
    public Script MentorTrapCompleteDialogue; // After finishing second grave set

    public Vector2Int MentorTrapTalkMove; // After talking once, moves forward
    public Vector2Int MentorTrapMove; // Moves to area before next guard

    [Header("Second Guard Section")]
    public Script MentorBushInitialDialogue; // Explains hiding in bushes
    public Script MentorBushCompleteDialogue; // After player makes it past second guard

    public Vector2Int MentorBushTeleport; // Teleports past second guard
    public Vector2Int MentorBushMove; // Moves to final area

    [Header("Final Mausoleum Section")]
    public Script MentorMausoleumInitialDialogue; // Tells player to break into mausoleum
    public Script MentorMausoleumRepeatDialogue; // Repeated interaction
    public Script MentorMausoleumFailureDialogue; // After player fails minigame

    public Vector2Int MentorRunAway; // Mentor runs towards exit

    // Progression flags
    private int GraveyardSection = 1;
    private bool InDialogue = false;
    private bool MentorInitialTalkedToOnce = false; // If the player has talked to the mentor at the start yet
    private bool MentorInitialComplete = false; // If the player has finished the first section
    private bool MentorGuardTalkedToOnce = false; // If the mentor has explained guards yet
    private bool MentorTrapTalkedToOnce = false; // If the mentor has been talked to at the trap graves
    private bool MentorTrapComplete = false; // If the player has finished the trapped graves
    private bool MentorBushTalkedToOnce = false; // If the mentor has explained bush stealth yet
    private bool MentorMausoleumTalkedToOnce = false; // If the mentor was talked to at mausoleum

    // Variables to hold the location of important game objects the player can interact with
    private GameObject Mentor;
    private GameObject Mausoleum;

    // Holds the player object
    private GameObject Player;

    // Holds invisible walls in the level
    private List<GameObject> InvisibleWalls;

    // Must be at most 0.0f in order to speak to mentor
    private float TalkCooldown = 0.0f;

    private void Start()
    {
        MG = GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<TutorialMapGen>();
        MC = GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapCommands>();
        Player = GameObject.FindGameObjectWithTag("Player");
        MG.SetMentorTalk(true);
    }

    private void Update()
    {
        // Checking for player to interact
        if (Input.GetButtonDown("Interact") && TalkCooldown <= 0)
        {
            Vector3 DistToMentor = Player.transform.position - Mentor.transform.position;
            DistToMentor.y = 0;

            // If the player is close enough to the mentor, enters a different dialogue function depending on the progression flag
            if (DistToMentor.magnitude <= 1.8f || InDialogue)
            {
                if (GraveyardSection == 1)
                {
                    MentorInitialInteraction();
                }
                else if (GraveyardSection == 2)
                {
                    MentorGuardInteract();
                }
                else if (GraveyardSection == 3)
                {
                    MentorTrapsInteract();
                }
                else if (GraveyardSection == 4)
                {
                    MentorBushInteraction();
                }
                else if (GraveyardSection == 5)
                {
                    MentorMausoleumInteraction();
                }
                else if (GraveyardSection == 7)
                {
                    MentorFailureInteraction();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //StartCoroutine(GameManager.Instance._SceneManager.ChangeSceneFade(SceneName.CourtTutorial));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //StartCoroutine(GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapCommands>().TeleportPlayer(new Vector2Int(75, 13)));
            //MentorMausoleumTalkedToOnce = true;
        }

        // Prevents the player from talking again immediatly after finishing dialogue
        if (TalkCooldown > 0)
        {
            TalkCooldown -= Time.deltaTime;
        }

        if (GraveyardSection == 1 && GameManager.Instance.GetGravesCompleted() >= 4)
        {
            MG.SetMentorTalk(true);
        }
        else if (GraveyardSection == 3 && GameManager.Instance.GetGravesCompleted() >= 8)
        {
            MG.SetMentorTalk(true);
        }
    }

    /// <summary>
    /// First interaction function for first set of graves.
    /// </summary>
    public void MentorInitialInteraction()
    {
        // If already talking, forwards dialogue and handles events after dialogue finishes
        if (InDialogue)
        {
            if (!GameManager.Instance._DialogueManager.NextLine()) // Moves dialogue forward, then passes if dialogue has finished
            {
                if (GameManager.Instance.GetGravesCompleted() >= 4 && MentorInitialTalkedToOnce)
                {
                    InDialogue = false;
                    MG.SetMentorTalk(false);
                    UIManager.Instance.DialogueDisable();
                    MC.UnFreezeScene();
                    MG.MoveMentor(MentorFirstGravesMove);
                    InvisibleWalls[2].SetActive(false);
                    GameManager.Instance._RunManager.SetTime(101);
                    GraveyardSection = 2;
                    MentorInitialComplete = true;
                    StartCoroutine(DelayedTalkEnable(5.5f));
                }
                else
                {
                    InDialogue = false;
                    UIManager.Instance.DialogueDisable();
                    MC.UnFreezeScene();

                    if (!MentorInitialTalkedToOnce)
                    {
                        MentorInitialTalkedToOnce = true;
                        MG.SetMentorTalk(false);
                        MG.GetTileData(6, 8).transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetDiggable(true);
                        MG.GetTileData(6, 4).transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetDiggable(true);
                        MG.GetTileData(10, 8).transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetDiggable(true);
                        MG.GetTileData(10, 4).transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetDiggable(true);
                        MG.MoveMentor(MentorFirstInteractionMove);
                    }
                }
                TalkCooldown = 1.0f;
            }
        }
        else if (!MentorInitialTalkedToOnce)
        {
            GameManager.Instance._DialogueManager.SetScript(MentorInitialDialogue);
            GameManager.Instance._DialogueManager.ResetDialogue();

            MC.FreezeScene();

            UIManager.Instance.DialogueEnable();

            InDialogue = true;
        }
        else if (GameManager.Instance.GetGravesCompleted() >= 4)
        {
            GameManager.Instance._DialogueManager.SetScript(MentorInitialCompleteDialogue);
            GameManager.Instance._DialogueManager.ResetDialogue();

            MC.FreezeScene();

            UIManager.Instance.DialogueEnable();

            InDialogue = true;
        }
        else
        {
            GameManager.Instance._DialogueManager.SetScript(MentorInitialRepeatDialogue);
            GameManager.Instance._DialogueManager.ResetDialogue();

            MC.FreezeScene();

            UIManager.Instance.DialogueEnable();

            InDialogue = true;
        }
    }

    /// <summary>
    /// Second interaction before the first guard obstacle.
    /// </summary>
    public void MentorGuardInteract()
    {
        if (InDialogue)
        {
            if (!GameManager.Instance._DialogueManager.NextLine())
            {
                if (!MentorGuardTalkedToOnce)
                {
                    UIManager.Instance.DialogueDisable();
                    StartCoroutine(TeleportMentorGuard(MentorGuardTeleport));
                }
                else
                {
                    InDialogue = false;
                    MG.SetMentorTalk(false);
                    UIManager.Instance.DialogueDisable();
                    MC.UnFreezeScene();
                    InvisibleWalls[4].SetActive(false);
                    GraveyardSection = 3;
                    MG.MoveMentor(MentorGuardMove);
                    StartCoroutine(DelayedTalkEnable(4.5f));
                }
                TalkCooldown = 1.0f;
            }
        }
        else if (!MentorGuardTalkedToOnce)
        {
            GameManager.Instance._DialogueManager.SetScript(MentorGuardDialogue);
            GameManager.Instance._DialogueManager.ResetDialogue();

            MC.FreezeScene();

            UIManager.Instance.DialogueEnable();

            InDialogue = true;
        }
        else
        {
            GameManager.Instance._DialogueManager.SetScript(MentorGuardCompleteDialogue);
            GameManager.Instance._DialogueManager.ResetDialogue();

            MC.FreezeScene();

            UIManager.Instance.DialogueEnable();

            InDialogue = true;
        }
    }

    /// <summary>
    /// Third interaction for graves with traps attached.
    /// </summary>
    public void MentorTrapsInteract()
    {
        if (InDialogue)
        {
            if (!GameManager.Instance._DialogueManager.NextLine())
            {
                if (GameManager.Instance.GetGravesCompleted() >= 8 && MentorTrapTalkedToOnce)
                {
                    InDialogue = false;
                    MG.SetMentorTalk(false);
                    UIManager.Instance.DialogueDisable();
                    MC.UnFreezeScene();
                    MG.MoveMentor(MentorTrapMove);
                    InvisibleWalls[5].SetActive(false);
                    GraveyardSection = 4;
                    MentorTrapComplete = true;
                    StartCoroutine(DelayedTalkEnable(5.5f));
                }
                else
                {
                    InDialogue = false;
                    UIManager.Instance.DialogueDisable();
                    MC.UnFreezeScene();

                    if (!MentorTrapTalkedToOnce)
                    {
                        MentorTrapTalkedToOnce = true;
                        MG.SetMentorTalk(false);
                        GameManager.Instance._RunManager.SetTime(201);
                        MG.GetTileData(46, 8).transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetDiggable(true);
                        MG.GetTileData(46, 4).transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetDiggable(true);
                        MG.GetTileData(50, 8).transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetDiggable(true);
                        MG.GetTileData(50, 4).transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetDiggable(true);
                        MG.MoveMentor(MentorTrapTalkMove);
                    }
                }
                TalkCooldown = 1.0f;
            }
        }
        else if (!MentorTrapTalkedToOnce)
        {
            GameManager.Instance._DialogueManager.SetScript(MentorTrapInitialDialogue);
            GameManager.Instance._DialogueManager.ResetDialogue();

            MC.FreezeScene();

            UIManager.Instance.DialogueEnable();

            InDialogue = true;
        }
        else if (GameManager.Instance.GetGravesCompleted() >= 8)
        {
            GameManager.Instance._DialogueManager.SetScript(MentorTrapCompleteDialogue);
            GameManager.Instance._DialogueManager.ResetDialogue();

            MC.FreezeScene();

            UIManager.Instance.DialogueEnable();

            InDialogue = true;
        }
        else
        {
            GameManager.Instance._DialogueManager.SetScript(MentorTrapRepeatDialogue);
            GameManager.Instance._DialogueManager.ResetDialogue();

            MC.FreezeScene();

            UIManager.Instance.DialogueEnable();

            InDialogue = true;
        }
    }

    /// <summary>
    /// Fourth interaction for guard with stealth bushes.
    /// </summary>
    public void MentorBushInteraction()
    {
        if (InDialogue)
        {
            if (!GameManager.Instance._DialogueManager.NextLine())
            {
                if (!MentorBushTalkedToOnce)
                {
                    UIManager.Instance.DialogueDisable();
                    StartCoroutine(TeleportMentorBush(MentorBushTeleport));
                }
                else
                {
                    InDialogue = false;
                    MG.SetMentorTalk(false);
                    UIManager.Instance.DialogueDisable();
                    MC.UnFreezeScene();
                    GraveyardSection = 5;
                    MG.MoveMentor(MentorBushMove);
                    StartCoroutine(DelayedTalkEnable(4.0f));
                }
                TalkCooldown = 1.0f;
            }
        }
        else if (!MentorBushTalkedToOnce)
        {
            GameManager.Instance._DialogueManager.SetScript(MentorBushInitialDialogue);
            GameManager.Instance._DialogueManager.ResetDialogue();

            MC.FreezeScene();

            UIManager.Instance.DialogueEnable();

            InDialogue = true;
        }
        else
        {
            GameManager.Instance._DialogueManager.SetScript(MentorBushCompleteDialogue);
            GameManager.Instance._DialogueManager.ResetDialogue();

            MC.FreezeScene();

            UIManager.Instance.DialogueEnable();

            InDialogue = true;
        }
    }

    /// <summary>
    /// Fifth interaction before mausoleum.
    /// </summary>
    public void MentorMausoleumInteraction()
    {
        if (InDialogue)
        {
            if (!GameManager.Instance._DialogueManager.NextLine())
            {
                InDialogue = false;
                UIManager.Instance.DialogueDisable();
                MC.UnFreezeScene();
                TalkCooldown = 1.0f;

                if (!MentorMausoleumTalkedToOnce)
                {
                    MentorMausoleumTalkedToOnce = true;
                    MG.SetMentorTalk(false);
                }
            }
        }
        else if (!MentorMausoleumTalkedToOnce)
        {
            GameManager.Instance._DialogueManager.SetScript(MentorMausoleumInitialDialogue);
            GameManager.Instance._DialogueManager.ResetDialogue();

            MC.FreezeScene();

            UIManager.Instance.DialogueEnable();

            InDialogue = true;
        }
        else
        {
            GameManager.Instance._DialogueManager.SetScript(MentorMausoleumRepeatDialogue);
            GameManager.Instance._DialogueManager.ResetDialogue();

            MC.FreezeScene();

            UIManager.Instance.DialogueEnable();

            InDialogue = true;
        }
    }

    /// <summary>
    /// Final interaction after failing minigame.
    /// </summary>
    public void MentorFailureInteraction()
    {
        if (InDialogue)
        {
            if (!GameManager.Instance._DialogueManager.NextLine())
            {
                InDialogue = false;
                UIManager.Instance.DialogueDisable();
                GraveyardSection = 8;
                StartCoroutine(CurtainCall());
            }
        }
    }

    /// <summary>
    /// Called by the mausoleum script when the minigame is failed
    /// </summary>
    public IEnumerator MausoleumFailureCall()
    {
        // Play sound effect here

        yield return new WaitForSeconds(1.0f);

        GraveyardSection = 7;

        GameManager.Instance._DialogueManager.SetScript(MentorMausoleumFailureDialogue);
        GameManager.Instance._DialogueManager.ResetDialogue();

        MC.FreezeScene();

        UIManager.Instance.DialogueEnable();

        InDialogue = true;
    }

    /// <summary>
    /// Plays out the final section of this tutorial section, then changes scenes to courtyard
    /// </summary>
    /// <returns></returns>
    private IEnumerator CurtainCall()
    {
        MG.MoveMentor(MentorRunAway);
        StealthManager.Instance.ResetAlert();
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(GameManager.Instance._SceneManager.ChangeSceneFade(SceneName.CourtTutorial));
        GameManager.Instance._RunManager.PurgeBodies();
    }

    /// <summary>
    /// Teleports mentor for first guard section.
    /// </summary>
    /// <param name="Pos">
    /// Teleportation point.
    /// </param>
    /// <returns></returns>
    private IEnumerator TeleportMentorGuard(Vector2Int Pos)
    {
        UIManager.Instance.EnableBlackScreen();
        yield return new WaitForSeconds(1.0f);
        MG.TeleportMentor(Pos);
        UIManager.Instance.DisableBlackScreen();
        yield return new WaitForSeconds(0.5f);
        InDialogue = false;
        InvisibleWalls[3].SetActive(false);
        MC.UnFreezeScene();
        MentorGuardTalkedToOnce = true;
    }

    /// <summary>
    /// Teleports mentor for second guard section.
    /// </summary>
    /// <param name="Pos">
    /// Teleportation point.
    /// </param>
    /// <returns></returns>
    private IEnumerator TeleportMentorBush(Vector2Int Pos)
    {
        UIManager.Instance.EnableBlackScreen();
        yield return new WaitForSeconds(1.0f);
        MG.TeleportMentor(Pos);
        UIManager.Instance.DisableBlackScreen();
        yield return new WaitForSeconds(0.5f);
        InDialogue = false;
        InvisibleWalls[6].SetActive(false);
        MC.UnFreezeScene();
        MentorBushTalkedToOnce = true;
    }

    /// <summary>
    /// Sets the mentor's talk bubble to active after set delay.
    /// </summary>
    /// <param name="Duration">
    /// Delay time in seconds.
    /// </param>
    /// <returns></returns>
    private IEnumerator DelayedTalkEnable(float Duration)
    {
        yield return new WaitForSeconds(Duration);
        MG.SetMentorTalk(true);
    }

    // If the player can interact with the mausoleum yet
    public bool GetMausoleumInteractable()
    {
        return MentorMausoleumTalkedToOnce;
    }

    public void SetMentor(GameObject Val)
    {
        Mentor = Val;
    }

    public void SetMausoleum(GameObject Val)
    {
        Mausoleum = Val;
    }

    public void SetInvisibleWalls(List<GameObject> Val)
    {
        InvisibleWalls = Val;
    }
}
