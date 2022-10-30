using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMentor : MonoBehaviour
{
    // The mentor's navmesh agent
    private UnityEngine.AI.NavMeshAgent Agent;

    // Talk marker
    public GameObject TalkSign;

    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void MoveToPosition(Transform Destination)
    {
        Agent.destination = Destination.position;
    }

    public void SetTalkSign(bool _Val)
    {
        TalkSign.SetActive(_Val);
    }
}
