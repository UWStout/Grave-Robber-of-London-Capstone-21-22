/*
 * Author: Declin Anderson
 * Version: 17.0.0
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This controls the UI for the Sales screen of corspes
/// </summary>
public class SalesInvoice : MonoBehaviour
{
    public GameObject Sale;
    public Text Total;

    /// <summary>
    /// This sells the bodies to the vendor which then will move the info the sales invoice
    /// </summary>
    public void SellBodiesToVendor()
    {
        int AmountMade = 0;
        int StartAmount = GameManager.Instance.GetMoney();
        GenerateSalesInvoice();
        List<Body> Bodies = GameManager.Instance.ReturnBodyList();
        List<Treasure> Treasure = GameManager.Instance.RetunrTreasure();
        if (Bodies.Count > 0)
        {
            for (int i = Bodies.Count - 1; i >= 0; i--)
            {
                GameManager.Instance.SellBody(Bodies[i]);
            }
            GameManager.Instance.Inventory.SoldInventory();
        }
        int EndAmount = GameManager.Instance.GetMoney();

        AmountMade = EndAmount - StartAmount;
        GameObject.Find("TotalValue").GetComponent<Text>().text = "Total Earned: £" + AmountMade;
    }

    /// <summary>
    /// Populates the UI for the Sales Invoice
    /// </summary>
    public void GenerateSalesInvoice()
    {
        GameObject newObj;

        for (int i = 0; i < GameManager.Instance.GetBodyCollection().Length; i++)
        {
            newObj = (GameObject)Instantiate(Sale, transform);
            if (GameManager.Instance.GetBodyCollection().Length > i)
            {
                int tempDecay = GameManager.Instance.ReturnBodyList()[i].GetDecay();
                newObj.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Freshness: " + tempDecay + "%";
                if(GameManager.Instance.ReturnBodyList()[i].GetQualityEnum() == BodyQuality.Skeleton)
                    newObj.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Value: £ " + (1 + (40 * (tempDecay) / 100));
                else
                    newObj.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Value: £ " + (40 * (tempDecay) / 100);
            }
        }
        for (int i = 0; i < GameManager.Instance.GetTreasureCollection().Length; i++)
        {
            newObj = (GameObject)Instantiate(Sale, transform);
            if (GameManager.Instance.RetunrTreasure().Count > i)
            {
                newObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Treasure: ";
                newObj.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Name: " + GameManager.Instance.GetTreasureCollection()[i].GetName();
                newObj.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Value: £ " + GameManager.Instance.GetTreasureCollection()[i].GetPrice();
            }
        }
    }

    /// <summary>
    /// Destroys all of the sales in the invoice when closed
    /// </summary>
    public void DestroySales()
    {
        Debug.Log(transform.childCount);
        if (transform.childCount > 0)
        {
            for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
