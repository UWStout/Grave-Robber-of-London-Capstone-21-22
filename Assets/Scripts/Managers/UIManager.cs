using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * Author: Declin Anderson
 * Version: 12.7.2021
 */

/// <summary>
/// UIManager will act as the system that controls the UI element changes, such as 
///     the coin amount, 
///     lantern status, 
///     and stealth counter
/// <param name="RunManagerObj">Gives a reference to the run manager to pause the game when needed</param>
/// <param name="UIElements"></param>
/// </summary>
public class UIManager : MonoBehaviour
{
    //singlton logic
    public static UIManager Instance { get; private set; }

    [SerializeField]
    public RunManager RunManagerObj;
    [SerializeField]
    private QuestManager _QuestManager;
    [SerializeField]
    private GameObject UIElements;
    [SerializeField]
    private GameObject PauseMenu;
    [SerializeField]
    private GameObject MissionSelectMenu;
    [SerializeField]
    private GameObject[] MissionSelectButtons = new GameObject[5];
    [SerializeField]
    private GameObject Inventory;
    [SerializeField]
    private GameObject Lantern;
    [SerializeField]
    private Text MoneyCounter;
    [SerializeField]
    private GameObject Dialogue;
    [SerializeField]
    private GameObject BribeOptions;
    [SerializeField]
    private GameObject AcceptBribe;

    // Stealth Objects Needed
    [SerializeField]
    private GameObject StealthMedal;
    [SerializeField]
    private Sprite NoStealth;
    [SerializeField]
    private Sprite LowStealth;
    [SerializeField]
    private Sprite MediumStealth;
    [SerializeField]
    private Sprite HighStealth;
    [SerializeField]
    private Image StealthBorder;
    [SerializeField]
    private Image BlackScreen;
    [SerializeField]
    private List<Sprite> FullBags;

    private Color BorderColor;
    private Color BlackScreenColor;

    private float CurrentStealth = 0;
    public bool IsPaused = false;
    private bool StealthBoarderEnabled = false;
    private bool BlackScreenEnabled = false;
    bool interactJournal = false;
    public bool InMiniGame = false;
    public GameObject ActiveGame;

    string CurrentSceneName = "";
    bool SceneChanged = false;

    private MedallionControl MedC;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            MedC = StealthMedal.GetComponent<MedallionControl>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the current Alert variable equal to the starting alert level
    /// </summary>
    void Start()
    {
        CurrentStealth = StealthManager.Instance.ReturnAlertLevel();
        StealthMedallionChange();
        BorderColor = StealthBorder.color;
        BlackScreenColor = BlackScreen.color;
    }

