/*
 * Author: Declin Anderson
 * Version: 17.0.0
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the controller for the Inventory UI
/// </summary>
public class InventoryBag : MonoBehaviour
{
    public GameObject InventorySlot;
    public GameObject ScrollBar;
    public GameObject HostObject;
    public GameObject DiscardBodyButton;

    int SpotToRemove = 0;
    int NextFreeSlot = 0;

    // Start is called before the first frame update
    void Start()
    {
        Populate();
        HostObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// This updates the bag to reset all of the values in it and to clear it out
    /// Also resets the variable that counts where to put the next body picked up
    /// </summary>
    public void SoldInventory()
    {
        for(int i = 0; i < GameManager.Instance._UpgradeManager.GetCurrentBodyBag().GetMaxBodies(); i++)
        {
            transform.GetChild(i).GetChild(1).GetComponent<Button>().enabled = false;
            transform.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "";
            transform.GetChild(i).GetChild(0).GetChild(2).gameObject.GetComponent<Image>().material = null;
        }
        NextFreeSlot = 0;
        DiscardBodyButton.SetActive(false);
    }

    /// <summary>
    /// This updates the bag to add in a new body that was just picked up 
    /// then it updates the variable to count the current position of the next body
    /// </summary>
    public void UpdateInventory()
    {
        int RunManagerPosition = GameManager.Instance._RunManager.GetBodies().Length-1;
        if (NextFreeSlot < 0)
        {
            NextFreeSlot = 0;
        }
        Debug.Log(NextFreeSlot);
        GameObject UpdatedSlot = gameObject.transform.GetChild(NextFreeSlot).gameObject;
        int tempDecay = GameManager.Instance._RunManager.GetBodies()[RunManagerPosition].GetDecay();
        UpdatedSlot.transform.GetChild(1).GetComponent<Button>().enabled = true;
        int TempSlot = NextFreeSlot;
        UpdatedSlot.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => BodyToRemove(TempSlot));
        UpdatedSlot.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Freshness: " + tempDecay + "%";
        UpdatedSlot.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Image>().material = GameManager.Instance._RunManager.GetBodies()[RunManagerPosition].GetMaterial();
        NextFreeSlot++;
    }

    /// <summary>
    /// Dynamically generates the bag to include the size of the current body bag and then update to keep any current bodies in the bag as well
    /// </summary>
    public void Populate()
    {
        for(int i = gameObject.transform.childCount; i < GameManager.Instance._UpgradeManager.GetCurrentBodyBag().GetMaxBodies(); i++)
        {
            GameObject newObj;
            newObj = (GameObject)Instantiate(InventorySlot, transform);
            if (GameManager.Instance.GetBodyCollection().Length > i)
            {
                int tempDecay = GameManager.Instance.GetBodyCollection()[i].GetDecay();
                newObj.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Freshness: " + tempDecay + "%";
                newObj.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Image>().material = GameManager.Instance.GetBodyCollection()[i].GetMaterial();
            }
            newObj.transform.GetChild(1).GetComponent<Button>().enabled = false;
        }
        ScrollBar.GetComponent<Scrollbar>().value = 1;
    }

    /// <summary>
    /// Call when the discard body button is pressed, remvoes the body that was selected
    /// </summary>
    /// <param name="BodyRemoved">Body slot that will have its body removed</param>
    public void DiscardBody()
    {
        int SpotIndex = 0;
        if(SpotToRemove >= GameManager.Instance.GetBodyCollection().Length)
        {
            SpotIndex = SpotToRemove - GameManager.Instance.GetBodyCollection().Length;
            //Debug.Log(SpotIndex);
            GameManager.Instance._RunManager.RemoveBody(SpotIndex);
        }
        else
        {
            SpotIndex = SpotToRemove;
            GameManager.Instance.ReturnBodyList().RemoveAt(SpotIndex);
        }
        ResetInventory();
        NextFreeSlot--;
        DiscardBodyButton.SetActive(false);
    }

    /// <summary>
    /// Resets the inventory to make sure the inventory is set right
    /// </summary>
    void ResetInventory()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "";
            transform.GetChild(i).GetChild(0).GetChild(2).gameObject.GetComponent<Image>().material = null;
            transform.GetChild(i).GetChild(1).gameObject.GetComponent<Button>().enabled = false;
        }
        for (int i = 0; i < GameManager.Instance.GetBodyCollection().Length; i++)
        {
            transform.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Freshness: " + GameManager.Instance.GetBodyCollection()[i].GetDecay() + "%";
            transform.GetChild(i).GetChild(0).GetChild(2).gameObject.GetComponent<Image>().material = GameManager.Instance.GetBodyCollection()[i].GetMaterial();
            transform.GetChild(i).GetChild(1).gameObject.GetComponent<Button>().enabled = true;

        }
        for (int i = 0; i < GameManager.Instance._RunManager.GetBodies().Length; i++)
        {
            transform.GetChild(i + GameManager.Instance.GetBodyCollection().Length).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Freshness: " + GameManager.Instance._RunManager.GetBodies()[i].GetDecay() + "%";
            transform.GetChild(i + GameManager.Instance.GetBodyCollection().Length).GetChild(0).GetChild(2).gameObject.GetComponent<Image>().material = GameManager.Instance._RunManager.GetBodies()[i].GetMaterial();
            transform.GetChild(i + GameManager.Instance.GetBodyCollection().Length).GetChild(1).gameObject.GetComponent<Button>().enabled = true;
        }
    }

    /// <summary>
    /// Updates the int that tells the discard which body to remove
    /// </summary>
    /// <param name="SelectedSlot">Button Pressed</param>
    public void BodyToRemove(int SelectedSlot)
    {
        SpotToRemove = SelectedSlot;
        DiscardBodyButton.SetActive(true);
    }
}
