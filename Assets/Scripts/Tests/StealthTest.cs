using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StealthTest
{
    /*
    * author: Aaron Tweden
    * 
    * version: 11/9/2021
    */

    StealthManager TestStealth = new StealthManager();
    StealthManager TestStealth2 = new StealthManager();
    // A Test behaves as an ordinary method
    [Test]
    public void StealthAlertLevelTest()
    {
        Assert.AreEqual(TestStealth.ReturnAlertLevel(), 0);
        TestStealth.AlertIncrease(1.0f);
        Assert.AreEqual(TestStealth.ReturnAlertLevel(), 1);
        TestStealth.AlertIncrease(1.0f);
        Assert.AreEqual(TestStealth.ReturnAlertLevel(), 2);
        TestStealth.AlertIncrease(1.0f);
        Assert.AreEqual(TestStealth.ReturnAlertLevel(), 3);
        TestStealth.AlertIncrease(1.0f);
        Assert.AreEqual(TestStealth.ReturnAlertLevel(), 3);
        Assert.AreNotEqual(TestStealth.ReturnAlertLevel(), 2);
        Assert.AreNotEqual(TestStealth.ReturnAlertLevel(), 4);
    }

    [Test]
    public void StealthDecayTest()
    {
        Assert.IsFalse(TestStealth2.AlertDecrease);
        Assert.AreEqual(TestStealth2.AlertDecayTime, 60);
        Assert.AreEqual(TestStealth2.ReturnAlertLevel(), 0);
        Assert.IsFalse(TestStealth2.AlertDecrease);
        TestStealth2.AlertIncrease(1.0f);

    }
}
