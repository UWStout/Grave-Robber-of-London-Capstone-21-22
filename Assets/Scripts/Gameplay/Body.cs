using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum that specifies the quality of the body. Used for indexing inside the game manager.
/// </summary>
[System.Serializable]
public enum BodyQuality
{
    Skeleton,
    Decayed,
    SlightDecay,
    Fresh
}

/// <summary>
/// A data structure to holds the quality of the body.
/// </summary>
[System.Serializable]
public class Body
{
    //-------------------------------------------------------------------------
    //  Variables
    //-------------------------------------------------------------------------

    [Header("Decay")]
    [SerializeField] private int Decay; /*! A percent quality of the body*/
    [SerializeField] private BodyQuality QualityEnum;
    [SerializeField] private Material BodyMat;
    //-------------------------------------------------------------------------
    //  Funcitons
    //-------------------------------------------------------------------------

    /// <summary>
    /// Default constructor. Sets the Decay variable to be 0%. or to a skeleton
    /// </summary>
    public Body()
    {
        SetQuality(0);
    }

    /// <summary>
    /// Overloaded constructor that will take in a quality and initialize it.
    /// </summary>
    /// <param name="_Quality">The quality of the body in percent. % out of 100.</param>
    /// <exception cref="ArgumentOutOfRangeException">Will throw and exception if the parameter <paramref name="_Quality"/>, <paramref name="_Treasures"/>, or <paramref name="_Decay"/> are out of range.</exception>
    public Body(int _Quality)
    {
        if(!SetQuality(_Quality))
        {
            throw new System.ArgumentOutOfRangeException("[Error]{Invalid Parameter Exception} _Decay(" + _Quality.ToString() + ") is out of the 0-100 range!");
        }
    }

    /// <summary>
    /// Getter for the quality of the body.
    /// </summary>
    /// <returns>The integer value of the percent quality of the body</returns>
    public int GetDecay()
    {
        return Decay;
    }

    /// <summary>
    /// Getter for the QualityEnum value of the bodies quality.
    /// </summary>
    /// <returns>The enum equivelent of the body quality</returns>
    public BodyQuality GetQualityEnum()
    {
        return QualityEnum;
    }

    /// <summary>
    /// Getter for the body sprite's material.
    /// </summary>
    /// <returns>
    /// The material object for the body.
    /// </returns>
    public Material GetMaterial()
    {
        return BodyMat;
    }

    /// <summary>
    /// Will set the quality of the body. Based on the percent it will also set the bodies quality in enum form also.
    /// </summary>
    /// <param name="_Decay">The quality of the body in percent</param>
    /// <returns>True if the quality was assigned and false if the <paramref name="_Decay"/> is out of the 0-100 range</returns>
    public bool SetQuality(int _Decay)
    {
        Decay = _Decay;
        if (_Decay >= 0 && _Decay <= 25)
        {
            QualityEnum = BodyQuality.Skeleton;
        }else if(_Decay > 25 && _Decay <= 50)
        {
            QualityEnum = BodyQuality.Decayed;
        }
        else if(_Decay > 50 && _Decay <= 75)
        {
            QualityEnum = BodyQuality.SlightDecay;
        }
        else if (_Decay > 75 && _Decay <= 100)
        {
            QualityEnum = BodyQuality.Fresh;
        }
        else
        {
            _Decay = 100;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Sets the material for the body, determining its sprite.
    /// </summary>
    /// <param name="_Material">
    /// The material for the body.
    /// </param>
    /// <returns>
    /// Returns true on succesful assignment.
    /// </returns>
    public bool SetMaterial(Material _Material)
    {
        BodyMat = _Material;
        return true;
    }
}