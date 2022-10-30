using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Author: Joseph Vang
 * Version: 11.11.2021
 */
[System.Serializable]
public enum GoalType
{
	Bodies,
	Minigame,
	Graveyards,
	Treasures
}

[System.Serializable]
public class QuestManager : MonoBehaviour
{
	[Header("GameObjects")]
	public GameObject QuestJournal;
	public GameObject FlyerParent;
	public GameObject QuestAcceptWindow;
	public GameObject QuestButtonParent;
	public GameObject QuestButtonPrefab;
	public GameObject TurnInButton;

	[Header("Accept Quest Window")]
	public TextMeshProUGUI AcceptTitleText;
	public TextMeshProUGUI AcceptDescriptionText;
	public TextMeshProUGUI AcceptRewardText;
	public TextMeshProUGUI AcceptRequiredTasks;

	[Header("Quest Journal Info")]
	public TextMeshProUGUI JournalTitleText;
	public TextMeshProUGUI JournalDescriptionText;
	public TextMeshProUGUI JournalMoneyText;
	public TextMeshProUGUI JournalTasksCompleted;
	public TextMeshProUGUI JournalRewardText;
	public TextMeshProUGUI JournalCurrencyName;

	[Header("Variables")]
	private int CurrentOpenQuest = 0;
	private int PendingQuestIndex = 0;

	[Header("Lists")] 
	[SerializeField] private Quest[] QuestList;
	private GameObject[] Buttons;
	public GameObject[] QuestFlyers;
	private Quest PendingQuest;

	[Header("Databases")]
	[SerializeField] private Quests[] QuestDatabase;

    private void Start()
    {
	    QuestList = new Quest[6];
	    Buttons = new GameObject[6];
        QuestFlyers = new GameObject[FlyerParent.transform.childCount - 1];
		for (int i = 0; i < FlyerParent.transform.childCount - 1; i++)
        {
			QuestFlyers[i] = FlyerParent.transform.GetChild(i).gameObject;
        }
	}

    /// <summary>
    /// When called the quest journal UI will be toggled on or off and the text of the currently completed will be updated.
    /// </summary>
    public void ToggleUI()
    {
		QuestJournal.SetActive(!QuestJournal.activeSelf);
		if(QuestList.Length > 0 && Buttons.Length > 0 && QuestList[CurrentOpenQuest] != null)
		{
			JournalTasksCompleted.text = $"{QuestList[CurrentOpenQuest].GetCurrentAmount()}/{QuestList[CurrentOpenQuest].GetRequiredAmount()}";
		}
	}

	/// <summary>
	/// Opens the quest turn in window
	/// </summary>
	public void TurnInWindowOn()
    {
		TurnInButton.SetActive(true);
		ToggleUI();
    }

	/// <summary>
	/// Closes the quest turn in window
	/// </summary>
	public void TurnInWindowOff()
	{
		TurnInButton.SetActive(false);
		ToggleUI();
	}

	/// <summary>
	/// A getter to get whether the Journal UI is currently being displayed.
	/// </summary>
	/// <returns>Will return true if journal is shown and false if it is hidden.</returns>
	public bool UIState()
    {
		return QuestJournal.activeSelf;
    }

	/// <summary>
	/// Updates the UI in the Quest Journal so that when a quest is clicked on in there 
	/// it will change all of the information regarding that specific quest
	/// </summary>
	/// <param name="index"></param>
	/// <remarks>
	/// Input: Data from a specific location in QuestList
	/// Output: Stored data replaces text in the Quest Journal GUI
	/// </remarks>
	void UpdateUI(int index)
	{
		//Takes a instance of the quest at a specific index
		Quest CurrentQuest = QuestList[index];
		Debug.Log($"[{index}] - Title: {CurrentQuest.GetTitle()}, Description: {CurrentQuest.GetDescription()}");
		//Update the UI for another quest
		JournalTitleText.text = CurrentQuest.GetTitle();
		JournalDescriptionText.text = CurrentQuest.GetDescription();
		JournalTasksCompleted.text = $"{CurrentQuest.GetCurrentAmount()}/{CurrentQuest.GetRequiredAmount()}";
		JournalRewardText.text = $"Rewards:";
		JournalMoneyText.text = CurrentQuest.GetReward().ToString();
		JournalCurrencyName.text = $"Pounds";
		CurrentOpenQuest = index;
		
	}

