using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    // The time until the scene is changed
    [Range(0, 120)] [SerializeField] private int time;
    void Start()
    {
        //Start timer for switching scenes
        StartCoroutine(Timer());
    }
    
    IEnumerator Timer()
    {
        GameManager.Instance._SceneManager.AsyncSceneLoad(GameManager.Instance._SceneManager.GetNextScene());
        yield return new WaitForSeconds(time);
        GameManager.Instance._SceneManager.SwitchAsyncScene();
    }
}
