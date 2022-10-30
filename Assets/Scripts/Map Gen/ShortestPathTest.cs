using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestPathTest : MonoBehaviour
{
    public Vector2Int StartPos;

    public Vector2Int EndPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            MapCommands MC = GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapCommands>();

            StartPos = MC.GetShortestPath(StartPos, EndPos);

            Debug.Log(StartPos);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            StealthManager.Instance.AddAlertPulse(999.9f, EndPos);
        }
    }
}
