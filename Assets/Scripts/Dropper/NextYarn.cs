using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Get the next yarn, can be used for the Next N Yarn script
/// </summary>
public class NextYarn : MonoBehaviour
{
    [SerializeField] GameObject[] _yarnPrefabs;
    private GameObject _currentPrefab;
    private GameObject _nextPrefab;
    public GameObject GetCurrent => _currentPrefab;
    public GameObject GetNext => _nextPrefab;

    public GameObject ChooseNextYarn()
    {
        _currentPrefab = _nextPrefab;
        _nextPrefab = _yarnPrefabs[Random.Range(0, _yarnPrefabs.Length)];
        return _nextPrefab;
    }
}
