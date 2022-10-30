/*
 * Author: Zachary Boehm
 * Version: 10.26.2021
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the changing of scenes and playing the loading screne
/// </summary>
public class SceneController : MonoBehaviour
{
    int Current = 0;
    int Max = 100;
    [SerializeField] private GameObject panel;
    private SceneName NextScene = SceneName.GraveYard;
    private AsyncOperation NewScene = new AsyncOperation();

    Scene translater = new Scene();
    // 
    // 
    /// <summary>
    /// Takes a Scene by name then loads it according the int value of the Scene num
    /// </summary>
    /// <param name="_Scene">The scene to be loaded and switched too.</param>
    public void ChangeScene(SceneName _Scene)
    {
        if (_Scene == SceneName.CourYard)
        {
            Debug.Log("Before Scene " + _Scene);
            NextScene = _Scene;
            Debug.Log("After Change " + _Scene);
            Debug.Log(GameManager.Instance._SceneManager.GetNextScene());
            SceneManager.LoadScene(translater.EnumToScene(SceneName.LoadingSceneCourtyard));
        }
        else if (_Scene == SceneName.GraveYard)
        {
            NextScene = _Scene;
            SceneManager.LoadScene(translater.EnumToScene(SceneName.LoadingScene));
        }
        else
        {
            SceneManager.LoadScene(translater.EnumToScene(_Scene));
        }
    }

    /// <summary>
    /// Avoids the loading Screen to get to a scene
    /// </summary>
    /// <param name="_Scene">Scene to load</param>
    public void AvoidLoadSceneChangeScene(int _SceneNumber)
    {
        SceneManager.LoadScene(translater.EnumToScene((SceneName)_SceneNumber));
    }

    /// <summary>
    /// Fades to black before running scene change.
    /// </summary>
    /// <param name="_Scene">The scene to be loaded and switched too.</param>
    /// <returns></returns>
    public IEnumerator ChangeSceneFade(SceneName _Scene)
    {
        UIManager.Instance.EnableBlackScreen();

        yield return new WaitForSeconds(1.5f);

        ChangeScene(_Scene);
    }

    /// <summary>
    /// Will start the async loading process for loading a new scene
    /// </summary>
    /// <param name="_Scene">The scene that is going to be loaded</param>
    /// <returns>True if async scene loading starts</returns>
    public bool AsyncSceneLoad(SceneName _Scene)
    {
        Debug.Log("Next Scene is: " + NextScene);
        NewScene = SceneManager.LoadSceneAsync(translater.EnumToScene(_Scene));
        NewScene.allowSceneActivation = false;
        return true;
    }

    /// <summary>
    /// Getter for the async scene
    /// </summary>
    /// <returns>AsyncOperations object</returns>
    public AsyncOperation GetAsyncScene()
    {
        return NewScene;
    }

    public void SwitchAsyncScene()
    {
        NewScene.allowSceneActivation = true;
    }

    /// <summary>
    /// Getter for the next scene that is going to be loaded
    /// </summary>
    /// <returns>SceneName</returns>
    public SceneName GetNextScene()
    {
        return NextScene;
    }

    /// <summary>
    /// Will initialize the value for the load screen progress and turn on the UI panel
    /// </summary>
    /// <param name="_Max">The goal or end point the progress is trying to get too.</param>
    public void LoadScreen_Init(int _Max)
    {
        Max = _Max;
        Current = 0;
        panel.SetActive(true);
    }

    /// <summary>
    /// Will update what the max/goal is for the loading screen
    /// </summary>
    /// <param name="_Max">The new max that will be added to the old one</param>
    public void LoadScreen_UpdateMax(int _Max)
    {
        Max += _Max;
    }

    /// <summary>
    /// Will take in a update value and add it to the current progress
    /// </summary>
    /// <param name="_Value">The value to be added to the current progress</param>
    public void UpdateLoadScreen(int _Value)
    {
        Current += _Value;
        if (Current >= Max)
        {
            panel.SetActive(false);
        }
    }

    /// <summary>
    /// Returns the current scene that the player is in
    /// </summary>
    /// <returns> current scene as a string</returns>
    public Scene GetScene()
    {
        return translater;
    }
}

/// <summary>
/// Reference to the scenes in the build settings. Does not depend on a certain order in the build settings.
/// </summary>
public class Scene
{

    private string GameManager = "GameManager";
    private string Title = "Title";
    private string CourtYard = "CourtYard";
    private string GraveYard = "GraveYard";
    private string GoalScene = "GoalScene";
    private string UIWalkthrough = "UIWalkthrough";
    private string Tutorial = "Tutorial";
    private string CourtTutorial = "CourtyardTutorial";
    private string IntroCutscene = "OpeningCutscene";
    private string LoadingScene = "LoadingScene";
    private string LoadingSceneCourtyard = "LoadingSceneCourtyard";
    private string QueensApproach = "Queen Approach";
    private string FinalArea = "Mausoleum Inside";

    public string EnumToScene(SceneName _Scene)
    {
        switch (_Scene)
        {
            case SceneName.GameManager:
                return GameManager;
                break;
            case SceneName.Title:
                return Title;
                break;
            case SceneName.CourYard:
                return CourtYard;
                break;
            case SceneName.GraveYard:
                return GraveYard;
                break;
            case SceneName.GoalScene:
                return GoalScene;
                break;
            case SceneName.UIWalkthrough:
                return UIWalkthrough;
                break;
            case SceneName.Tutorial:
                return Tutorial;
                break;
            case SceneName.CourtTutorial:
                return CourtTutorial;
                break;
            case SceneName.IntroCutscene:
                return IntroCutscene;
                break;
            case SceneName.LoadingScene:
                return LoadingScene;
            case SceneName.LoadingSceneCourtyard:
                return LoadingSceneCourtyard;
            case SceneName.QueensApproach:
                return QueensApproach;
            case SceneName.FinalArea:
                return FinalArea;
            default:
                return "Null";
        }
    }
}

/// <summary>
/// The different scenes that are playable
/// </summary>
public enum SceneName
{
    GameManager,
    Title,
    CourYard,
    GraveYard,
    GoalScene,
    UIWalkthrough,
    Tutorial,
    CourtTutorial,
    IntroCutscene,
    LoadingScene,
    LoadingSceneCourtyard,
    QueensApproach,
    FinalArea
}
