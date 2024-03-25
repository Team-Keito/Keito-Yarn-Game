using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Pref", menuName = "SO/Player Pref")]
public class PlayerPrefSO : ScriptableObject
{
    public enum Key
    {
        Best_Time,
        Master_Volume,
        Music_Volume,
        Sound_Volume,
        Mouse_Sensitivity
    }

    public Key currKey;
}
