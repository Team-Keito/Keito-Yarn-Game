using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Tag", menuName ="SO/Tag")]
public class TagSO : ScriptableObject
{
    [SerializeField]
    private string _tag;

    public string Tag => _tag;
}
