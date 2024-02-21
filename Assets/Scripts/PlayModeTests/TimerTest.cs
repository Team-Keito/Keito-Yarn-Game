using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TimerTest
{
    GameManager testManager = new GameObject().AddComponent<GameManager>();

    /// <summary>
    /// Tests if the current time exceeds the best time and overrides it if it is
    /// </summary>
    [Test]
    public void BestTimeTest()
    {
        testManager.CurrentTime = 10;
        testManager.BestTime = 0;
        testManager.BestTime = testManager.CurrentTime;

        Assert.AreEqual(10, testManager.BestTime);
    }

    /// <summary>
    /// Tests if the best time does not exceed the current time and does not override it if it is
    /// </summary>
    [Test]
    public void CurrentTimeTest()
    {
        testManager.CurrentTime = 1;
        testManager.BestTime = 10;
        testManager.BestTime = testManager.CurrentTime;

        Assert.AreEqual(10, testManager.BestTime);
    }
}
