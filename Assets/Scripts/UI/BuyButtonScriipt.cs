using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script to control buttons on buying UI
/// </summary>
public class BuyButtonScriipt : MonoBehaviour
{
    // Current item type that buttons should set
    public SellingScreenScript.ScreenOptions CurrentItem;
    public SellingScreenScript SellScript;

    /// <summary>
    /// Buying an item
    /// </summary>
    /// <param name="ButtonNumber">Number of item in list to unlock</param>
    public void BuyItem (int ButtonNumber)
    {
        switch (CurrentItem)
        {
            case (SellingScreenScript.ScreenOptions.Shovel):
                GameManager.Instance._UpgradeManager.UpgradeShovel(ButtonNumber);
                break;
            case (SellingScreenScript.ScreenOptions.BoltCutters):
                GameManager.Instance._UpgradeManager.UpgradeBoltCutter(ButtonNumber);
                break;
            case (SellingScreenScript.ScreenOptions.BodyBag):
                GameManager.Instance._UpgradeManager.UpgradeBodyBag(ButtonNumber);
                break;
            case (SellingScreenScript.ScreenOptions.Shoes):
                GameManager.Instance._UpgradeManager.UpgradeShoes(ButtonNumber);
                break;
        }
    }

    /// <summary>
    /// Equips an upgrade
    /// </summary>
    /// <param name="ButtonNumber">Number of item in list to unlock</param>
    public void EquipItem (int ButtonNumber)
    {
        switch (CurrentItem)
        {
            case (SellingScreenScript.ScreenOptions.Shovel):
                GameManager.Instance._UpgradeManager.SetCurrentShovel(ButtonNumber);
                SellScript.SetupScreen((int)SellingScreenScript.ScreenOptions.Shovel);
                break;
            case (SellingScreenScript.ScreenOptions.BoltCutters):
                GameManager.Instance._UpgradeManager.SetCurrentBoltCutter(ButtonNumber);
                SellScript.SetupScreen((int)SellingScreenScript.ScreenOptions.BoltCutters);
                break;
            case (SellingScreenScript.ScreenOptions.BodyBag):
                GameManager.Instance._UpgradeManager.SetCurrentBodyBag(ButtonNumber);
                SellScript.SetupScreen((int)SellingScreenScript.ScreenOptions.BodyBag);
                break;
            case (SellingScreenScript.ScreenOptions.Shoes):
                GameManager.Instance._UpgradeManager.SetCurrentShoes(ButtonNumber);
                SellScript.SetupScreen((int)SellingScreenScript.ScreenOptions.Shoes);
                break;
        }
    }
}
