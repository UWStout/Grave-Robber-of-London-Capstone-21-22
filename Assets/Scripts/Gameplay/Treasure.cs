using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A treasure which can be looted through gameplay. It has an image along with a name,description, and price
/// </summary>
[System.Serializable]
public class Treasure
{
    [ContextMenuItem("RandomValues", "DefaultValues")]
    [SerializeField] private string Name;
    [SerializeField] private int Price;
    [SerializeField] private string Description;

    /// <summary>
    /// Override constructor for the treasure
    /// </summary>
    /// <param name="_Name">The name of the treasure</param>
    /// <param name="_Price">The price of the treasure</param>
    /// <param name="_Description">The description of the treasure</param>
    public Treasure(string _Name, int _Price, string _Description)
    {
        Name = _Name;
        Price = _Price;
        Description = _Description;
    }

    /// <summary>
    /// Default treasure construcor
    /// </summary>
    public Treasure()
    {
        Name = "Default";
        Price = 0;
        Description = "Default Description";
    }

    /// <summary>
    /// Gets the name of the treasure
    /// </summary>
    /// <returns>The name of the body as a string</returns>
    public string GetName()
    {
        return Name;
    }

    /// <summary>
    /// Gets the price of the treasure
    /// </summary>
    /// <returns>The price of the body as and int</returns>
    public int GetPrice()
    {
        return Price;
    }

    /// <summary>
    /// Gets the treasure's description
    /// </summary>
    /// <returns>The description of the body as a string</returns>
    public string GetDescription()
    {
        return Description;
    }

    /// <summary>
    /// Sets the name of the treasure
    /// </summary>
    /// <param name="_Name">The new name of the treasure as a string</param>
    public void SetName(string _Name)
    {
        Name = _Name;
    }

    /// <summary>
    /// Sets the price of the treasure
    /// </summary>
    /// <param name="_Price">The new price of the treasure as an int</param>
    public void SetPrice(int _Price)
    {
        Price = _Price;
    }

    /// <summary>
    /// Sets the description of the treasure
    /// </summary>
    /// <param name="_Description">The new treasure description as a string</param>
    public void SetDescription(string _Description)
    {
        Description = _Description;
    }

    /// <summary>
    /// A function that will set default values to the current object
    /// </summary>
    public void DefaultValues()
    {
        Name = "Queen's Golden Ring";
        Price = 25;
        Description = "The Queen's Golden Ring has been passed down for generations. It is thought to originally be a gift to the royal family by the King Alexander.";
    }
}

