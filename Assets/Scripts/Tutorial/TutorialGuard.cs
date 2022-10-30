using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGuard : Guard
{
    // An array of map points the guard will walk through, loops back to start after completion
    public Transform[] Goals;

    private Vector2Int ResetPos = Vector2Int.zero;

    protected override void PatrolState()
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

        if (DistToPlayer.magnitude < 4.0f - StealthManager.Instance.GetHiddenBonus(transform) && !AlertCooldown && LineOfSight())
        {
            StopAllCoroutines();

            CoChase = Caught();
            StartCoroutine(CoChase);
            Debug.Log("Sight alerted");

            return;
        }

        Vector3 DistToGoal = CurrentGoal.position - transform.position;
        DistToGoal.y = 0;
        if (DistToGoal.magnitude < 0.5)
        {
            Agent.areaMask = 15;

            RouteCheckpoint = (RouteCheckpoint + 1) % Goals.Length;

            CurrentGoal = Goals[RouteCheckpoint];

            CoStop = ShortGoalStop();

            StartCoroutine(CoStop);
        }


        float MarkTransparancy = Mathf.Clamp(((Mathf.Floor(CurrentNoise) + 8) - StealthManager.Instance.GetHearingRange()) / 8, 0, 1);
        QMarkMaterial.color = new Color(1.0f, 1.0f, 1.0f, MarkTransparancy * MarkTransparancy);
    }

    protected override IEnumerator ShortGoalStop()
    {
        yield return new WaitForSeconds(0.75f);
        Agent.destination = CurrentGoal.position;
    }

    protected override IEnumerator Caught()
    {
        Agent.destination = transform.position;
        AlertCooldown = true;
        IsInvestigating = false;
        GState = GuardState.CAUGHT;
        AlertedMark.SetActive(true);
        GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapCommands>().FreezeScene();
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapCommands>().TeleportPlayer(ResetPos));
        AlertedMark.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapCommands>().UnFreezeScene();
        GState = GuardState.PATROL;
        Agent.speed = StealthManager.Instance.GetPatrolSpeed();
        Agent.destination = CurrentGoal.position;
        DetectionRange = BaseDetectionRange;
        StartCoroutine(CooldownAlert(1.0f));
    }

    /// <summary>
    /// Called when instantiating the guard prefab to populate its patrol route arrays.
    /// </summary>
    /// <param name="InGoals">A list of points in the guard's patrol.</param>
    /// <param name="InLookoutGoals">A list of which points in the patrol the guard should call its longstop for.</param>
    public void GenerateGuard(Transform[] InGoals, Vector2Int InLookoutGoals, Vector2Int ResPoint)
    {
        Goals = InGoals;
        LookoutGoals = InLookoutGoals;

        BaseDetectionRange = DetectionRange;
        BasePursuitRange = PursuitRange;
        BaseCatchRange = CatchRange;

        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = Player.GetComponent<Transform>();
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        MG = GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapGeneration>();

        CurrentGoal = Goals[1];
        Agent.destination = CurrentGoal.position;

        ResetPos = ResPoint;
    }
}
