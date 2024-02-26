using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimationPlay : MonoBehaviour
{
    [SerializeField] Animator _catAnim;
    [SerializeField] AnimationClip _catRejectAnim;

    private void Start()
    {
        if (!_catAnim) _catAnim = GetComponent<Animator>();
    }

    public void PlayRejectAnimation()
    {
        if (_catAnim && _catRejectAnim)
        {
            _catAnim.Play(_catRejectAnim.name);
        }
    }
}
