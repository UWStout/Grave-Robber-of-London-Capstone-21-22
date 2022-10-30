/*
Author: Zachary Boehm
Verson: 10.26.2021

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is a way of storing and managin the lines in a specific order.
/// Has getters for any given line and a random line.
/// Has a setter that takes in lines and sets/adds it to the list of lines
/// </summary>
[CreateAssetMenu(fileName = "Dialogue Script", menuName = "ScriptableObjects/New Dialogue Script", order = 1)]
[System.Serializable]
public class Script: ScriptableObject
{
    [SerializeField] private List<Line> Lines = new List<Line>(); /*! List of lines in the order they will be called*/

    /// <summary>
    /// A method that will add a new line the dialogue script in runtime
    /// </summary>
    /// <param name="_NewLine">A line that will be appended to the list of lines</param>
    public void Add(Line _NewLine)
    {
        Lines.Add(_NewLine);
    }

    /// <summary>
    /// A method that will add a new line the dialogue script in runtime
    /// </summary>
    /// <param name="_NewLines">An array of lines that will all be appended to the list of lines</param>
    public void Add(Line[] _NewLines)
    {
        foreach(Line line in _NewLines)
        {
            Lines.Add(line);
        }
    }
    
    
    /// <summary>
    /// Will get a line based on index
    /// </summary>
    /// <param name="LineNumber">The current line in the list of lines</param>
    /// <returns>The line at a certain index based on parameter</returns>
    /// <remarks>
    /// ---
    /// **NOTE**
    ///
    /// Will return the last line if the passed in index is greater than the lists size.
    /// Will return the first line if the passed in index is less than 0.
    ///
    /// ---
    /// </remarks>
    public Line GetLine(int LineNumber)
    {
        //As long as the last line has not been used
        if (LineNumber < Lines.Count && LineNumber >= 0)
        {
            //return the next line and then increment the line incrementor
            return Lines[LineNumber];
        }
        else if (LineNumber >= Lines.Count)
        { //If the end of the script has been reached the return null
            return Lines[Lines.Count - 1];
        }else
        {
            return Lines[0];
        }
    }

    /// <summary>
    /// when called, this function will randomly choose a line from the dialogue script
    /// and return the line.
    /// </summary>
    /// <returns>The randomly picked line form the dialogue script</returns>
    public Line GetRandomLine()
    {
        var rand = new System.Random();
        return Lines[rand.Next(Lines.Count)];
    }

    /// <summary>
    /// Getter for the total number of lines in the list of lines
    /// </summary>
    /// <returns>Number of lines in the lines list</returns>
    public int GetNumLines()
    {
        return Lines.Count;
    }
}
