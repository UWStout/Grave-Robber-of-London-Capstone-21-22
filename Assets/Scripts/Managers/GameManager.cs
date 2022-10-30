/*
Author: Zachary Boehm
Version: 12.7.2021
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// The manager that will act as a brain. Will call functions and other scripts for highest level logic. Will also hold global variables
/// </summary>
/// <remarks>
/// Creation of GameManager Singleton
/// Referenced from: https://forum.unity.com/threads/question-about-creating-game-manager-using-singleton-design-concept.1045828/
/// </remarks>
[RequireComponent(typeof(SceneManager))]
public class GameManager : MonoBehaviour
{
    // Check to see if we're about to be destroyed.
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static GameManager m_Instance;
    private GameObject CurrentActiveMiniGame;

    public static GameManager Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(GameManager) +
                    "' already destroyed. Returning null.");
                return null;
            }
            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = (GameManager)FindObjectOfType(typeof(GameManager));
                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<GameManager>();
                        singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";
                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return m_Instance;
            }
        }
    }



    //-------------------------------------------------------------------------------------------------


    [Header("Managers")]
    [SerializeField] public QuestManager _QuestManager;
    [SerializeField] public SceneController _SceneManager;
    [SerializeField] public RunManager _RunManager;
    [SerializeField] public UpgradeManager _UpgradeManager;
    [SerializeField] public Dialogue _DialogueManager;

    [Header("Player Information")]
    [SerializeField] private int Money = 0;
    private int MoneyCap = 10000;
    private bool Hidden = false;
    [SerializeField] private List<Body> BodyCollection;
    [SerializeField] public InventoryBag Inventory;
    [SerializeField] private List<Treasure> TreasureCollection;

    [Header("Quests")]
    [SerializeField] private List<Quests> _Quests = new List<Quests>();

    [Header("UI")]
    [SerializeField] public bool MiniGameState = false;
    [Header("Graveyard Run UI")]
    [SerializeField] TextMeshProUGUI Time = null;
    [SerializeField] GameObject RunUI;
    [Header("Scenes")]
    [SerializeField] SceneName TitleScene;
    [SerializeField] SceneName ReturnScene;
    [SerializeField] SceneName GraveyardScene;


    //-------------------------------------------------------------------------------------------------
    // UI Logic
    //-------------------------------------------------------------------------------------------------


    /// <summary>
    /// Will toggle on or off the minigame UI and keep track of its current on/off state.
    /// </summary>
    /// <param name="MiniGame">The canvas that is being toggled for minigames.</param>
    /// <remarks>
    /// Called with: GameManager.Instance.ToggleMinigame(GameObject);
    /// </remarks>
    public void ToggleMinigame(GameObject MiniGame, bool _State)
    {
        MiniGame.SetActive(_State);
        CurrentActiveMiniGame = MiniGame;
        MiniGameState = _State;
    }

    public void InturptMiniGame()
    {
        CurrentActiveMiniGame.SetActive(false);
    }


    //-------------------------------------------------------------------------------------------------
    // Getters
    //-------------------------------------------------------------------------------------------------

    /// <summary>
    /// Will return the current money/currency that the player has.
    /// </summary>
    /// <returns>Value of how much money the player has</returns>
    public int GetMoney()
    {
        return Money;
    }

    /// <summary>
    /// Will get the money cap for the player
    /// </summary>
    /// <returns>The money cap as an integer</returns>
    public int GetMoneyCap()
    {
        return MoneyCap;
    }

    /// <summary>
    /// A getter for the collection of bodies
    /// </summary>
    /// <returns>The collection of bodies in the form of a int array</returns>
    public Body[] GetBodyCollection()
    {
        return BodyCollection.ToArray();
    }

    /// <summary>
    /// returns the list of bodies to other scrips
    /// </summary>
    /// <returns></returns>
    public List<Body> ReturnBodyList()
    {
        return BodyCollection;
    }

    /// <summary>
    /// A getter for the run manager's grave completion number
    /// </summary>
    /// <returns>The number of graves that have been completed</returns>
    public int GetGravesCompleted()
    {
        return _RunManager.GetCompletedGraves();
    }

    /// <summary>
    /// Gets the Treasure collection
    /// </summary>
    /// <returns>A array of Treasure objects</returns>
    public Treasure[] GetTreasureCollection()
    {
        return TreasureCollection.ToArray();
    }
    /// <summary>
    /// returns the list of treasure to other scrips
    /// </summary>
    /// <returns></returns>
    public List<Treasure> RetunrTreasure()
    {
        return TreasureCollection;
    }

    /// <summary>
    /// Takes the new amount of money and applies it to the total Can be used for both getting and spending money
    /// </summary>
    /// <param name="_Amount">+- money value to be applied</param>
    /// <returns>State of success for applying the new money amount</returns>
    public bool UpdateMoney(int _Amount)
    {
        int newMoney = Money + _Amount;
        if (newMoney >= 0)
        {
            Money = newMoney;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// When Called it will return the players current hidden state
    /// </summary>
    /// <returns>The value of the player's hidden state</returns>
    /// <remarks>
    /// Call -> <c>GameManager.Instance.IsHidden();</c>
    /// </remarks>
    public bool IsHidden()
    {
        return _RunManager.GetHidden();
    }

    //-------------------------------------------------------------------------------------------------
    // Setters
    //-------------------------------------------------------------------------------------------------

    /// <summary>
    /// Can be passed either an int or the enum equivalent of the scene that is to be loaded
    /// </summary>
    /// <param name="_Scene">The int or Scene value of the specific scene that will be loaded</param>
    public void ChangeScene(int _Scene)
    {
        _SceneManager.ChangeScene((SceneName)_Scene);
    }

    /// <summary>
    /// Will set the collection of bodies to the list that is passed in
    /// </summary>
    /// <param name="_Bodies">List of bodies to be set to the inventory</param>
    public void SetBodyCollection(Body[] _Bodies)
    {
        BodyCollection.Clear();
        if (_Bodies != null)
        {
            for (int i = 0; i < _Bodies.Length; i++)
            {
                BodyCollection.Add(_Bodies[i]);
            }
        }
    }

    /// <summary>
    /// Sets the current list of treasures to what is given
    /// </summary>
    /// <param name="_Treasures">The new list of treasures</param>
    public void SetTreasureCollection(Treasure[] _Treasures)
    {
        TreasureCollection.Clear();
        if (_Treasures != null)
        {
            for (int i = 0; i < _Treasures.Length; i++)
            {
                TreasureCollection.Add(_Treasures[i]);
            }
        }
    }

    /// <summary>
    /// Will set the money max
    /// </summary>
    /// <param name="_Cap">The new money maximum</param>
    public void SetMoneyCap(int _Cap)
    {
        MoneyCap = _Cap;
    }

    /// <summary>
    /// Can be passed either an int or the enum equivalent of the scene that is to be loaded
    /// </summary>
    /// <param name="_Scene">The int or Scene value of the specific scene that will be loaded</param>
    public void ChangeScene(SceneName _Scene)
    {
        _SceneManager.ChangeScene(_Scene);
    }

    //-------------------------------------------------------------------------------------------------
    // Functions
    //-------------------------------------------------------------------------------------------------

    /// <summary>
    /// Will initialize all variables for the game manager
    /// </summary>
    private void Start()
    {
        _RunManager.ResetRun();
        Save SaveObject = FindObjectOfType<Save>();
        SaveObject.LoadFromJson();
    }

    /// <summary>
    /// When called the game will exit.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Will take the type of goal that is completed and pass it to the
    /// quest manager.This will increment 1 on every quest that has this
    /// quest type.
    /// </summary>
    /// <param name="_Type">The type of task that has been completed so it can be recorded 
    /// on in the quest system</param>
    /// <remarks>
    /// Call -> <c>GameManager.Instance.CompleteTask(GoalType.DigGrave);</c>
    /// </remarks>
    public void CompleteTask(GoalType _Type, BodyQuality _Quality)
    {
        _QuestManager.CompleteTask(_Type, _Quality);
    }

    /// <summary>
    /// When You talk with an NPC that turns in quests it they will reward you per quest 
    /// that meets the requirements.
    /// </summary>
    public void QuestTurnInOn()
    {
        _QuestManager.TurnInWindowOn();
    }

    /// <summary>
    /// When You talk with an NPC that turns in quests it they will reward you per quest 
    /// that meets the requirements.
    /// </summary>
    public void QuestTurnInOff()
    {
        _QuestManager.TurnInWindowOff();
    }

    /// <summary>
    /// When Called it will toggle the players hidden state
    /// </summary>
    /// <remarks>
    /// Call -> <c>GameManager.Instance.ToggleHidden();</c>
    /// </remarks>
    public void ToggleHidden()
    {
        _RunManager.ToggleHidden();
    }

    /// <summary>
    /// Will take the index of what quest will be added
    /// Then will be passed to the quest manager.
    /// </summary>
    /// <param name="_Index">The index of the quest in the game managers database</param>
    public void CreateQuest(int _Index)
    {
        _QuestManager.NewQuest(_Quests[_Index]);
    }
    /// <summary>
    /// Will take a new quest and
    /// Then will be passed to the quest manager.
    /// </summary>
    /// <param name="_NewQuest">Quests object that will be passed to the quest manager</param>
    public void CreateQuest(Quests _NewQuest)
    {
        _QuestManager.NewQuest(_NewQuest);
    }

    /// <summary>
    /// Will add a body to the list of bodies within the game manager.
    /// </summary>
    /// <param name="_NewBody">The new body that is to be added to the list of bodies</param>
    public void AddBody(Body _NewBody)
    {
        if (_NewBody != null)
        {
            _RunManager.AddBody(_NewBody);
            Inventory.UpdateInventory();
        }
    }

    /// <summary>
    /// Will take all the bodies from the run and add it to the total bodies that the player has.
    /// </summary>
    /// <param name="_NewBodies">The array of bodies from the run</param>
    public void MergeBodies(Body[] _NewBodies)
    {
        //Add each type of body into the list of body qualities.
        for(int i = 0; i < _NewBodies.Length; ++i)
        {
            BodyCollection.Add(_NewBodies[i]);
        }
    }

    /// <summary>
    /// Will take in the number of graves generated for a given run and apply it to the run manager.
    /// </summary>
    /// <param name="_Total">The total amount of graves generated.</param>
    public void UpdateGraveTotal(int _Total)
    {
        _RunManager.SetTotalGraves(_Total);
    }

    /// <summary>
    /// Will sell an amount of a certain quality of bodies.
    /// </summary>
    /// <param name="_Quality">The quality of body that will be sold</param>
    /// <param name="_Amount">The amount of a certain quality of body that will be sold</param>
    /// <returns>True if the <paramref name="_Amount"/> of bodies could be sold. False if <paramref name="_Amount"/> could not be sold.</returns>
    public bool SellBody(Body _Body)
    {
        int Decay = _Body.GetDecay();
        BodyQuality DecayEnum = _Body.GetQualityEnum();
        Debug.Log("I'm Selling a Body");
        int BodyCost = 0;
        switch (DecayEnum)
        {
            //the base sell price of bodies is 20 and then that is modfiied by there decay percent
            case BodyQuality.Skeleton:
                BodyCost += 1 + (40 * (Decay) / 100);  
                break;
            case BodyQuality.Decayed:
                BodyCost += (40 * (Decay) / 100); 
                break;
            case BodyQuality.SlightDecay:
                BodyCost += (40 * (Decay) / 100);  
                break;
            case BodyQuality.Fresh:
                BodyCost += (40 * (Decay) / 100);  
                break;
        }

        if (UpdateMoney(BodyCost))
        {
            BodyCollection.Remove(_Body);
            CompleteTask(GoalType.Bodies, DecayEnum);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Sell a specific treasure based on the index in the TreasureCollection list
    /// </summary>
    /// <param name="Index">The index of where the treasure is located</param>
    /// <returns>True if money was successfully added to the players inventory</returns>
    public bool SellTreasure(int Index)
    {
        return UpdateMoney(TreasureCollection[Index].GetPrice());
    }

    /// <summary>
    /// When called it will initialize the run variable, start the timer, and load the run and the correct UI.
    /// </summary>
    public void StartRun()
    {
        _RunManager.ResetRun();
        RunUI.SetActive(true);
        Debug.Log($"[{RunUI.name} Active State]: {RunUI.activeSelf}");
        CompleteTask(GoalType.Graveyards, BodyQuality.Fresh);
        //Load into the graveyard scene
        ChangeScene(GraveyardScene);
        _RunManager.StartTimer();
        //throw new System.ArgumentException("[Error] Not fully implemented : GameManager.cs : Line(316)");
    }

    /// <summary>
    /// Will Take all the information from the run and commit it to the game manager. Then it will load back to menu or courtyard.
    /// </summary>
    public void EndRun()
    {
        MergeBodies(_RunManager.GetBodies());
        _RunManager.StopTimer();
        StealthManager.Instance.ResetAlert();
        UIManager.Instance.DisableStealth();
        UIManager.Instance.CloseBribe();

        int flyersMade = 0;
        for (int i = 0; i < _QuestManager.QuestFlyers.Length && flyersMade < 4; i++)
        {
            if (_QuestManager.QuestFlyers[i].GetComponent<Button>().interactable == false)
            {
                _QuestManager.GenerateFlyer(i);
                flyersMade += 1;
            }
        }

        //Load back into courtyard or menu
        ChangeScene(ReturnScene);
        //throw new System.ArgumentException("[Error] Not fully implemented : GameManager.cs : Line(327)");
    }
}

