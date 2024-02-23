using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NextColor
{
    [SerializeField] private int _count = 3;
    [SerializeField] private GameObject[] _prefabs;

    public Queue<Color> NextColors = new();
    public Queue<GameObject> NextYarns = new();

    public void Setup()
    {
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
        int RandInt = Random.Range(0, _prefabs.Length);
        return _prefabs[RandInt];
    }
}
