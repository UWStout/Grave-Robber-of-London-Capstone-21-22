/*
 * Author: Damian Link
 * Version: 4/12/22
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the controls of the Coffin cage minigame
/// </summary>
/// /// <remarks>
/// ### Variables
/// 
/// * TargetNumberOfCuts - Target number of cuts
/// * PossiblePos - A list of possible positions for the cutting points
/// * ButtonSprite - Sprite of the buttons
/// </remarks>
public class CoffinCageMiniGame : MiniGame
{
    // A list of possible positions for the cutting points
    public List<Vector3> PossiblePos = new List<Vector3>();
    // sprite of the buttons
    public Sprite ButtonSprite;
    // prefab of the dig slider
    [SerializeField] private GameObject SliderPrefab;

    public GraveInfoRefactor GraveInfo;

    // Array of types of break points
    [SerializeField] private GameObject[] BreakPointPrefabs;

    // text to show instructions
    [SerializeField] private Text InstuctText;
    private string SelectingInstruct = "Click on rusted parts of cage";
    private string CuttingInstruct = "Press Space/E to cut the cage";

    // current number of cuts
    private int CurrentNumberOfCuts = 0;
    // target number of cuts
    [SerializeField] private int TargetNumberOfCuts = 1;

    // the current dig slider
    private GameObject CurrentSlider = null;
    // The current progress slider showing progress of breaking bar
    private Slider CurrentProgressSlider = null;
    // list of current buttons
    private List<Button> AvailableButtons = new List<Button>();
    private  Button CurrentButton;
    // Image for place of image
    public Image FakeCursorObj;

    /// <summary>
    /// Increases the dificulty by 
    /// </summary>
    /// <param name="CutNumber">Number of cuts</param>
    public void IncreaseDificulty (int CutNumber = 0)
    {
        if (CutNumber > 0)
        {
            TargetNumberOfCuts = CutNumber;
        } 
    }

    /// <summary>
    /// Cuts the trap, creates a slider minigame object
    /// </summary>
    /// <param name="Button">button that has been pressed</param>
    public void CutTrap(Button NewButton = null)
    {
        if (NewButton != null)
        {
            CurrentButton = NewButton;
            if (CurrentSlider == null)
            {
                InstuctText.text = CuttingInstruct;
                CurrentSlider = Instantiate(SliderPrefab, Vector3.zero, Quaternion.identity, transform);
                CurrentSlider.transform.localScale = new Vector3(1, 1, 1);
                CurrentSlider.transform.localPosition = Vector3.zero;
                CurrentSlider.gameObject.GetComponent<DigSliderControl>().DigProgressChange = 0.2f;
                CurrentSlider.gameObject.GetComponent<DigSliderControl>().DiggingMinigame = false;
                CurrentSlider.SetActive(true);


                CurrentProgressSlider = CurrentSlider.transform.GetChild(1).gameObject.GetComponent<Slider>();
            }
        }
        // makes all except the current button not interactable
        for (int i = 0; i < AvailableButtons.Count; i++)
        {
            if (CurrentSlider.gameObject != AvailableButtons[i].gameObject)
            {
                AvailableButtons[i].enabled = false;
            }
        }
    }

    /// <summary>
    /// makes the avalable buttons interactable
    /// </summary>
    private void UnlockButtons ()
    {
        for (int i = 0; i < AvailableButtons.Count; i++)
        {
            AvailableButtons[i].enabled = true;
        }
    }

    /// <summary>
    /// gets if the trap was disarmed/Won
    /// </summary>
    /// <returns>wheather or not trap has been disarmed/Won</returns>
    public override bool CheckWin ()
    {
        return (CurrentNumberOfCuts == TargetNumberOfCuts);
    }

    /// <summary>
    /// sets the number of cuts completed
    /// </summary>
    /// <param name="count">int of the wanted Cut Number</param>
    public void SetCutNumber(int count = 0)
    {
        CurrentNumberOfCuts = count;
    }

    /// <summary>
    /// gets the current number of the cuts
    /// </summary>
    /// <returns>int of the current cuts</returns>
    public int GetCurrentNumberOfCuts()
    {
        return CurrentNumberOfCuts;
    }

    /// <summary>
    /// creates a new button for a breaking point
    /// </summary>
    private void generateCutSpot()
    {
        Button NewCrack = Instantiate(BreakPointPrefabs[Random.Range(0, BreakPointPrefabs.Length)], GameObject.Find("BreakPoints").transform).GetComponent<Button>();
        NewCrack.onClick.AddListener(() => CutTrap(NewCrack));
 
        int crackPos = Random.Range(0, PossiblePos.Count);
        NewCrack.gameObject.transform.localPosition = PossiblePos[crackPos];
        PossiblePos.RemoveAt(crackPos);

        AvailableButtons.Add(NewCrack);
    }

    /// <summary>
    /// adds new break points to the cage
    /// </summary>
    void Awake () {
        for (int i = 0; i < TargetNumberOfCuts; i++)
        {
            generateCutSpot();
        }

        InstuctText.text = SelectingInstruct;
    }
    
    /// <summary>
    /// Changes cursor to bolt cutters
    /// </summary>
    private void OnEnable()
    {
        GraveInfo = transform.parent.parent.GetComponentInChildren(typeof(GraveInfoRefactor)) as GraveInfoRefactor;
        // Checks if player has a boltcutter
        if (GameManager.Instance._UpgradeManager.GetCurrentBolltCutters() != null)
        {
            Cursor.visible = false;
            FakeCursorObj.sprite = GameManager.Instance._UpgradeManager.GetCurrentBolltCutters().GetClosedtexture();
        } else
        {
            FakeCursorObj.enabled = false;
            // makes all except the current button not interactable
            for (int i = 0; i < AvailableButtons.Count; i++)
            {
                AvailableButtons[i].enabled = false;
            }
        }
    }

    /// <summary>
    /// Sets the cursror to be visible
    /// </summary>
    private void OnDisable()
    {
        Cursor.visible = true;
    }

    /// <summary>
    /// Sets the cursror to be visible
    /// </summary>
    private void OnDestroy()
    {
        Cursor.visible = true;
    }

    /// <summary>
    /// updates the progress, moves cursor
    /// </summary>
    void Update()
    {
        FakeCursorObj.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - (FakeCursorObj.rectTransform.rect.height / 3), 0);

        if (CurrentProgressSlider != null)
        {
            if (CurrentProgressSlider.value == CurrentProgressSlider.maxValue)
            {
                InstuctText.text = SelectingInstruct;
                CurrentNumberOfCuts++;
                CurrentProgressSlider = null;
                Destroy(CurrentSlider);
                CurrentSlider = null;
                UnlockButtons();
                AvailableButtons.Remove(CurrentButton);
                CurrentButton.interactable = false;
            }
        }

        if(CurrentNumberOfCuts == TargetNumberOfCuts)
        {
            GraveInfo.TurnOffCage();
        }
    }
}
