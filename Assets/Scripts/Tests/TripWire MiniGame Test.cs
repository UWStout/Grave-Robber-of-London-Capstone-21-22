/*
 * Author: Damian Link
 * Version: 10/12/21
 */
using UnityEngine;
using NUnit.Framework;

/// <summary>
/// Unit test cases for tripwire minigame
/// </summary>
public class TripWireMiniGameTest
{
    // object being tested
    TripWireMiniGame testMinigame = new TripWireMiniGame();

    /// <summary>
    /// tests if minigame is won, before and after actual win
    /// </summary>
    [Test]
    public void CheckWinTest()
    {
        //Test to make sure if false before won
        Assert.IsFalse(testMinigame.CheckWin(), "Before Win");
        testMinigame.ForceWin();
        //Test to make sure when player is won
        Assert.IsTrue(testMinigame.CheckWin(), "After Win");
    }
}
