using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NextColor
{
    [SerializeField, Tooltip("Number of yarn balls in queue")] private int _count = 3;
    [SerializeField] private GameManager _gameManager;

    public Queue<Color> NextColors = new();
    public Queue<GameObject> NextYarns = new();

    public void Setup(GameManager gameManager)
    {
        _gameManager = gameManager;
        Random.InitState(System.DateTime.Now.Millisecond);
        for (int i = 0; i < _count; i++)
        {
            Add();
        }
    }

    public GameObject GetPrefab()
    {
        return NextYarns.Peek();
    }

    public Color GetColor()
    {
        return NextColors.Peek();
    }

    public void Remove()
    {
        NextYarns.Dequeue();
        NextColors.Dequeue();
        Add();
    }

    private void Add()
    {
        GameObject go = GetRandomPrefab();
        NextYarns.Enqueue(go);
        NextColors.Enqueue(go.GetComponent<Renderer>().sharedMaterial.color);
    }

    private GameObject GetRandomPrefab()
    {
        return _gameManager.GetRandomColorYarn();
    }
}