    /// <summary>
    /// The Update method that watchs for changes in
    ///     escape key press
    ///         opening the pause menu
    ///     coin value
    ///         changes of the player current coins
    ///     alert level
    ///         guards have been alerted to the presence of the player
    ///     m key press
    ///         opening the mission select menu
    ///     player is hidden
    ///         does the stealth animation for the player
    /// </summary>
    void Update()
    {
        // When the user presses Escape it will turn on the menu UI or turn it off, or if the minigame is open it will close the minigame first
        if (Input.GetButtonDown("Cancel") && PauseMenu.activeSelf == false && IsPaused == false && NoOpenMenus() == true && InMiniGame == false && GameManager.Instance.MiniGameState == false)
        {
            EnteredPauseMenu();
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            MissionOff();
            BoardOff();
            InventoryClosed();
            if(GameManager.Instance.MiniGameState == true)
                CloseMiniGame();
            GameObject Journal = transform.GetChild(3).gameObject;
            Journal.SetActive(false);
            GameObject PickSale = transform.GetChild(12).gameObject;
            PickSale.SetActive(false);
            GameObject SalesInvoice = transform.GetChild(11).gameObject;
            SalesInvoice.SetActive(false);
            CloseStore();
            ExitedPauseMenu();
        }

        // When the user presses M it will turn on the menu select UI or turn it off
        if (Input.GetButtonDown("MissionUI") && MissionSelectMenu.activeSelf == false && NoOpenMenus() == true && SceneManager.GetActiveScene().name == "CourtYard")
        {
            MissionOn();
            MedallionOn();
        }
        else if (Input.GetButtonDown("MissionUI") && MissionSelectMenu.activeSelf == true)
        {
            MissionOff();
        }

        // When the user press i it will turn on the inventory bag
        if (Input.GetButtonDown("Inventory") && Inventory.activeSelf == false && NoOpenMenus() == true && (SceneManager.GetActiveScene().name == "GraveYard" || SceneManager.GetActiveScene().name == "CourtYard" || SceneManager.GetActiveScene().name == "Tutorial"))
        {
            InventoryOpened();
        }
        else if (Input.GetButtonDown("Inventory") && Inventory.activeSelf == true)
        {
            InventoryClosed();
        }

        // Updates the Coin Value to the current value
        MoneyCounter.text = GameManager.Instance.GetMoney().ToString();

        // When the alert level changes it will call the StealthMedallionChange Function to update the image
        if (StealthManager.Instance.ReturnAlertLevel() != CurrentStealth)
        {
            StealthMedallionChange();
            CurrentStealth = StealthManager.Instance.ReturnAlertLevel();
        }

        // Starts the process of either enabling the border for stealth or disabling it
        if (StealthBoarderEnabled)
        {
            if (BorderColor.a < 0.8f)
            {
                BorderColor.a += 0.02f;
                StealthBorder.color = BorderColor;
            }
        }
        else
        {
            if (BorderColor.a > 0.0f)
            {
                BorderColor.a -= 0.03f;
                StealthBorder.color = BorderColor;
            }
            else if (StealthBorder.enabled)
            {
                StealthBorder.enabled = false;
            }
        }

        // Makes a black screen appear over the screen
        if (BlackScreenEnabled)
        {
            if (BlackScreenColor.a < 1.0f)
            {
                BlackScreenColor.a += 0.05f;
                BlackScreen.color = BlackScreenColor;
            }
        }
        else
        {
            if (BlackScreenColor.a > 0.0f)
            {
                BlackScreenColor.a -= 0.05f;
                BlackScreen.color = BlackScreenColor;
            }
            else if (BlackScreen.enabled)
            {
                BlackScreen.enabled = false;
            }
        }

        //Add in key input for journal opening
        if (Input.GetButtonDown("Journal") && !interactJournal && NoOpenMenus() == true && (SceneManager.GetActiveScene().name == "GraveYard" || SceneManager.GetActiveScene().name == "CourtYard" || SceneManager.GetActiveScene().name == "Tutorial"))
        {
            interactJournal = true;
            //if (MiniGameState == false)
            //{
            ToggleJournal();
            //}
        }
        else if (Input.GetButtonDown("Journal") && interactJournal && (SceneManager.GetActiveScene().name == "GraveYard" || SceneManager.GetActiveScene().name == "CourtYard" || SceneManager.GetActiveScene().name == "Tutorial"))
        {
            interactJournal = false;
            ToggleJournal();
        }

        // When a scene is changed it makes sure to update the UI to keep only what is needed
        string CheckForSceneChange = SceneManager.GetActiveScene().name;
        if(CheckForSceneChange != CurrentSceneName)
        {
            NeededUI();
        }

        CurrentBagLook();
    }

    /// <summary>
    /// When the Player changes their alert level, the function changes the image to fit the current level
    /// </summary>
    void StealthMedallionChange()
    {
        if (StealthManager.Instance.ReturnAlertLevel() < 100.0f)
            StealthMedal.GetComponent<Image>().sprite = NoStealth;
        else if (StealthManager.Instance.ReturnAlertLevel() < 200.0f)
            StealthMedal.GetComponent<Image>().sprite = LowStealth;
        else if (StealthManager.Instance.ReturnAlertLevel() < 300.0f)
            StealthMedal.GetComponent<Image>().sprite = MediumStealth;
        else
            StealthMedal.GetComponent<Image>().sprite = HighStealth;
    }

    /// <summary>
    /// When the player pauses the game it adjusts the UI elements and pauses the game
    /// </summary>
    public void EnteredPauseMenu()
    {
        // Changes UIs
        GameObject NoticeBoardUI = transform.GetChild(2).transform.GetChild(1).gameObject;
        UIElements.SetActive(false);
        PauseMenu.SetActive(true);
        NoticeBoardUI.SetActive(false);

        // Stops the Timer
        if(SceneManager.GetActiveScene().name == "GraveYard")
            RunManagerObj.StopTimer();
        Time.timeScale = 0;
        IsPaused = true;
    }

