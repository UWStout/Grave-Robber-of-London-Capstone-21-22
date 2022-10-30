/**
 * author: Damian Link
 * version: 10/31/2021
 */
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the Torpedo Minigame
/// </summary>
/// <remarks>
/// ### Variables
/// 
/// * TimerTextObject - Game object that stores the text for timer
/// * TimerStartingNum - Timers starting number
/// * ButtonSprite - Sprite of the buttons
/// * CoffinOpeningDelay - delay to display button
/// * ButtonNumber - total number of buttons
/// * GoalClickedButtons - target of number of buttons to click
/// </remarks>
public class TorpedoMinigame : MiniGame
{
    // game object that stores the text for timer
    [SerializeField] private GameObject TimerTextObject = null;
    // Prefab for the buttons
    [SerializeField] private GameObject ButtonPrefab;

    //  Timers starting number
    [SerializeField] private int TimerStartingNum = 1000;
    // timer for the quick action
    private int TorpedoTimer = 1000;
    // stores if the timer has started
    private bool TimerStarted = false;
    // delay to display button
    [SerializeField] private int CoffinOpeningDelay = 10;

    // total number of buttons
    private int ButtonNumber = 1;
    // current number of buttons
    private int CurrentButtonNumber = 0;

    // number of buttons clicked
    private int ClickedButtons = 0;
    // target of number of buttons to click
    public int GoalClickedButtons = 1;

    // If the player has failed the minigame
    private bool MinigameLost = false;

    [SerializeField] private Text InstructText;
    private string OpenCoffinText = "Click on the coffin to open it";
    private string DisarmTorpedoText = "Click the buttons to disarm it";

    /// <summary>
    /// gets if the trap is disarmed
    /// </summary>
    /// <returns>true if disarmed/false if not</returns>
    public override bool CheckWin ()
    {
        return (ClickedButtons == GoalClickedButtons);
    }

    /// <summary>
    /// Checks for lose
    /// </summary>
    /// <returns>Lost minigame</returns
    public override bool CheckLose()
    {
        return MinigameLost;
    }

    /// <summary>
    /// starts timer and sets coffin open to true 
    /// </summary>
    public void CoffinOpened ()
    {
        TimerStarted = true;
        TimerTextObject.SetActive(true);
        transform.GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
    }

    /// <summary>
    /// disarms the trap
    /// </summary>
    /// <param name="Button">button that was pressed</param>
    public void DisarmTrap (Button Button = null)
    {
        ClickedButtons++;
        if (Button != null)
        {
            Destroy(Button.gameObject);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Torpedo Disarm");
        }
        if (GoalClickedButtons == ClickedButtons) {
            TimerStarted = false;
        }
        
    }

    /// <summary>
    /// sets the timer to starting value and gets the timer object
    /// </summary>
    private void Awake()
    {
        ButtonNumber = GoalClickedButtons;
        TorpedoTimer = TimerStartingNum;
        if (InstructText != null)
        {
            InstructText.text = OpenCoffinText;

        }
    }

    /// <summary>
    /// places the quick action button
    /// </summary>
    private void PlaceButton ()
    {
        Button QuickActionButton = Instantiate(ButtonPrefab, GameObject.Find("QuickAction").transform).transform.GetChild(0).GetComponent<Button>();

        QuickActionButton.onClick.AddListener(() => DisarmTrap(QuickActionButton));
        float Y = Random.Range(-102, 102);
        float X = Random.Range(-30, 30);
        if (Y == 46)
        {
            X = Random.Range(-56, 56);
        }
        Vector3 ButtonPos = new Vector3(X, Y, 10);
        QuickActionButton.gameObject.transform.localPosition = ButtonPos;

    }

    /// <summary>
    /// when the timer has been started count it down and place the quick action buttons at certain times
    /// </summary>
    void Update()
    {
        if (TimerStarted)
        {
            TorpedoTimer--;

            InstructText.text = DisarmTorpedoText;
            TimerTextObject.GetComponent<Text>().text = TorpedoTimer.ToString();

            if (ButtonNumber > CurrentButtonNumber && TorpedoTimer == TimerStartingNum - (CoffinOpeningDelay * (CurrentButtonNumber + 1))) 
            {
                PlaceButton();
                CurrentButtonNumber++;
            }
            // if timer gets to 0, the minigame is lost
            if (TorpedoTimer == 0)
            {
                StealthManager.Instance.AlertIncrease(100.0f);
                StealthManager.Instance.AddAlertPulse(200.0f, GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapGeneration>().GetTilePos(GameObject.FindGameObjectWithTag("Player").transform));
                StealthManager.Instance.AddAlertPulse(200.0f, GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapGeneration>().GetTilePos(GameObject.FindGameObjectWithTag("Player").transform));
                // force trap to be disarmed
                ClickedButtons = GoalClickedButtons;
                TimerStarted = false;
                MinigameLost = true;
                Debug.Log("Lost");
            }
        }   
    }


    /// <summary>
    /// Sets the goal number of buttons
    /// </summary>
    /// <param name="_Val">New number of buttons</param>
    public void SetGoalClickedButtons(int _Val)
    {
        GoalClickedButtons = _Val;
    }

    /// <summary>
    /// Sets the opening delay
    /// </summary>
    /// <param name="_Val">New opening delay</param>
    public void SetCoffinOpeningDelay(int _Val)
    {
        CoffinOpeningDelay = _Val;
    }

    /// <summary>
    /// Sets the starting number
    /// </summary>
    /// <param name="_Val">New starting number</param>
    public void SetTimerStartingNumber(int _Val)
    {
        TimerStartingNum = _Val;
    }
}
