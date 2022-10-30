using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TerrainTest
{
    MapGeneration MG = new MapGeneration();
    public Texture2D TestImageOne;

    // Tests that the path generation returns only one path at a time
    [Test]
    public void PlaceHolder()
    {
        Assert.AreEqual(1, 1);
    }
}
