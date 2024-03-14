using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Animator),typeof(Image))]
public class SignPostNotification : MonoBehaviour
{   
    public float AnimationTime = 0.5f;
    public float DisplayTime = 2f;

    private Animator _animator;
    private Image _spriteImage;

    private static string ISOPEN = "IsOpen";
    private static string OPEN = "Open";
    private static string CLOSE = "Close";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteImage = GetComponent<Image>();

        _animator.speed = 1 / AnimationTime;
    }

    public void SetSprite(Sprite Image)
    {
        _spriteImage.sprite = Image;        
    }

    public void Open(Sprite Image)
    {
        SetSprite(Image);
        Open();
    }

    public void Open()
    {
        _animator.SetBool(ISOPEN, true);
        StartCoroutine(Wait(AnimationTime, () => StartCoroutine(Wait(DisplayTime, Close))));
    }

    private IEnumerator Wait(float time, System.Action cb)
    {
        yield return new WaitForSeconds(time);
        cb();
    }

    public void Close()
    {
        float animationTime = Mathf.Clamp(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 0.0f, 1.0f);        
        
        _animator?.Play(CLOSE, 0, 1-animationTime);
        _animator?.SetBool(ISOPEN, false);

        Destroy(gameObject, AnimationTime);
    }
}
