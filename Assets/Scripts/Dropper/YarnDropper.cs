using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Code for dropping yarn ball from a specific point
/// </summary>
public class YarnDropper : Base_InputSystem
{
    private bool _onCoolDown = false;
    private int _currentYarnChoice = 0;
    [SerializeField] Camera _mainCam;
    [SerializeField] int _remainingYarn = 20;
    [SerializeField] GameObject[] _yarnPrefabs;
    [SerializeField] float _dropHeight = 10;
    [SerializeField] LayerMask _floorLayer;
    [SerializeField] float _dropperCoolDown = 10f;
    public UnityEvent OnGameEnd = new();

    public float DropperHeight => _dropHeight;
    public Color CurrentColor() => _yarnPrefabs[_currentYarnChoice].gameObject.GetComponent<MeshRenderer>().sharedMaterial.color;
    public int YarnRemaining => _remainingYarn;
    /// <summary>
    /// Initializes the dropper
    /// </summary>
    void Start()
    {
        transform.position = new(0, _dropHeight, 0);
        _currentYarnChoice = Random.Range(0, _yarnPrefabs.Length);
        if (!_mainCam) _mainCam = Camera.main;
        if (_yarnPrefabs.Length == 0) Debug.LogError("No yarn ball prefabs assigned!");

        _input.Player.Fire.performed += Fire_performed;
    }

    private void Fire_performed(InputAction.CallbackContext obj)
    {
        if (!_onCoolDown && Time.timeScale != 0)
        {
            SpawnYarnBall();
        }        
    }

    void Update()
    {
        transform.position = CalculateDropperPosition();
    }

    /// <summary>
    /// Calculate where the dropper should go from mouse position
    /// </summary>
    /// <returns></returns>
    public Vector3 CalculateDropperPosition()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Ray mouseRay = _mainCam.ScreenPointToRay(mouseScreenPosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity, _floorLayer))
        {
            // Calculate dropper upwards from where on the floor it was hit from
            return hit.point + Vector3.up * _dropHeight;
        }
        else
        {
            // Default to current position
            return transform.position;
        }
    }

    /// <summary>
    /// Spawns a random yarn ball prefab
    /// </summary>
    public void SpawnYarnBall()
    {
        if (_remainingYarn > 0)
        {
            _remainingYarn--;
            var yarn = _yarnPrefabs[_currentYarnChoice];

            Vector3 randomOffset =  new Vector3(Random.value*0.01f, 0, Random.value *0.01f);
            Instantiate(yarn, transform.position + randomOffset, Random.rotation);
            _currentYarnChoice = NextYarnChoice();
            StartCoroutine(RunCoolDown());
        }
        else
        {
            OnGameEnd.Invoke();
        }
    }

    /// <summary>
    /// Coroutine for waiting between drops. Currently based on wait time
    /// </summary>
    /// <returns></returns>
    IEnumerator RunCoolDown()
    {
        _onCoolDown = true;
        yield return new WaitForSeconds(_dropperCoolDown);
        _onCoolDown = false;
    }

    /// <summary>
    /// Chooses the next yarn item
    /// </summary>
    /// <returns>An index of the yarn prefabs array</returns>
    public int NextYarnChoice()
    {
        // TODO: Make next yarn choice smarter
        return (_currentYarnChoice + 1) % _yarnPrefabs.Length;
    }
}
