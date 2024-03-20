using System.Collections.Generic;
using Cinemachine;
using Manager.Score;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Object to handle moving focal point between yarn balls in level
/// </summary>
public class YarnBallFocuser : MonoBehaviour
{

    // TODO: Could work for other slingshots (dropper/tosser)
    [SerializeField] SlingShot _slingShot;
    [SerializeField] GameManager _gameManager;
    [SerializeField] Transform _focalPoint;
    private List<CameraSwitch> _yarnBallSwitchers = new(4);
    private CinemachineFreeLook _camera;
    private int current = 0;

    private GameObject _heldYarn;

    private void Start()
    {
        if (!_slingShot) _slingShot = FindObjectOfType<SlingShot>();
        if (!_gameManager) _gameManager = FindObjectOfType<GameManager>();
        if (!_camera) _camera = FindObjectOfType<CinemachineFreeLook>();
        if (!_focalPoint) Debug.LogError("No focal point found, did you forget to assign it in the inspector!");
    }

    private void OnEnable()
    {
        // Add listeners for:
        // 1. Input focus press
        InputManager.Input.Player.Focus.performed += SwitchCamera;
        // 2. Slingshot events to add yarn ball
        if (!_slingShot) return;
        _slingShot.OnStartThrow.AddListener(PutNewYarnOnHold);
        _slingShot.OnThrow.AddListener(AddNewYarn);
        // 3. Game Manager scoring and yarn ball combine to remove yarn ball
        if (!_gameManager) return;
        _gameManager.OnCatScored.AddListener(RunDeleteAll);
        if (_yarnBallSwitchers.Count > 0)
        {
            foreach (var cameraSwitch in _yarnBallSwitchers)
            {
                var ballCombine = cameraSwitch.GetComponent<BallCombine>();
                if (ballCombine) // is not null
                {
                    ballCombine.OnCombine.AddListener(RunDeleteAll);
                }
            }
        }
    }

    private void OnDisable()
    {
        // Remove listeners for:
        // 1. Input focus press
        InputManager.Input.Player.Focus.performed -= SwitchCamera;
        // 2. Slingshot events to add yarn ball
        if (!_slingShot) return;
        _slingShot.OnStartThrow.RemoveListener(PutNewYarnOnHold);
        _slingShot.OnThrow.RemoveListener(AddNewYarn);
        // 3. Game Manager scoring and yarn ball combine to remove yarn ball
        if (!_gameManager) return;
        _gameManager.OnCatScored.RemoveListener(RunDeleteAll);
        if (_yarnBallSwitchers.Count > 0)
        {
            foreach (var cameraSwitch in _yarnBallSwitchers)
            {
                var ballCombine = cameraSwitch.GetComponent<BallCombine>();
                if (ballCombine) // is not null
                {
                    ballCombine.OnCombine.RemoveListener(RunDeleteAll);
                }
            }
        }
    }

    public void SwitchCamera(InputAction.CallbackContext ctx) => SwitchCamera();
    public void SwitchCamera()
    {
        // Nothing to switch - focus on original focal point
        if (_yarnBallSwitchers.Count == 0)
        {
            _camera.Follow = _focalPoint;
            _camera.LookAt = _focalPoint;
            return;
        };

        // Next and cycle if overflow
        current++;
        current %= _yarnBallSwitchers.Count;
        var cameraSwitch = _yarnBallSwitchers[current];
        if (cameraSwitch) // is not null
        {
            cameraSwitch.ReFocusCamera();
        }
        else
        {
            var removeIndex = current;
            // Go back and cycle if overflow
            current--;
            current %= _yarnBallSwitchers.Count;
            // Do remove because we encountered a null
            _yarnBallSwitchers.RemoveAt(removeIndex);
            // Try again
            SwitchCamera();
        }
    }

    public void PutNewYarnOnHold()
    {
        _heldYarn = _slingShot.CurrentBall;
    }

    public void AddNewYarn()
    {
        if (!_heldYarn) return;
        var cameraSwitch = _heldYarn.GetComponent<CameraSwitch>();
        if (cameraSwitch) // is not null
        {
            _yarnBallSwitchers.Add(cameraSwitch);
            var ballCombine = _heldYarn.GetComponent<BallCombine>();
            if (ballCombine) // is not null
            {
                ballCombine.OnCombine.AddListener(RunDeleteAll);
            }
            _heldYarn = null;
        }
    }

    public void RunDeleteAll(ScoreData scoreData) => RunDeleteAll();

    public void RunDeleteAll()
    {
        _yarnBallSwitchers.RemoveAll(switchers => switchers == null);
    }
}