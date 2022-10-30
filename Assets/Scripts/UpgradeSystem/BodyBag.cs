/*
 * Author:Damian Link
 * Version:2/21/22
 */
using UnityEngine;

/// <summary>
/// The Item that Carrys the bodies and determines how many can be carryed
/// </summary>
[System.Serializable]
public class BodyBag : Item
{
    //--------------------------------------
    // Varables
    //--------------------------------------
    // The maximum bodies that can be carried/ collected in one night
    [SerializeField ]private int MaxBodies;

    //-------------------------------------
    // Constructor
    //--------------------------------------
    /// <summary>
    /// Creates a new BodyBag with given information
    /// </summary>
    /// <param name="NewName">Name of the BodyBag</param>
    /// <param name="NewIcon">Icon of the BodyBag</param>
    /// <param name="NewCost">Cost of the BodyBag</param>
    /// <param name="NewMaxBodies">Max number of bodies able to be held</param>
    /// <param name="NewDesc">Description of BodyBag</param>
    public BodyBag(string NewName = "", Sprite NewIcon = null, int NewCost = 0, int NewMaxBodies = 1, string NewDesc = "") : base(NewName, NewIcon, NewCost, NewDesc)
    {
        if (NewMaxBodies > 0.0f)
        {
            MaxBodies = NewMaxBodies;
        }
    }

    //---------------------------------------
    // Getters
    //---------------------------------------

    /// <summary>
    /// Gets the max bodies that can be carried/collected in one night
    /// </summary>
    /// <returns></returns>
    public int GetMaxBodies()
    {
        return MaxBodies;
    }
}
