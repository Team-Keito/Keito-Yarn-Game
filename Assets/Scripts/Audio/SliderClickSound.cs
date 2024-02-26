using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderClickSound : MonoBehaviour
{
    public AK.Wwise.Event ClickSound; // Reference to the Wwise sound event

    private Slider slider; // Reference to the slider component

    private void Start()
    {
        // Get the slider component
        slider = GetComponent<Slider>();

        // Add this script as an event listener to detect pointer down events on the slider handle
        if (slider != null)
        {
            slider.onValueChanged.AddListener((value) => { }); // Ensure slider handles value change
        }
        else
        {
            Debug.LogWarning("Slider component not found!");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Check if the handle is clicked
        if (RectTransformUtility.RectangleContainsScreenPoint(slider.handleRect, eventData.position))
        {
            // Play the ClickSound event
            if (ClickSound != null)
            {
                ClickSound.Post(gameObject);
            }
            else
            {
                Debug.LogWarning("ClickSound event is not assigned!");
            }
        }
    }
}
