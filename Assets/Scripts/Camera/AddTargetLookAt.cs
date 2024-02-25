using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AddTargetLookAt : MonoBehaviour
{
    private int _memberIndex = -1;
    [SerializeField] CinemachineTargetGroup _targetGroup;
    [SerializeField, Tooltip("The transform to find with a given tag")]
    private TagSO _lookAtTag;
    [SerializeField, Tooltip("Transform to look at, overridding the lookAtTag")]
    private Transform _inLevelLookAt;
    [SerializeField] float _weight = 1;
    [SerializeField] float _radius = 1;
    [SerializeField] bool _isWeightChanging = false;
    [SerializeField] float _changeDelay = 5;
    [SerializeField, Tooltip("If weight will change, use this to control how weight changes.")]
    private KeyframeCurve _weightOverTime;


    IEnumerator Start()
    {
        if (!_targetGroup) _targetGroup = GetComponent<CinemachineTargetGroup>();
        // Wait for all other items to spawn in if at start.
        yield return new WaitForEndOfFrame();
        // Get transform if not already assigned using the tag provided
        if (!_inLevelLookAt && _lookAtTag && GameObject.FindGameObjectWithTag(_lookAtTag.Tag) is GameObject found)
        {
            _inLevelLookAt = found.transform;
        }
        // If transform is there, add it to the target group
        if (_inLevelLookAt)
        {
            _targetGroup.AddMember(_inLevelLookAt, _weight, _radius);
            _memberIndex = _targetGroup.FindMember(_inLevelLookAt);
        }
        yield return new WaitForEndOfFrame();
        // Change item influence on Camera
        if (_isWeightChanging && -1 < _memberIndex)
        {
            StartCoroutine(ChangeWeight());
        }
    }

    /// <summary>
    /// Changing weight based on provided keyframe curve
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeWeight()
    {
        // Delay before running target weight change
        if (_changeDelay > 0)
        {
            yield return new WaitForSeconds(_changeDelay);
        }
        _weightOverTime.InitializeTime(Time.time);
        // Run target weight change
        for (float currentTime = Time.time; !_weightOverTime.IsFinished(currentTime); currentTime = Time.time)
        {
            _targetGroup.m_Targets[_memberIndex].weight = _weightOverTime.GetCurrentValue(currentTime);
            yield return new WaitForEndOfFrame();
        }
    }
}

/// <summary>
/// A holder of keyframe data as a curve that can be accessed over time
/// </summary>
[System.Serializable]
public class KeyframeCurve
{
    [SerializeField]
    private AnimationCurve _curve;
    [SerializeField]
    private float _startTime;
    [SerializeField]
    private float _duration;

    public void InitializeTime(float startTime)
    {
        _startTime = startTime;
    }

    public float GetTimePercent(float currentTime) => Mathf.Clamp01((currentTime - _startTime) / _duration);
    public float GetCurrentValue(float currentTime) => _curve.Evaluate(GetTimePercent(currentTime));
    public bool IsFinished(float currentTime) => currentTime - _startTime >= _duration;
}