	/// <summary>
	/// Takes information from a ScriptableObject Quests, updates the text in a NPC's quest
	/// window and then displays that window.
	/// </summary>
	/// <param name="_Quest"></param>
	/// <remarks>
	/// Input: Data from a ScriptableObject Quests
	/// Output: Stored data replaces texts in the NPC Quest Window GUI
	/// </remarks>
	public void NewQuest(Quests _Quest)
	{
		//Create New instance of the quest
		PendingQuest = new Quest(_Quest.GetTag(), _Quest.GetBodyQuality(), _Quest.GetTitle(), _Quest.GetDescription(), _Quest.GetReward(), 0, _Quest.GetRequiredAmount(), true);
		//Update the UI for the accept quest.
		AcceptTitleText.text = PendingQuest.GetTitle();
		AcceptDescriptionText.text = PendingQuest.GetDescription();
		AcceptRewardText.text = PendingQuest.GetReward().ToString();
		AcceptRequiredTasks.text = PendingQuest.GetRequiredAmount().ToString();
		//Turn on the UI for accepting a quest
		QuestAcceptWindow.SetActive(true);

	}

	/// <summary>
	/// Will generate a flyer from the array of scriptable objects
	/// </summary>
	/// <param name="FlyerIndex"></param>
	public void GenerateFlyer(int FlyerIndex)
    {
		Quests[] _Quests = GetAllScriptableQuests();

		int randomIndex = Random.Range(1, 1000);
		randomIndex = randomIndex % QuestDatabase.Length;
		UpdateFlyerUI(FlyerIndex, QuestDatabase[randomIndex]);
	}

	/// <summary>
	/// Updates an empty button to show an new quest and adds a listener that places a new scriptable quest
	/// onto that button and then sets it to let the player use the button
	/// </summary>
	/// <param name="FlyerIndex"></param>
	/// <param name="_Quest"></param>
	void UpdateFlyerUI(int FlyerIndex, Quests _Quest)
	{
		// Updates the UI on th button to place the title of a new quest on it
		QuestFlyers[FlyerIndex].GetComponentInChildren<TextMeshProUGUI>().text = _Quest.GetTitle();
		// Adds listeners to the button 
		QuestFlyers[FlyerIndex].GetComponent<Button>().onClick.AddListener(() => NewQuest(_Quest));
		QuestFlyers[FlyerIndex].GetComponent<Button>().onClick.AddListener(() => SelectedFlyer(FlyerIndex));
		// Makes the button interactable again with a different quest
		QuestFlyers[FlyerIndex].GetComponent<Button>().interactable = true;
	}

	/// <summary>
	/// When the quest is accepted it will not let the player click on that button again
	/// clearing the title of the quest and removing all listeners on the button
	/// </summary>
	/// <param name="FlyerIndex"></param>
	private void ClearFlyerUI(int FlyerIndex)
    {
		// Makes the button not interactable
		QuestFlyers[FlyerIndex].GetComponent<Button>().interactable = false;
		// Clear the UI information removing all listeners on the button
		QuestFlyers[FlyerIndex].GetComponentInChildren<TextMeshProUGUI>().text = "";
		QuestFlyers[FlyerIndex].GetComponent<Button>().onClick.RemoveAllListeners();
	}

	/// <summary>
	/// Stores the index of the current button clicked on the notice board
	/// </summary>
	/// <param name="FlyerIndex"></param>
	public void SelectedFlyer(int FlyerIndex)
    {
		PendingQuestIndex = FlyerIndex;
    }

	/// <summary>
	/// Closes the quest accept window if the player doesn't want to take the quest
	/// </summary>
	public void DeclineQuest()
    {
		QuestAcceptWindow.SetActive(false);
    }

