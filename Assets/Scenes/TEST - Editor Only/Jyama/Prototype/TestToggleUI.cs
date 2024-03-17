using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class TestToggleUI : MonoBehaviour
{
    private readonly string _hide = "Finish";
    private readonly string _show = "EditorOnly";

    public readonly List<GameObject> _list = new();
    private Toggle _toggle;

    // Start is called before the first frame update
    void Start()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(HandleToggle);

        GameObject[] list = GameObject.FindGameObjectsWithTag(_hide);

        foreach(GameObject item in list)
        {
            item.SetActive(false);
        }

        _list.AddRange(list);
        _list.AddRange(GameObject.FindGameObjectsWithTag(_show));

        _list.ConvertAll<string>(new Converter<GameObject, string>((GameObject obj) => obj.name));
        print(String.Join<GameObject>(" , ", _list));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _toggle.isOn = !_toggle.isOn;
        }
    }

    private void HandleToggle(bool state)
    {
        _list.ForEach((GameObject go) => go.SetActive(!go.activeSelf));
    }
}
