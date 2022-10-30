/**
 * Author: Damian Llink
 * Version: 3/24/22
 */
using UnityEngine;

/// <summary>
/// Class that holds infromation about the bolt cutters
/// </summary>
[System.Serializable]
public class BoltCutters : Item
{
    // Boost that the BoltCutters receives
    [SerializeField] private float Boost = 0.0f;
    // Sprite of the open cutters
    [SerializeField] private Sprite OpenTexture;
    // Sprite of the closed cutters
    [SerializeField] private Sprite ClosedTexture;

    /// <summary>
    /// Creates a new BoltCutters with given information
    /// </summary>
    /// <param name="NewName">Name of the shovel</param>
    /// <param name="NewIcon">Icon of the shovel</param>
    /// <param name="NewCost">Cost of the shevel</param>
    /// <param name="NewBoost">Boost of the shovel</param>
    public BoltCutters(string NewName = "", Sprite NewIcon = null, int NewCost = 0, float NewBoost = 0.0f) : base(NewName, NewIcon, NewCost)
    {
        if (NewBoost > 0.0f)
        {
            Boost = NewBoost;
        }
    }

    /// <summary>
    /// Gets the Boost that the BoltCutters receives
    /// </summary>
    /// <returns>The Boost that the BoltCutters receives</returns>
    public float GetBoost()
    {
        return Boost;
    }
    
    /// <summary>
    /// gets the sprite of the open cutters
    /// </summary>
    /// <returns>sprite of open cutters</returns>
    public Sprite GetOpentexture ()
    {
        return OpenTexture;
    }

    /// <summary>
    /// gets the sprite of the open cutters
    /// </summary>
    /// <returns>sprite of open cutters</returns>
    public Sprite GetClosedtexture()
    {
        return ClosedTexture;
    }
}