	/// <summary>
	/// Adds the a new quest information to the QuestList then creates a button in 
	/// the Quest Journal taking the name of the newly added quest
	/// </summary>
	/// <remarks>
	/// Input: Title of the most recent accepted quest
	/// Output: A generated button with the title of most recent quest
	/// </remarks>
	public void AcceptQuest()
	{
		//Turns off the accept quest UI
		QuestAcceptWindow.SetActive(false);

		//Add to list of buttons and give it a listener to a method that is called when clicked
		for (int i = 0; i < Buttons.Length; i++)
		{
			if (Buttons[i] == null)
			{
				//Create a new button for the journal UI
				GameObject NewButton = (GameObject)Instantiate(QuestButtonPrefab, QuestButtonParent.transform);

				Buttons[i] = NewButton;
				QuestList[i] = PendingQuest;
				Buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = PendingQuest.GetTitle();
				Buttons[i].GetComponent<Button>().onClick.AddListener(() => UpdateUI(i));
				ClearFlyerUI(PendingQuestIndex);
				break;
			}
		}
	}

	/// <summary>
	/// Checks each quests in the Quest Journal to see if it is completed. It will determine what type of quest it is
	/// and if it has been accepted. If it is it will calculate if it meets the requirement incrementing it if it doesn't.
	/// </summary>
	/// <param name="Type"></param>
	/// <param name="Decay"></param>
	/// <remarks>
	/// Input: A GoalType enum
	/// Output: A completed and removed quest and quest button
	/// </remarks>
	public void CompleteTask(GoalType Type, BodyQuality Decay = 0)
	{
		for(int i = 0; i < QuestList.Length; i++)
		{
			if (QuestList[i] != null && QuestList[i].GetTag() == Type && QuestList[i].GetActive() == true)
			{
				if (QuestList[i].GetTag() == GoalType.Bodies)
				{
					if (Decay == QuestList[i].GetBodyQuality())
					{
						QuestList[i].SetCurrentAmount(QuestList[i].GetCurrentAmount() + 1);
					}
				}
				else
				{
					QuestList[i].SetCurrentAmount(QuestList[i].GetCurrentAmount() + 1);
				}
			}
		}
	}

    /// <summary>
    /// When the requirement is ment it will add the reward to your currency and mark the quest complete 
	/// when you go to turn it in at the npc
    /// </summary>
    public void TurnInQuest()
    {

		if (QuestList[CurrentOpenQuest].Evaluate())
        {
			GameManager.Instance.UpdateMoney(QuestList[CurrentOpenQuest].GetReward());
            QuestList[CurrentOpenQuest] = null;
			Destroy(Buttons[CurrentOpenQuest]);
        }
    }

    /// <summary>
    /// Getter to retrieve the Quest list
    /// </summary>
    /// <returns>The quest list as an array of Quests</returns>
    public Quest[] GetQuests()
    {
		return QuestList;
    }

	/// <summary>
    /// Setter to store an array of quest into the quest list
    /// </summary>
    /// <param name="_quests"></param>
	public void SetQuests(Quest[] _Quests)
    {
		if (_Quests != null)
		{
			QuestList = _Quests;
		}
    }

	/// <summary>
	/// Takes in a name parameter and creates an array of all scriptable quest objects and checks to see if 
	/// it matches with that name and then returns the quest with that name.
	/// </summary>
	/// <param name="_Name"></param>
	/// <returns></returns>
	private Quests GetScriptableQuest(string _Name)
	{
		Quests[] _Quests = Resources.FindObjectsOfTypeAll<Quests>();
		foreach(Quests _Quest in _Quests)
		{
			if (_Quest.GetTitle() == _Name)
			{
				return _Quest;
			}
		}
		return null;
	}

	/// <summary>
	/// Grabs all quest scriptable objects
	/// </summary>
	/// <returns></returns>
	private Quests[] GetAllScriptableQuests()
	{
		return Resources.FindObjectsOfTypeAll<Quests>();
	}
}
