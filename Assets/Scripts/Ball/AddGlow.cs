using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGlow : MonoBehaviour
{
    [SerializeField, Range(1, 10)] float _intensity = 5f;

    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.SetColor("_EmissionColor", _renderer.material.color * _intensity);
       
    }

    public void Toggle()
    {
        if (_renderer.material.IsKeywordEnabled("_EMISSION"))
        {
            DisableGlowEffect();
        }
        else
        {
            EnableGlowEffect();
        }
    }

    public void EnableGlowEffect()
    {
        _renderer.material.EnableKeyword("_EMISSION");
    }

    public void DisableGlowEffect()
    {
        _renderer.material.DisableKeyword("_EMISSION");
    }
}
