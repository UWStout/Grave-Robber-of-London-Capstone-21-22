/**
 * @Author Damian Link
 * 
 * @Version 2/8/2022
 */
using UnityEngine;

/// <summary>
/// Class to store Item data
/// </summary>
[System.Serializable]
public class Item
{
    // Name of the item
    [SerializeField] private string Name = "Unknown Item";
    // Icon of the item
    [SerializeField] private Sprite Icon;
    // Cost of the item
    [SerializeField] private int Cost = 0;
    // Description of the Item
    [SerializeField] private string Description = "...";
    // If the item is Unlocked
    [SerializeField] private bool Owned = false;


    /// <summary>
    /// Creates new Item object
    /// </summary>
    /// <param name="NewName">Name of the item</param>
    /// <param name="NewIcon">Icon of the item</param>
    /// <param name="NewCost">Cost of the item</param>
    /// <param name="NewDesc">Description of item</param>
    /// <param name="NewUnlocked">Description of item</param>
    public Item(string NewName = "Unknown Item", Sprite NewIcon = null, int NewCost = 0, string NewDesc = "...")
    {
        if (NewName != "")
        {
            Name = NewName;
        }
        if (NewCost > 0)
        {
            Cost = NewCost;
        }
        if (NewDesc != "")
        {
            Description = NewDesc;
        } else
        {
            Description = "...";
        }
        if (NewIcon != null)
        {
            Icon = NewIcon;
        } else
        {
            Debug.LogError("Icon for Item is Null");
        }
    }

    //----------------------------------------------------------
    // Getters
    //----------------------------------------------------------

    /// <summary>
    /// Gets the name of the item
    /// </summary>
    /// <returns>name of the item</returns>
    public string GetName ()
    {
        return Name;
    }

    /// <summary>
    /// Gets the sprite icon of the item
    /// </summary>
    /// <returns>Sprite icon of the item</returns>
    public Sprite GetIcon ()
    {
        return Icon;
    }

    /// <summary>
    /// Gets the cost of the item
    /// </summary>
    /// <returns>Cost of the item</returns>
    public int GetCost ()
    {
        return Cost;
    }

    /// <summary>
    /// Gets the description of the item
    /// </summary>
    /// <returns>Description of the item</returns>
    public string GetDescription ()
    {
        return Description;
    }

    /// <summary>
    /// Gets the status of the item being Owned
    /// </summary>
    /// <returns>Status of the item being owned</returns>
    public bool IsOwned ()
    {
        return Owned;
    }

    //---------------------------------------------------
    // Setters
    //---------------------------------------------------

    /// <summary>
    /// 
    /// </summary>
    /// <param name="NewOwned"></param>
    public void SetOwned (bool NewOwned)
    {
        Owned = NewOwned;
    }
}
