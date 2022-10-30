/*
 * Author: Damian Link
 * Version: 10/12/21
 */
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// Unit testing for the Coffin cage
/// </summary>
public class CoffinCageMinigameTest
{

    // object being tested
    CoffinCageMiniGame testMinigame = new CoffinCageMiniGame();
    CoffinCageMiniGame testMinigame2 = new CoffinCageMiniGame();
    
    /// <summary>
    /// tests the trap disarming and if the disarmed varable is false by default, adjusted by forcing win
    /// </summary>
    [Test]
    public void TrapDisarming()
    {
        // Test to make sure that Disabled is false by default
        Assert.IsFalse(testMinigame.CheckWin());
        testMinigame.SetCutNumber(1);
        // Test to make sure that Disabled is false by default
        Assert.IsTrue(testMinigame.CheckWin());
    }

    
    //tests the trap disarming and if the disarmed varable is false by default, adjusted by cutting a trap
    
    [Test]
    public void TrapDisarmingByDisarm()
    {
        // Test to make sure that Disabled is false by default
        Assert.IsFalse(testMinigame2.CheckWin());
        Assert.AreEqual(0, testMinigame2.GetCurrentNumberOfCuts());
        //testMinigame2.CutTrap();
        // Test to make sure that Disabled is false by default
        //Assert.AreEqual(1, testMinigame2.GetCurrentNumberOfCuts());
       // Assert.IsTrue(testMinigame2.CheckWin());
    } 
}
