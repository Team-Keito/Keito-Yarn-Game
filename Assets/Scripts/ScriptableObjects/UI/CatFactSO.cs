using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cat Fact", menuName = "SO/Cat Fact")]
public class CatFactSO : ScriptableObject
{
    public string title;

    [Multiline(3)]
    public string factText;
}
