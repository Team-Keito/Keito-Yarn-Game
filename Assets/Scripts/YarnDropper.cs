using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Code for dropping yarn ball from a specific point
/// </summary>
public class YarnDropper : MonoBehaviour
{
    [SerializeField] GameObject yarnPrefab;
    [SerializeField] float dropHeight = 10;

    public float DropperHeight => dropHeight;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new(0, dropHeight, 0);
        if (!yarnPrefab) Debug.LogError("Yarn ball not assigned!");
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Make more generic with Input Action
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SpawnYarnBall();
        }
    }

    /// <summary>
    /// Spawns a yarn ball prefab
    /// </summary>
    public void SpawnYarnBall()
    {
        Instantiate(yarnPrefab, transform.position, transform.rotation);
    }
}
