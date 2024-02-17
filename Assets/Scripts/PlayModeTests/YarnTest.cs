using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class YarnTest
{
    GameManager testObject = new GameObject().AddComponent<GameManager>();

    // A Test behaves as an ordinary method
    [Test]
    public void AddYarnTest()
    {
        testObject.NumOfYarn += 1;

        Assert.AreEqual(1, testObject.NumOfYarn);
    }

    [Test]
    public void RemoveYarnTest()
    {
        testObject.NumOfYarn -= 1;

        Assert.AreEqual(0, testObject.NumOfYarn);
    }
}
