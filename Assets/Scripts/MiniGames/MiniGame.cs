/*
 * Author: Damian Link
 * Version: 4/12/22
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for minigames
/// </summary>
public class MiniGame : MonoBehaviour
{
    /// <summary>
    /// Checks for win
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckWin()
    {
        return false;
    }

    /// <summary>
    /// Check for lose
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckLose()
    {
        return false;
    }

    // increase dificulty
    public virtual void IncreaseDificulty()
    {

    }
}
