using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    // An array of the points in the guard patrol route they will stop at to look for the player
    public Vector2Int LookoutGoals;

    // The previous goal on the guard's patrol
    private Vector2Int PreviousGoal;

    // the next goal in the guard's patrol
    private Vector2Int NextGoal;

    // The point on the map the guard will continuely take paths towards
    public Vector2Int LongTermGoal;

    // If the guard should move towards its long term goal
    private bool LongTermActive = false;

    // The guard's navmesh agent component
    protected UnityEngine.AI.NavMeshAgent Agent;

    // The transform of the tile the guard currently has in their patrol route
    protected Transform CurrentGoal;

    // Used to hold the point that the guard is currently investigating
    private Vector3 CurrentInvestigation = new Vector3();

    // The position of the tile in the guard's patrol route array that they are currently moving to
    protected int RouteCheckpoint = 1;

    // The distance the guard has to be from the player to be alerted (Does not catch the player yet)
    public float DetectionRange = 4.0f;

    // The amount of noise needed to alert the guard, does not directly indicate distance from noise
    private float HearingRange = 7.5f;

    // The base detection range without any modifiers
    protected float BaseDetectionRange;

    // The distance the guard has to be from the player to update their pathing while in alert state
    public float PursuitRange = 5.0f;

    // The base pursuit range without any modifiers
    protected float BasePursuitRange;

    // The distance the guard has to be from the player to catch them
    public float CatchRange = 1.0f;

    // The base catch range without any modifiers
    protected float BaseCatchRange;

    // Multiplier for how long it will take a guard to react to noise
    private float ReactionMultiplier = 5.0f;

    // Holds the guards current AI state
    public GuardState GState = GuardState.PATROL;

    // Used to check if guard is currently moving to investigate a noise
    protected bool IsInvestigating = false;

    // Used to check if guard is currently at their investigation point
    private bool IsOnLookout = false;

    // A cooldown boolean to prevent a guard from immediatly investigating a new noise after finishing another investigation
    public bool AlertCooldown = false;

    // Bonus to detection while at stop location
    public float DetectionRangeBonus = 1.2f;

    // Range at which the guard can see tampered objects and hidden players 
    public int CloseVision = 4;

    // Used to check if guard should be frozen for dialogue
    protected bool Paused = false;

    // Holds the guard's previous speed if they are paused
    private float StoreSpeed = 0;

    // IEnumerator objects to control the guard's actions
    protected IEnumerator CoStop;
    protected IEnumerator CoChase;
    private IEnumerator CoInvestigate;

    // Reference to the player object
    protected GameObject Player;
    protected Transform PlayerTransform;

    // Reference to the exclimation mark
    public GameObject AlertedMark;

    // Reference to the question mark
    public GameObject QuestionMark;
    protected Material QMarkMaterial;

    protected MapGeneration MG;
    protected MapCommands MC;

    [SerializeField]
    protected MeshRenderer MR;
    [SerializeField]
    protected Material RoyalTexture;

    // Start is called before the first frame update
    void Awake()
    {
        QMarkMaterial = QuestionMark.GetComponent<MeshRenderer>().material;

        MC = GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapCommands>();

        if (GameManager.Instance._RunManager.GetMapInfo().GetLevelTier() == GraveyardTier.HIGH)
        {
            MR.material = RoyalTexture;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!Paused)
        {
            switch (GState)
            {
                case GuardState.PATROL:
                    PatrolState();
                    break;
                case GuardState.ALERT:
                    AlertState();
                    break;
                case GuardState.CAUGHT:
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Guard will move along route from checkpoint to checkpoint\.
    /// ALERT: State changes to alert upon having the player enter their detection range.
    /// </summary>
    protected virtual void PatrolState()
    {
        Vector3 DistToPlayer = PlayerTransform.position - transform.position;
        DistToPlayer.y = 0;
        float CurrentNoise = MG.GetTileData(transform).GetNoise();
        if (CurrentNoise > StealthManager.Instance.GetHearingRange() / 2)
        {
            if (!LineOfSight())
            {
                CurrentNoise = StealthManager.Instance.GetHearingRange() / 2;
            }
        }
        if (DistToPlayer.magnitude < StealthManager.Instance.GetSightRange() - StealthManager.Instance.GetHiddenBonus(transform) && !AlertCooldown && LineOfSight())
        {
            StopAllCoroutines();

            CoChase = Alerted(PlayerTransform);
            StartCoroutine(CoChase);
            Debug.Log("Sight alerted");

            return;
        }
        else if (CurrentNoise >= StealthManager.Instance.GetHearingRange() && !AlertCooldown)
        {
            StopAllCoroutines();

            Vector2Int NoiseSource = MG.GetTileData(transform).GetNoiseSource();
            CoChase = Alerted(MG.GetTile(NoiseSource[0], NoiseSource[1]).transform);

            StartCoroutine(CoChase);
            Debug.Log("Noise alerted");

            return;
        }

        Vector3 DistToGoal = CurrentGoal.position - transform.position;
        DistToGoal.y = 0;
        if (DistToGoal.magnitude < 0.5)
        {
            //var CheckLookout = System.Array.Exists(LookoutGoals, x => x == RouteCheckpoint);
            Agent.areaMask = 15;

            //RouteCheckpoint = (RouteCheckpoint + 1) % Goals.Length;

            //CurrentGoal = Goals[RouteCheckpoint];
            /*
            if (CheckLookout)
            {
                CoStop = LongGoalStop();
            }
            else
            {
                CoStop = ShortGoalStop();
            }
            */

            if (NextGoal.x == LongTermGoal.x && NextGoal.y == LongTermGoal.y)
            {
                LongTermActive = false;
            }

            CoStop = GoalDecision();

            StartCoroutine(CoStop);
        }


        float MarkTransparancy = Mathf.Clamp(((Mathf.Floor(CurrentNoise) + 8) - StealthManager.Instance.GetHearingRange()) / 8, 0, 1);
        QMarkMaterial.color = new Color(1.0f, 1.0f, 1.0f, MarkTransparancy * MarkTransparancy);
    }

    /// <summary>
    /// The guard will move towards the player's last known position and watch for the player along the way.
    /// PATROL: The guard will return to patrol if they finish their investigation without finding the player.
    /// CAUGHT: The guard will enter the caught state if the player enters their catch range.
    /// </summary>
    private void AlertState()
    {
        Vector3 DistToPlayer = PlayerTransform.position - transform.position;
        DistToPlayer.y = 0;
        if (DistToPlayer.magnitude < CatchRange)
        {
            AlertedMark.SetActive(false);

            if (CoChase != null)
            {
                StopCoroutine(CoChase);
            }
            CoChase = Caught();
            StartCoroutine(CoChase);

            return;
        }
        else if (DistToPlayer.magnitude < PursuitRange)
        {
            UpdatePursuit();
        }

        Vector3 DistToInvestigation = CurrentInvestigation - transform.position;
        if (DistToInvestigation.magnitude < 0.5f && IsInvestigating)
        {
            StopAllCoroutines();

            CoInvestigate = Investigate();
            StartCoroutine(CoInvestigate);
        }
    }

    private IEnumerator GoalDecision()
    {
        IEnumerator Decision = DecisionError();

        List<int> Weights = new List<int>();
        int TotalWeight = 0;
        List<IEnumerator> Options = new List<IEnumerator>();

        if (true)
        {
            Options.Add(ShortGoalStop());
            Weights.Add(50);
            TotalWeight += 50;
        }

        if (StealthManager.Instance.GetAlertLevel() <= 150)
        {
            Options.Add(LongGoalStop());
            Weights.Add(50);
            TotalWeight += 50;
        }

        if (LongTermActive)
        {
            return SprintToNoise();
        }

        int RandomInt = Random.Range(0, TotalWeight);

        for (int i = 0; i < Weights.Count; i++)
        {
            RandomInt -= Weights[i];

            if (RandomInt < 0)
            {
                Decision = Options[i];
                break;
            }
        }

        return Decision;
    }

    private IEnumerator DecisionError()
    {
        Debug.LogError("Error in GoalDecision function of guard script!");

        yield return new WaitForSeconds(0.0f);
    }

    /// <summary>
    /// Changes guard's navmesh agent destination to next goal.
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator ShortGoalStop()
    {
        ChooseNextGoal();
        CurrentGoal = MG.GetTile(NextGoal[0], NextGoal[1]).transform;
        yield return new WaitForSeconds(0.75f);
        Agent.speed = StealthManager.Instance.GetPatrolSpeed();
        Agent.destination = CurrentGoal.position;
    }

    /// <summary>
    /// Used to simulate the guard "looking around" while also updating destination to next goal.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LongGoalStop()
    {
        ChooseNextGoal();
        CurrentGoal = MG.GetTile(NextGoal[0], NextGoal[1]).transform;
        yield return new WaitForSeconds(1.0f);
        DetectionRange = BaseDetectionRange * DetectionRangeBonus;
        yield return new WaitForSeconds(2.0f);
        DetectionRange = BaseDetectionRange;
        Agent.speed = StealthManager.Instance.GetPatrolSpeed();
        Agent.destination = CurrentGoal.position;
    }

    private IEnumerator SprintToNoise()
    {
        Vector2Int NoiseGoal = StealthManager.Instance.GetAlertPulseOrigin();
        NoiseGoal = MC.GetNearestCorner(NoiseGoal);

        PreviousGoal = NextGoal;
        NextGoal = MC.GetShortestPath(PreviousGoal, NoiseGoal);

        if (NextGoal.x != -1)
        {
            CurrentGoal = MG.GetTile(NextGoal[0], NextGoal[1]).transform;

            yield return new WaitForSeconds(0.1f);

            Agent.speed = StealthManager.Instance.GetSprintingSpeed();

            Agent.destination = CurrentGoal.position;
        }
        else
        {
            StartCoroutine(ShortGoalStop());
        }
    }

    private void ChooseNextGoal()
    {
        List<Vector2Int> Options = MC.PathingOptions(NextGoal, PreviousGoal);

        int RandChoice = Random.Range(0, Options.Count);

        PreviousGoal = NextGoal;

        NextGoal = Options[RandChoice];
    }

    /// <summary>
    /// Sets the guard to the alerted state. Setting areaMask to -1 allows the guard to walk off the path.
    /// </summary>
    /// <param name="Goal">
    /// Position the guard will begin moving towards.
    /// </param>
    /// <returns></returns>
    protected IEnumerator Alerted(Transform Goal)
    {
        IsInvestigating = false;
        LongTermActive = false;
        GState = GuardState.ALERT;
        Agent.speed = StealthManager.Instance.GetChaseSpeed();
        Agent.destination = transform.position;
        Agent.areaMask = -1;
        CurrentInvestigation = new Vector3(Goal.position.x, transform.position.y, Goal.position.z);
        AlertedMark.SetActive(true);
        QuestionMark.SetActive(false);
        StealthManager.Instance.AlertIncrease(50.0f);
        Debug.Log("Who goes there?");
        yield return new WaitForSeconds(0.3f * (ReactionMultiplier));
        IsInvestigating = true;
        CatchRange = BaseCatchRange * 2.0f;
        DetectionRange = 0;
        AlertedMark.SetActive(false);
        Agent.destination = CurrentInvestigation;
    }

    /// <summary>
    /// Calls the caught functionality if the guard manages to find the player.
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Caught()
    {
        GameManager.Instance._RunManager.DisableMinigame();
        Agent.destination = transform.position;
        AlertCooldown = true;
        IsInvestigating = false;
        GState = GuardState.CAUGHT;
        //Debug.Log("Come on now, there's no need to be violent.");
        //GameManager.Instance.EndRun();
        GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapCommands>().FreezeScene();
        yield return new WaitForSeconds(1.0f);
        transform.GetComponent<GuardCaughtInteraction>().StartBribe();
    }

    public void BribedReturnToRoute()
    {
        GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapCommands>().UnFreezeScene();
        GState = GuardState.PATROL;
        Agent.speed = StealthManager.Instance.GetPatrolSpeed();
        Agent.destination = CurrentGoal.position;
        DetectionRange = BaseDetectionRange;
        StartCoroutine(CooldownAlert(5.0f));
    }

    protected IEnumerator CooldownAlert(float Duration)
    {
        yield return new WaitForSeconds(Duration);
        AlertCooldown = false;
        QuestionMark.SetActive(true);
    }

    /// <summary>
    /// Has the guard stop at an area of interest for a short time before returning to their route.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Investigate()
    {
        Debug.Log("Investigate Start");
        IsInvestigating = false;
        IsOnLookout = true;
        yield return new WaitForSeconds(0.5f);
        DetectionRange = BaseDetectionRange * 1.5f;
        yield return new WaitForSeconds(2.5f);
        GState = GuardState.PATROL;
        Agent.speed = StealthManager.Instance.GetPatrolSpeed();
        QuestionMark.SetActive(true);
        DetectionRange = BaseDetectionRange;
        Agent.destination = CurrentGoal.position;
        IsOnLookout = false;
        Debug.Log("Investigate End");
    }

    /// <summary>
    /// Updates the position the guard is moving towards while alerted
    /// </summary>
    private void UpdatePursuit()
    {
        if (CoInvestigate != null)
        {
            StopCoroutine(CoInvestigate);
        }

        if (IsInvestigating || IsOnLookout)
        {
            CurrentInvestigation = PlayerTransform.position;
            Agent.destination = CurrentInvestigation;
            IsInvestigating = true;
            IsOnLookout = false;
        }
    }

    protected bool LineOfSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (PlayerTransform.position - transform.position), out hit, 100, 1 << LayerMask.NameToLayer("Ray Contact")))
        {
            return hit.transform == PlayerTransform;
        }
        return false;
    }

    public void PauseGuard()
    {
        StoreSpeed = Agent.speed;
        Agent.speed = 0;
        QMarkMaterial.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Paused = true;
    }

    public void UnPauseGuard()
    {
        Agent.speed = StoreSpeed;
        Paused = false;
    }

    public void TogglePauseGuard()
    {
        if (Paused)
        {
            UnPauseGuard();
        }
        else
        {
            PauseGuard();
        }
    }

    public void BeginLongTermActive(Vector2Int Goal)
    {
        LongTermActive = true;
        LongTermGoal = MC.GetNearestCorner(Goal);
    }

    /// <summary>
    /// Called when instantiating the guard prefab to populate its patrol route arrays.
    /// </summary>
    /// <param name="InGoals">A list of points in the guard's patrol.</param>
    /// <param name="InLookoutGoals">A list of which points in the patrol the guard should call its longstop for.</param>
    public virtual void GenerateGuard(Transform[] InGoals, Vector2Int InLookoutGoals)
    {
        LookoutGoals = InLookoutGoals;

        BaseDetectionRange = DetectionRange;
        BasePursuitRange = PursuitRange;
        BaseCatchRange = CatchRange;

        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = Player.GetComponent<Transform>();
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        MG = GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapGeneration>();

        PreviousGoal = MG.GetTilePos(InGoals[0]);
        NextGoal = MG.GetTilePos(InGoals[0]);

        ChooseNextGoal();

        CurrentGoal = MG.GetTile(NextGoal[0], NextGoal[1]).transform;
        Agent.destination = CurrentGoal.position;
    }
}
