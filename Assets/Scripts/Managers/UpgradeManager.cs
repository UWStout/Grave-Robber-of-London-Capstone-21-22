/*
 * Author: Damian Link
 * Version: 2/8/2022
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the upgrade system of buying tools
/// </summary>
public class UpgradeManager : MonoBehaviour
{

    //-------------------------
    // Varables
    //--------------------------
    [Header("Control")]
    [SerializeField] private bool FreeUpgrades = false;
    [SerializeField] private SellingScreenScript SellScreen;

    [Header("Available Upgrades")]
    // Shovels that are available
    [SerializeField] private List<Shovel> ShovelUpgrades;
    // Shoes that are availabe
    [SerializeField] private List<Shoes> ShoesUpgrades;
    // Bolt cutters that are available
    [SerializeField] private List<BoltCutters> BoltCutterUpgrades;
    // Body Bags that are available
    [SerializeField] private List<BodyBag> BodyBagsUpgrades;

    [Header("Equipped Upgrades")]
    // Current Shovel
    [SerializeField] private int CurrentShovel = 0;
    // Current Shoes
    [SerializeField] private int CurrentShoes = 0;
    // Current Boltcutters
    [SerializeField] private int CurrentBoltCutters = 0;
    // Current BodyBag
    [SerializeField] private int CurrentBodyBag = 0;


    //-------------------------------
    // Functions
    //-------------------------------

    /// <summary>
    /// Upgrades the shovel level to the given level
    /// </summary>
    /// <param name="ShovelLevel"> The Level of the shovel to equipt</param>
    public void UpgradeShovel(int ShovelLevel = 0)
    {
        if (ShovelLevel >= 0 && ShovelLevel < ShovelUpgrades.Count)
        {
            if (ShovelUpgrades[ShovelLevel].GetCost() <= GameManager.Instance.GetMoney() || FreeUpgrades == true)
            {
                if (FreeUpgrades == false)
                {
                    GameManager.Instance.UpdateMoney(-ShovelUpgrades[ShovelLevel].GetCost());
                }  
                CurrentShovel = ShovelLevel;
                for (int i = ShovelLevel; i  >= 0; i--)
                {
                    ShovelUpgrades[i].SetOwned(true);
                }
                
                SellScreen.SetupScreen((int)SellingScreenScript.ScreenOptions.Shovel);
            } else
            {
                SellScreen.NotEnoughMoneyTalk();
                //Debug.LogError("Not Enough Money");
            }
        } else
        {
            Debug.LogError("Shovel Level outside Bounds " + ShovelLevel);
        } 
    }

    /// <summary>
    /// Upgrades the shoes level to the given level
    /// </summary>
    /// <param name="ShoeLevel"> The Level of the shoes to equipt</param>
    public void UpgradeShoes(int ShoeLevel)
    {
        if (ShoeLevel >= 0 && ShoeLevel < ShoesUpgrades.Count)
        {
            if (ShoesUpgrades[ShoeLevel].GetCost() <= GameManager.Instance.GetMoney() || FreeUpgrades == true)
            {
                if (FreeUpgrades == false)
                {
                    GameManager.Instance.UpdateMoney(-ShoesUpgrades[ShoeLevel].GetCost());
                }
                CurrentShoes = ShoeLevel;
                for (int i = ShoeLevel; i >= 0; i--)
                {
                    ShoesUpgrades[i].SetOwned(true);
                }
                SellScreen.SetupScreen((int)SellingScreenScript.ScreenOptions.Shoes);

            }
            else
            {
                SellScreen.NotEnoughMoneyTalk();
                //Debug.LogError("Not Enough Money");
            }
        }
        else
        {
            Debug.LogError("Shoes Level outside Bounds " + ShoeLevel);
        }
    }

    /// <summary>
    /// Upgrades the boltcutters level to the given level
    /// </summary>
    /// <param name="BoltCuttersLevel"> The Level of the boltcutters to equipt</param>
    public void UpgradeBoltCutter(int BoltCuttersLevel)
    {
        if (BoltCuttersLevel >= 0 && BoltCuttersLevel < BoltCutterUpgrades.Count)
        {
            if (BoltCutterUpgrades[BoltCuttersLevel].GetCost() <= GameManager.Instance.GetMoney() || FreeUpgrades == true)
            {
                if (FreeUpgrades == false)
                {
                    GameManager.Instance.UpdateMoney(-BoltCutterUpgrades[BoltCuttersLevel].GetCost());
                }
                CurrentBoltCutters = BoltCuttersLevel;
                for (int i = BoltCuttersLevel; i >= 0; i--)
                {
                    BoltCutterUpgrades[i].SetOwned(true);
                }
                
                SellScreen.SetupScreen((int)SellingScreenScript.ScreenOptions.BoltCutters);
            }
            else
            {
                SellScreen.NotEnoughMoneyTalk();
                //Debug.LogError("Not Enough Money");
            }
        }
        else
        {
            Debug.LogError("Cutters Level outside Bounds " + BoltCuttersLevel);
        }
    }

    /// <summary>
    /// Upgrades the body bag level to the given level
    /// </summary>
    /// <param name="BodyBagLevel"> The Level of the body bag to equipt</param>
    public void UpgradeBodyBag(int BodyBagLevel)
    {
        if (BodyBagLevel >= 0 && BodyBagLevel < BodyBagsUpgrades.Count)
        {
            if (BodyBagsUpgrades[BodyBagLevel].GetCost() <= GameManager.Instance.GetMoney() || FreeUpgrades == true)
            {
                if (FreeUpgrades == false)
                {
                    GameManager.Instance.UpdateMoney(-BodyBagsUpgrades[BodyBagLevel].GetCost());
                }
                CurrentBodyBag = BodyBagLevel;
                for (int i = BodyBagLevel; i >= 0; i--)
                {
                    BodyBagsUpgrades[i].SetOwned(true);
                }
                SellScreen.SetupScreen((int)SellingScreenScript.ScreenOptions.BodyBag);
                //Debug.Log(GetCurrentBodyBag().GetName());
                GameObject.Find("InventoryButton").GetComponent<Image>().sprite = GetCurrentBodyBag().GetIcon();
            }
            else
            {
                SellScreen.NotEnoughMoneyTalk();
                //Debug.LogError("Not Enough Money");
            }
        }
        else
        {
            Debug.LogError("Shoes Level outside Bounds " + BodyBagLevel);
        }
        GameManager.Instance.Inventory.Populate();
    }

    //-------------------------------
    // Getters
    //-------------------------------

    /// <summary>
    /// Gets the Current Shovel
    /// </summary>
    /// <returns>Current Shovel in use</returns>
    public Shovel GetCurrentShovel ()
    {
        return ShovelUpgrades[CurrentShovel];
    }

    /// <summary>
    /// Gets the list of shovel upgrades
    /// </summary>
    /// <returns>List of shovel upgrades</returns>
    public List<Shovel> GetShovelUpgrades ()
    {
        return ShovelUpgrades;
    }

    /// <summary>
    /// Gets the Current Shoes
    /// </summary>
    /// <returns>Current Shoes in use</returns>
    public Shoes GetCurrentShoes ()
    {
        return ShoesUpgrades[CurrentShoes];
    }

    /// <summary>
    /// Gets the list of shoe upgrades
    /// </summary>
    /// <returns>List of shoe upgrades</returns>
    public List<Shoes> GetShoesUpgrades ()
    {
        return ShoesUpgrades;
    }

    /// <summary>
    /// Gets the Current BoltCutters
    /// </summary>
    /// <returns>Current BoltCutters in use</returns>
    public BoltCutters GetCurrentBolltCutters()
    {
        if (CurrentBoltCutters >= 0)
        {
            return BoltCutterUpgrades[CurrentBoltCutters];
        } else
        {
           return null;
        }
    }

    /// <summary>
    /// Gets the list of Boltcutter upgrades
    /// </summary>
    /// <returns>List of Boltcutter upgrades</returns>
    public List<BoltCutters> GetBoltCuttersUpgrades ()
    {
        return BoltCutterUpgrades;
    }

    /// <summary>
    /// Gets the Current BodyBag
    /// </summary>
    /// <returns>Current BodyBag in use</returns>
    public BodyBag GetCurrentBodyBag()
    {
        return BodyBagsUpgrades[CurrentBodyBag];
    }

    /// <summary>
    /// Gets the list of Bodybag upgrades
    /// </summary>
    /// <returns>List of Bodybag upgrades</returns>
    public List<BodyBag> GetBodyBagsUpgrades()
    {
        return BodyBagsUpgrades;
    }

    //------------------------------------------------------------
    // Setters
    // -----------------------------------------------------------

    public void SetCurrentShovel(int NewShovelIndex)
    {
        CurrentShovel = NewShovelIndex;
    }

    public void SetCurrentShoes(int NewShoesIndex)
    {
        CurrentShoes = NewShoesIndex;
    }

    public void SetCurrentBodyBag(int NewBodyBagIndex)
    {
        CurrentBodyBag = NewBodyBagIndex;
    }

    public void SetCurrentBoltCutter(int NewBoltCutterIndex)
    {
        CurrentBoltCutters = NewBoltCutterIndex;
    }

    /// <summary>
    /// Will take the current objects information and convert it into a Upgrades object
    /// </summary>
    /// <returns>Upgrades class</returns>
    public Upgrades Convert()
    {
        return new Upgrades(ShovelUpgrades.ToArray(), ShoesUpgrades.ToArray(), BoltCutterUpgrades.ToArray(), BodyBagsUpgrades.ToArray(), CurrentShovel, CurrentShoes, CurrentBoltCutters, CurrentBodyBag);
    }

    /// <summary>
    /// Will take the upgrades class information and store it on the current object
    /// </summary>
    /// <param name="_Upgrades"></param>
    public void Convert(Upgrades _Upgrades)
    {
        ShovelUpgrades.AddRange(_Upgrades.ShovelUpgrades);
        ShoesUpgrades.AddRange(_Upgrades.ShoesUpgrades);
        BoltCutterUpgrades.AddRange(_Upgrades.BoltCutterUpgrades);
        BodyBagsUpgrades.AddRange(_Upgrades.BodyBagsUpgrades);
        CurrentShovel = _Upgrades.CurrentShovel;
        CurrentShoes = _Upgrades.CurrentShoes;
        CurrentBoltCutters = _Upgrades.CurrentBoltCutters;
        CurrentBodyBag = _Upgrades.CurrentBodyBag;
    }
}

/// <summary>
/// A data container for all the upgrade information
/// </summary>
[System.Serializable]
public class Upgrades
{
    public Upgrades(Shovel[] _Shovels, Shoes[] _Shoes, BoltCutters[] _BoltCutters, BodyBag[] _BodyBags, int _CurrShovel, int _CurrShoes, int _CurrBoltCutters, int _CurrBodyBag)
    {
        ShovelUpgrades = _Shovels;
        ShoesUpgrades = _Shoes;
        BoltCutterUpgrades = _BoltCutters;
        BodyBagsUpgrades = _BodyBags;
        CurrentShovel = _CurrShovel;
        CurrentShoes = _CurrShoes;
        CurrentBodyBag = _CurrBodyBag;
        CurrentBoltCutters = _CurrBoltCutters;
    }

    [Header("Available Upgrades")]
    // Shovels that are available
    [SerializeField] public Shovel[] ShovelUpgrades;
    // Shoes that are availabe
    [SerializeField] public Shoes[] ShoesUpgrades;
    // Bolt cutters that are available
    [SerializeField] public BoltCutters[] BoltCutterUpgrades;
    // Body Bags that are available
    [SerializeField] public BodyBag[] BodyBagsUpgrades;

    [Header("Equipped Upgrades")]
    // Current Shovel
    [SerializeField] public int CurrentShovel = 0;
    // Current Shoes
    [SerializeField] public int CurrentShoes = 0;
    // Current Boltcutters
    [SerializeField] public int CurrentBoltCutters = 0;
    // Current BodyBag
    [SerializeField] public int CurrentBodyBag = 0;
}
