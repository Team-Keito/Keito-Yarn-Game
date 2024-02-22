using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NextColor 
{
    [SerializeField] private int _count = 3;
    [SerializeField] private GameObject[] _prefabs;    

    public LinkedList<GameObject> NextYarns = new LinkedList<GameObject>();

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
        return NextYarns.First.Value;
    }

    public void Remove()
    {
        NextYarns.RemoveFirst();
        Add();
    }

    public Color GetColor()
    {
        return GetPrefab().GetComponent<Renderer>().sharedMaterial.color;
    }

    private void Add()
    {
        NextYarns.AddLast(GetRandomPrefab());
    }

    private GameObject GetRandomPrefab()
    {        
        int RandInt = Random.Range(0, _prefabs.Length);

        return _prefabs[RandInt];
    }
}
