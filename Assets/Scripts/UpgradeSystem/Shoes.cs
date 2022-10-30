/*
 * Author: Damian Link
 * Version: 4/12/22
 */
using UnityEngine;

/// <summary>
/// Stores information of a shoes
/// </summary>
[System.Serializable]
public class Shoes : Item
{
    // Boost that the shoes give on stealth
    [SerializeField ]private float StealthBoost = 0.0f;
    // Boost that the shoes give on speed
    [SerializeField] private float SpeedBoost = 0.0f;

    /// <summary>
    /// Creates a new shovel with given information
    /// </summary>
    /// <param name="NewName">Name of the shoes</param>
    /// <param name="NewIcon">Icon of the shoes</param>
    /// <param name="NewCost">Cost of the shoes</param>
    /// <param name="NewDesc">Description of shoes</param>
    /// <param name="NewSpeedBoost">Speed boost of the shoes</param>
    /// <param name="NewStealthBoost">Stealth boost of the shoes</param>
    public Shoes(string NewName = "", Sprite NewIcon = null, int NewCost = 0, string NewDesc = "", float NewSpeedBoost = 0.0f, float NewStealthBoost = 0.0f) : base(NewName, NewIcon, NewCost, NewDesc)
    {
        if (NewSpeedBoost > 0.0f)
        {
            SpeedBoost = NewSpeedBoost;
        }
        if (NewStealthBoost > 0.0f)
        {
            StealthBoost = NewStealthBoost;
        }
    }

    /// <summary>
    /// Gets the Boost that the shoes give on stealth receives
    /// </summary>
    /// <returns>The Boost that the shoes give on stealth receives</returns>
    public float GetStealthBoost()
    {
        return StealthBoost;
    }

    /// <summary>
    /// Gets the Boost that the shoes give on speed receives
    /// </summary>
    /// <returns>The Boost that the shoes give on speed receives</returns>
    public float GetSpeedBoost()
    {
        return SpeedBoost;
    }
}
