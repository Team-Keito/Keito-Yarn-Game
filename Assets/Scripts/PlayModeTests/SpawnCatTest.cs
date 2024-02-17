using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SpawnCatTest
{
    GameManager testManager = new GameObject().AddComponent<GameManager>();

    // A Test behaves as an ordinary method
    [Test]
    public void CatChangedLocationTest()
    {
        LinkedList<int> lastKnownLoc = new LinkedList<int>();

        // Set up two test game objects for positions
        testManager.spawnLocPrefab = new GameObject[2];

        // Set up the cat game object
        testManager.catGameObject = new GameObject();

        // Generates random points to test different locations
        for (int i = 0; i < testManager.spawnLocPrefab.Length; i++)
        {
            Vector3 randVec3 = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));

            testManager.spawnLocPrefab[i] = new GameObject();

            testManager.spawnLocPrefab[i].transform.position = randVec3;

            lastKnownLoc.AddLast(i);
        }

        testManager.ChangeCatLocation();

        Assert.AreNotEqual(testManager.spawnLocPrefab[lastKnownLoc.Last.Value].transform.position, testManager.catGameObject.transform.position);
    }

    [Test]
    public void FirstPosTest()
    {
        LinkedList<int> lastKnownLoc = new LinkedList<int>();

        lastKnownLoc.AddLast(0);

        Assert.AreEqual(0, lastKnownLoc.Last.Value);
    }

    [Test]
    public void NextPosSameTest()
    {
        LinkedList<int> lastKnownLoc = new LinkedList<int>();

        lastKnownLoc.AddLast(0);
        lastKnownLoc.AddLast(0);

        Assert.AreEqual(0, lastKnownLoc.Last.Value);
    }

    [Test]
    public void NextPosDifferentTest()
    {
        LinkedList<int> lastKnownLoc = new LinkedList<int>();

        lastKnownLoc.AddLast(0);
        lastKnownLoc.AddLast(1);

        Assert.AreNotEqual(0, lastKnownLoc.Last.Value);
    }

    [Test]
    public void PrevPosRemovedTest()
    {
        LinkedList<int> lastKnownLoc = new LinkedList<int>();

        lastKnownLoc.AddLast(0);
        lastKnownLoc.AddLast(1);
        lastKnownLoc.RemoveLast();

        Assert.AreEqual(0, lastKnownLoc.Last.Value);
    }
}
