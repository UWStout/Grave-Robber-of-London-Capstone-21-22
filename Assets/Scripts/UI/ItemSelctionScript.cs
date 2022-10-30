/*
 * Author: Damian Link
 * Version: 4/20/22
 */
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Script to change the description based on what is being havered over
/// </summary>
public class ItemSelctionScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Spot in list the item is
    [SerializeField] private int SelectionSpot;
    // Text of description
    [SerializeField] private Text DescriptionText;
    // Button script that determins the item type being shown
    [SerializeField] private BuyButtonScriipt SellScript;

    /// <summary>
    /// change the description based on what is being havered over
    /// </summary>
    /// <param name="eventData">Event data</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        switch(SellScript.CurrentItem)
        {
            case SellingScreenScript.ScreenOptions.BodyBag:
                DescriptionText.text = GameManager.Instance._UpgradeManager.GetBodyBagsUpgrades()[SelectionSpot].GetDescription();
                break;
            case SellingScreenScript.ScreenOptions.Shovel:
                DescriptionText.text = GameManager.Instance._UpgradeManager.GetShovelUpgrades()[SelectionSpot].GetDescription();
                break;
            case SellingScreenScript.ScreenOptions.BoltCutters:
                DescriptionText.text = GameManager.Instance._UpgradeManager.GetBoltCuttersUpgrades()[SelectionSpot].GetDescription();
                break;
                
            case SellingScreenScript.ScreenOptions.Shoes:
                DescriptionText.text = GameManager.Instance._UpgradeManager.GetShoesUpgrades()[SelectionSpot].GetDescription();
                break;
        }        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        switch(SellScript.CurrentItem)
        {
            case SellingScreenScript.ScreenOptions.BodyBag:
                DescriptionText.text = GameManager.Instance._UpgradeManager.GetCurrentBodyBag().GetDescription();
            break;
            case SellingScreenScript.ScreenOptions.Shovel:
                DescriptionText.text = GameManager.Instance._UpgradeManager.GetCurrentShovel().GetDescription();
            break;
            case SellingScreenScript.ScreenOptions.BoltCutters:
                DescriptionText.text = GameManager.Instance._UpgradeManager.GetCurrentBolltCutters().GetDescription();
            break;
                
            case SellingScreenScript.ScreenOptions.Shoes:
                DescriptionText.text = GameManager.Instance._UpgradeManager.GetCurrentShoes().GetDescription();
            break;
        }
    }
}
