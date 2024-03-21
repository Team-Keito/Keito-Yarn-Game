using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour, IDamageable, IRepairable
{
    [SerializeField] private ColorSO _color;
    [SerializeField] private float _damageMod = 0.5f;
    [SerializeField] private float _repairMod = 1f;
    
    private bool _isDamaged = false;

    public ColorSO Color => _color;
    private Renderer _render;

    private void Start()
    {
        _render = gameObject.GetComponent<Renderer>();
    }

    private void SetColor(float modifier = 1f)
    {
        _render.material.color = _color.Color * modifier;
    }

    public void Damage()
    {
        if (!_isDamaged)
        {
            _isDamaged = true;
            SetColor(0.5f);
        }        
    }

    public void Repair()
    {
        _isDamaged = false;
        SetColor();
    }

    public bool isDamaged()
    {
        return _isDamaged;
    }
}
