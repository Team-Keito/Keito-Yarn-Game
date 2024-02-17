using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ScoreTest
{
    GameManager testObject = new GameObject().AddComponent<GameManager>();

    // A Test behaves as an ordinary method
    [Test]
    public void AddScoreTest()
    {
        testObject.Score += 5;

        Assert.AreEqual(5, testObject.Score);
    }

    [Test]
    public void RemoveScoreTest()
    {
        testObject.Score -= 5;

        Assert.AreEqual(0, testObject.Score);
    }

    [Test]
    public void ScoreExceedsHighScoreTest()
    {
        testObject.Score = 0;
        testObject.HighScore = 0;
        testObject.Score += 10;
        testObject.HighScore = testObject.Score;

        Assert.AreEqual(10, testObject.HighScore);
    }

    [Test]
    public void ScoreLessThanHighScoreTest()
    {
        testObject.Score = 0;
        testObject.HighScore = 10;
        testObject.HighScore = testObject.Score;

        Assert.AreNotEqual(10, testObject.HighScore);
    }
}
