using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/*
 * @Author: Zachary Boehm
 * @Version: 12.2.2021
 */

/// <summary>
/// This class will hold all the temporary information logic for a given instanced run.
/// </summary>
public class RunManager: MonoBehaviour
{
    /*! Timer */
    [Header("Timer Variables")]
    [SerializeField] private int CurrentTime;
    [SerializeField] private int MAX = 300;
    [SerializeField] private Slider Candle;
    [SerializeField] private TextMeshProUGUI Time;

    [Header("Run Progress Variables")]
    [SerializeField] private int TotalGraves;
    [SerializeField] private int CompletedGraves;
    [SerializeField] private bool Hidden;
    [SerializeField] private List<Body> bodies;
    [SerializeField] public bool AutoStop;

    [Header("Temp Variables")]
    [SerializeField] Sprite[] LanternStages;
    [SerializeField] GameObject Lantern;
    [SerializeField] int LanternStage = -1;

    [Header("Map Information Variables")]
    [SerializeField] MapInformation MapInfo;

    private GraveInfoRefactor ActiveGrave;

    /// <summary>
    /// Default Constructor. Initializes the timer and information variables
    /// </summary>
    /// <param name="_Time">The Text for the time that will be updated</param>
    /// <param name="_Slider">The slider that will be updated for the lantern timer</param>
    public void Start()
    {
        CurrentTime = 0;
        TotalGraves = 0;
        Hidden = false;
        Lantern.GetComponent<Image>().sprite = LanternStages[0];
    }

    /// <summary>
    /// Will reset all of the variables of the run manager.
    /// </summary>
    public void ResetRun()
    {
        CurrentTime = 0;
        TotalGraves = 0;
        Hidden = false;
        bodies.Clear();
        Lantern.GetComponent<Image>().sprite = LanternStages[0];
    }
    /// <summary>
    /// Will add a body to the list of bodies within the game manager.
    /// </summary>
    /// <param name="_NewBody">The new body that is to be added to the list of bodies</param>
    public void AddBody(Body _NewBody)
    {
        if(_NewBody != null)
        {
            bodies.Add(_NewBody);
            CompletedGraves++;
        }
    }

    /// <summary>
    /// Removes a body at the index of BodyToRemove
    /// </summary>
    /// <param name="BodyToRemove">index of the body to be removed</param>
    public void RemoveBody(int BodyToRemove)
    {
        if (bodies.Count > BodyToRemove)
        {
            bodies.RemoveAt(BodyToRemove);
        }
        
    }

    //--------------------------------------------------
    //  Getters
    //--------------------------------------------------

    /// <summary>
    /// Getter for the array of bodies that have been retrieved during the run.
    /// </summary>
    /// <returns>All the bodies from the run as int[]</returns>
    public Body[] GetBodies()
    {
        return bodies.ToArray();
    }

    public void PurgeBodies()
    {
        bodies.Clear();
        GameManager.Instance.Inventory.SoldInventory();
    }

    /// <summary>
    /// Getter for the total number of graves completed.
    /// </summary>
    /// <returns></returns>
    public int GetCompletedGraves()
    {
        return CompletedGraves;
    }

    /// <summary>
    /// Getter for whether the player is hidden or not. This is instanced to the run.
    /// </summary>
    /// <returns>The hidden state of the player as a bool</returns>
    public bool GetHidden()
    {
        return Hidden;
    }

    /// <summary>
    /// Getter for the current time on the timer in analog form. This is what game time it is starting from midnight.
    /// </summary>
    /// <returns>The analog time in game as a string</returns>
    public string GetTimeAnalog()
    {
        return $"{12 + GetTimeHour()}:{GetTimeMinute()}";
    }

    /// <summary>
    /// Getter for the current time on the timer.
    /// </summary>
    /// <returns>The current time on the timer in seconds</returns>
    public int GetTimeSec()
    {
        return CurrentTime*60;
    }

    /// <summary>
    /// Getter for the current time on the timer.
    /// </summary>
    /// <returns>The current time on the timer in minutes</returns>
    public int GetTimeMinute()
    {
        //throw new System.ArgumentException("[Error] Not implemented : RunManager.cs : Line(86)");
        return CurrentTime;
    }

