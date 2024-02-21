using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SaveAndLoadTest
{
    private PlayerPrefSO playerPrefSO = new PlayerPrefSO();

    // A Test behaves as an ordinary method
    [Test]
    public void SavedIntDataTest()
    {
        string BestTimeString = playerPrefSO.currKey.ToString();
        PlayerPrefs.SetInt(BestTimeString, 10);

        Assert.AreEqual(10, PlayerPrefs.GetInt(BestTimeString));
    }

    [Test]
    public void LoadedIntDataTest()
    {
        string BestTimeString = playerPrefSO.currKey.ToString();
        PlayerPrefs.SetInt(BestTimeString, 10);

        Assert.AreEqual(10, PlayerPrefs.GetInt(BestTimeString));
    }

    [Test]
    public void DeletedIntDataTest()
    {
        string BestTimeString = playerPrefSO.currKey.ToString();
        PlayerPrefs.SetInt(BestTimeString, 10);
        PlayerPrefs.DeleteKey(BestTimeString);

        Assert.IsFalse(PlayerPrefs.HasKey(BestTimeString));
    }
}
