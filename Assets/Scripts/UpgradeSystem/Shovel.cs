/*
 * Author: Damian Link
 * Version: 3/12/22
 */
using UnityEngine;

/// <summary>
/// Stores information of a shovel
/// </summary>
[System.Serializable]
public class Shovel : Item
{
    // Boost that the shovel receives
    [SerializeField] private float Boost = 0.0f;

    /// <summary>
    /// Creates a new shovel with given information
    /// </summary>
    /// <param name="NewName">Name of the shovel</param>
    /// <param name="NewIcon">Icon of the shovel</param>
    /// <param name="NewCost">Cost of the shevel</param>
    /// <param name="NewBoost">Boost of the shovel</param>
    /// <param name="NewDesc">Description of shovel</param>
    public Shovel(string NewName = "", Sprite NewIcon = null, int NewCost = 0, float NewBoost = 0.0f, string NewDesc = "") : base(NewName, NewIcon, NewCost, NewDesc)
    {
        if  (NewBoost > 0.0f)
        {
            Boost = NewBoost;
        }   
    }

    /// <summary>
    /// Gets the Boost that the shovel receives
    /// </summary>
    /// <returns>The Boost that the shovel receives</returns>
    public float GetBoost ()
    {
        return Boost;
    }

}