    /// <summary>
    /// Getter for the current time on the timer.
    /// </summary>
    /// <returns>The total time on the timer in hours.</returns>
    public int GetTimeHour()
    {
        //throw new System.ArgumentException("[Error] Not implemented : RunManager.cs : Line(92)");
        return CurrentTime/60;
    }

    //--------------------------------------------------
    //  Setters
    //--------------------------------------------------

    /// <summary>
    /// Will toggle the hidden state of the player
    /// </summary>
    public void ToggleHidden()
    {
        Hidden = !Hidden;    
    }

    /// <summary>
    /// Will set the total number of graves that are in the grave yard for the run.
    /// </summary>
    /// <param name="_total">The total amount of graves as an int</param>
    public void SetTotalGraves(int _total)
    {
        TotalGraves = _total;
    }

    //--------------------------------------------------
    //  Funcitons
    //--------------------------------------------------

    /// <summary>
    /// When called the Timer coroutine will start. Will start the timer.
    /// </summary>
    public void StartTimer()
    {
        StartCoroutine(Timer());
    }

    /// <summary>
    /// When called it will stop the Timer coroutine. Will stop the timer.
    /// </summary>
    public void StopTimer()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Will set the current time in game. This will also update the UI
    /// </summary>
    /// <param name="_Time">The new current time</param>
    public void SetTime(int _Time)
    {
        CurrentTime = _Time;
        UpdateUI();
    }

    /// <summary>
    /// Will set the max/goal. When current time reaches this value the run ends.
    /// </summary>
    /// <param name="_Max">The new time max/goal</param>
    public void SetMax(int _Max)
    {
        MAX = _Max;
    }
    

    /// <summary>
    /// Coroutine that handles the logic of a timer. Parallel logic that will wait 1 second then increment the timer by 1.
    /// </summary>
    /// <returns>Coroutine Return Statement</returns>
    public IEnumerator Timer()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            if (CurrentTime < MAX)
            {
                CurrentTime += 1;
                UpdateUI();
            }
            else if (CurrentTime == MAX)
            {
                if (AutoStop) { 
                    GameManager.Instance.EndRun();
                }
            }
        }
    }

    /// <summary>
    /// Will update the UI for the run
    /// </summary>
    public void UpdateUI()
    {
        //UI elements are optional for the logic to work
        if (Candle != null)
        {
            Candle.value -= CurrentTime;
        }
        if (Lantern != null && LanternStages != null)
        {
            if (CurrentTime <= 100 && CurrentTime > 0 && LanternStage != 0)
            {
                Lantern.GetComponent<Image>().sprite = LanternStages[0];
                LanternStage = 0;
            }
            else if (CurrentTime <= 200 && CurrentTime > 100 && LanternStage != 1)
            {
                Lantern.GetComponent<Image>().sprite = LanternStages[1];
                LanternStage = 1;
            }
            else if (CurrentTime <= 280 && CurrentTime > 200 && LanternStage != 2)
            {
                Lantern.GetComponent<Image>().sprite = LanternStages[2];
                LanternStage = 2;
            }
            else if (CurrentTime <= 300 && CurrentTime > 280 && LanternStage != 3)
            {
                Lantern.GetComponent<Image>().sprite = LanternStages[3];
                LanternStage = 3;
            }
        }
        if (Time != null)
        {
            Time.text = GetTimeAnalog();
        }
    }

    public void DisableMinigame()
    {
        if (ActiveGrave != null)
        {
            List<GameObject> Traps = ActiveGrave.MiniGameList;

            if (Traps.Count > 0)
            {
                GameManager.Instance.ToggleMinigame(Traps[0], false);
                ActiveGrave.SetPlayerIn(false);
            }
        }
    }

    /// <summary>
    /// Returns map info ScriptableObject.
    /// </summary>
    /// <returns>
    /// The current MapInformation object.
    /// </returns>
    public MapInformation GetMapInfo()
    {
        return MapInfo;
    }

    /// <summary>
    /// Sets the MapInformation object for the next level.
    /// </summary>
    /// <param name="Val">
    /// The MapInformation object being passed in.
    /// </param>
    public void SetMapInfo(MapInformation Val)
    {
        MapInfo = Val;
    }

    /// <summary>
    /// Sets the currently active grave.
    /// </summary>
    /// <param name="_Val"></param>
    public void SetActiveGrave(GraveInfoRefactor _Val)
    {
        ActiveGrave = _Val;
    }
}