    /// <summary>
    /// When the player unpauses the game it adjusts the UI elements and unpauses the game
    /// </summary>
    public void ExitedPauseMenu()
    {
        // Changes UIs
        UIElements.SetActive(true);
        PauseMenu.SetActive(false);

        NeededUI();

        // Stops the Timer
        if(SceneManager.GetActiveScene().name == "GraveYard")
            RunManagerObj.StartTimer();
        Time.timeScale = 1;

        IsPaused = false;
    }

    /// <summary>
    /// Enables the stealth border to appear when the player is in stealth
    /// </summary>
    public void EnableStealth()
    {
        StealthBoarderEnabled = true;
        StealthBorder.enabled = true;
        BorderColor.a = 0;
        StealthBorder.color = BorderColor;
    }

    /// <summary>
    /// Disables the stealth border element in the scene
    /// </summary>
    public void DisableStealth()
    {
        StealthBoarderEnabled = false;
    }

    /// <summary>
    /// Enables a fade to black
    /// </summary>
    public void EnableBlackScreen()
    {
        BlackScreenEnabled = true;
        BlackScreen.enabled = true;
        BlackScreenColor.a = 0;
        BlackScreen.color = BlackScreenColor;
    }

    /// <summary>
    /// Fades back from black
    /// </summary>
    public void DisableBlackScreen()
    {
        BlackScreenEnabled = false;
    }

    /// <summary>
    /// Switches dialogue's current state.
    /// </summary>
    public void DialogueToggle()
    {
        Dialogue.SetActive(!Dialogue.activeSelf);
        GameManager.Instance._DialogueManager.ResetDialogue();
    }

    /// <summary>
    /// Enables the dialogue box.
    /// </summary>
    public void DialogueEnable()
    {
        Dialogue.SetActive(true);
        GameManager.Instance._DialogueManager.ResetDialogue();
    }

    /// <summary>
    /// Disables the dialogue box.
    /// </summary>
    public void DialogueDisable()
    {
        Dialogue.SetActive(false);
    }

    /// <summary>
    /// When called it will call the quest manager and tell it to toggle it's UI.
    /// </summary>
    public void ToggleJournal()
    {
        _QuestManager.ToggleUI();
    }

    /// <summary>
    /// When called it will swap the current state of the Inventory screen
    /// </summary>
    public void ToggleInventory()
    {
        if(Inventory.activeSelf == false)
        {
            InventoryOpened();
        }
        else
        {
            InventoryClosed();
        }
    }

    /// <summary>
    /// When called from Courtyard Collisions script it will set the Notice Board UI 
    /// to True and the interaction indicator to false
    /// </summary>
    public void BoardOn()
    {
        GameObject NoticeBoardUI = transform.GetChild(2).transform.GetChild(0).gameObject;
        _QuestManager.TurnInButton.SetActive(false);
        NoticeBoardUI.SetActive(true);
    }

    /// <summary>
    /// When called from Courtyard Collisions script it will set the Notice Board UI 
    /// to false and the interaction indicator to true
    /// </summary>
    public void BoardOff()
    {
        GameObject NoticeBoardUI = transform.GetChild(2).transform.GetChild(0).gameObject;
        GameObject Quest = transform.GetChild(2).transform.GetChild(1).gameObject;
        _QuestManager.TurnInButton.SetActive(false);
        NoticeBoardUI.SetActive(false);
        Quest.SetActive(false);
    }

    /// <summary>
    /// Enables the Mission Select Menu UI
    /// </summary>
    public void MissionOn()
    {
        MissionSelectMenu.SetActive(true);
        foreach (GameObject B in MissionSelectButtons)
        {
            B.GetComponent<MissionSelectManager>().PullInfo();
        }
    }

    /// <summary>
    /// Disables the Mission
    /// </summary>
    public void MissionOff()
    {
        MissionSelectMenu.SetActive(false);
    }

    public void RandomizeLevels()
    {
        int NumOptions = MissionSelectButtons.Length;

        MapInformation[] RandMaps = GetComponent<MapObjectHolder>().GetRandomMaps(NumOptions);

        for (int i = 0; i < NumOptions; i++)
        {
            MissionSelectButtons[i].GetComponent<MapInfoHolder>().SetMI(RandMaps[i]);
        }
    }

    /// <summary>
    /// Enables the Mission Select Menu UI
    /// </summary>
    public void InventoryOpened()
    {
        Inventory.SetActive(true);
        Inventory.transform.GetChild(0).GetChild(2).GetComponent<Scrollbar>().value = 1;
    }

