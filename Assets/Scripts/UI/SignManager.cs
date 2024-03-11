using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SignPostTypes
{
    HappyCat,
    AngryCat,
    BlueYarn,
    GreenYarn,
    RedYarn,
}


public class SignManager : MonoBehaviour
{
    [SerializeField] private GameObject _signPrefab;
    [SerializeField] private float _animationTime = 0.5f;
    [SerializeField] private float _displayTime = 2f;

    [Space(5)]

    [SerializeField] private Sprite _angryCat;
    [SerializeField] private Sprite _happyCat;
    [SerializeField] private Sprite _redYarn;
    [SerializeField] private Sprite _greenYarn;
    [SerializeField] private Sprite _blueYarn;

    private SignPostNotification _previous;

    private void Start()
    {
        SignPostNotification preset = _signPrefab.GetComponent<SignPostNotification>();
        preset.AnimationTime = _animationTime;
        preset.DisplayTime = _displayTime;
    }

    public void Open(SignPostTypes type)
    {
        switch (type)
        {
            case SignPostTypes.AngryCat: Open(_angryCat); break;
            case SignPostTypes.HappyCat: Open(_happyCat); break;
            case SignPostTypes.BlueYarn: Open(_blueYarn); break;
            case SignPostTypes.GreenYarn: Open(_greenYarn); break;
            case SignPostTypes.RedYarn: Open(_redYarn); break;
        }
    }

    public void Open(Sprite sprite)
    {
        if(_previous != null)
        {
            _previous?.Close();
        }

        _previous = CreateSign(sprite);
    }

    private SignPostNotification CreateSign(Sprite sprite)
    {
        GameObject go = Instantiate(_signPrefab, transform);
        SignPostNotification sign = go.GetComponent<SignPostNotification>();
        sign.Open(sprite);

        return sign;
    }
}
