using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Get the next yarn, can be used for the Next N Yarn script
/// </summary>
public class NextYarn : MonoBehaviour
{
    [SerializeField] GameObject[] _yarnPrefabs;
    private GameObject _currentYarn;
    private GameObject _nextYarn;
    public GameObject GetCurrent() => _currentYarn;
    public GameObject GetNext() => _nextYarn;

    private void Awake()
    {
        ChooseNextYarn();
    }

    public GameObject ChooseNextYarn()
    {
        _currentYarn = _nextYarn != null ? _nextYarn : _yarnPrefabs[Random.Range(0, _yarnPrefabs.Length)];
        _nextYarn = _yarnPrefabs[Random.Range(0, _yarnPrefabs.Length)];
        return _nextYarn;
    }
}