    /// <summary>
    /// Disables the Mission
    /// </summary>
    public void InventoryClosed()
    {
        Inventory.SetActive(false);
    }

    /// <summary>
    /// Enables the image of the stealth medallion UI object.
    /// </summary>
    /// <returns>
    /// Whether the medallion was enabled or disabled before calling the function.
    /// </returns>
    public bool MedallionOn()
    {
        bool OldState = StealthMedal.GetComponent<Image>().enabled;
        StealthMedal.GetComponent<Image>().enabled = true;
        MedC.ToggleSpikes(true);
        return OldState;
    }

    /// <summary>
    /// Disables the image of the stealth medallion UI object.
    /// </summary>
    /// <returns>
    /// Whether the medallion was enabled or disabled before calling the function.
    /// </returns>
    public bool MedallionOff()
    {
        bool OldState = StealthMedal.GetComponent<Image>().enabled;
        StealthMedal.GetComponent<Image>().enabled = false;
        MedC.ToggleSpikes(false);
        return OldState;
    }

    /// <summary>
    /// Changes the image of the medallion spikes on the stealth medallion.
    /// </summary>
    /// <param name="Dir">
    /// Which spike should be changed.
    /// </param>
    /// <param name="_Val">
    /// Which image should it be changed with.
    /// </param>
    /// <returns>
    /// Whether the function was succesful or not.
    /// </returns>
    public bool SetSpike(Direction Dir, int _Val)
    {
        return MedC.SetSpike(Dir, _Val);
    }

    /// <summary>
    /// Sets the buyingUI to true
    /// </summary>
    public void OpenStore()
    {
        GameObject BuyingUIObject = transform.Find("BuyingUI").gameObject;
        BuyingUIObject.SetActive(true);
        SellingScreenScript SellScriptObject = transform.GetComponentInChildren<SellingScreenScript>();
    }

    /// <summary>
    /// Sets the buyingUI to false
    /// </summary>
    public void CloseStore()
    {
        GameObject BuyingUIObject = transform.GetChild(7).gameObject;
        BuyingUIObject.SetActive(false);
    }

