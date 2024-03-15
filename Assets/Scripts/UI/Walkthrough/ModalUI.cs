using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModalUI : MonoBehaviour, IWalkThroughUI
{
    [SerializeField] Button _nextButton;
    [SerializeField] Image _imageUI;
    [SerializeField] TextMeshProUGUI _textUI;

    private void Awake()
    {
        ImmidateAlphaChange();
    }

    private void ImmidateAlphaChange(float value = 1)
    {
        _textUI.alpha = value;
        var colorUpdate = _imageUI.color;
        colorUpdate.a = value;
        _imageUI.color = colorUpdate;
        colorUpdate = _nextButton.image.color;
        colorUpdate.a = value;
        _nextButton.image.color = colorUpdate;
    }

    public IEnumerator Activate(float animationTime = 0)
    {
        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(gameObject);
        if (animationTime > 0)
        {
            float totalTime = Application.targetFrameRate * animationTime;
            for (int i = 0; i <= totalTime; i++)
            {
                ImmidateAlphaChange(i / totalTime);
                yield return null;
            }

        }
        else
        {
            ImmidateAlphaChange(1);
        }
    }

    public void AddListener(UnityAction call)
    {
        _nextButton.onClick.AddListener(call);
    }

    public void ChangeImage(Sprite newImage)
    {
        _imageUI.sprite = newImage;
    }

    public void ChangeText(string newText)
    {
        _textUI.text = newText;
    }

    public IEnumerator Deactivate(float animationTime = 0)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (animationTime > 0)
        {
            float totalTime = Application.targetFrameRate * animationTime;
            for (int i = 0; i <= totalTime; i++)
            {
                ImmidateAlphaChange(1 - (i / totalTime));
                yield return null;
            }

        }
        else
        {
            ImmidateAlphaChange(0);
        }
        gameObject.SetActive(false);
    }

    public void RemoveListener(UnityAction call)
    {
        _nextButton.onClick.RemoveListener(call);
    }
}
