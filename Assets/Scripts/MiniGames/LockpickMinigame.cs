using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LockpickMinigame : MiniGame //IEndDragHandler, IBeginDragHandler,
{
    public FMOD.Studio.EventInstance Lockpick;
    public FMODUnity.EventReference fmodEvent10;
    public Image[] LockPick;
    public List<GameObject> LockpickPuzzleList = new List<GameObject>();
    public GameObject[] Tumbler;
    public GameObject[] UnSolvedLock;   //put the unlock pics here left first then right
    public GameObject[] SolvedLock;     //put the lock pics here left first then right
    int RandomLockpickPuzzle;
    public bool Finished = false;
    int NextSolve = 0;
    int LocksSolved = 0;
    void OnEnable()
    {
        RandomLockpickPuzzle = Random.Range(0, LockpickPuzzleList.Count);
        LockpickPuzzleList[RandomLockpickPuzzle].gameObject.SetActive(true);
        Lockpick = FMODUnity.RuntimeManager.CreateInstance(fmodEvent10);
    }

    public void Solve()
    {
        if(LocksSolved < 2)
        {
            Lockpick.start();
            UnSolvedLock[(RandomLockpickPuzzle * 2) + NextSolve].SetActive(false);
            SolvedLock[(RandomLockpickPuzzle * 2) + NextSolve].SetActive(true);
            LocksSolved++;
            NextSolve++;
        }
        if(LocksSolved == 2)
        {
            Debug.Log("i finished");
            Finished = true;
        }
    }
    public override bool CheckWin()
    {
        return Finished;
    }
}
