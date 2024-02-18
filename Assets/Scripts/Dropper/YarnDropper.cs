using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code for dropping yarn ball from a specific point
/// </summary>
public class YarnDropper : MonoBehaviour
{
    [SerializeField] GameObject[] yarnPrefabs;
    [SerializeField] float dropHeight = 10;

    public float DropperHeight => dropHeight;

    /// <summary>
    /// Initializes the dropper
    /// </summary>
    void Start()
    {
        transform.position = new(0, dropHeight, 0);
        if (yarnPrefabs.Length == 0) Debug.LogError("No yarn ball prefabs assigned!");
    }

#if UNITY_EDITOR
    void Update()
    {
        // If in editor, test spawning with left mouse button
        if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        {
            SpawnYarnBall();
        }
    }
#endif
    /// <summary>
    /// Spawns a random yarn ball prefab
    /// </summary>
    public void SpawnYarnBall()
    {
        var yarn = yarnPrefabs[NextYarnChoice()];
        Instantiate(yarn, transform.position, transform.rotation);
    }

    /// <summary>
    /// Chooses the next yarn item
    /// </summary>
    /// <returns>An index of the yarn prefabs array</returns>
    public int NextYarnChoice()
    {
        // TODO: Make next yarn choice smarter
        return Random.Range(0, yarnPrefabs.Length);
    }
}
