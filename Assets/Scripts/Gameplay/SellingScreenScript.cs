/**
 * Author: Damian Link
 * Version: 3/3/22
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the selling screen
/// </summary>
public class SellingScreenScript : MonoBehaviour
{
    //--------------------------------------------------
    // Varables
    //--------------------------------------------------
    public enum ScreenOptions {Shovel, Shoes, BoltCutters, BodyBag };

    //    [Header("")]
    // Image to show the current Item
    [SerializeField] private Image CurrentIconImage;
    // Text location of the Current balance
    [SerializeField] private Text MoneyText;
    // Text location of the description of the currente item
    [SerializeField] private Text DescriptionText;
    // List of buttons to buy new items
    [SerializeField] private List<Button> BuyButtons;
    // List of images to show when an item is bought
    [SerializeField] private List<Image> OwnedImages;
    //
    [SerializeField] private List<Button> EquipButtons; 
    // image of the seller
    public Sprite SellerGuy;
    // Text location of the what seller is saying
    [SerializeField] private Text SpeechText;
    // Text that show the prices of items
    [SerializeField] private List<Text> PriceTexts;
    // Default image sprite
    private Sprite DefualtItemImage;
    // Buttons object holding BuybuttonScript
    public BuyButtonScriipt ButtonObj;

    [SerializeField] private Image[] BuyTiles;
    [Header("Tile Options")]
    [SerializeField] private Sprite[] ShovelBuyingTiles;
    [SerializeField] private Sprite[] BagBuyingTiles;
    [SerializeField] private Sprite[] CutterBuyingTiles;
    [SerializeField] private Sprite[] ShoesBuyingTiles;


    [SerializeField] private Button[] ItemSelection;
    [SerializeField] private UIManager _UIManager;

    // phrase that was replaced with not enough money
    private string PastPhrase;
    // text for not having enough money
    private string NotEnoughMoneyPhrase = "You do not have enough money for that.";
    // An array that holds all the phrases that the seller says, 
    private string[] ChatOptions = {"Which shovel would you like?", "Which shoes would you like?" , 
        "Which boltcutters would you like?" , "Which bodybag would you like?" };

    //-------------------------------------------------
    // Functions
    //-------------------------------------------------

    /// <summary>
    /// Sets up the buying screen for a specific item
    /// </summary>
    /// <param name="SetupItem">Item that shows what to set the screen for, in terms of int</param>
    public void SetupScreen(int SetupItemName)
    {
        ButtonObj.SellScript = this;
        UpdateMoney();
        switch ((ScreenOptions)SetupItemName)
        {
            case ScreenOptions.Shovel:
                ShovelBuyUpgradeScreenSetup();
                break;
            case ScreenOptions.Shoes:
                ShoesBuyUpgradeScreenSetup();
                break;
            case ScreenOptions.BoltCutters:
                BoltCutterBuyUpgradeScreenSetup();
                break;
            case ScreenOptions.BodyBag:
                BodyBagBuyUpgradeScreenSetup();
                break;
        }
        //SetEquipButtons((ScreenOptions)SetupItemName);
    }

    /// <summary>
    /// Sets up the buy screen for the shovel upgrades
    /// </summary>
    public void ShovelBuyUpgradeScreenSetup()
    {
        // Changes the image of items being sold
        for (int i = 0; i < ShovelBuyingTiles.Length; i++)
        {
            BuyTiles[i].sprite = ShovelBuyingTiles[i];
        }
        
        // Sets the chat text to the item being sold
        SpeechText.text = ChatOptions[(int)ScreenOptions.Shovel];

        // sets up current stats
        CurrentIconImage.sprite = GameManager.Instance._UpgradeManager.GetCurrentShovel().GetIcon();
        DescriptionText.text = GameManager.Instance._UpgradeManager.GetCurrentShovel().GetDescription();

        // disables the button for shovels
        for (int i = 0; i < ItemSelection.Length; i++)
        {
            ItemSelection[i].interactable = true;
        }
        ItemSelection[(int)ScreenOptions.Shovel].interactable = false;

        ButtonObj.CurrentItem = ScreenOptions.Shovel;

        // sets up the buttons/owned
        for (int i = 0; i < GameManager.Instance._UpgradeManager.GetShovelUpgrades().Count; i++)
        {
            if (GameManager.Instance._UpgradeManager.GetShovelUpgrades()[i].IsOwned())
            {
                BuyButtons[i].transform.parent.gameObject.SetActive(false);
                OwnedImages[i].gameObject.SetActive(true);
            }
            else
            {
                BuyButtons[i].transform.parent.gameObject.SetActive(true);
                OwnedImages[i].gameObject.SetActive(false);
                PriceTexts[i].text = GameManager.Instance._UpgradeManager.GetShovelUpgrades()[i].GetCost().ToString();
            }
        }
    }

    /// <summary>
    /// Sets up the buy screen for the boltcutter upgrades
    /// </summary>
    public void BoltCutterBuyUpgradeScreenSetup()
    {
        // Changes the image of items being sold
        for (int i = 0; i < CutterBuyingTiles.Length; i++)
        {
            BuyTiles[i].sprite = CutterBuyingTiles[i];
        }

        // Sets the chat text to the item being sold
        SpeechText.text = ChatOptions[(int)ScreenOptions.BoltCutters];

        // sets up current stats
        if (GameManager.Instance._UpgradeManager.GetCurrentBolltCutters() != null)
        {
            CurrentIconImage.sprite = GameManager.Instance._UpgradeManager.GetCurrentBolltCutters().GetIcon();
            DescriptionText.text = GameManager.Instance._UpgradeManager.GetCurrentBolltCutters().GetDescription();
        }

        // disables the button for shovels
        for (int i = 0; i < ItemSelection.Length; i++)
        {
            ItemSelection[i].interactable = true;
        }
        ItemSelection[(int)ScreenOptions.BoltCutters].interactable = false;

        ButtonObj.CurrentItem = ScreenOptions.BoltCutters;
        // sets up the buttons/owned
        for (int i = 0; i < GameManager.Instance._UpgradeManager.GetBoltCuttersUpgrades().Count; i++)
        {
            if (GameManager.Instance._UpgradeManager.GetBoltCuttersUpgrades()[i].IsOwned())
            {
                BuyButtons[i].transform.parent.gameObject.SetActive(false);
                OwnedImages[i].gameObject.SetActive(true);
            }
            else
            {
                OwnedImages[i].gameObject.SetActive(false);
                BuyButtons[i].transform.parent.gameObject.SetActive(true);
                PriceTexts[i].text = GameManager.Instance._UpgradeManager.GetBoltCuttersUpgrades()[i].GetCost().ToString();
            }
        }
    }

    /// <summary>
    /// Sets up the buy screen for the body bag upgrades
    /// </summary>
    public void BodyBagBuyUpgradeScreenSetup()
    {
        // Changes the image of items being sold
        for (int i = 0; i < BagBuyingTiles.Length; i++)
        {
            BuyTiles[i].sprite = BagBuyingTiles[i];
        }

        // Sets the chat text to the item being sold
        SpeechText.text = ChatOptions[(int)ScreenOptions.BodyBag];

        // sets up current stats
        CurrentIconImage.sprite = GameManager.Instance._UpgradeManager.GetCurrentBodyBag().GetIcon();
        DescriptionText.text = GameManager.Instance._UpgradeManager.GetCurrentBodyBag().GetDescription();

        // disables the button for shovels
        for (int i = 0; i < ItemSelection.Length; i++)
        {
            ItemSelection[i].interactable = true;
        }
        ItemSelection[(int)ScreenOptions.BodyBag].interactable = false;

        ButtonObj.CurrentItem = ScreenOptions.BodyBag;

        // sets up the buttons/owned
        for (int i = 0; i < GameManager.Instance._UpgradeManager.GetBodyBagsUpgrades().Count; i++)
        {
            if (GameManager.Instance._UpgradeManager.GetBodyBagsUpgrades()[i].IsOwned())
            {
                BuyButtons[i].transform.parent.gameObject.SetActive(false);
                OwnedImages[i].gameObject.SetActive(true);
            }
            else
            {
                OwnedImages[i].gameObject.SetActive(false);
                BuyButtons[i].transform.parent.gameObject.SetActive(true);
                PriceTexts[i].text = GameManager.Instance._UpgradeManager.GetBodyBagsUpgrades()[i].GetCost().ToString();
            }
        }
    }

    /// <summary>
    /// Sets up the buy screen for the shoe upgrades.
    /// </summary>
    public void ShoesBuyUpgradeScreenSetup()
    {
        // Changes the image of items being sold
        for (int i = 0; i < ShoesBuyingTiles.Length; i++)
        {
            BuyTiles[i].sprite = ShoesBuyingTiles[i];
        }

        // Sets the chat text to the item being sold
        SpeechText.text = ChatOptions[(int)ScreenOptions.Shoes];

        // sets up current stats
        CurrentIconImage.sprite = GameManager.Instance._UpgradeManager.GetCurrentShoes().GetIcon();
        DescriptionText.text = GameManager.Instance._UpgradeManager.GetCurrentShoes().GetDescription();

        // disables the button for shovels
        for (int i = 0; i < ItemSelection.Length; i++)
        {
            ItemSelection[i].interactable = true;
        }
        ItemSelection[(int)ScreenOptions.Shoes].interactable = false;

        ButtonObj.CurrentItem = ScreenOptions.Shoes;

        // sets up the buttons/owned
        for (int i = 0; i < GameManager.Instance._UpgradeManager.GetShoesUpgrades().Count; i++)
        {
            if (GameManager.Instance._UpgradeManager.GetShoesUpgrades()[i].IsOwned())
            {
                BuyButtons[i].transform.parent.gameObject.SetActive(false);
                OwnedImages[i].gameObject.SetActive(true);
            }
            else
            {
                OwnedImages[i].gameObject.SetActive(false);
                BuyButtons[i].transform.parent.gameObject.SetActive(true);
                PriceTexts[i].text = GameManager.Instance._UpgradeManager.GetShoesUpgrades()[i].GetCost().ToString();
            }
        }

    }

    /// <summary>
    /// Sets up default screen
    /// </summary>
    private void OnEnable()
    {
        DefualtItemImage = CurrentIconImage.sprite;
        UpdateMoney();
        SetupScreen((int)ScreenOptions.Shovel);
    }

    /// <summary>
    /// Function that is called to close the store from the "close" button
    /// </summary>
    public void CloseButtonFunction ()
    {
        _UIManager.CloseStore();
    }

    /// <summary>
    /// Updates the text showing the current money
    /// </summary>
    public void UpdateMoney()
    {
        MoneyText.text = GameManager.Instance.GetMoney().ToString();
    }

    /// <summary>
    /// NOt enough money speech text
    /// </summary>
    public void NotEnoughMoneyTalk ()
    {
        if (SpeechText.text != NotEnoughMoneyPhrase)
        {
            PastPhrase = SpeechText.text;
            SpeechText.text = NotEnoughMoneyPhrase;
            StartCoroutine(DisplayDelay());
        }
        
    }

    /// <summary>
    /// Adds delay for the speech text
    /// </summary>
    /// <returns>delay of 1 second</returns>
    IEnumerator DisplayDelay ()
    {
        //Debug.Log(PastPhrase);
        yield return new WaitForSeconds(1f);
        SpeechText.text = PastPhrase;
    }


    public void SetEquipButtons(ScreenOptions CurrentItem)
    {
        bool EquipedActive = false;
        
        for (int i = 0; i < EquipButtons.Count; i++)
        {
            switch(CurrentItem)
            {
                case ScreenOptions.Shovel:
                    EquipedActive = GameManager.Instance._UpgradeManager.GetShovelUpgrades()[i].IsOwned() == true && GameManager.Instance._UpgradeManager.GetShovelUpgrades()[i] != GameManager.Instance._UpgradeManager.GetCurrentShovel();
                    break;
                case ScreenOptions.Shoes:
                    EquipedActive = GameManager.Instance._UpgradeManager.GetShoesUpgrades()[i].IsOwned() == true && GameManager.Instance._UpgradeManager.GetShoesUpgrades()[i] != GameManager.Instance._UpgradeManager.GetCurrentShoes();
                    break;
                case ScreenOptions.BoltCutters:
                    EquipedActive = GameManager.Instance._UpgradeManager.GetBoltCuttersUpgrades()[i].IsOwned() == true && GameManager.Instance._UpgradeManager.GetBoltCuttersUpgrades()[i] != GameManager.Instance._UpgradeManager.GetCurrentBolltCutters();
                    break;
                case ScreenOptions.BodyBag:
                    EquipedActive = GameManager.Instance._UpgradeManager.GetBodyBagsUpgrades()[i].IsOwned() == true && GameManager.Instance._UpgradeManager.GetBodyBagsUpgrades()[i] != GameManager.Instance._UpgradeManager.GetCurrentBodyBag();
                    break;
            }
            
            if (EquipedActive)
            {
                EquipButtons[i].gameObject.SetActive(true);
            } else
            {
                EquipButtons[i].gameObject.SetActive(false);
            }
        }
    }
}
