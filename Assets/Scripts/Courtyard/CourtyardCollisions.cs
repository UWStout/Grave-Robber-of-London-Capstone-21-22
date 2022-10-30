using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Joseph Vang
/// </summary>
public class CourtyardCollisions : MonoBehaviour
{
	public FMOD.Studio.EventInstance Shopkeeper;
	[Header("Notice Board")]
	private GameObject PlayerUI;
	public bool Interact = false;
	bool inside = false;
	bool LetsSell = false;

	/// <summary>
	/// Upon entering the scene, the notice board will search for the game object of the name "Notice Board UI"
	/// </summary>
    void Awake()
    {
		PlayerUI = GameObject.Find("PlayerUI");
    }

    private void Update()
    {
			// When the interact button is pressed it checks what the object it is for conditions
		if (Input.GetButtonDown("Interact") && inside)
		{
			Interact = true;
		}
	}

	/// <summary>
	/// While in range of the range of a collider with a specified tag the player can press space or e to
	/// interact with that object. If they are done they can press escape to exit out of it.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerStay(Collider other)
	{
		inside = true;
		if (Interact)
        {
			Interact = false;
			// If the object this script is the Notice Board then it runs through this
			if (this.gameObject.CompareTag("NoticeBoard"))
			{
				if (PlayerUI.transform.GetChild(2).transform.GetChild(1).gameObject.activeSelf == false)
				{
					PlayerUI.GetComponent<UIManager>().BoardOn();
					PlayerUI.GetComponent<UIManager>().InventoryClosed();
				}
				else if (PlayerUI.transform.GetChild(2).transform.GetChild(1).gameObject.activeSelf == true)
					PlayerUI.GetComponent<UIManager>().BoardOff();
			}
			// If the object this script is the Deliverer then it runs through this
			else if (this.gameObject.name == "SellerNPC")
            {
				PlayShopkeeper();
				if (PlayerUI.transform.GetChild(12).gameObject.activeSelf == false)
				{
					PlayerUI.transform.GetChild(12).gameObject.SetActive(true);
					PlayerUI.GetComponent<UIManager>().InventoryClosed();
				}
				else if (PlayerUI.transform.GetChild(12).gameObject.activeSelf == true)
					PlayerUI.transform.GetChild(12).gameObject.SetActive(false);
			}
			// If the object this script is the Mission NPC then it runs through this
			else if (this.gameObject.name == "MissionNPC")
			{
				if (PlayerUI.transform.GetChild(6).gameObject.activeSelf == false)
				{
					PlayerUI.GetComponent<UIManager>().MissionOn();
					PlayerUI.GetComponent<UIManager>().InventoryClosed();
				}
				else if (PlayerUI.transform.GetChild(6).gameObject.activeSelf == true)
					PlayerUI.GetComponent<UIManager>().MissionOff();
			}
		}
	}

	/// <summary>
	/// When the player leaves the range of the collider 
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			//InteractIndicator.SetActive(false);
			inside = false;
			Interact = false;
			PlayerUI.GetComponent<UIManager>().MissionOff();
			PlayerUI.GetComponent<UIManager>().BoardOff();
			PlayerUI.transform.GetChild(12).gameObject.SetActive(false);
			PlayerUI.transform.GetChild(11).gameObject.SetActive(false);
			if (PlayerUI.transform.GetChild(3).gameObject.activeSelf)
			{
				GameManager.Instance.QuestTurnInOff();
			}
		}
	}
	void PlayShopkeeper()
    {
		Shopkeeper = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Shopkeeper");
	    Shopkeeper.start();
		Shopkeeper.release();
    }
}
