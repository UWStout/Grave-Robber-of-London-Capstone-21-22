/*
Author: Zachary Boehm
Version: 10.26.2021
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A data structure that holds two strings and an int.
/// 
/// Holds a title, content, and the portrait index.
/// </summary>
/// <remarks>
/// ### Variables
/// 
/// * Title - The title or name to be displayed
/// * ScriptLine - The dialogue to be displayed
/// * Portrait - The index of the image to be displayed
/// </remarks>
[System.Serializable]
public class Line
{
    public string Title = ""; /*! The title or name of this line of dialogue*/
    public string ScriptLine = ""; /*! The dialogue for this line*/
    public int Portrait = -1; /*! The portait associated with this line*/

    /// <summary>
    /// Constructor that will take in a title, line, portrait, and initialize the inner values.
    /// </summary>
    /// <param name="_Title">The title or name of the current displayed dialogue.</param>
    /// <param name="_Line">The content of what is being said by the character</param>
    /// <param name="_Portrait">The index of an image that will be displayed (handled elsewhere)</param>
    public Line(string _Title, string _Line, int _Portrait)
    {
      Title = _Title;
      ScriptLine = _Line;
      Portrait = _Portrait;
    }

    /// <summary>
    /// Constructor that takes a line and initializes this scripts variables with the parameters.
    /// </summary>
    /// <param name="_NewLine">A line that will be assigned to the current instance</param>
    public Line(Line _NewLine)
    {
      Title = _NewLine.Title;
      ScriptLine = _NewLine.ScriptLine;
      Portrait = _NewLine.Portrait;
    }

    
    /// <summary>
    /// Will take in a title, line, and index and set those values.
    /// </summary>
    /// <param name="_Title">The new title to be displayed</param>
    /// <param name="_Line">The new content to be displayed</param>
    /// <param name="_Portrait">The new index of the image to be displayed</param>
    public void SetLine(string _Title, string _Line, int _Portrait)
    {
      Title = _Title;
      ScriptLine = _Line;
      Portrait = _Portrait;
    }

    /// <summary>
    /// Will take in a line and set it as the current information
    /// </summary>
    /// <param name="_NewLine">The new line to be used as information</param>
    public void SetLine(Line _NewLine)
    {
      Title = _NewLine.Title;
      ScriptLine = _NewLine.ScriptLine;
      Portrait = _NewLine.Portrait;
    }
   
}
