using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpawnBall : MonoBehaviour
{
    [SerializeField] private List<YarnColor> _colors;
    [SerializeField] private float _ballDropHeight = 10;

    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _cusorIndicator;

    private int _colorIndex = 0;
    private IA_Controls inputActions;

    private YarnColor YarnColor => _colors[_colorIndex];
    private Color CurrentColor => YarnColor.Color;
    

    private void Awake()
    {
        inputActions = new IA_Controls();

        inputActions.Dev.SpawnBall.performed += SpawnBall_performed;
        inputActions.Dev.ChangeColor.performed += ChangeColor_performed;

        _image.color = CurrentColor;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    /// <summary>
    /// Move spawn cube indcator
    /// </summary>
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hitData))
        {
            _cusorIndicator.transform.position = hitData.point;
        }
    }

    private void ChangeColor_performed(InputAction.CallbackContext obj)
    {
        _colorIndex = (_colorIndex + 1) % _colors.Count;
        _image.color = CurrentColor;
    }

    private void SpawnBall_performed(InputAction.CallbackContext obj)
    {
        GameObject go = Instantiate(_ballPrefab, _cusorIndicator.transform.position + _cusorIndicator.transform.up * _ballDropHeight, _cusorIndicator.transform.rotation);

        go.GetComponent<Ball>().SetColor(YarnColor);
    }
}
