using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IWalkThroughUI
{
    public IEnumerator Activate(float animationTime = 0);
    public void AddListener(UnityAction call);
    public void ChangeText(string newText);
    public void ChangeImage(Sprite newImage);
    public void RemoveListener(UnityAction call);
    public IEnumerator Deactivate(float animationTime = 0);
}