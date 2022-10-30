using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Joseph Vang
 * Version: 11.9.2021
 */
[System.Serializable]
[CreateAssetMenu(fileName ="Quest Description", menuName ="ScriptableObjects/New Quest", order = 2)]
public class Quests: ScriptableObject
{
	[SerializeField] private GoalType Tag;
	[SerializeField] private BodyQuality _BodyQuality;
	[SerializeField] private bool QuestIsActive;
	[SerializeField] private string Title;
	[SerializeField] private string Description;
	[SerializeField] private int Reward;
	[SerializeField] private int CurrentAmount;
	[SerializeField] private int RequiredAmount;

	/*
	 * Constructor
	 * 
	 * @param GoalType
	 * @param string
	 * 
	 */
	public Quests(GoalType Tag, BodyQuality Quality, string Title, string Description, int Reward, int Required,int CurrentAmount = 0,	 bool _Active = false)
	{
		this.Tag = Tag;
		this._BodyQuality = Quality;
		this.Title = Title;
		this.Description = Description;
		this.Reward = Reward;
		this.RequiredAmount = Required;
		this.QuestIsActive = _Active;
	}


	/*
	* Checks if current amount is less than or equal to the required amount
	* then returns a bool of true or false
	* 
	* @returns T or F
	*/
	public bool Evaluate()
	{
		return (CurrentAmount >= RequiredAmount);
	}


	/*
	 * 
	 *
	 * @param Title
	 */
	public void SetTitle(string Title)
	{
		this.Title = Title;
	}

	/*
	 * 
	 * 
	 * @param Description
	 */
	public void SetDescription(string Description)
	{
		this.Description = Description;
	}

	/*
	 * 
	 * 
	 * @param CurrentAmount
	 */
	public void SetCurrentAmount(int CurrentAmount)
	{
		this.CurrentAmount = CurrentAmount;
	}

	public void SetRequiredAmount(int _Required)
	{
		RequiredAmount = _Required;
	}
	public void SetReward(int _Reward)
	{
		Reward = _Reward;
	}

	/// <summary>
    /// Will set the quests active state based on the passed boolean
    /// </summary>
    /// <param name="_State">The true or false active state of the quest</param>
	public void SetActive(bool _State)
    {
		QuestIsActive = _State;
    }

	/// <summary>
    /// Getter to return the Tag value
    /// </summary>
    /// <returns>The tag as an int value</returns>
	public GoalType GetTag()
    {
		return Tag;
    }

	public BodyQuality GetBodyQuality()
	{
		return _BodyQuality;
	}

	/*
	 * 
	 * 
	 * @return Title
	 */
	public string GetTitle()
	{
		return Title;
	}

	/*
	 * 
	 * 
	 * @return Description
	 */
	public string GetDescription()
	{
		return Description;
	}

	/*
	 * 
	 * 
	 * @return Reward
	 */
	public int GetReward()
	{
		return Reward;
	}

	/*
	 * 
	 * 
	 * @return CurrentAmount
	 */
	public int GetCurrentAmount()
	{
		return CurrentAmount;
	}

	/*
	 * 
	 * 
	 * @return RequiredAmount
	 */
	public int GetRequiredAmount()
	{
		return RequiredAmount;
	}
	
	/// <summary>
    /// The active state of the quest
    /// </summary>
    /// <returns>true if the quest is active</returns>
	public bool GetActive()
    {
		return QuestIsActive;
    }

	public Quest GetPrimType()
    {
		return new Quest(Tag, _BodyQuality, Title, Description, Reward, CurrentAmount, RequiredAmount, QuestIsActive);
    }
}

[System.Serializable]
public class Quest
{
	[SerializeField] public GoalType Tag;
	[SerializeField] public BodyQuality _BodyQuality;
	[SerializeField] public bool QuestIsActive;
	[SerializeField] public string Title;
	[SerializeField] public string Description;
	[SerializeField] public int Reward;
	[SerializeField] public int CurrentAmount;
	[SerializeField] public int RequiredAmount;

	public Quest(GoalType _Tag, BodyQuality Quality, string _Title, string _Description, int _Reward, int _CurrentAmount, int _RequiredAmount, bool _QuestIsActive)
    {
		Tag = _Tag;
		_BodyQuality = Quality;
		Title = _Title;
		Description = _Description;
		Reward = _Reward;
		CurrentAmount = _CurrentAmount;
		RequiredAmount = _RequiredAmount;
		QuestIsActive = _QuestIsActive;
	}

	/*
* Checks if current amount is less than or equal to the required amount
* then returns a bool of true or false
* 
* @returns T or F
*/
	public bool Evaluate()
	{
		return (CurrentAmount >= RequiredAmount);
	}


	/*
	 * 
	 *
	 * @param Title
	 */
	public void SetTitle(string Title)
	{
		this.Title = Title;
	}

	/*
	 * 
	 * 
	 * @param Description
	 */
	public void SetDescription(string Description)
	{
		this.Description = Description;
	}

	/*
	 * 
	 * 
	 * @param CurrentAmount
	 */
	public void SetCurrentAmount(int CurrentAmount)
	{
		this.CurrentAmount = CurrentAmount;
	}

	public void SetRequiredAmount(int _Required)
	{
		RequiredAmount = _Required;
	}
	public void SetReward(int _Reward)
	{
		Reward = _Reward;
	}

	/// <summary>
	/// Will set the quests active state based on the passed boolean
	/// </summary>
	/// <param name="_State">The true or false active state of the quest</param>
	public void SetActive(bool _State)
	{
		QuestIsActive = _State;
	}

	/// <summary>
	/// Getter to return the Tag value
	/// </summary>
	/// <returns>The tag as an int value</returns>
	public GoalType GetTag()
	{
		return Tag;
	}

	/// <summary>
	/// Getter to return the Quality value
	/// </summary>
	/// <returns></returns>
	public BodyQuality GetBodyQuality()
	{
		return _BodyQuality;
	}

	/*
	 * 
	 * 
	 * @return Title
	 */
	public string GetTitle()
	{
		return Title;
	}

	/*
	 * 
	 * 
	 * @return Description
	 */
	public string GetDescription()
	{
		return Description;
	}

	/*
	 * 
	 * 
	 * @return Reward
	 */
	public int GetReward()
	{
		return Reward;
	}

	/*
	 * 
	 * 
	 * @return CurrentAmount
	 */
	public int GetCurrentAmount()
	{
		return CurrentAmount;
	}

	/*
	 * 
	 * 
	 * @return RequiredAmount
	 */
	public int GetRequiredAmount()
	{
		return RequiredAmount;
	}

	/// <summary>
	/// The active state of the quest
	/// </summary>
	/// <returns>true if the quest is active</returns>
	public bool GetActive()
	{
		return QuestIsActive;
	}
}