    /// <summary>
    /// Checks to see if there is any elements open in the PlayerUI Prefab
    /// </summary>
    /// <returns> true if none of the menus are open, otherwise returns false if any are open</returns>
    public bool NoOpenMenus()
    {
        GameObject BuyingUIObject = transform.GetChild(7).gameObject;
        GameObject NoticeBoardUI = transform.GetChild(2).transform.GetChild(0).gameObject;
        GameObject Journal = transform.GetChild(3).gameObject;
        GameObject PickSale = transform.GetChild(12).gameObject;
        GameObject SalesInvoice = transform.GetChild(11).gameObject;
        if (BuyingUIObject.activeSelf == false && NoticeBoardUI.activeSelf == false && Journal.activeSelf == false && MissionSelectMenu.activeSelf == false && Inventory.activeSelf == false && PickSale.activeSelf == false && SalesInvoice.activeSelf == false)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Disables all UI under the main UIElements object, primarily used during graveyards.
    /// </summary>
    /// <returns>
    /// Whether the UI was enabled or disabled before calling.
    /// </returns>
    public bool DisableGameUI()
    {
        bool Temp = UIElements.activeSelf;

        UIElements.SetActive(false);

        return Temp;
    }

    /// <summary>
    /// Enables all UI under the main UIElements object, primarily used during graveyards.
    /// </summary>
    /// <returns>
    /// Whether the UI was enabled or disabled before calling.
    /// </returns>
    public bool EnableGameUI()
    {
        bool Temp = UIElements.activeSelf;

        UIElements.SetActive(true);

        return Temp;
    }

    /// <summary>
    /// Sets the enabled of the guard's bribe option buttons to the passed in value.
    /// </summary>
    /// <param name="Val">
    /// Whether to enable or disable the buttons.
    /// </param>
    /// <param name="CanAfford">
    /// Whether to enable or disable the accept bribe button.
    /// </param>
    /// <returns>
    /// Whether the UI was enabled or disabled before calling.
    /// </returns>
    public bool SetBribeOptions(bool Val, bool CanAfford)
    {
        bool Temp = BribeOptions.activeSelf;

        BribeOptions.SetActive(Val);
        AcceptBribe.GetComponent<Button>().interactable = CanAfford;
        Debug.Log("set bribe options");
        Debug.Log(Val);
        return Temp;
    }

    /// <summary>
    /// Sets the Bribe options where the user can't afford
    /// </summary>
    /// <param name="Val"> Whether to enable or disable the buttons</param>
    /// <returns> Whether the UI was enabled</returns>
    public bool SetBribeOptions(bool Val)
    {
        bool Temp = BribeOptions.activeSelf;

        BribeOptions.SetActive(Val);

        return Temp;
    }

    public void CloseBribe()
    {
        transform.GetChild(8).gameObject.SetActive(false);
    }

    /// <summary>
    /// Gets the BribeOption object.
    /// </summary>
    /// <returns>
    /// The BribeOption object.
    /// </returns>
    public GameObject GetBribeOptions()
    {
        return BribeOptions;
    }

    /// <summary>
    /// This will check where the user is currently and it will:
    ///     Disable UI interaction that are not to be used in that scene
    ///     Disable UI images that are not to be used in that scene
    /// </summary>
    public void NeededUI()
    {
        if (SceneManager.GetActiveScene().name == "CourtYard")
        {
            UIElements.SetActive(true);
            StealthMedal.SetActive(false);
            Lantern.SetActive(false);
        }
        else if(SceneManager.GetActiveScene().name == "CourtyardTutorial")
        {
            UIElements.SetActive(true);
            Lantern.SetActive(false);
        }
        else if(SceneManager.GetActiveScene().name == "GraveYard")
        {
            UIElements.SetActive(true);
            StealthMedal.SetActive(true);
            Lantern.SetActive(true);
        }
        else if(SceneManager.GetActiveScene().name == "LoadingScene" || SceneManager.GetActiveScene().name == "OpeningCutscene" || SceneManager.GetActiveScene().name == "LoadingSceneCourtyard")
        {
            UIElements.SetActive(false);
            Inventory.SetActive(false);
        }
        CurrentSceneName = SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// Updates the bag image to be the current Bag Tier
    /// </summary>
    public void CurrentBagLook()
    {
        if (SceneManager.GetActiveScene().name == "CourtYard")
        {
            if (GameManager.Instance._UpgradeManager.GetCurrentBodyBag().GetMaxBodies() <= GameManager.Instance.GetBodyCollection().Length)
            {
                switch (GameManager.Instance._UpgradeManager.GetCurrentBodyBag().GetIcon().name)
                {
                    case "BodyBagCanvas(T1)":
                        UIElements.transform.GetChild(9).GetComponent<Image>().sprite = FullBags[0];
                        break;
                    case "BodyBagWool(T2)":
                        UIElements.transform.GetChild(9).GetComponent<Image>().sprite = FullBags[1];
                        break;
                    case "BodyBagLinen(T3)":
                        UIElements.transform.GetChild(9).GetComponent<Image>().sprite = FullBags[2];
                        break;
                    case "BodyBagSilk(T4)":
                        UIElements.transform.GetChild(9).GetComponent<Image>().sprite = FullBags[3];
                        break;
                }
            }
            else
            {
                UIElements.transform.GetChild(9).GetComponent<Image>().sprite = GameManager.Instance._UpgradeManager.GetCurrentBodyBag().GetIcon();
            }
        }
        else
        {
            if (GameManager.Instance._UpgradeManager.GetCurrentBodyBag().GetMaxBodies() <= (GameManager.Instance.GetBodyCollection().Length + GameManager.Instance._RunManager.GetBodies().Length))
            {
                switch (GameManager.Instance._UpgradeManager.GetCurrentBodyBag().GetIcon().name)
                {
                    case "BodyBagCanvas(T1)":
                        UIElements.transform.GetChild(9).GetComponent<Image>().sprite = FullBags[0];
                        break;
                    case "BodyBagWool(T2)":
                        UIElements.transform.GetChild(9).GetComponent<Image>().sprite = FullBags[1];
                        break;
                    case "BodyBagLinen(T3)":
                        UIElements.transform.GetChild(9).GetComponent<Image>().sprite = FullBags[2];
                        break;
                    case "BodyBagSilk(T4)":
                        UIElements.transform.GetChild(9).GetComponent<Image>().sprite = FullBags[3];
                        break;
                }
            }
            else
            {
                UIElements.transform.GetChild(9).GetComponent<Image>().sprite = GameManager.Instance._UpgradeManager.GetCurrentBodyBag().GetIcon();
            }
        }
    }
    public void CloseMiniGame()
    {
        GameManager.Instance.ToggleMinigame(ActiveGame, false);
    }
}
