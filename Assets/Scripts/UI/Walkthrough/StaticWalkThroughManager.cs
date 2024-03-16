using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StaticWalkThroughManager : MonoBehaviour
{
    /// <summary>
    /// Walkthrough Slide helper class
    /// </summary>
    [System.Serializable]
    public class WalkThroughSlide
    {
        [TextArea(3, 5)]
        public string text;
        public bool usesImage;
        public Sprite image;
    }

    [SerializeField] GameManager _gameManager;

    // FIXME: Assumes that dialog and modal are just buttons with extra stuff

    [SerializeField, Tooltip("Walkthrough UI that is used for text only.")]
    DialogUI _walkThroughDialog;
    [SerializeField, Tooltip("Walkthrough UI that is used for text and image combo.")]
    ModalUI _walkThroughModal;

    [SerializeField] WalkThroughSlide[] _slides;
    // TODO: Set to 0 to immidiately transition, need to fix animation 
    [SerializeField] float _slideAnimateOutTime = 0;
    [SerializeField] float _slideAnimateInTime = 0;
    [SerializeField] UnityEvent OnWalkThroughFinish = new();

    private int _currentSlide = -1;
    private CursorLockMode originalLockMode;

    private void Start()
    {
        if (!_gameManager) _gameManager = FindAnyObjectByType<GameManager>();
        if (!_walkThroughDialog) Debug.LogError("Dialog UI object not assigned!");
        if (!_walkThroughModal) Debug.LogError("Modal UI object not assigned!");
        _currentSlide = -1;
        StartCoroutine(_walkThroughDialog.Deactivate());
        StartCoroutine(_walkThroughModal.Deactivate());
        RunNextSlide();
    }

    private void OnEnable()
    {
        if (Time.timeScale != 0) Time.timeScale = 0;
        InputManager.SwitchControls(ControlMap.UI);
    }

    private void OnDisable()
    {
        if (Time.timeScale == 0) Time.timeScale = 1;
        InputManager.SwitchControls(ControlMap.Player);

    }


    private void Update()
    {
        // FIXME: This is a hack to keep the game paused during static walkthrough
        OnEnable();
    }

    public void RunNextSlide()
    {
        StartCoroutine(NextSlide());
    }

    public IEnumerator NextSlide()
    {
        // TODO: fix fading animation
        if (_currentSlide >= 0)
        {
            // Get Old slide references
            IWalkThroughUI objectToFadeOut = _slides[_currentSlide].usesImage ? _walkThroughModal : _walkThroughDialog;
            // Deactive old slide
            objectToFadeOut.RemoveListener(RunNextSlide);
            yield return objectToFadeOut.Deactivate(_slideAnimateOutTime);
        }

        // Go to next slide
        _currentSlide++;
        if (_currentSlide >= _slides.Length)
        {
            // Finish if over slides length
            FinishWalkThrough();
        }
        else
        {
            // Fade In new slide
            IWalkThroughUI objectToFadeIn = _slides[_currentSlide].usesImage ? _walkThroughModal : _walkThroughDialog;
            // Add listener for button click for next slide
            objectToFadeIn.AddListener(RunNextSlide);
            // replaced text (and images if modal)
            objectToFadeIn.ChangeText(_slides[_currentSlide].text);
            if (_slides[_currentSlide].usesImage) objectToFadeIn.ChangeImage(_slides[_currentSlide].image);
            yield return objectToFadeIn.Activate(_slideAnimateInTime);
        }
    }

    public void FinishWalkThrough()
    {
        OnWalkThroughFinish.Invoke();
        this.enabled = false;
    }
}