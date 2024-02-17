using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_CatHandler : MonoBehaviour
{
    public int Score = 0;

    [SerializeField]
    private Text _tempTextField;

    private AudioSource _catAudioSource;

    private void Awake()
    {
        _catAudioSource = GetComponent<AudioSource>();
    }

    public void OnUpdateScore(int value)
    {
        Score += value;
        _tempTextField.text = string.Format("Score: {0}\n+{1}", Score,value);
        Debug.Log(Score);

        _catAudioSource.Play();
    }
}
