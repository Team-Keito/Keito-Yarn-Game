using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Static Walkthrough", menuName = "SO/Static Walkthrough")]
public class StaticWalkthroughSO : ScriptableObject
{
    public string titleText;

    public Texture2D image;

    public string descriptionText;
}
